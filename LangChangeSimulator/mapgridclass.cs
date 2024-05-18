using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace LangChangeSimulator
{
    class mapgridclass
    {
        public double lat = 0;
        public double lon = 0;
        public int ilat = 0;
        public int ilon = 0;
        public terrainclass terrain = new terrainclass();
        public nasaclass climate = new nasaclass();
        public string climatestring = "unknown";

        public static int imax = 0;
        public static int jmax = 0;
        public static mapgridclass[,] basemap;
        public static cellclass[,] map;
        public static string mapregion = "World";
        public static string startregion = "World";
        public static int km = -1;
        public static int basetravelcost = parameterclass.p.get<int>("basetravelcost");

        public static int infinitecost = 999999;
        public static Dictionary<string, double> travelcostdict = new Dictionary<string, double>()
        {
            {"flat",1},
            {"high-mountains",4},
            {"hilly",1.5},
            {"hilly high",2},
            {"low-mountains",2.5},
            {"medium-mountains",3},
            {"somewhat hilly",1.2},
            {"somewhat hilly high",1.5},
            {"very flat",1},
            {"flat high",1},
            {"very flat high",1}
        };
        public static Dictionary<string, double> ridingfactordict = new Dictionary<string, double>()
        {//used for both riding and wheels
            {"flat",0.3},
            {"high-mountains",1},
            {"hilly",0.6},
            {"hilly high",0.7},
            {"low-mountains",0.8},
            {"medium-mountains",0.9},
            {"somewhat hilly",0.4},
            {"somewhat hilly high",0.5},
            {"very flat",0.3},
            {"flat high",0.3},
            {"very flat high",0.3}
        };

        public static Dictionary<string, Rectangle> regiondict = new Dictionary<string, Rectangle>()
        {{"World",new Rectangle(-180,-70,360,140)},
        {"Australia",new Rectangle(110,-45,50,45)},
        {"South America",new Rectangle(-83,-57,51,71)},
        {"Africa",new Rectangle(-18,-35,71,73)},
        {"Africa Subsaharan",new Rectangle(-18,-35,71,40)},
        {"New Guinea",new Rectangle(125,-10,25,10)}
        };

        public string river = ""; //name of major river, if any

        public string makejson()
        {
            string s = JsonConvert.SerializeObject(this);
            return s;
        }

        public static void read_basemap(string region, string fn, string startregionpar)
        {
            mapregion = region;
            startregion = startregionpar;
            int imin = 9999;
            imax = -1;
            int jmin = 9999;
            jmax = -1;

            string rex = @"_(\d+)x";
            foreach (Match m in Regex.Matches(fn,rex))
            {
                km = util.tryconvert(m.Groups[1].Value);
                basetravelcost *= km;
            }
            List<mapgridclass> mglist = new List<mapgridclass>();
            using (StreamReader sr = new StreamReader(fn))
            {
                while (!sr.EndOfStream)
                {
                    string js = sr.ReadLine();
                    mapgridclass mg = JsonConvert.DeserializeObject<mapgridclass>(js);
                    if (!mg.inregion(region))
                        continue;
                    if (mg.ilat < imin)
                        imin = mg.ilat;
                    if (mg.ilat > imax)
                        imax = mg.ilat;
                    if (mg.ilon < jmin)
                        jmin = mg.ilon;
                    if (mg.ilon > jmax)
                        jmax = mg.ilon;
                    mglist.Add(mg);
                }
            }
            imax++;
            jmax++;
            basemap = new mapgridclass[imax, jmax];
            map = new cellclass[imax, jmax];
            for (int i = 0; i < imax; i++)
                for (int j = 0; j < jmax; j++)
                {
                    basemap[i, j] = null;
                }
            foreach (mapgridclass mg in mglist)
            {
                mg.terrain.roughness = mg.terrain.roughness * (mg.terrain.variance + 20); //undoing dubious normalization
                basemap[mg.ilat, mg.ilon] = mg;
            }

        }

        private bool inregion(string region)
        {
            return inregion(this, region);
        }

        public static bool inregion(mapgridclass mg, string region)
        {
            if (region == "World")
                return true;

            if (!regiondict.ContainsKey(region))
                return true;

            if (mg.lat > regiondict[region].Bottom)
                return false;
            if (mg.lat < regiondict[region].Top)
                return false;
            if (mg.lon < regiondict[region].Left)
                return false;
            if (mg.lon > regiondict[region].Right)
                return false;
            return true;

        }

        public static void read_basemap(string region, string startregion)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Form1.folder;
                openFileDialog.Filter = "JsonL files (*.jsonl)|*.jsonl|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string fn = openFileDialog.FileName;
                    read_basemap(region,fn, startregion);
                }
            }
        }

        public static void make_mainmap()
        {
            map = new cellclass[imax, jmax];
            for (int i = 0; i < imax; i++)
                for (int j = 0; j < jmax; j++)
                {
                    if (basemap[i, j] == null)
                        map[i, j] = null;
                    else
                        map[i, j] = new cellclass(basemap[i, j]);
                }
            int dummy = 0;
        }

        public static void fill_missing_climate()
        {
            //int imax = mapgridclass.basemap.GetLength(0);
            //int jmax = mapgridclass.basemap.GetLength(1);
            for (int i = 0; i < imax; i++)
                for (int j = 0; j < jmax; j++)
                {
                    if (basemap[i, j] != null)
                    {
                        int u = 0;
                        while (String.IsNullOrEmpty(basemap[i, j].climatestring) && u < imax)
                        {
                            u++;
                            if (i - u >= 0)
                                if (basemap[i - u, j] != null)
                                    if (!String.IsNullOrEmpty(basemap[i - u, j].climatestring))
                                        basemap[i, j].climatestring = basemap[i - u, j].climatestring;
                            if (i + u < imax)
                                if (basemap[i + u, j] != null)
                                    if (!String.IsNullOrEmpty(basemap[i + u, j].climatestring))
                                        basemap[i, j].climatestring = basemap[i + u, j].climatestring;
                        }
                        u = 0;
                        while (basemap[i, j].climate.landcover < 0 && u < imax)
                        {
                            u++;
                            if (i - u >= 0)
                                if (basemap[i - u, j] != null)
                                    if (basemap[i - u, j].climate.landcover > 0)
                                        basemap[i, j].climate.landcover = basemap[i - u, j].climate.landcover;
                            if (i + u < imax)
                                if (basemap[i + u, j] != null)
                                    if (basemap[i + u, j].climate.landcover > 0)
                                        basemap[i, j].climate.landcover = basemap[i + u, j].climate.landcover;
                        }
                    }
                }
        }

        public static bool inmap(int ilat, int ilon)
        {
            if (ilat < 0)
                return false;
            if (ilon < 0)
                return false;
            if (ilat >= imax)
                return false;
            if (ilon >= jmax)
                return false;
            return true;
        }

        public int travelcost(int destlat, int destlon, cultureclass culture)
        {
            if (!inmap(destlat, destlon))
                return infinitecost;

            if ((basemap[destlat, destlon] == null) && (!culture.knows("oceangoing")))
                return infinitecost;

            int tc = this.movecost(culture)/2;
            int bc = parameterclass.p.get<int>("basetravelcost");
            int u = 0;
            if (destlat < this.ilat)
                u = -1;
            else if (destlat > this.ilat)
                u = 1;
            int v = 0;
            if (destlon < this.ilon)
                v = -1;
            else if (destlon > this.ilon)
                v = 1;

            if (this.iscoast(u,v))
            {
                if (!culture.knows("boats"))
                    return infinitecost;
            }

            int clat = this.ilat;
            int clon = this.ilon;
            bool atriver = !String.IsNullOrEmpty(this.river);

            while (clat != destlat || clon != destlon)
            {
                if (clat != destlat)
                    clat += u;
                if (clon != destlon)
                    clon += v;

                if (basemap[clat, clon] == null)
                {
                    if (culture.knows("oceangoing"))
                        tc += bc / 5;
                    else
                        return infinitecost;
                }
                else
                {
                    double coastfactor = 1;
                    if (basemap[clat,clon].iscoast(u, v))
                    {
                        if (culture.knows("oceangoing"))
                            coastfactor = 0.15;
                        if (culture.knows("boats"))
                            coastfactor = 0.3;
                    }
                    double riverfactor = 1;
                    if (String.IsNullOrEmpty(basemap[clat, clon].river))
                    {
                        if (atriver)
                        {
                            if (culture.knows("boats"))
                                riverfactor = 1.3; //crossing river
                            else
                                riverfactor = 1.8; //crossing river without boats
                        }
                    }
                    else
                    {
                        if (atriver)
                        {
                            if (culture.knows("boats"))
                                riverfactor = 0.4; //along river
                            else
                                riverfactor = 0.8; //along river without boats
                        }
                    }
                    tc += (int)(basemap[clat, clon].movecost(culture) * coastfactor * riverfactor);

                    atriver = !String.IsNullOrEmpty(basemap[clat,clon].river);
                }
            }
            if (basemap[destlat, destlon] != null)
                tc += basemap[destlat, destlon].movecost(culture) / 2;
            else
                tc += bc / 20;

            return tc;
        }

        public bool iscoast(int u, int v)
        {
            if (u < 0)
            {
               return this.terrain.coastdict['N'];
            }
            else if ( u > 0)
            {
                return this.terrain.coastdict['S'];
            }
            else
            {
                if ( v < 0)
                {
                    return this.terrain.coastdict['W'];
                }
                else
                {
                    return this.terrain.coastdict['E'];
                }
            }
            
        }

        public int movecost(cultureclass culture)
        {
            double bc = basetravelcost;

            bc *= travelcostdict[this.terrain.type0()];
            if (culture.knows("riding"))
                bc *= ridingfactordict[this.terrain.type0()];
            if (culture.knows("wheel"))
                bc *= ridingfactordict[this.terrain.type0()];
            return (int)bc;
        }

        public bool hasriver()
        {
            return !String.IsNullOrEmpty(this.river);
        }
    }
}
