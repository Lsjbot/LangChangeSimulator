using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace LangChangeSimulator
{
    class swadeshclass
    {
        public static Dictionary<string, int> conceptcodedict = new Dictionary<string, int>();
        public static Dictionary<int, string> codeconceptdict = new Dictionary<int, string>();
        public static Dictionary<int, double> conceptchangerate = new Dictionary<int, double>();
        public static Dictionary<int, int> colexratedict = new Dictionary<int, int>();
        public static Dictionary<int, Dictionary<int, int>> colexdict = new Dictionary<int, Dictionary<int, int>>();

        public static int colexmax = 1;
        public static string swadeshtype = "";

        public static void fillconceptdicts()
        {
            Random rnd = new Random();
            var q = from c in Form1.dbclics3.ConcepticonTable select c;
            foreach (ConcepticonTable ct in q)
            {
                conceptcodedict.Add(ct.Concepticon_Gloss, ct.ID);
                codeconceptdict.Add(ct.ID, ct.Concepticon_Gloss);
                conceptchangerate.Add(ct.ID, 0.5 + rnd.NextDouble());
            }

            string fn = Form1.folder + "colexratedict.txt";
            if (File.Exists(fn))
            {
                using (StreamReader sr = new StreamReader(fn))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] words = line.Split('\t');
                        int cid = util.tryconvert(words[0]);
                        int totalcolex = util.tryconvert(words[2]);
                        colexratedict.Add(cid, totalcolex);
                        if (totalcolex > colexmax)
                            colexmax = totalcolex;
                    }
                }
            }
            else
            {
                foreach (int cid in codeconceptdict.Keys)
                {
                    var qcl = from c in Form1.dbclics3.ConcepticonLink
                              where (c.Concepticon1 == cid) || (c.Concepticon2 == cid)
                              select c.Strength;
                    int totalcolex = qcl.Count() > 0 ? qcl.Sum() : 0;
                    colexratedict.Add(cid, totalcolex);
                    if (totalcolex > colexmax)
                        colexmax = totalcolex;

                }

            }
            save_colexrate(fn);
        }

        public static void save_colexrate(string fn)
        {
            using (StreamWriter sw = new StreamWriter(fn))
            {
                foreach (int i in colexratedict.Keys)
                {
                    sw.WriteLine(i + "\t" + codeconceptdict[i] + "\t" + colexratedict[i]);
                }
            }
        }

        public static List<string> conceptlist(string listtype)
        {
            swadeshtype = listtype;
            if (listtype.Contains("100"))
                return swadesh100;
            else if (listtype.Contains("200"))
                return swadesh200;
            else if (listtype.Contains("numbers"))
                return numbersonly;
            else if (listtype.ToLower().Contains("clics"))
            {
                return (from c in Form1.dbclics3.ConcepticonTable select c.Concepticon_Gloss).ToList();
            }
            else
                return new List<string>();
        }

        public static Dictionary<string, List<int>> conceptcodelistdict = new Dictionary<string, List<int>>();

        public static List<int> conceptcodelist(string listtype)
        {
            if (conceptcodelistdict.ContainsKey(listtype))
                return conceptcodelistdict[listtype];
            swadeshtype = listtype;
            if (conceptcodedict.Count == 0)
                fillconceptdicts();
            if (listtype.ToLower().Contains("swad")|| listtype.ToLower().Contains("numbers"))
            {
                List<string> lss = new List<string>();
                if (listtype.Contains("100"))
                    lss = swadesh100;
                else if (listtype.Contains("200"))
                    lss = swadesh200;
                else if (listtype.Contains("numbers"))
                    lss = numbersonly;
                var q = from c in Form1.dbclics3.ConcepticonTable
                        where lss.Contains(c.Concepticon_Gloss)
                        select c.ID;
                conceptcodelistdict.Add(listtype, q.ToList());
                return q.ToList();
            }
            else if (listtype.ToLower().Contains("clics"))
            {
                var q = from c in Form1.dbclics3.ConcepticonTable select c.ID;
                conceptcodelistdict.Add(listtype, q.ToList());
                return q.ToList();
            }
            else
                return new List<int>();

        }

        public static void write_swadeshtable(string fn, string src)
        {
            Dictionary<int, List<wordclass>> classdict = new Dictionary<int, List<wordclass>>();
            string fncog = fn.Replace("swadesh", "truecognatetable");
            using (StreamWriter sw = new StreamWriter(fn))
            using (StreamWriter swcog = new StreamWriter(fncog))
            {
                StringBuilder sb = new StringBuilder("Lang\tLatitude\tLongitude");
                StringBuilder sbcog = new StringBuilder("Lang\tFamily");
                foreach (int i in swadeshclass.conceptcodelist(swadeshclass.swadeshtype))
                {
                    sb.Append("\t"+swadeshclass.codeconceptdict[i]);
                    sbcog.Append("\t" + swadeshclass.codeconceptdict[i]);
                }
                sw.WriteLine(sb.ToString());
                swcog.WriteLine(sbcog.ToString());

                var q = from c in languageclass.langdict.Values where c.speakers > 0 select c;
                if (!String.IsNullOrEmpty(src))
                    q = from c in q where c.source == src select c;
                foreach (languageclass lc in q)
                {
                    sb = new StringBuilder(lc.id + "\t"+lc.latitude()+"\t"+lc.longitude());
                    sbcog = new StringBuilder(lc.id + "\t"+lc.source);
                    foreach (int i in swadeshclass.conceptcodelist(swadeshclass.swadeshtype))
                    {
                        sb.Append("\t");
                        sbcog.Append("\t");
                        if (lc.lexicon.concepts.ContainsKey(i))
                            foreach (int w in lc.lexicon.concepts[i])
                            {
                                wordclass wc = lc.lexicon.getword(w);
                                sb.Append(wc.getform() + "   ");
                                if (!classdict.ContainsKey(wc.root))
                                {
                                    classdict.Add(wc.root, new List<wordclass>());
                                }
                                if (!classdict[wc.root].Contains(wc))
                                    classdict[wc.root].Add(wc);
                                sbcog.Append(wc.root + "   ");
                            }
                    }
                    sw.WriteLine(sb.ToString());
                    swcog.WriteLine(sbcog.ToString());

                }
            }

            string fnclass = fn.Replace("swadesh", "truecognateclass").Replace(".txt",".json");
            using (StreamWriter swcc = new StreamWriter(fnclass))
            {
                swcc.WriteLine("[");
                foreach (int iroot in wordclass.globalrootdict.Keys)
                {
                    if (classdict.ContainsKey(iroot))
                    {
                        StringBuilder sbc = new StringBuilder("{\"id\":" + iroot + 
                            ",\n\"originalform\":\""+wordclass.globalrootdict[iroot].getform()+
                            "\",\n\"originalmeaning\":"+wordclass.globalrootdict[iroot].getconceptjson()+",\n\"words\":[\n");
                        bool first = true;
                        foreach (wordclass wc in classdict[iroot])
                        {
                            if (!first)
                                sbc.Append(",\n");
                            first = false;
                            string family = languageclass.langdict[wc.language].source;
                            sbc.Append("{\"language\":" + wc.language + ",\"family\":\""+family+ "\",\"form\":\"" + wc.getform() + "\",\"concept\":"+wc.getconceptjson());
                            sbc.Append("}");
                        }
                        sbc.Append("]},");
                        swcc.WriteLine(sbc.ToString());
                    }
                }
                swcc.WriteLine("]");
            }

                string fntree = fn.Replace("swadesh", "truetree");
            //memo("Writing to " + fntree);
            using (StreamWriter swtree = new StreamWriter(fntree))
            {
                if (String.IsNullOrEmpty(src))
                {
                    foreach (string s in langtreeclass.treedict.Keys)
                    {
                        swtree.WriteLine(langtreeclass.treedict[s].metadata());
                        swtree.WriteLine(langtreeclass.treedict[s].ToJson());
                    }
                }
                else
                {
                    swtree.WriteLine(langtreeclass.treedict[src].ToJson());
                }
            }

        }

        public static void write_treetranslate(StreamWriter sw,string src)
        {
            var q = from c in languageclass.langdict.Values where c.speakers > 0 select c;
            if (!String.IsNullOrEmpty(src))
                q = from c in q where c.source == src select c;
            sw.WriteLine("\tTranslate");
            int nlang = q.Count();
            int ilang = 0;
            foreach (languageclass lc in q)
            {
                ilang++;
                string s = "\t\t" + lc.id + " " + lc.source + lc.id;
                if (ilang < nlang)
                    s += ",";
                sw.WriteLine(s);
            }
            sw.WriteLine("\t;");
        }

        public static void write_nexustree(string fn,string src)
        {

            string fntree = fn.Replace("swadesh", "truetree_nexus_"+src).Replace(".txt",".tre");
            //memo("Writing to " + fntree);
            using (StreamWriter swtree = new StreamWriter(fntree))
            {
                swtree.WriteLine("#NEXUS");
                swtree.WriteLine();
                swtree.WriteLine("BEGIN TREES;");
                if (String.IsNullOrEmpty(src))
                {
                    write_treetranslate(swtree, "");
                    foreach (string s in langtreeclass.treedict.Keys)
                    {
                        //swtree.WriteLine(langtreeclass.treedict[s].metadata());
                        swtree.WriteLine("tree 'true"+s+"' = "+langtreeclass.treedict[s].ToNexus());
                        swtree.WriteLine(";");
                    }
                }
                else
                {
                    write_treetranslate(swtree, src);
                    swtree.WriteLine("tree '" + src + "' = " + langtreeclass.treedict[src].ToNexus());
                    swtree.WriteLine(";");
                }
                swtree.WriteLine("END;");
            }

        }

        public static void write_CLDF(string fn, string src)
        {
            Dictionary<int, List<wordclass>> classdict = new Dictionary<int, List<wordclass>>();
            string fncldf = fn.Replace("swadesh", "CLDFcognatelist");
            string fncog = fn.Replace("swadesh", "truecognatelist");
            using (StreamWriter sw = new StreamWriter(fncldf))
            using (StreamWriter swcog = new StreamWriter(fncog))
            {
                //StringBuilder sb = new StringBuilder("Lang\tLatitude\tLongitude");
                //StringBuilder sbcog = new StringBuilder("Lang\tFamily");
                //foreach (int i in swadeshclass.conceptcodelist(swadeshclass.swadeshtype))
                //{
                //    sb.Append("\t" + swadeshclass.codeconceptdict[i]);
                //    sbcog.Append("\t" + swadeshclass.codeconceptdict[i]);
                //}
                //sw.WriteLine(sb.ToString());
                //swcog.WriteLine(sbcog.ToString());

                var q = from c in languageclass.langdict.Values where c.speakers > 0 select c;
                if (!String.IsNullOrEmpty(src))
                    q = from c in q where c.source == src select c;
                foreach (languageclass lc in q)
                {
                    //sb = new StringBuilder(lc.id + "\t" + lc.latitude() + "\t" + lc.longitude());
                    //sbcog = new StringBuilder(lc.id + "\t" + lc.source);
                    foreach (int i in swadeshclass.conceptcodelist(swadeshclass.swadeshtype))
                    {
                        if (lc.lexicon.concepts.ContainsKey(i))
                            foreach (int w in lc.lexicon.concepts[i])
                            {
                                wordclass wc = lc.lexicon.getword(w);
                                sw.WriteLine(wc.id + "\t" + swadeshclass.codeconceptdict[i] + "\t" + lc.source+lc.id + "\t" + wc.getform());
                                //sb.Append(wc.getform() + "   ");
                                //if (!classdict.ContainsKey(wc.root))
                                //{
                                //    classdict.Add(wc.root, new List<wordclass>());
                                //}
                                //if (!classdict[wc.root].Contains(wc))
                                //    classdict[wc.root].Add(wc);
                                //sbcog.Append(wc.root + "   ");
                                swcog.WriteLine(wc.id + "\t" + wc.root);
                            }
                    }
                    //sw.WriteLine(sb.ToString());
                    //swcog.WriteLine(sbcog.ToString());

                }
            }

        }

        static public void write_nexusdataline(StreamWriter swnex, languageclass lc, Dictionary<int, Dictionary<int, char>> symboldict, Dictionary<int, char> maxsymbdict,string prefix)
        {
            StringBuilder sb = new StringBuilder("\t" +prefix+lc.source+ lc.id + " ");
            foreach (int i in swadeshclass.conceptcodelist(swadeshclass.swadeshtype))
            {
                if (lc.lexicon.concepts.ContainsKey(i) && lc.lexicon.concepts[i].Count > 0)
                    foreach (int w in lc.lexicon.concepts[i])
                    {
                        wordclass wc = lc.lexicon.getword(w);
                        if (!symboldict[i].ContainsKey(wc.root))
                        {
                            maxsymbdict[i]++;
                            symboldict[i].Add(wc.root, maxsymbdict[i]);
                        }
                        sb.Append(symboldict[i][wc.root]);
                        break;
                    }
                else
                {
                    sb.Append("-");
                }
            }
            swnex.WriteLine(sb.ToString());

        }

        static public void write_nexus(string fn,string src)
        {
            Dictionary<int, List<wordclass>> classdict = new Dictionary<int, List<wordclass>>();
            string fnnex = fn.Replace("swadesh", "truecognate-"+src).Replace(".txt",".nex");
            using (StreamWriter swnex = new StreamWriter(fnnex))
            {
                swnex.WriteLine("#NEXUS");
                swnex.WriteLine("BEGIN TAXA;");
                var q = from c in languageclass.langdict.Values where c.speakers > 0 select c;
                if (!String.IsNullOrEmpty(src))
                    q = from c in q where c.source == src select c;
                swnex.WriteLine("\tDimensions NTax=" + (q.Count()+1) + ";");
                StringBuilder sbtax = new StringBuilder("\tTaxLabels");

                foreach (languageclass lc in q)
                    sbtax.Append(" " + lc.source+lc.id);
                sbtax.Append(" proto" + src + languageclass.sourcedict[src].id);
                sbtax.Append(";");
                swnex.WriteLine(sbtax.ToString());
                swnex.WriteLine("END;");
                swnex.WriteLine();

                swnex.WriteLine("BEGIN CHARACTERS;");
                swnex.WriteLine("\tDimensions NChar=" + swadeshclass.conceptcodelist(swadeshclass.swadeshtype).Count + ";");
                swnex.WriteLine("\tformat datatype=standard symbols=\"A~Z\" gap =-;");
                StringBuilder sbchar = new StringBuilder("\tcharlabels ");
                Dictionary<int, Dictionary<int, char>> symboldict = new Dictionary<int, Dictionary<int, char>>();
                Dictionary<int, char> maxsymbdict = new Dictionary<int, char>();// char maxsymb = '@'; // A-1
                foreach (int i in swadeshclass.conceptcodelist(swadeshclass.swadeshtype))
                {
                    sbchar.Append(" '" + swadeshclass.codeconceptdict[i]+"'");
                    maxsymbdict.Add(i, '@');
                    symboldict.Add(i, new Dictionary<int, char>());
                }
                sbchar.Append(";");
                swnex.WriteLine(sbchar.ToString());

                swnex.WriteLine("\tMatrix");
                foreach (languageclass lc in q)
                {
                    write_nexusdataline(swnex, lc,symboldict,maxsymbdict,"");

                }
                write_nexusdataline(swnex, languageclass.sourcedict[src], symboldict, maxsymbdict,"proto");
                swnex.WriteLine(";");
                swnex.WriteLine("END;");
                swnex.WriteLine();
                swnex.WriteLine("BEGIN PAUP;");
                swnex.WriteLine("\toutgroup proto"+src+ languageclass.sourcedict[src].id+";");
                swnex.WriteLine("\tset criterion=parsimony;");
                swnex.WriteLine("\thsearch nreps=25;");
                swnex.WriteLine("\tdescribe 1 / plot=phylogram;");
                swnex.WriteLine("\tsavetrees file="+fnnex.Replace(".nex",".tre")+" brlens;");
                swnex.WriteLine("\tcontree /treeFile=" + fnnex.Replace(".nex", "-consensus.nex;"));
                //swnex.WriteLine("QUIT;");
                swnex.WriteLine("END;");

            }


        }

        public static List<string> numbersonly = new List<string>()
        {
            {"ONE"},
            {"TWO"},
            {"THREE"},
            {"FOUR"},
            {"FIVE"},
            {"SIX"},
            {"SEVEN"},
            {"EIGHT"},
            {"NINE"},
            {"TEN"}
        };


        public static List<string> swadesh100 = new List<string>()
        {
            //https://concepticon.clld.org/contributions/Swadesh-1955-100
            {"ALL"},
            {"ASH"},
            {"BARK"},
            {"BELLY"},
            {"BIG"},
            {"BIRD"},
            {"BITE"},
            {"BLACK"},
            {"BLOOD"},
            {"BONE"},
            {"BREAST"},
            {"BURN"},
            {"CLAW"},
            {"CLOUD"},
            {"COLD"},
            {"COME"},
            {"DIE"},
            {"DOG"},
            {"DRINK"},
            {"DRY"},
            {"EAR"},
            {"EARTH (SOIL)"},
            {"EAT"},
            {"EGG"},
            {"EYE"},
            {"FAT (ORGANIC SUBSTANCE)"},
            {"FEATHER"},
            {"FIRE"},
            {"FISH"},
            {"FLY (MOVE THROUGH AIR)"},
            {"FOOT"},
            {"FULL"},
            {"GIVE"},
            {"GOOD"},
            {"GREEN"},
            {"HAIR"},
            {"HAND"},
            {"HEAD"},
            {"HEAR"},
            {"HEART"},
            {"HORN (ANATOMY)"},
            {"I"},
            {"KILL"},
            {"KNEE"},
            {"KNOW (SOMETHING)"},
            {"LEAF"},
            {"LIE (REST)"},
            {"LIVER"},
            {"LONG"},
            {"LOUSE"},
            {"MAN"},
            {"MANY"},
            {"FLESH OR MEAT"},
            {"MOON"},
            {"MOUNTAIN"},
            {"MOUTH"},
            {"NAME"},
            {"NECK"},
            {"NEW"},
            {"NIGHT"},
            {"NOSE"},
            {"NOT"},
            {"ONE"},
            {"PERSON"},
            {"RAINING OR RAIN"},
            {"RED"},
            {"ROAD"},
            {"ROOT"},
            {"ROUND"},
            {"SAND"},
            {"SAY"},
            {"SEE"},
            {"SEED"},
            {"SIT"},
            {"SKIN"},
            {"SLEEP"},
            {"SMALL"},
            {"SMOKE (EXHAUST)"},
            {"STAND"},
            {"STAR"},
            {"STONE"},
            {"SUN"},
            {"SWIM"},
            {"TAIL"},
            {"THAT"},
            {"THIS"},
            {"THOU"},
            {"TONGUE"},
            {"TOOTH"},
            {"TREE"},
            {"TWO"},
            {"WALK"},
            {"HOT OR WARM"},
            {"WATER"},
            {"WE"},
            {"WHAT"},
            {"WHITE"},
            {"WHO"},
            {"WOMAN"},
            {"YELLOW"}

        };

        public static List<string> swadesh200 = new List<string>()
        {
            //https://concepticon.clld.org/contributions/Swadesh-1952-200
            {"ALL"},
            {"AND"},
            {"ANIMAL"},
            {"ASH"},
            {"AT"},
            {"BACK"},
            {"BAD"},
            {"BARK"},
            {"BECAUSE"},
            {"BELLY"},
            {"BERRY"},
            {"BIG"},
            {"BIRD"},
            {"BITE"},
            {"BLACK"},
            {"BLOOD"},
            {"BLOW (OF WIND)"},
            {"BONE"},
            {"BREATHE"},
            {"BURNING"},
            {"CHILD (YOUNG HUMAN)"},
            {"CLOUD"},
            {"COLD (OF WEATHER)"},
            {"COME"},
            {"COUNT"},
            {"CUT"},
            {"DAY (NOT NIGHT)"},
            {"DIE"},
            {"DIG"},
            {"DIRTY"},
            {"DOG"},
            {"DRINK"},
            {"DRY"},
            {"BLUNT"},
            {"DUST"},
            {"EAR"},
            {"EARTH (SOIL)"},
            {"EAT"},
            {"EGG"},
            {"EYE"},
            {"FALL"},
            {"FAR"},
            {"FAT (ORGANIC SUBSTANCE)"},
            {"FATHER"},
            {"FEAR (BE AFRAID)"},
            {"FEATHER"},
            {"FEW"},
            {"FIGHT"},
            {"FIRE"},
            {"FISH"},
            {"FIVE"},
            {"FLOAT"},
            {"FLOW"},
            {"FLOWER"},
            {"FLY (MOVE THROUGH AIR)"},
            {"FOG"},
            {"FOOT"},
            {"FOUR"},
            {"FREEZE"},
            {"GIVE"},
            {"GOOD"},
            {"GRASS"},
            {"GREEN"},
            {"GUTS"},
            {"HAIR"},
            {"HAND"},
            {"HE"},
            {"HEAD"},
            {"HEAR"},
            {"HEART"},
            {"HEAVY"},
            {"HERE"},
            {"HIT"},
            {"HOLD"},
            {"HOW"},
            {"HUNT"},
            {"HUSBAND"},
            {"I"},
            {"ICE"},
            {"IF"},
            {"IN"},
            {"KILL"},
            {"KNOW (SOMETHING)"},
            {"LAKE"},
            {"LAUGH"},
            {"LEAF"},
            {"LEFT"},
            {"LEG"},
            {"LIE (REST)"},
            {"BE ALIVE"},
            {"LIVER"},
            {"LONG"},
            {"LOUSE"},
            {"MALE PERSON"},
            {"MANY"},
            {"FLESH OR MEAT"},
            {"MOTHER"},
            {"MOUNTAIN"},
            {"MOUTH"},
            {"NAME"},
            {"NARROW"},
            {"NEAR"},
            {"NECK"},
            {"NEW"},
            {"NIGHT"},
            {"NOSE"},
            {"NOT"},
            {"OLD"},
            {"ONE"},
            {"OTHER"},
            {"PERSON"},
            {"PLAY"},
            {"PULL"},
            {"PUSH"},
            {"RAIN (RAINING)"},
            {"RED"},
            {"CORRECT (RIGHT)"},
            {"RIGHT"},
            {"RIVER"},
            {"ROAD"},
            {"ROOT"},
            {"ROPE"},
            {"ROTTEN"},
            {"RUB"},
            {"SALT"},
            {"SAND"},
            {"SAY"},
            {"SCRATCH"},
            {"SEA"},
            {"SEE"},
            {"SEED"},
            {"SEW"},
            {"SHARP"},
            {"SHORT"},
            {"SING"},
            {"SIT"},
            {"SKIN (HUMAN)"},
            {"SKY"},
            {"SLEEP"},
            {"SMALL"},
            {"SMELL (PERCEIVE)"},
            {"SMOKE (EXHAUST)"},
            {"SMOOTH"},
            {"SNAKE"},
            {"SNOW"},
            {"SOME"},
            {"SPIT"},
            {"SPLIT"},
            {"SQUEEZE"},
            {"STAB"},
            {"STAND"},
            {"STAR"},
            {"STICK"},
            {"STONE"},
            {"STRAIGHT"},
            {"SUCK"},
            {"SUN"},
            {"SWELL"},
            {"SWIM"},
            {"TAIL"},
            {"THAT"},
            {"THERE"},
            {"THEY"},
            {"THICK"},
            {"THIN"},
            {"THINK"},
            {"THIS"},
            {"THOU"},
            {"THREE"},
            {"THROW"},
            {"TIE"},
            {"TONGUE"},
            {"TOOTH"},
            {"TREE"},
            {"TURN AROUND"},
            {"TWO"},
            {"VOMIT"},
            {"WALK"},
            {"WARM (OF WEATHER)"},
            {"WASH"},
            {"WATER"},
            {"WE"},
            {"WET"},
            {"WHAT"},
            {"WHEN"},
            {"WHERE"},
            {"WHITE"},
            {"WHO"},
            {"WIDE"},
            {"WIFE"},
            {"WIND"},
            {"WING"},
            {"WIPE"},
            {"WITH"},
            {"WOMAN"},
            {"FOREST"},
            {"WORM"},
            {"YOU"},
            {"YEAR"},
            {"YELLOW"}

        };
    }
}
