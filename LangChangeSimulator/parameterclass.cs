using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace LangChangeSimulator
{
    class parameterclass
    {
        //class to gather all adjustable parameters in the simulation

        public static parameterclass p = new parameterclass(); //actual params

        public static parameterclass defaultparams = new parameterclass(); //default params

        public Dictionary<string, string> pdict = new Dictionary<string, string>();

        public static void InitDefaults()
        {
            defaultparams.pdict.Add("defaultsubsistence", "hunter-gatherer");
            defaultparams.pdict.Add("nlanguage", "1");
            defaultparams.pdict.Add("languageorigin", "checked");//checked, random, scratch
            defaultparams.pdict.Add("languageset", "swe|eng");
            defaultparams.pdict.Add("conceptset", "swadesh100");//swadesh100, swadesh200, clics3
            defaultparams.pdict.Add("basecarryingcapacity", "0.3"); //per km2
            //defaultparams.pdict.Add("relcarryingcapacity-tundra", "0");
            defaultparams.pdict.Add("startingpop", "500"); 
            defaultparams.pdict.Add("timestep", "40"); //years per simulation step
            defaultparams.pdict.Add("maxtime", "");
            defaultparams.pdict.Add("basepopgrowth", "0.02"); //population growth per year if unchecked
            defaultparams.pdict.Add("basetravelcost", "20"); //per km
            defaultparams.pdict.Add("basetravelbudget", "400");
            defaultparams.pdict.Add("minimumviable", "150"); //smallest viable language community
            defaultparams.pdict.Add("soundchangeprob", "0.02"); //per year per inventory
            defaultparams.pdict.Add("unconditionalfraction", "0.4"); //fraction of sound changes that are unconditional
            defaultparams.pdict.Add("baseinventionrate", "0.000002"); //per year
            defaultparams.pdict.Add("wordborrowingrate", "0.0001"); //per lexicon per contact per year
            defaultparams.pdict.Add("techborrowingrate", "0.002"); //per contact per year
            defaultparams.pdict.Add("wefactorbase", "2"); //enhanced borrowing between related peoples
            defaultparams.pdict.Add("polysemyloss", "0.001"); //per year per polysemy
            defaultparams.pdict.Add("synonymyloss", "0.001"); //per year per synonymy
            defaultparams.pdict.Add("colexrate", "0.0005"); //per year per concept
            defaultparams.pdict.Add("soundchangeenabled", "true");
            defaultparams.pdict.Add("unconditionalenabled", "true");
            defaultparams.pdict.Add("conditionalenabled", "true");
            defaultparams.pdict.Add("soundborrowingenabled", "true");
            defaultparams.pdict.Add("contactenabled", "true");
            defaultparams.pdict.Add("semanticshiftenabled", "true");
            defaultparams.pdict.Add("switchrate", "100"); //rate of minority switch to majority language
            defaultparams.pdict.Add("switchborrow", "0.05");
            defaultparams.pdict.Add("metathesisrate", "0.01"); //per year per lexicon
            defaultparams.pdict.Add("soundlossfrac", "0.05"); //fraction of sound changes that are sound loss
            defaultparams.pdict.Add("soundgainfrac", "0.05"); //fraction of sound changes into double sounds
            defaultparams.pdict.Add("arealradius", "1"); //radius around focus square for areal effects
            defaultparams.pdict.Add("arealcenterbonus", "3"); //extra weight for focus square in areal effects
            defaultparams.pdict.Add("arealminlang", "10"); //min #languages with arealradius to consider areal effects
            defaultparams.pdict.Add("arealwordborrowingrate", "0.0001"); //per lexicon per year
            defaultparams.pdict.Add("arealsoundborrowingrate", "0.0001"); //per inventory per year
            //defaultparams.pdict.Add("", "");
            //defaultparams.pdict.Add("", "");
            //defaultparams.pdict.Add("", "");

            //defaultparams.pdict.Add("", "");
            //defaultparams.pdict.Add("", "");

            foreach (string s in defaultparams.pdict.Keys)
                p.pdict.Add(s, defaultparams.pdict[s]);
        }

        public string get(string param)
        {
            if (pdict.ContainsKey(param.ToLower()))
                return pdict[param.ToLower()];
            else if (defaultparams.pdict.ContainsKey(param.ToLower()))
                return defaultparams.pdict[param.ToLower()];
            else
            {
                Console.WriteLine("Unknown parameter " + param);
                return "";
            }
        }

        public void save(string fn)
        {
            if (this.pdict.Count == 0)
                defaultparams.save(fn);
            else
            {
                using (StreamWriter sw = new StreamWriter(fn))
                {
                    foreach (string s in this.pdict.Keys)
                    {
                        sw.WriteLine(s + "\t" + pdict[s]);
                    }
                }
            }
        }

        public void load()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Form1.folder;
                openFileDialog.Filter = "All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string fn = openFileDialog.FileName;
                    load(fn);
                }
            }
        }

        public void load(string fn)
        {
            if (File.Exists(fn))
            {
                using (StreamReader sr = new StreamReader(fn))
                {
                    //this.pdict.Clear();
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] words = line.Split('\t');
                        if (words.Length >= 2)
                        {
                            this.put(words[0], words[1]);
                        }
                    }
                }
            }
        }

        public T get<T>(string param)
        {
            if (typeof(T) == typeof(string))
                return (T)(object)get(param);
            else if(typeof(T) == typeof(int))
            {
                return (T)(object)util.tryconvert(get(param));
            }
            else if (typeof(T) == typeof(double))
            {
                return (T)(object)util.tryconvertdouble(get(param));
            }
            else if (typeof(T) == typeof(bool))
            {
                return (T)(object)(get(param) == "true");
            }
            else
                return default(T);
        }

        public void put<T>(string param, T value)
        {
            this.put(param, value.ToString());
        }

        public void put(string param, string value)
        {
            if (this.pdict.ContainsKey(param))
                pdict[param] = value;
            else
                pdict.Add(param, value);
        }


    }
}
