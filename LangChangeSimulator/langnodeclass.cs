using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangChangeSimulator
{
    class langnodeclass
    {
        public static int maxid = 0;

        public int id;
        public int year;
        public langedgeclass edgeabove = null;
        public langedgeclass[] edgebelow = new langedgeclass[2];

        public langnodeclass(int time)
        {
            maxid++;
            id = maxid;
            year = time;
        }
    }
}
