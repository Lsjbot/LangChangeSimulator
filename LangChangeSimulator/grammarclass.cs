using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LangChangeSimulator
{
    public class grammarclass
    {
        Dictionary<string, int?> features = new Dictionary<string, int?>();

        public grammarclass()
        {
            Random rnd = new Random();
            foreach (gramfeatureclass gf in gramfeatureclass.gramfeatures.Values)
            {
                int i = rnd.Next(gf.values.Count);
                features.Add(gf.id, gf.values[i]);
            }
        }

        public grammarclass clone()
        {
            grammarclass gc = new grammarclass();
            gc.features.Clear();
            foreach (string gf in this.features.Keys)
                gc.features.Add(gf, this.features[gf]);
            return gc;
        }

        public void mutate(Random rnd, double stepinventionrate, languageclass lc)
        {
            double irate = stepinventionrate;

            if (rnd.NextDouble() < irate)
            {
                int jf = rnd.Next(features.Count);
                string sf = features.Keys.ToList()[jf];
                int jv = rnd.Next(gramfeatureclass.gramfeatures[sf].values.Count);
                features[sf] = gramfeatureclass.gramfeatures[sf].values[jv];
            }
        }

        public static void write_grammartable(string fn, string src)
        {
            using (StreamWriter sw = new StreamWriter(fn))
            {
                StringBuilder sb = new StringBuilder("Lang\tLatitude\tLongitude");
                foreach (string gf in gramfeatureclass.gramfeatures.Keys)
                {
                    sb.Append("\t" + gramfeatureclass.gramfeatures[gf].name);
                }
                sw.WriteLine(sb.ToString());

                var q = from c in languageclass.langdict.Values where c.speakers > 0 select c;
                if (!String.IsNullOrEmpty(src))
                    q = from c in q where c.source == src select c;
                foreach (languageclass lc in q)
                {
                    sb = new StringBuilder(lc.id + "\t" + lc.latitude() + "\t" + lc.longitude());
                    foreach (string gf in gramfeatureclass.gramfeatures.Keys)
                    {
                        sb.Append("\t");
                        if (lc.grammar.features.ContainsKey(gf) && lc.grammar.features[gf] != null)
                            sb.Append(lc.grammar.features[gf].ToString());
                    }
                    sw.WriteLine(sb.ToString());
                }
            }

        }

    }
}
