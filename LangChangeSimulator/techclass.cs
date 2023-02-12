using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LangChangeSimulator
{
    class techclass
    {
        //zero tech means paleolithic tech:
        // stone tools, bone tools, basic clothing
        // hunting, gathering, fishing from shore

        //tech list:
        //arctic
        //coldgear
        //boats
        //oceangoing

        public static string knowntech = "";

        public static Dictionary<string, techclass> techdict = new Dictionary<string, techclass>();

        public static void init_tech(string folder)
        {
            using (StreamReader sr = new StreamReader(folder + "tech.txt"))
            {
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split('\t');
                    techclass tc = new techclass();
                    tc.name = words[0];
                    tc.difficulty = util.tryconvert(words[1]);
                    if (!String.IsNullOrEmpty(words[2]))
                        foreach (string s in words[2].Split(','))
                            tc.prerequisites.Add(s.Trim());
                    tc.unlocks_subsistence = words[3];
                    tc.concept = words[4];
                    tc.conceptcode = swadeshclass.conceptcodedict[tc.concept];
                    techdict.Add(tc.name, tc);
                }
            }
        }

        public string name = "";
        public int difficulty = 10;
        public string unlocks_subsistence = "";
        public List<string> prerequisites = new List<string>();
        public string concept = "";
        public int conceptcode = -1;
        string category = "";
        bool found = false;
    }
}
