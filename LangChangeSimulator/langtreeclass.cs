using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangChangeSimulator
{
    class langtreeclass
    {
        public static Dictionary<string, langtreeclass> treedict = new Dictionary<string, langtreeclass>();

        public int rootlang;
        string src = "";
        double lat = 0.0;
        double lon = 0.0;
        langedgeclass stem;

        public langtreeclass(languageclass lc)
        {
            rootlang = lc.id;
            src = lc.source;
            lat = mapgridclass.basemap[lc.ilat, lc.ilon].lat;
            lon = mapgridclass.basemap[lc.ilat, lc.ilon].lon;
            stem = new langedgeclass(lc, null);

        }

        public string metadata()
        {
            return "{\"source\":\"" + this.src + "\",\"latitude\":" + this.lat + ",\"longitude\":" + this.lon + "}";
        }

        public langedgeclass getleaf(languageclass lc)
        {
            langedgeclass le = this.stem;
            while (le.nodebelow != null)
            {
                if (le.nodebelow.edgebelow[0].langbelow.Contains(lc.id))
                    le = le.nodebelow.edgebelow[0];
                else
                    le = le.nodebelow.edgebelow[1];
            }
            return le;
        }

        public void addnode(languageclass oldlang, languageclass newlang, int time)
        {
            langedgeclass leaf = getleaf(oldlang);
            langnodeclass nc = new langnodeclass(time);
            leaf.nodebelow = nc;
            nc.edgeabove = leaf;
            nc.edgebelow[0] = new langedgeclass(oldlang, nc);
            nc.edgebelow[1] = new langedgeclass(newlang, nc);

            if (leaf.nodeabove != null)
            {
                do
                {
                    leaf.langbelow.Add(newlang.id);
                    leaf = leaf.nodeabove.edgeabove;
                }
                while (leaf.nodeabove != null);
            }
            stem.langbelow.Add(newlang.id);
        }

        public int findsplit(int l1, int l2)
        {
            if (!stem.langbelow.Contains(l1))
                return -1;
            if (!stem.langbelow.Contains(l2))
                return -1;

            langedgeclass le = stem;
            while (le.nodebelow != null)
            {
                if (le.nodebelow.edgebelow[0].langbelow.Contains(l1))
                {
                    if (le.nodebelow.edgebelow[0].langbelow.Contains(l2))
                        le = le.nodebelow.edgebelow[0];
                    else
                        return le.nodebelow.year;
                }
                else
                {
                    if (le.nodebelow.edgebelow[1].langbelow.Contains(l2))
                        le = le.nodebelow.edgebelow[1];
                    else
                        return le.nodebelow.year;
                }
            }
            return -1;
        }

        public override string ToString()
        {
            return printbranch(stem, "");
        }

        public string printbranch(langedgeclass le, string prefix)
        {
            if (le.nodebelow == null)
                return prefix + le.lang + languageclass.getstatus(le.lang)+" |\n";
            string s = prefix + le.lang + "-"+le.nodebelow.year+"\n";
            s += prefix + printbranch(le.nodebelow.edgebelow[0], prefix + "|   ");
            s += prefix + printbranch(le.nodebelow.edgebelow[1], prefix + "    ");
            return s;
        }

        public string ToJson()
        {
            return jsonbranch(stem,'{','}',"",0);
        }
        public string ToNexus()
        {
            return jsonbranch(stem, '(', ')',this.src,0);
        }
        public string jsonbranch(langedgeclass le,char left,char right,string prefix,int extratime)
        {
            if (le.nodebelow == null)
                return prefix+le.lang.ToString()+":"+(le.timespan()+extratime);
            else if (le.nodebelow.edgebelow[0].alive() && le.nodebelow.edgebelow[1].alive())
            {
                string s = left + jsonbranch(le.nodebelow.edgebelow[0],left,right,prefix,0) + "," + jsonbranch(le.nodebelow.edgebelow[1],left,right,prefix,0) + right+":"+(le.timespan()+extratime);
                return s;
            }
            else if (le.nodebelow.edgebelow[0].alive())
            {
                return jsonbranch(le.nodebelow.edgebelow[0], left, right, prefix,le.timespan()+extratime);
            }
            else if (le.nodebelow.edgebelow[1].alive())
            {
                return jsonbranch(le.nodebelow.edgebelow[1], left, right, prefix,le.timespan()+extratime);
            }
            else
            {
                return prefix + le.lang.ToString() + ":" + (le.timespan() + extratime);
            }

        }
    }
}
