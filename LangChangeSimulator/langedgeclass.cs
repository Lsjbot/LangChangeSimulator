using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangChangeSimulator
{
    class langedgeclass
    {
        public static int maxid = 0;
        public int id;
        public int lang;
        public langnodeclass nodeabove = null;
        public langnodeclass nodebelow = null;
        public List<int> langbelow = new List<int>();


        public langedgeclass(languageclass lc, langnodeclass mama)
        {
            maxid++;
            id = maxid;
            lang = lc.id;
            langbelow.Add(lc.id);
            nodeabove = mama;
        }

        public bool alive()
        {
            //if (!languageclass.langdict.ContainsKey(lang))
            //    return false;
            if (languageclass.langdict.ContainsKey(lang) && languageclass.langdict[lang].speakers > 0)
                return true;
            if (nodebelow == null)
                return false;
            if (nodebelow.edgebelow[0].alive() || nodebelow.edgebelow[1].alive())
                return true;

            return false;
        }

        public int timespan()
        {
            if (nodeabove == null)
                return 100;
            if (nodebelow == null)
                return 100;
            return nodebelow.year - nodeabove.year;
        }
    }
}
