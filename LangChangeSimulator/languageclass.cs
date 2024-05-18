using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangChangeSimulator
{
    public class languageclass
    {
        public static Dictionary<int, languageclass> langdict = new Dictionary<int, languageclass>();
        public static Dictionary<string, languageclass> sourcedict = new Dictionary<string, languageclass>();
        static int maxid = 0;
        public static int ndead = 0;

        public int id;
        public int ancestor; //id of immediate ancestor
        public int root; //id of original root of stock
        public List<int> descendants = new List<int>();
        public string source = "";
        public soundsystemclass inventory;
        public int speakers;
        public lexiconclass lexicon;
        public grammarclass grammar;
        public double fitness = 1;

        public int birthyear = 0;
        public int deathyear = -1;

        public cultureclass culture = new cultureclass();
        public int ilat=0;
        public int ilon=0;

        public string based_on = ""; //if based on real language, iso3-code

        public languageclass(string sourcepar, List<int> conceptlist) //new language 
        {
            maxid++;
            this.id = maxid;
            this.ancestor = -1;
            this.root = this.id;
            this.source = sourcepar;
            this.speakers = parameterclass.p.get<int>("startingpop");
            this.birthyear = 0;
            langdict.Add(id, this);

            this.lexicon = new lexiconclass(this, conceptlist);
            this.inventory = new soundsystemclass(this.lexicon);
            this.grammar = new grammarclass();
            //startingwords(conceptlist);
            startingposition();

            langtreeclass.treedict.Add(this.source, new langtreeclass(this));

            languageclass.sourcedict.Add(this.source, new languageclass(this, 0, 0, 0, -1));

        }

        public languageclass(languageclass parent, int pop, int ilatpar, int ilonpar, int year) //creates a clone of parent language
        {
            maxid++;
            this.id = maxid;
            this.root = parent.root;
            this.ancestor = parent.id;
            this.source = parent.source;
            this.speakers = pop;
            this.inventory = util.DeepCopy<soundsystemclass>(parent.inventory);
            this.lexicon = new lexiconclass(this,parent.lexicon);
            this.culture = util.DeepCopy<cultureclass>(parent.culture);
            this.grammar = parent.grammar.clone();
            this.birthyear = year;

            this.ilat = ilatpar;
            this.ilon = ilonpar;
            if (year >= 0)
            {
                mapgridclass.map[ilat, ilon].addlanguage(this);

                langdict.Add(id, this);

                langtreeclass.treedict[this.source].addnode(parent, this, year);
            }
        }

        public languageclass()
        {

        }

        private void startingposition() //place new language at random position
        {
            Random rnd = new Random();
            int latoffset = 0;
            int lonoffset = 0;
            int latmax = mapgridclass.imax;
            int lonmax = mapgridclass.jmax;
            bool goodspot = true;
            do
            {
                this.ilat = latoffset + rnd.Next(latmax);
                this.ilon = lonoffset + rnd.Next(lonmax);
                goodspot = true;
                if (!mapgridclass.inmap(ilat, ilon))
                    goodspot = false;
                else if (mapgridclass.map[ilat, ilon] == null)
                    goodspot = false;
                else if (mapgridclass.map[ilat, ilon].languages.Count > 0)
                    goodspot = false;
                else if (mapgridclass.startregion != "World" && !mapgridclass.inregion(mapgridclass.basemap[this.ilat, this.ilon], mapgridclass.startregion))
                    goodspot = false;
                else if (cellclass.carryingcapacity(ilat, ilon, culture) == 0)
                    goodspot = false;
            }
            while ( !goodspot );

            mapgridclass.map[ilat, ilon].addlanguage(this);
        }

        public int addspeakers(int dpop,int year)
        {
            this.speakers += dpop;
            
            mapgridclass.map[ilat, ilon].population += dpop;
            if (this.speakers <= 0)
            {
                this.kill(year);
            }
            return this.speakers;
        }

        public void kill(int year) //kills the language
        {
            mapgridclass.map[ilat, ilon].population -= this.speakers;
            mapgridclass.map[ilat, ilon].languages.Remove(this.id);
            mapgridclass.map[ilat, ilon].aa.remove(this);
            this.speakers = 0;
            this.deathyear = year;
            this.lexicon = null;
            this.inventory = null;

            ndead++;
        }

        public languageclass postmortem() //creates a placeholder record for a dead language
        {
            languageclass ll = new languageclass();
            ll.id = this.id;
            ll.root = this.root;
            ll.ancestor = this.ancestor;
            ll.source = this.source;
            ll.speakers = 0;
            ll.birthyear = this.birthyear;
            return ll;
        }



        public int addspeakers(double dpop,int year)
        {
            return addspeakers((int)dpop,year);
        }

        public List<int> ancestors()
        {
            List<int> ls = new List<int>();

            languageclass lc = this;
            while (lc.ancestor >= 0)
            {
                ls.Add(lc.ancestor);
                lc = languageclass.langdict[lc.ancestor];
            }
            return ls;
        }

        public int lcatime(languageclass lc2) //get time of last common ancestor with LC2; -1 if different stock
        {
            if (this.source != lc2.source)
                return -1;
            //List<int> lc2ancestors = lc2.ancestors();
            //foreach (int ia in this.ancestors())
            //{
            //    if (lc2ancestors.Contains(ia))
            //    {
            //        return languageclass.langdict[ia].birthyear;
            //    }
            //}

            return langtreeclass.treedict[this.source].findsplit(this.id,lc2.id);
        }

        public double mutually_understandable(languageclass lc2,int time)
        {
            int lca = lcatime(lc2);
            if (lca < 0)
                return 0;
            if (time - lca > 2000)
                return 0;
            else
                return 1 - (time - lca) / (double)2000;
        }

        public void move(int newlat,int newlon)
        {
            mapgridclass.map[ilat, ilon].population -= this.speakers;
            mapgridclass.map[ilat, ilon].languages.Remove(this.id);
            ilat = newlat;
            ilon = newlon;
            mapgridclass.map[ilat, ilon].addlanguage(this);
        }

        public static string getstatus(int ilang)
        {
            if (!langdict.ContainsKey(ilang))
                return " dead";
            else if (langdict[ilang].speakers <= 0)
                return " dead";
            else
                return " " + langdict[ilang].speakers;
        }

        public double latitude()
        {
            return mapgridclass.basemap[this.ilat, this.ilon].lat;
        }
        public double longitude()
        {
            return mapgridclass.basemap[this.ilat, this.ilon].lon;
        }

        public void unconditional_soundchange(int oldsound, int newsound)
        {
            int index = this.inventory.sounds.IndexOf(oldsound);
            unconditional_soundchange(index, oldsound, newsound);
        }

        public void unconditional_soundchange(int index, int oldsound, int newsound)
        {
            this.lexicon.unconditional_soundchange(oldsound, newsound);
            if (this.inventory.sounds.Contains(newsound))
            {
                this.inventory.sounds.Remove(oldsound);
                mapgridclass.map[ilat, ilon].aa.remove(oldsound);
            }
            else
            {
                this.inventory.sounds[index] = newsound;
                mapgridclass.map[ilat, ilon].aa.remove(oldsound);
                mapgridclass.map[ilat, ilon].aa.add(newsound);
            }
        }


    }
}
