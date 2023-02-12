using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangChangeSimulator
{
    public class lexiconclass
    {
        public Dictionary<int, List<int>> concepts = new Dictionary<int, List<int>>();
        //public Dictionary<int, List<int>> words = new Dictionary<int, List<int>>();
        public List<wordclass> words = new List<wordclass>(); //words currently in the language
        public List<wordclass> oldwords = new List<wordclass>(); //words formerly in the language

        public lexiconclass(languageclass lc, List<int> conceptlist) //new lexicon for new language
        {
            Dictionary<string, wordclass> formdict = new Dictionary<string, wordclass>();

            if (lc.source.Length == 3) //words from existing language
            {

                var q = from c in Form1.dbclics3.CodedFormTable where c.LanguageTable2.Iso == lc.source select c;
                foreach (CodedFormTable cft in q)
                {
                    if (cft.CodedForm.Trim().Length == 0)
                        continue;
                    if (!conceptlist.Contains(cft.Concepticon))
                        continue;

                    int wordid = -1;
                    wordclass wc;
                    if (formdict.ContainsKey(cft.CodedForm))
                    {
                        wc = formdict[cft.CodedForm];
                        wordid = wc.id;
                        //wc = wordclass.globalworddict[wordid];
                    }
                    else
                    {
                        wc = new wordclass(cft.CodedForm, -1, null, lc.id);
                        wordid = wc.id;
                        formdict.Add(cft.CodedForm, wc);
                        words.Add(wc);
                    }
                    if (!concepts.ContainsKey(cft.Concepticon))
                        concepts.Add(cft.Concepticon, new List<int>());
                    if (!concepts[cft.Concepticon].Contains(wordid))
                        concepts[cft.Concepticon].Add(wordid);
                    //if (!words.Contains(wordid))
                    //    words.Add(wordid);
                    wc.addconcept(cft.Concepticon);
                    //if (!words[wordid].Contains(cft.Concepticon))
                    //    words[wordid].Add(cft.Concepticon);
                }
            }
            else
            {
                //create new words from scratch in some way
            }

        }

        public lexiconclass(languageclass lc,lexiconclass lx) //clone lexicon from existing language
        {
            foreach (wordclass w in lx.words)
            {
                //wordclass wc = new wordclass(wordclass.getform(w), wordclass.globalworddict[w].root, wordclass.globalworddict[w].ancestor, lc.id);
                wordclass wc = new wordclass(w, lc.id);
                this.addword_noarea(wc);
            }
        }

        public void addword(wordclass wc)
        {
            words.Add(wc);
            foreach (int ic in wc.concepts)
            {
                if (!this.concepts.ContainsKey(ic))
                    this.concepts.Add(ic, new List<int>());
                this.concepts[ic].Add(wc.id);
            }

            getarea(wc).add(wc);
        }

        public void addword_noarea(wordclass wc)
        {
            words.Add(wc);
            foreach (int ic in wc.concepts)
            {
                if (!this.concepts.ContainsKey(ic))
                    this.concepts.Add(ic, new List<int>());
                this.concepts[ic].Add(wc.id);
            }

        }

        public wordclass getword(int wordid)
        {
            return (from c in this.words where c.id == wordid select c).FirstOrDefault();
        }

        public void removesense(int wordid,int concept)
        {
            wordclass w = getword(wordid);
            getarea(w).remove(w, concept);
            w.concepts.Remove(concept);
            if (w.concepts.Count == 0)
            {
                oldwords.Add(w);
                words.Remove(w);
            }
            concepts[concept].Remove(wordid);
        }

        public void addsense(int wordid,int concept)
        {
            wordclass w = getword(wordid);
            if (w.concepts.Contains(concept))
                return;
            w.concepts.Add(concept);
            if (!concepts.ContainsKey(concept))
                concepts.Add(concept, new List<int>());
            if (!concepts[concept].Contains(wordid))
                concepts[concept].Add(wordid);
            getarea(w).add(w,concept);
        }

        public areaclass getarea(wordclass w)
        {
            return mapgridclass.map[languageclass.langdict[w.language].ilat, languageclass.langdict[w.language].ilon].aa;
        }

        public List<string> segmentsused()
        {
            List<string> ls = new List<string>();
            foreach (wordclass wc in this.words)
            {
                foreach (char k in wc.codedform.ToCharArray())
                {
                    string s = segmentclass.getfullseg(k);
                    if (!ls.Contains(s))
                        ls.Add(s);
                }
            }
            return ls;
        }
        public List<int> segidsused()
        {
            List<int> ls = new List<int>();
            foreach (wordclass wc in this.words)
            {
                foreach (char k in wc.codedform.ToCharArray())
                {
                    if (!ls.Contains((int)k))
                        ls.Add((int)k);
                }
            }
            return ls;
        }

        public string contextbuilder(char segtype)
        {
            StringBuilder sb = new StringBuilder(wordclass.thischar.ToString());
            Random rnd = new Random();
            char otherseg = (segtype == 'C') ? 'V' : 'C';
            double rn = rnd.NextDouble();
            if (rn < 0.3)
            {
                sb.Insert(0, wordclass.wordstart);
                sb.Append(otherseg);
            }
            else if (rn < 0.6)
            {
                sb.Insert(0, otherseg);
                sb.Append(wordclass.wordend);
            }
            else
            {
                sb.Insert(0, otherseg);
                sb.Append(otherseg);
            }
            return sb.ToString();
        }

        public string contextstring(char oldc,char newc,string context)
        {
            StringBuilder sb = new StringBuilder("");
            sb.Append(segmentclass.segmentdict[(int)oldc].fullseg);
            sb.Append("→");
            sb.Append(segmentclass.segmentdict[(int)newc].fullseg+" / "+context);
            return sb.ToString();
        }

        public void unconditional_soundchange(int oldsound, int newsound)
        {
            char oldc = (char)oldsound;
            char newc = (char)newsound;
            foreach (wordclass wc in this.words)
                wc.replacesound(oldc, newc);
        }
        public void conditional_soundchange(int oldsound, int newsound)
        {
            char oldc = (char)oldsound;
            char segtype = segmentclass.segmentdict[oldsound].soundtype;
            char newc = (char)newsound;
            string context = contextbuilder(segtype);
            //Console.WriteLine(contextstring(oldc, newc, context));
            foreach (wordclass wc in this.words)
                wc.replacesound(oldc, newc,context);
        }
    }
}
