using Colexification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LangChangeSimulator
{
    public class wordclass
    {
        public static int maxid = 0;
        public static Dictionary<int, wordclass> globalrootdict = new Dictionary<int, wordclass>();
        public static char wordstart = '#';
        public static char wordend = '#';
        public static char thischar = '_';
        public static string startthis = wordstart.ToString() + thischar.ToString();
        public static string thisend = thischar.ToString() + wordend.ToString();

        public int id;
        public string codedform = "";
        public int root = -1;  //id of original ancestor of cognate family; index to globalrootdict; identifies cognate family
        public wordclass ancestor = null; //immediate ancestor word; null if this is root
        public int language = -1; //id of language
        public List<int> concepts = new List<int>(); //list of word meanings
        public List<int> oldconcepts = new List<int>(); //list of former word meanings

        public wordclass(string cform, int rootpar, wordclass ancestorpar, int lang)
        {
            maxid++;
            this.id = maxid;
            this.codedform = cform;
            if (rootpar >= 0)
                this.root = rootpar;
            else
            {
                this.root = this.id;
                globalrootdict.Add(this.id, this);
            }
            this.ancestor = ancestorpar;
            this.language = lang;

        }

        public wordclass(wordclass w, int lang) //make a clone of word w
        {
            maxid++;
            this.id = maxid;
            this.codedform = w.codedform;
            this.root = w.root;
            this.ancestor = w;
            this.language = lang;
            foreach (int ic in w.concepts)
                this.concepts.Add(ic);
            //globalworddict.Add(this.id, this);
        }

        public wordclass(soundsystemclass ss, int conceptcode, string cvcv, int lang) //coin a brand-new word
        {
            maxid++;
            this.id = maxid;
            Random rnd = new Random();
            string s = "";
            foreach (char c in cvcv)
            {
                s += (char)ss.soundtype(c,rnd);
            }
            //MessageBox.Show("New word " + segmentclass.DecodeForm(s));
            this.codedform = s;
            this.root = this.id;
            globalrootdict.Add(this.id, this);
            this.language = lang;
            if (conceptcode >= 0)
                this.concepts.Add(conceptcode);
        }

        public string cvcvform() //returns word as "#CVCCV#"
        {
            StringBuilder sb = new StringBuilder(wordstart);
            foreach (char c in this.codedform)
            {
                sb.Append(segmentclass.segmentdict[c].soundtype);
            }
            return sb.Append(wordend).ToString();
        }

        public string cvcvform(char replacechar) //returns word as "#CVCCV#"
        {
            StringBuilder sb = new StringBuilder(wordstart);
            foreach (char c in this.codedform)
            {
                if (c == replacechar)
                    sb.Append(thischar);
                else
                    sb.Append(segmentclass.segmentdict[c].soundtype);
            }
            return sb.Append(wordend).ToString();
        }

        public void addconcept(int conceptcode)
        {
            if (!concepts.Contains(conceptcode))
                concepts.Add(conceptcode);
        }

        //public static string getform(int wordid)
        //{
        //    return segmentclass.DecodeForm(globalrootdict[wordid].codedform);
        //}

        public string getform()
        {
            return segmentclass.DecodeForm(codedform);
        }

        public static List<string> getmeanings(wordclass w)
        {
            List<string> ls = new List<string>();
            foreach (int ic in w.concepts)
                ls.Add(swadeshclass.codeconceptdict[ic]);
            return ls;
        }

        public void replacesound(char oldc,char newc) //unconditional sound change
        {
            this.codedform = this.codedform.Replace(oldc, newc);
        }

        public void replacesound(char oldc, char newc,string context) //conditional sound change
        {
            string old = this.codedform;
            string cvcv = this.cvcvform(oldc);
            if (context.StartsWith(startthis))
            {
                if (cvcv.StartsWith(context))
                {
                    this.codedform = util.ReplaceFirstOccurrence(this.codedform, oldc.ToString(), newc.ToString());
                }
            }
            else if (context.EndsWith(thisend))
            {
                if (cvcv.EndsWith(context))
                {
                    this.codedform = util.ReplaceLastOccurrence(this.codedform, oldc.ToString(), newc.ToString());
                }
            }
            else if (cvcv.Contains(context))
            {
                int oldcpos = cvcv.IndexOf(context) + context.IndexOf(thischar);
                this.codedform = this.codedform.Remove(oldcpos, 1).Insert(oldcpos, newc.ToString());
            }
            //if (this.codedform != old)
            //{
            //    Console.WriteLine(segmentclass.DecodeForm(old) + " → " + segmentclass.DecodeForm(this.codedform));
            //}
        }

        public string getconceptjson()
        {
            bool f2 = true;
            StringBuilder sbc = new StringBuilder("[");
            foreach (int concept in this.concepts)
            {
                if (!f2)
                    sbc.Append(",");
                f2 = false;
                sbc.Append("{\"" + swadeshclass.codeconceptdict[concept] + "\":" + concept + "}");
            }
            sbc.Append("]");
            return sbc.ToString();
        }

        public void metathesis(Random rnd)
        {
            if (codedform.Length == 1)
                return;
            int i = rnd.Next(codedform.Length - 2);
            if (codedform.Length < 4)
                i = 0;
            char[] cf = codedform.ToCharArray();
            char c = cf[i];
            int d = rnd.Next(1)+1;
            if (i + d >= codedform.Length)
                d = 1;
            cf[i] = cf[i + d];
            cf[i + d] = c;
            codedform = new string(cf);
            //Console.WriteLine("metathesis");
        }

        public bool close(wordclass wc2,int maxdist)
        {
            int dist = Levenshtein.EditDistance(this.codedform, wc2.codedform);
            if (dist <= maxdist)
                return true;
            else
                return false;
        }
    }
}
