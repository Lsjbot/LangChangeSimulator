using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangChangeSimulator
{
    class cellclass
    {
        public List<int> languages = new List<int>();

        public int population = 0;
        public mapgridclass mg;
        public areaclass aa = new areaclass();
        public Tuple<int, int> lastgoodmigration = null;
        public Tuple<int, int> lastbadmigration = null;


        public static Random rnd = new Random();

        public static double bcc = parameterclass.p.get<double>("basecarryingcapacity")*mapgridclass.km*mapgridclass.km;
        

        public static Dictionary<string, Dictionary<string, double>> carrydict = new Dictionary<string, Dictionary<string, double>>()
        {
{"arctic",new Dictionary<string,double>(){{"hunter-gatherer",0},{"agriculture",0},{"hunter-gatherer+river",0.2},{"agriculture+river",0},{"horticulture",0},{"horticulture+river",0.1},{"herding",0},{"herding+river",0}}},
{"boreal",new Dictionary<string,double>(){{"hunter-gatherer",0.2},{"agriculture",0.3},{"hunter-gatherer+river",0.4},{"agriculture+river",0.5},{"horticulture",0.25},{"horticulture+river",0.4},{"herding",0.5},{"herding+river",0.7}}},
{"continental",new Dictionary<string,double>(){{"hunter-gatherer",0.4},{"agriculture",0.8},{"hunter-gatherer+river",0.6},{"agriculture+river",1.2},{"horticulture",0.6},{"horticulture+river",1},{"herding",0.6},{"herding+river",1}}},
{"continental subarctic",new Dictionary<string,double>(){{"hunter-gatherer",0.1},{"agriculture",0.1},{"hunter-gatherer+river",0.3},{"agriculture+river",0.1},{"horticulture",0.1},{"horticulture+river",0.3},{"herding",0.1},{"herding+river",0.3}}},
{"desert",new Dictionary<string,double>(){{"hunter-gatherer",0},{"agriculture",0},{"hunter-gatherer+river",0.4},{"agriculture+river",1},{"horticulture",0},{"horticulture+river",0.7},{"herding",0.3},{"herding+river",0.7}}},
{"desert cold",new Dictionary<string,double>(){{"hunter-gatherer",0},{"agriculture",0},{"hunter-gatherer+river",0.5},{"agriculture+river",0.7},{"horticulture",0},{"horticulture+river",0.6},{"herding",0.4},{"herding+river",0.6}}},
{"desert hot",new Dictionary<string,double>(){{"hunter-gatherer",0},{"agriculture",0},{"hunter-gatherer+river",0.4},{"agriculture+river",1.2},{"horticulture",0},{"horticulture+river",0.8},{"herding",0.3},{"herding+river",0.8}}},
{"hemiboreal",new Dictionary<string,double>(){{"hunter-gatherer",0.6},{"agriculture",1.3},{"hunter-gatherer+river",0.8},{"agriculture+river",1.6},{"horticulture",1},{"horticulture+river",1.2},{"herding",1},{"herding+river",1.2}}},
{"humid subtropical",new Dictionary<string,double>(){{"hunter-gatherer",1},{"agriculture",2},{"hunter-gatherer+river",1.2},{"agriculture+river",2},{"horticulture",1.5},{"horticulture+river",1.6},{"herding",1},{"herding+river",1.6}}},
{"mediterranean",new Dictionary<string,double>(){{"hunter-gatherer",0.8},{"agriculture",2},{"hunter-gatherer+river",1.3},{"agriculture+river",3},{"horticulture",1.4},{"horticulture+river",2.2},{"herding",2},{"herding+river",3}}},
{"monsoon",new Dictionary<string,double>(){{"hunter-gatherer",1},{"agriculture",4},{"hunter-gatherer+river",1.3},{"agriculture+river",5},{"horticulture",2.5},{"horticulture+river",3},{"herding",2},{"herding+river",2.5}}},
{"oceanic",new Dictionary<string,double>(){{"hunter-gatherer",1},{"agriculture",3},{"hunter-gatherer+river",1.2},{"agriculture+river",3},{"horticulture",2},{"horticulture+river",3.1},{"herding",2.5},{"herding+river",2.6}}},
{"rainforest",new Dictionary<string,double>(){{"hunter-gatherer",0.4},{"agriculture",1},{"hunter-gatherer+river",0.7},{"agriculture+river",1.5},{"horticulture",0.7},{"horticulture+river",1.3},{"herding",0.5},{"herding+river",0.8}}},
{"savanna",new Dictionary<string,double>(){{"hunter-gatherer",1},{"agriculture",2},{"hunter-gatherer+river",1.5},{"agriculture+river",3},{"horticulture",1.5},{"horticulture+river",2.2},{"herding",2},{"herding+river",3}}},
{"steppe",new Dictionary<string,double>(){{"hunter-gatherer",0.5},{"agriculture",0.5},{"hunter-gatherer+river",1},{"agriculture+river",1.5},{"horticulture",0.5},{"horticulture+river",1.3},{"herding",1.5},{"herding+river",2.5}}},
{"steppe cold",new Dictionary<string,double>(){{"hunter-gatherer",0.4},{"agriculture",0.4},{"hunter-gatherer+river",1},{"agriculture+river",1},{"horticulture",0.4},{"horticulture+river",1},{"herding",1.5},{"herding+river",2.5}}},
{"steppe hot",new Dictionary<string,double>(){{"hunter-gatherer",0.5},{"agriculture",0.5},{"hunter-gatherer+river",1},{"agriculture+river",1.5},{"horticulture",0.5},{"horticulture+river",1.3},{"herding",1.5},{"herding+river",2.5}}},
{"temperate",new Dictionary<string,double>(){{"hunter-gatherer",1},{"agriculture",2},{"hunter-gatherer+river",1.2},{"agriculture+river",2.7},{"horticulture",1.5},{"horticulture+river",2},{"herding",1.5},{"herding+river",2}}},
{"tropical",new Dictionary<string,double>(){{"hunter-gatherer",0.7},{"agriculture",1.5},{"hunter-gatherer+river",1},{"agriculture+river",1.8},{"horticulture",1.1},{"horticulture+river",1.4},{"herding",1},{"herding+river",1.2}}},
{"tropical highland",new Dictionary<string,double>(){{"hunter-gatherer",1},{"agriculture",2},{"hunter-gatherer+river",1.1},{"agriculture+river",2.4},{"horticulture",1.5},{"horticulture+river",1.8},{"herding",1},{"herding+river",1.2}}},
{"tundra",new Dictionary<string,double>(){{"hunter-gatherer",0.2},{"agriculture",0},{"hunter-gatherer+river",0.2},{"agriculture+river",0.3},{"horticulture",0.2},{"horticulture+river",0.3},{"herding",0.7},{"herding+river",1.2}}},
{"unknown",new Dictionary<string,double>(){{"hunter-gatherer",0},{"agriculture",0},{"hunter-gatherer+river",0},{"agriculture+river",0},{"horticulture",0},{"horticulture+river",0},{"herding",0},{"herding+river",0}}},
       };



        public cellclass(mapgridclass mgpar)
        {
            this.mg = mgpar;
        }

        public static double basecarryingcapacity = parameterclass.p.get<double>("basecarryingcapacity");

        public static int carryingcapacity(int ilat, int ilon, cultureclass culture)
        {
            return carryingcapacity(ilat, ilon, culture, culture.subsistence.name);
        }
        public static int carryingcapacity(int ilat, int ilon, cultureclass culture,string subsub)
        {
            if (mapgridclass.map[ilat, ilon] == null)
                return 0;

            double cc = 1;
            string cs = mapgridclass.basemap[ilat, ilon].climatestring.Split('|')[0];
            if (carrydict.ContainsKey(cs))
            {
                string sub = subsub;
                if (mapgridclass.basemap[ilat, ilon].hasriver())
                    sub += "+river";
                if (carrydict[cs].ContainsKey(sub))
                    cc *= carrydict[cs][sub];
                else
                {
                    Console.WriteLine("Default subsistence " + cs + " " + sub);
                    cc *= carrydict[cs][parameterclass.p.get("defaultsubsistence")];
                }
                if (mapgridclass.basemap[ilat,ilon].terrain.landfraction < 0.95)
                {
                    cc += 0.5;
                    if (culture.knows("boats"))
                        cc += 0.8;
                    if (culture.knows("oceangoing"))
                        cc += 1.5;
                }
                if ((mapgridclass.basemap[ilat, ilon].climate.landcover == 16) || (mapgridclass.basemap[ilat, ilon].climate.temp_min < -20))
                { //arctic weather
                    if (!culture.knows("arctic"))
                        cc = 0;
                }
                else if (mapgridclass.basemap[ilat, ilon].climate.temp_min < -12)
                { //cold weather
                    if (!culture.knows("coldgear"))
                        cc = 0;
                }
                else if (culture.knows("desertgear"))
                { //dry climate
                    if ((mapgridclass.basemap[ilat, ilon].climate.rainfall < 500)&&(mapgridclass.basemap[ilat, ilon].climate.rainfall > 0))
                        cc += 0.3;
                }
                //else if (mapgridclass.basemap[ilat, ilon].climate.landcover == 17)
                //{ //barren lands
                //    if (!sub.Contains("river"))
                //        cc = 0;
                //}
                else if (mapgridclass.basemap[ilat, ilon].climate.landcover > 12)
                { //modern-day agriculture
                    if (sub.Contains("agri") || sub.Contains("horti"))
                        cc *= 1.5;
                }
                if (sub.Contains("herd"))
                {
                    if (culture.knows("riding"))
                        cc *= 1.5;
                }
            }
            else
            {
                Console.WriteLine("climate not found: " + mapgridclass.basemap[ilat, ilon].climatestring);
                Console.WriteLine("ilat,ilon = " + ilat + ", " + ilon);
            }

            cc *= basecarryingcapacity;
            //cc *= mapgridclass.basemap[ilat, ilon].terrain.landfraction;
            cc *= mapgridclass.km * mapgridclass.km;

            int icc = (int)cc;
            int fluctuation = rnd.Next(icc/2) - rnd.Next(icc / 3);
            icc += fluctuation;
            if (icc < 0)
                return 0;
            else
                return icc;
        }

        public static int carryingcapacity(languageclass lc)
        {
            return carryingcapacity(lc.ilat, lc.ilon, lc.culture);
        }

        public void addlanguage(languageclass lc)
        {
            languages.Add(lc.id);
            population += lc.speakers;
            aa.add(lc);
        }

        public int biggestlanguage()
        {
            int ilang = -1;
            int max = -1;
            foreach (int il in languages)
                if (languageclass.langdict[il].speakers > ilang)
                {
                    ilang = il;
                    max = languageclass.langdict[il].speakers;
                }
            return ilang;
        }
    }
}
