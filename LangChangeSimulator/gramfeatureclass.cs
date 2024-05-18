using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangChangeSimulator
{
    class gramfeatureclass
    {
        public static Dictionary<string, gramfeatureclass> gramfeatures = new Dictionary<string, gramfeatureclass>();
        public string id;
        public string name;
        public List<int> values;

        public static void fill_gramfeatures()
        {
            if (gramfeatures.Count > 0)
                return;

            foreach (Grambank_parameter gbp in Form1.dblang.Grambank_parameter)
            {
                var gf = new gramfeatureclass();
                gf.id = gbp.Id;
                gf.name = gbp.Name;
                gf.values = new List<int>();
                gramfeatures.Add(gbp.Id, gf);
            }

            foreach (Grambank_code gc in Form1.dblang.Grambank_code)
            {
                gramfeatures[gc.Parameter].values.Add((int)gc.Value);
            }
        }
    }
}
