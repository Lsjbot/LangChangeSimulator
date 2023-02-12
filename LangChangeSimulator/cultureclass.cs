using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LangChangeSimulator
{
    [Serializable()]
    public class cultureclass
    {


        public List<string> tech = new List<string>();
        public List<string> available_subsistence = new List<string>() { parameterclass.p.get("defaultsubsistence") };
    
        public subsistenceclass subsistence = new subsistenceclass();
        public double openness = 1; //likelihood for change, relative to base likelihood

        public cultureclass()
        {
            Random rnd = new Random();
            openness = 0.5 + rnd.NextDouble();
        }
        public bool knows(string techstring)
        {
            return this.tech.Contains(techstring);
        }

        public wordclass addtech(string t, languageclass tolang, languageclass fromlang)
        {
            tech.Add(t);
            if (!techclass.knowntech.Contains(t))
                techclass.knowntech += " " + t;
            //MessageBox.Show("Inventing " + t);
            if (!String.IsNullOrEmpty(techclass.techdict[t].unlocks_subsistence))
            {
                available_subsistence.Add(techclass.techdict[t].unlocks_subsistence);
            }
            wordclass wc = null;
            int concept = techclass.techdict[t].conceptcode;
            if (fromlang != null)
            {
                //borrow word for tech
                if (fromlang.lexicon.concepts.ContainsKey(concept))
                {
                    if (fromlang.lexicon.concepts[concept].Count > 0)
                    {
                        wc = new wordclass(fromlang.lexicon.getword(fromlang.lexicon.concepts[concept].First()), tolang.id);
                    }
                }
            }

            if (wc == null) //make a new word from scratch
            {
                wc = new wordclass(tolang.inventory, concept, "CVCV", tolang.id);
            }

            return wc;
        }

        public void mutate(Random rnd, double stepinventionrate, languageclass lc)
        {
            openness += 0.01 * rnd.NextDouble();
            if (openness > 0.25)
                openness -= 0.01 * rnd.NextDouble();

            double irate = openness * stepinventionrate;

            if (rnd.NextDouble() < irate)
            {
                foreach (string t in techclass.techdict.Keys)
                {
                    if (this.knows(t))
                        continue;
                    bool possible = true;
                    foreach (string tp in techclass.techdict[t].prerequisites)
                        if (!this.knows(tp))
                            possible = false;
                    if (possible)
                    {
                        if (1/rnd.NextDouble() > techclass.techdict[t].difficulty)
                        {// Invent!
                            wordclass wc = this.addtech(t,lc,null);
                            lc.lexicon.addword(wc);
                        }
                    }
                }
            }
        }

        //boats - coastal navigation
        //oceangoing - crossing ocean squares
    }
}
