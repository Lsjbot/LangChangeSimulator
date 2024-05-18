using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LangChangeSimulator
{
    public partial class FormSimulation : Form
    {
        int timestep = 20;
        int time = 0;
        bool running = false;
        bool pause = false;
        bool stopthread = false;
        Thread thread = null;
        FormSimDisplay fsd = null;
        FormMap fm;
        public static int maxtime;
        public FormSimulation()
        {
            InitializeComponent();
            TB_timestep.Text = parameterclass.p.get("timestep");

            CB_soundchange.Checked = parameterclass.p.get<bool>("soundchangeenabled");
            CB_contact.Checked = parameterclass.p.get<bool>("contactenabled");
            CB_semantics.Checked = parameterclass.p.get<bool>("semanticshiftenabled");
            CB_conditional.Enabled = CB_soundchange.Checked;
            CB_unconditional.Enabled = CB_soundchange.Checked;
            CB_soundborrowing.Enabled = CB_soundchange.Checked;
            CB_conditional.Checked = parameterclass.p.get<bool>("conditionalenabled");
            CB_unconditional.Checked = parameterclass.p.get<bool>("unconditionalenabled");
            CB_soundborrowing.Checked = parameterclass.p.get<bool>("soundborrowingenabled");

            fm = new FormMap();
            fm.languagemap();
            fm.Show();
        }

        public void memo(string s)
        {
            richTextBox1.AppendText(s + "\n");
            richTextBox1.ScrollToCaret();
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
         //   thread.Abort();   
            this.Close();
        }

        private void CB_soundchange_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_soundchange.Checked)
                parameterclass.p.put("soundchangeenabled", "true");
            else
                parameterclass.p.put("soundchangeenabled", "false");

            CB_conditional.Enabled = CB_soundchange.Checked;
            CB_unconditional.Enabled = CB_soundchange.Checked;
            CB_soundborrowing.Enabled = CB_soundchange.Checked;

        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            simulate();
            //if (thread == null)
            //{
            //    timestep = util.tryconvert(TB_timestep.Text);
            //    if (timestep < 0)
            //        timestep = 20;

            //    thread = new Thread(new ThreadStart(simulate));
            //    thread.Priority = ThreadPriority.BelowNormal;
            //    thread.Start();
            //    running = true;
            //    RunButton.Text = "Pause simulation";
            //}
            //else if (running)
            //{
            //    pause = true;
            //    running = false;
            //    RunButton.Text = "Resume simulation";
            //}
            //else
            //{
            //    pause = false;
            //    running = true;
            //    RunButton.Text = "Pause simulation";
            //}
        }

        Dictionary<int, Dictionary<int, int>> soundchangedict = new Dictionary<int, Dictionary<int, int>>();
        Dictionary<int, int> soundchangetodict = new Dictionary<int, int>();

        private void do_soundchange(List<int> origlang)
        {
            Random rnd = new Random();
            double baseprob = parameterclass.p.get<double>("soundchangeprob");
            double metathesisprob = parameterclass.p.get<double>("metathesisrate");
            double unconditionalprob = parameterclass.p.get<double>("unconditionalfraction");
            //memo("Doing sound change...");
            DateTime starttime = DateTime.Now;
            foreach (int il in origlang)
            {
                languageclass lc = languageclass.langdict[il];
                if (lc.speakers > 0)
                {
                    double prob = baseprob * lc.culture.openness * timestep;
                    if (rnd.NextDouble() < prob)
                    {
                        int i = rnd.Next(lc.inventory.sounds.Count);
                        int isound = lc.inventory.sounds[i];
                        int ntry = 0;
                        int newsound = -1;
                        while (newsound < 0 && ntry < 10)
                        {
                            newsound = segmentclass.segmentdict[isound].get_changecandidate(rnd);
                            ntry++;
                        }
                        if (newsound >= 0)
                        {
                            if (!soundchangedict.ContainsKey(isound))
                            {
                                soundchangedict.Add(isound, new Dictionary<int, int>());
                            }
                            if (!soundchangedict[isound].ContainsKey(newsound))
                                soundchangedict[isound].Add(newsound, 1);
                            else
                                soundchangedict[isound][newsound]++;
                            if (!soundchangetodict.ContainsKey(newsound))
                                soundchangetodict.Add(newsound, 1);
                            else
                                soundchangetodict[newsound]++;

                            //memo("Changing " + segmentclass.segmentdict[isound].fullseg + " to " + segmentclass.segmentdict[newsound].fullseg + " in " + lc.id + ":" + lc.source);
                            if (rnd.NextDouble() < unconditionalprob)
                            {
                                if (CB_unconditional.Checked)
                                {
                                    lc.unconditional_soundchange(i, isound, newsound);
                                }
                            }
                            else
                            {
                                if (CB_conditional.Checked)
                                {
                                    lc.lexicon.conditional_soundchange(isound, newsound);
                                    mapgridclass.map[lc.ilat, lc.ilon].aa.remove(lc.inventory);
                                    lc.inventory = new soundsystemclass(lc.lexicon);
                                    mapgridclass.map[lc.ilat, lc.ilon].aa.add(lc.inventory);
                                }
                            }
                        }
                    }
                    double metaprob = metathesisprob * lc.culture.openness * timestep;
                    if (rnd.NextDouble() < metaprob)
                    {
                        int i = rnd.Next(lc.lexicon.words.Count);
                        string oldword = lc.lexicon.words[i].getform();
                        lc.lexicon.words[i].metathesis(rnd);
                        string newword = lc.lexicon.words[i].getform();
                        //memo("meta: " + oldword + " -> " + newword);
                    }
                }
            }
            memo("Sound changes done in "+(DateTime.Now-starttime).TotalSeconds.ToString("F2"));

        }

        private bool travelsuccess(int cost, int budget, Random rnd)
        {
            if (cost >= mapgridclass.infinitecost)
                return false;

            double dd = 0.8*budget / cost;
            return (dd > rnd.NextDouble());
        }

        private Tuple<int,int> randomdir(Random rnd)
        {
            Tuple<int, int> uv;
            switch (rnd.Next(8))
            {
                case 0:
                    uv = new Tuple<int, int>(-1, -1);
                    break;
                case 1:
                    uv = new Tuple<int, int>(-1, 0);
                    break;
                case 2:
                    uv = new Tuple<int, int>(-1, 1);
                    break;
                case 3:
                    uv = new Tuple<int, int>(0, -1);
                    break;
                case 4:
                    uv = new Tuple<int, int>(0, 1);
                    break;
                case 5:
                    uv = new Tuple<int, int>(1, -1);
                    break;
                case 6:
                    uv = new Tuple<int, int>(1, 0);
                    break;
                case 7:
                    uv = new Tuple<int, int>(1, 1);
                    break;
                default:
                    uv = new Tuple<int, int>(1, 1);
                    break;
            }
            return uv;
        }

        private Tuple<int,int> oneshottravel(languageclass lc,Tuple<int,int> uv, double distfactor)
        {
            //returns final lat/lon
            int budget = (int)(parameterclass.p.get<int>("basetravelbudget")*timestep*distfactor);
            int bestlat = -1;
            int bestlon = -1;

            Random rnd = new Random();

            if (uv == null)
            {
            }
            randomdir(rnd);
            int u = uv.Item1;
            int v = uv.Item2;
            int prevlat = lc.ilat;
            int prevlon = lc.ilon;
            int radius = 1;

            while (budget > 0)
            {
                int currentlat = lc.ilat + u * radius;
                int currentlon = lc.ilon + v * radius;
                if (!mapgridclass.inmap(currentlat, currentlon))
                    break;
                int tc = mapgridclass.infinitecost;
                if (mapgridclass.basemap[prevlat, prevlon] != null)
                    tc = mapgridclass.basemap[prevlat, prevlon].travelcost(currentlat, currentlon, lc.culture);
                else if (lc.culture.knows("oceangoing"))
                    tc = parameterclass.p.get<int>("basetravelcost") / 10;
                if (travelsuccess(tc, budget, rnd))
                {
                    if (mapgridclass.map[currentlat, currentlon] != null)
                    {
                            bestlat = currentlat;
                            bestlon = currentlon;
                    }
                }
                else
                    break;

                budget -= tc;
                radius++;
                prevlat = currentlat;
                prevlon = currentlon;
            }

            return new Tuple<int, int>(bestlat, bestlon);
        }

        private void do_migration(languageclass lc, int migrants)
        {
            //Console.WriteLine("Migrate!");
            //DateTime starttime = DateTime.Now;
            int radius = 1;
            //bool onwards = false;
            int budget = parameterclass.p.get<int>("basetravelbudget")*timestep;
            int bestcarry = -1;
            int bestlat = -1;
            int bestlon = -1;
            int bestemptycarry = -1;
            int bestemptylat = -1;
            int bestemptylon = -1;

            Random rnd = new Random();

            int oldcc = cellclass.carryingcapacity(lc.ilat, lc.ilon, lc.culture);


            Tuple<int, int> uv = mapgridclass.map[lc.ilat, lc.ilon].lastgoodmigration;
            if (uv == null)
            {
                bool bad = false;
                int ntry = 0;
                do
                {
                    uv = randomdir(rnd);
                    if (mapgridclass.map[lc.ilat, lc.ilon].lastbadmigration != null)
                        if (mapgridclass.map[lc.ilat, lc.ilon].lastbadmigration.Equals(uv))
                            bad = true;
                    ntry++;
                    if (ntry > 5)
                        break;
                }
                while (bad);
            }
            //randomdir(rnd);
            int u = uv.Item1;
            int v = uv.Item2;
            int prevlat = lc.ilat;
            int prevlon = lc.ilon;

            while (budget > 0)
            {
                int currentlat = lc.ilat + u * radius;
                int currentlon = lc.ilon + v * radius;
                if (!mapgridclass.inmap(currentlat, currentlon))
                    break;
                int tc = mapgridclass.infinitecost;
                if (mapgridclass.basemap[prevlat, prevlon] != null)
                    tc = mapgridclass.basemap[prevlat, prevlon].travelcost(currentlat, currentlon, lc.culture);
                else if (lc.culture.knows("oceangoing"))
                    tc = parameterclass.p.get<int>("basetravelcost") / 10;
                if (travelsuccess(tc, budget, rnd))
                {
                    if (mapgridclass.map[currentlat, currentlon] != null)
                    {
                        int carry = cellclass.carryingcapacity(currentlat, currentlon, lc.culture) - mapgridclass.map[currentlat, currentlon].population;
                        if (carry > bestcarry)
                        {
                            bestcarry = carry;
                            bestlat = currentlat;
                            bestlon = currentlon;
                        }
                        if ((mapgridclass.map[currentlat, currentlon].languages.Count == 0) && (carry > bestemptycarry))
                        {
                            bestemptycarry = carry;
                            bestemptylat = currentlat;
                            bestemptylon = currentlon;
                        }
                    }
                }
                else
                    break;
                if (bestemptycarry >= oldcc)
                {
                    bestlat = bestemptylat;
                    bestlon = bestemptylon;
                    break;
                }
                if (bestcarry >= 2*oldcc)
                {
                    break;
                }

                budget -= tc;
                radius++;
                prevlat = currentlat;
                prevlon = currentlon;
            }
            //Console.WriteLine("radius=" + radius);
            if (bestemptycarry >= migrants)
            {
                bestlat = bestemptylat;
                bestlon = bestemptylon;
            }
            //Console.WriteLine("bestcarry = " + bestcarry);
            if ((bestlat >= 0) && (bestcarry >= migrants))
            {
                //fsd.memo("Migration " + migrants + " to " + bestlat + ", " + bestlon);
                //Console.WriteLine("Migration " + migrants + " to " + bestlat + ", " + bestlon);
                mapgridclass.map[lc.ilat, lc.ilon].lastgoodmigration = uv;

                bool merged = false;
                if (mapgridclass.map[bestlat,bestlon].languages.Count > 0)
                {
                    //Console.WriteLine("Seeking merge "+ mapgridclass.map[bestlat, bestlon].languages.Count);

                    foreach (int ill in mapgridclass.map[bestlat, bestlon].languages)
                        if (languageclass.langdict[ill].speakers > 0)
                        {
                            //Console.WriteLine("ill = " + ill);
                            //int lca = lc.lcatime(languageclass.langdict[ill]);
                            if (lc.mutually_understandable(languageclass.langdict[ill], time) > 0.7) //still the same language
                            {
                                //Console.WriteLine("Merging into " + ill);
                                languageclass.langdict[ill].addspeakers(migrants, time);
                                merged = true;
                                break;
                            }
                        }

                    //if (mapgridclass.map[bestlat, bestlon].languages.Count > 3)
                    //{
                    //    foreach (int ill in mapgridclass.map[bestlat, bestlon].languages)
                    //    {
                    //        Console.WriteLine(ill+": "+languageclass.langdict[ill].speakers);
                    //    }
                    //    Console.WriteLine("Population = "+mapgridclass.map[bestlat, bestlon].population);
                    //}

                }
                if (!merged)
                {
                    if (migrants >= lc.speakers) //whole group moves; no language split
                    {
                        lc.move(bestlat, bestlon);
                        migrants = 0; //set to 0 in order not to double-subtract
                    }
                    else                    //Console.WriteLine("New language");
                    {
                        languageclass lcnew = new languageclass(lc, migrants, bestlat, bestlon, time);
                        lc.descendants.Add(lcnew.id);
                    }
                }
            }
            else
            {
                //fsd.memo("Migration failed from " + lc.ilat + ", " + lc.ilon);
                //Console.WriteLine("Migration failed from " + lc.ilat + ", " + lc.ilon);
                mapgridclass.map[lc.ilat, lc.ilon].lastgoodmigration = null;
                mapgridclass.map[lc.ilat, lc.ilon].lastbadmigration = uv;
            }
            //Console.WriteLine("Subtracting migrants");
            lc.addspeakers(-migrants, time);

            //memo("Migration done in " + (DateTime.Now - starttime).TotalSeconds.ToString("F2"));
            //if (tt.Seconds > 10)
            //{
            //    Console.WriteLine(tt);
            //}
            //Console.WriteLine("End of do_migration");
        }

        // OLD VERSION
        //private void do_migration(languageclass lc, int migrants)
        //{
        //    int radius = 1;
        //    bool onwards = false;
        //    int budget = parameterclass.p.get<int>("basetravelbudget");
        //    int bestcarry = -1;
        //    int bestlat = -1;
        //    int bestlon = -1;
        //    int bestemptycarry = -1;
        //    int bestemptylat = -1;
        //    int bestemptylon = -1;

        //    Random rnd = new Random();

        //    do
        //    {
        //        for (int u = -1; u <= 1; u++)
        //            for (int v = -1; v <= 1; v++)
        //            {
        //                if (u != 0 || v != 0)
        //                {
        //                    int tc = mapgridclass.basemap[lc.ilat, lc.ilon].travelcost(lc.ilat + u * radius, lc.ilon + v * radius, lc.culture);
        //                    if (travelsuccess(tc, budget, rnd))
        //                    {
        //                        int carry = cellclass.carryingcapacity(lc.ilat + u * radius, lc.ilon + v * radius, lc.culture);
        //                        if (carry > bestcarry)
        //                        {
        //                            bestcarry = cellclass.carryingcapacity(lc.ilat + u * radius, lc.ilon + v * radius, lc.culture);
        //                            bestlat = lc.ilat + u * radius;
        //                            bestlon = lc.ilon + v * radius;
        //                        }
        //                        if ((mapgridclass.map[lc.ilat + u * radius, lc.ilon + v * radius].languages.Count == 0) && (carry > bestemptycarry))
        //                        {
        //                            bestemptycarry = cellclass.carryingcapacity(lc.ilat + u * radius, lc.ilon + v * radius, lc.culture);
        //                            bestemptylat = lc.ilat + u * radius;
        //                            bestemptylon = lc.ilon + v * radius;
        //                        }
        //                    }
        //                    if (budget - tc > budget / 3)
        //                        onwards = true;
        //                }
        //            }
        //        if (bestemptycarry >= cellclass.carryingcapacity(lc.ilat, lc.ilon, lc.culture))
        //        {
        //            bestlat = bestemptylat;
        //            bestlon = bestemptylon;
        //            break;
        //        }
        //        if (bestcarry >= 2 * cellclass.carryingcapacity(lc.ilat, lc.ilon, lc.culture))
        //        {
        //            break;
        //        }
        //        radius++;
        //    }
        //    while (onwards);

        //    if (bestemptycarry >= migrants)
        //    {
        //        bestlat = bestemptylat;
        //        bestlon = bestemptylon;
        //    }
        //    if (bestemptylat >= 0)
        //    {
        //        fsd.memo("Migration " + migrants + " to " + bestlat + ", " + bestlon);
        //        Console.WriteLine("Migration " + migrants + " to " + bestlat + ", " + bestlon);

        //        languageclass lcnew = new languageclass(lc, migrants, bestlat, bestlon, time);
        //        lc.descendants.Add(lcnew.id);
        //    }
        //    else
        //    {
        //        fsd.memo("Migration failed from " + lc.ilat + ", " + lc.ilon);
        //    }
        //    lc.addspeakers(-migrants, time);

        //}

        private void do_population(List<int> origlang)
        {
            double basepopgrowth = parameterclass.p.get<double>("basepopgrowth");
            int minimumviable = parameterclass.p.get<int>("minimumviable");
            DateTime starttime = DateTime.Now;
            //Console.WriteLine("do_population " + origlang.Count);
            foreach (int il in origlang)
            {
                languageclass lc = languageclass.langdict[il];
                int migrants = 0;
                if (lc.speakers > 0)
                {
                    if (lc.speakers < minimumviable)
                    {
                        lc.kill(time);
                        languageclass.langdict[il] = lc.postmortem();
                        continue;
                    }
                    int cc = cellclass.carryingcapacity(lc);
                    if (lc.speakers >= cc)
                        migrants = lc.speakers - cc + lc.speakers / 3;
                    else if (mapgridclass.map[lc.ilat,lc.ilon].population >= cc)
                    {
                        if (lc.speakers < mapgridclass.map[lc.ilat, lc.ilon].population / 4)
                            migrants = lc.speakers; //minority: whole group migrates
                        else
                            migrants = lc.speakers / 3;
                    }
                    else
                    {
                        double popgrowth = basepopgrowth;
                        if (lc.culture.subsistence.name == "agriculture")
                            popgrowth *= 2;
                        else if (lc.culture.subsistence.name == "horticulture")
                            popgrowth *= 1.5;
                        else if (lc.culture.subsistence.name == "herding")
                            popgrowth *= 1.2;
                        if (2 * lc.speakers > cc)
                            popgrowth = popgrowth * (1.5 * cc - lc.speakers)/cc;
                        double dpop = lc.speakers * popgrowth * timestep;
                        if (dpop > cc)
                            Console.WriteLine("popgrowth, dpop = " + popgrowth + ", " + dpop);
                        lc.addspeakers(dpop,time);
                    }
                    if (migrants > 10000)
                    {
                        Console.WriteLine("lc.speakers, migrants " + lc.speakers + ", " + migrants);
                    }
                    if (migrants > 0)
                        do_migration(lc, migrants);
                }
            }
            memo("Population & migration done in " + (DateTime.Now - starttime).TotalSeconds.ToString("F2"));
        }

        private void borrow_word(languageclass sourcelang, languageclass destlang)
        {
            foreach (int cc in sourcelang.lexicon.concepts.Keys)
            {
                if (sourcelang.lexicon.concepts[cc].Count > 0)
                {
                    if (!destlang.lexicon.concepts.ContainsKey(cc) || destlang.lexicon.concepts[cc].Count == 0)
                    {
                        wordclass oldwc = sourcelang.lexicon.getword(sourcelang.lexicon.concepts[cc].First());
                        wordclass newwc = new wordclass(oldwc, destlang.id);
                        //MessageBox.Show("Borrowing missing concept " + swadeshclass.codeconceptdict[cc] + " " + segmentclass.DecodeForm(newwc.codedform));
                        destlang.lexicon.addword(newwc);
                        return;
                    }
                }
            }

            Random rnd = new Random();
            int icc = rnd.Next(sourcelang.lexicon.words.Count);
            wordclass nwc = new wordclass(sourcelang.lexicon.words.ElementAt(icc),destlang.id);
            if (!destlang.lexicon.hasword(nwc))
                destlang.lexicon.addword(nwc);
            //MessageBox.Show("Borrowing random word " + swadeshclass.codeconceptdict[nwc.concepts.First()] + " " + segmentclass.DecodeForm(nwc.codedform));
        }

        private void borrow_tech(languageclass sourcelang, languageclass destlang)
        {
            foreach (string t in sourcelang.culture.tech)
                if (!destlang.culture.knows(t))
                {
                    wordclass wc = destlang.culture.addtech(t,destlang,sourcelang);
                    destlang.lexicon.addword(wc);

                    //MessageBox.Show("Borrowing tech " + t + "with word "+segmentclass.DecodeForm(destlang.lexicon.getword(destlang.lexicon.concepts[techclass.techdict[t].conceptcode].First()).codedform));
                    break;
                }
        }

        private void contactsquare(languageclass lc, int ilat, int ilon, int dist, Random rnd)
        {
            if (!mapgridclass.inmap(ilat, ilon))
                return;
            if (mapgridclass.map[ilat, ilon] == null)
                return;

            double wbr = parameterclass.p.get<double>("wordborrowingrate");
            double tbr = parameterclass.p.get<double>("techborrowingrate");
            double webase = parameterclass.p.get<double>("wefactorbase");

            double distancefactor = 1 / (dist+0.5); 

            foreach (int ilang in mapgridclass.map[ilat,ilon].languages)
            {
                if (ilang != lc.id)
                {
                    double mutual = lc.mutually_understandable(languageclass.langdict[ilang],time);
                    double wefactor = 1 + webase*mutual; //enhanced borrowing from people we understand
                    double wrate = wbr * distancefactor * wefactor * timestep* lc.culture.openness;
                    double trate = tbr * distancefactor * wefactor * timestep* lc.culture.openness;

                    if (CB_wordborrowing.Checked && rnd.NextDouble() < wrate) //find a word to borrow
                    {
                        borrow_word(languageclass.langdict[ilang], lc);
                    }

                    if (CB_techborrowing.Checked && rnd.NextDouble() < trate) //look for tech to borrow
                    {
                        borrow_tech(languageclass.langdict[ilang], lc);
                    }
                }
            }
        }

        private void do_contact(List<int> origlang)
        {
            Random rnd = new Random();
            DateTime starttime = DateTime.Now;
            int switchrate = parameterclass.p.get<int>("switchrate");
            double switchborrow = parameterclass.p.get<double>("switchborrow");
            int arealradius = parameterclass.p.get<int>("arealradius");
            int centerbonus = parameterclass.p.get<int>("arealcenterbonus");
            int arealminlang = parameterclass.p.get<int>("arealminlang");
            double arealwordborrowingrate = parameterclass.p.get<double>("arealwordborrowingrate");
            double arealsoundborrowingrate = parameterclass.p.get<double>("arealsoundborrowingrate");

            int rmax = 1;
            foreach (int il in origlang)
            {
                languageclass lc = languageclass.langdict[il];
                if (lc.speakers > 0)
                {
                    if (mapgridclass.map[lc.ilat, lc.ilon].languages.Count > 0)
                    {
                        int lmax = mapgridclass.map[lc.ilat, lc.ilon].biggestlanguage();
                        if ( lmax != lc.id)
                        {
                            int switchers = (switchrate * languageclass.langdict[lmax].speakers) / lc.speakers;
                            if (switchers >= lc.speakers)
                            {
                                languageclass.langdict[lmax].addspeakers(lc.speakers, time);
                                lc.kill(time);
                                languageclass.langdict[il] = lc.postmortem();
                            }
                            else
                            {
                                int nborrow = (int)(switchers * switchborrow);
                                for (int ib = 0; ib < nborrow; ib++)
                                    borrow_word(lc, languageclass.langdict[lmax]);
                                languageclass.langdict[lmax].addspeakers(switchers, time);
                                lc.addspeakers(-switchers, time);
                            }
                        }
                        if (lc.speakers > 0)
                            contactsquare(lc, lc.ilat, lc.ilon, 0, rnd);
                    }
                    if (lc.speakers > 0)
                    {
                        for (int r = 1; r <= rmax; r++)
                        {
                            for (int u = -1; u <= 1; u++)
                                for (int v = -1; v <= 1; v++)
                                    if (u != 0 || v != 0)
                                    {
                                        contactsquare(lc, lc.ilat + r * u, lc.ilat + r * v, r, rnd);
                                    }
                        }

                        if (CB_longrange.Checked)
                        {
                            //do one long-range contact: travel in random direction until end of range, then borrow there
                            Tuple<int, int> tuv = randomdir(rnd);
                            Tuple<int, int> contact = oneshottravel(lc, tuv,3);
                            if (contact.Item1 > 0)
                            {
                                contactsquare(lc, contact.Item1, contact.Item2, 1, rnd);
                            }
                        }
                        if (CB_areal.Checked)
                        {
                            //do areal effects:
                            double wrate = arealwordborrowingrate * timestep * lc.culture.openness;
                            double srate = arealsoundborrowingrate * timestep * lc.culture.openness;
                            int nlregion = areaclass.nlangregion(lc.ilat, lc.ilon, arealradius, centerbonus);
                            if (nlregion >= arealminlang)
                            {
                                areaclass regionsum = areaclass.sumregion(lc.ilat, lc.ilon, arealradius, centerbonus);
                                if (CB_arealword.Checked && rnd.NextDouble() < wrate)
                                {
                                    wordclass wc = regionsum.findmajorityword(lc);
                                    if (wc != null)
                                    {
                                        lc.lexicon.addword(wc);
                                        //memo("Area borrow " + wc.getform());
                                    }
                                    //else
                                    //{
                                    //    memo("Area borrow failed");
                                    //}
                                }
                                if (CB_soundborrowing.Checked && rnd.NextDouble() < srate)
                                {
                                    int areasound = regionsum.findmajoritysound(lc);
                                    if (areasound >= 0)
                                    {
                                        int oldsound = lc.inventory.findnearest(areasound);
                                        if (oldsound >= 0)
                                        {
                                            lc.unconditional_soundchange(oldsound, areasound);
                                            //memo("areasound " + oldsound + ", " + areasound);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            memo("Contact done in " + (DateTime.Now - starttime).TotalSeconds.ToString("F2"));

        }

        private void do_culturechange(List<int> origlang)
        {
            Random rnd = new Random();
            DateTime starttime = DateTime.Now;
            double stepinventionrate = timestep * parameterclass.p.get<double>("baseinventionrate");
            foreach (int il in origlang)
            {
                languageclass lc = languageclass.langdict[il];
                if (lc.speakers > 0)
                {
                    lc.culture.mutate(rnd, stepinventionrate,lc);
                    if ((lc.culture.available_subsistence.Count > 1) && (rnd.Next(10)== 1))
                    {
                        int oldcc = cellclass.carryingcapacity(lc.ilat, lc.ilon, lc.culture);
                        foreach (string s in lc.culture.available_subsistence)
                        {
                            if (s != lc.culture.subsistence.name)
                            {
                                if (cellclass.carryingcapacity(lc.ilat,lc.ilon,lc.culture,s) > oldcc)
                                {
                                    //MessageBox.Show("Subsistence " + s);
                                    lc.culture.subsistence.name = s;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            memo("Culture done in " + (DateTime.Now - starttime).TotalSeconds.ToString("F2"));

        }



        private void do_semantics(List<int> origlang)
        {
            Random rnd = new Random();
            double polysemyloss = parameterclass.p.get<double>("polysemyloss");
            double synonymyloss = parameterclass.p.get<double>("synonymyloss");
            double colexrate = parameterclass.p.get<double>("colexrate");
            DateTime starttime = DateTime.Now;
            double skipfactor = 10; // every 10th step only, for any one language
            //double stepinventionrate = timestep * parameterclass.p.get<double>("baseinventionrate");
            foreach (int il in origlang)
            {
                languageclass lc = languageclass.langdict[il];
                if ((lc.speakers > 0) && (rnd.NextDouble() < 1/skipfactor))
                {
                    List<int> cdummy = lc.lexicon.concepts.Keys.ToList();
                    foreach (int concept in cdummy)
                    {
                        if (lc.lexicon.concepts[concept].Count > 1)
                        {
                            if (rnd.NextDouble() < synonymyloss * timestep * skipfactor * swadeshclass.conceptchangerate[concept]) 
                            {
                                //lose one word for that sense
                                int wloss = lc.lexicon.concepts[concept][rnd.Next(lc.lexicon.concepts[concept].Count)];
                                lc.lexicon.removesense(wloss, concept);
                                //Console.WriteLine("synonym lost");
                            }
                        }
                    }
                    List<wordclass> wdummy = lc.lexicon.words;

                    foreach (wordclass w in wdummy)
                    {
                        if (w.concepts.Count > 1)
                        {
                            if (rnd.NextDouble() < polysemyloss*timestep * skipfactor)
                            {
                                //lose one sense from that word
                                int closs = w.concepts[rnd.Next(w.concepts.Count)];
                                lc.lexicon.removesense(w.id, closs);
                                //Console.WriteLine("polysemy lost");
                            }
                        }
                    }

                    foreach (int concept in cdummy)
                    {
                        if (rnd.NextDouble() < colexrate * timestep * skipfactor * swadeshclass.conceptchangerate[concept])
                        {
                            if (rnd.NextDouble() * swadeshclass.colexmax < swadeshclass.colexratedict[concept])
                            {
                                int wordid = -1;
                                if (lc.lexicon.concepts[concept].Count == 0)
                                    continue;
                                else if (lc.lexicon.concepts[concept].Count == 1)
                                    wordid = lc.lexicon.concepts[concept].First();
                                else
                                    wordid = lc.lexicon.concepts[concept][rnd.Next(lc.lexicon.concepts[concept].Count)];
                                int colex = -1;
                                int r = rnd.Next(swadeshclass.colexratedict[concept]);
                                if (!swadeshclass.colexdict.ContainsKey(concept))
                                {
                                    swadeshclass.colexdict.Add(concept, new Dictionary<int, int>());
                                    var qc = from c in Form1.dbclics3.ConcepticonLink
                                             where (c.Concepticon1 == concept) || (c.Concepticon2 == concept)
                                             select c;
                                    foreach (ConcepticonLink cl in qc)
                                    {
                                        if (cl.Concepticon1 == concept)
                                            colex = cl.Concepticon2;
                                        else
                                            colex = cl.Concepticon1;
                                        swadeshclass.colexdict[concept].Add(colex, cl.Strength);
                                    }
                                }
                                int rsum = 0;
                                colex = -1;
                                foreach (int cl in swadeshclass.colexdict[concept].Keys)
                                {
                                    rsum += swadeshclass.colexdict[concept][cl];
                                    if (rsum > r)
                                    {
                                        colex = cl;
                                        break;
                                    }
                                }
                                //foreach (ConcepticonLink cl in qc)
                                //{
                                //    rsum += cl.Strength;
                                //    if (rsum > r)
                                //    {
                                //        if (cl.Concepticon1 == concept)
                                //            colex = cl.Concepticon2;
                                //        else
                                //            colex = cl.Concepticon1;
                                //        break;
                                //    }
                                //}
                                if (colex >= 0)
                                {
                                    lc.lexicon.addsense(wordid, colex);
                                    //Console.WriteLine("colex added "+swadeshclass.codeconceptdict[concept]+" "+swadeshclass.codeconceptdict[colex]);
                                }

                            }
                        }
                    }
                }
            }
            memo("Semantics done in " + (DateTime.Now - starttime).TotalSeconds.ToString("F2"));

        }

        private void do_grammar(List<int> origlang)
        {
            Random rnd = new Random();
            DateTime starttime = DateTime.Now;
            double grammarrate = timestep * parameterclass.p.get<double>("basegrammarrate");
            foreach (int il in origlang)
            {
                languageclass lc = languageclass.langdict[il];
                if (lc.speakers > 0)
                {
                    lc.grammar.mutate(rnd, grammarrate, lc);
                }
            }
            memo("Grammar done in " + (DateTime.Now - starttime).TotalSeconds.ToString("F2"));

        }

        string maptype = "bylanguage";

        private void simulate()
        {
            //fsd = new FormSimDisplay();
            //fsd.Show();
            ////FormMap fm = new FormMap();
            ////fm.Show();
            ////fm.BringToFront();
            //fsd.BringToFront();
            timestep = util.tryconvert(TB_timestep.Text);
            if (timestep < 0)
                timestep = 20;
            int startyear = util.tryconvert(TB_startyear.Text);
            parameterclass.p.put<int>("timestep", timestep);
            int memlimit = util.tryconvert(TB_memlimit.Text);
            int nlang = languageclass.langdict.Count;
            bool abort = false;
            maxtime = util.tryconvert(TB_maxtime.Text);
            if (maxtime < 0)
                maxtime = 99999;
            parameterclass.p.put<int>("maxtime", maxtime);
            DateTime lastloop = DateTime.Now;
            do
            {
                time += timestep;
                nasaclass.set_tempoffset(startyear + time);
                memo("tempoffset = " + nasaclass.tempoffset);
                //memo(time.ToString());
                List<int> origlang = languageclass.langdict.Keys.ToList(); //dummy list to avoid loop problems when adding languages
                Console.WriteLine("Year " + time.ToString() + ": " + origlang.Count);
                //fsd.memo("Year "+time.ToString()+": "+origlang.Count);
                //fsd.Invalidate();
                if (origlang.Count > nlang)
                {
                    nlang = origlang.Count;
                    //fm.languagemap();
                }

                Thread.Sleep(10);

                do_population(origlang);

                if (CB_soundchange.Checked)
                    do_soundchange(origlang);

                if (CB_contact.Checked)
                    do_contact(origlang);

                do_culturechange(origlang);

                if (CB_semantics.Checked)
                    do_semantics(origlang);

                if (CBgrammar.Checked)
                    do_grammar(origlang);

                //if (stopthread)
                //    break;
                //while (pause)
                //    Thread.Sleep(100);

                List<int> langlist = languageclass.langdict.Keys.ToList();
                foreach (int ilang in langlist)
                    if (languageclass.langdict[ilang].speakers == 0)
                    {
                        //languageclass.langdict.Remove(ilang);
                        if (languageclass.langdict[ilang].lexicon != null)
                        {
                            //foreach (int iw in languageclass.langdict[ilang].lexicon.words)
                            //{
                            //    wordclass.globalworddict.Remove(iw);
                            //}
                            languageclass.langdict[ilang].lexicon = null;
                        }
                    }

                if (CBscreenshot.Checked)
                    fm.languagemap("bylanguage", time, true);

                if ((time % 100 == 0)||((DateTime.Now-lastloop).TotalSeconds > 20))
                {
                    memo(maptype);
                    lastloop = DateTime.Now;
                    fm.languagemap(maptype,0,false);
                    if (maptype == "bylanguage")
                        maptype = "bydensity";
                    else if (maptype == "bydensity")
                        maptype = "bysubsistence";
                    else
                        maptype = "bylanguage";
                    statbutton_Click(null, null);
                }

                if (time % 200 == 0)
                {
                    var memory = getmemory();
                    memo("Memory = " + memory+"\tM/lang "+((double)memory/languageclass.langdict.Count)
                        +"\tM/(lang+dead) "+((double)memory/(languageclass.langdict.Count+languageclass.ndead)));
                    if (memory > memlimit)
                    {
                        Dictionary<int, languageclass> ldnew = new Dictionary<int, languageclass>();
                        foreach (int il in languageclass.langdict.Keys)
                            ldnew.Add(il, languageclass.langdict[il]);
                        memo("Memory during dictionary replacement = " + getmemory());
                        languageclass.langdict.Clear();
                        languageclass.langdict = ldnew;
                        memo("Memory after dictionary replacement = " + getmemory());

                        GC.Collect();
                        memo("Memory after GC = " + getmemory());

                        if (getmemory() > memlimit)
                            abort = true;
                    }
                }

                bringoutyourdead();
                if (time > maxtime)
                    abort = true;
            }
            while (!abort);

            memo("=======================================");
            memo("========  DONE                =========");
            memo("=======================================");
        }

        public long getmemory()
        {
            using (Process proc = Process.GetCurrentProcess())
            {
                // The proc.PrivateMemorySize64 will returns the private memory usage in byte.
                // Would like to Convert it to Megabyte? divide it by 2^20
                return proc.PrivateMemorySize64 / (1024 * 1024);
            }

        }

        private void bringoutyourdead()
        {
            List<int> origlang = languageclass.langdict.Keys.ToList(); //dummy list to avoid loop problems when adding languages
            foreach (int i in origlang)
                if (languageclass.langdict[i].speakers == 0)
                {
                    languageclass.langdict.Remove(i);
                }
            //languageclass.langdict.TrimExcess();
            GC.Collect();
        }

        private void RefreshMapButton_Click(object sender, EventArgs e)
        {
            if (maptype == "bylanguage")
                maptype = "bydensity";
            else if (maptype == "bydensity")
                maptype = "bysubsistence";
            else
                maptype = "bylanguage";
            memo(maptype);
            fm.languagemap(maptype,0,CBscreenshot.Checked);
        }

        private void statbutton_Click(object sender, EventArgs e)
        {
            memo("==============================");
            memo("Language statistics year " + time);
            memo("==============================");
            memo("# live languages: " + languageclass.langdict.Count);
            //int ndead = (from c in languageclass.langdict.Values where c.speakers == 0 select c).Count();
            //memo(" of which alive: " + (languageclass.langdict.Count-languageclass.ndead));
            memo("# dead languages: " + languageclass.ndead);
            int nocc = 0;
            foreach (cellclass cc in mapgridclass.map)
                if (cc != null)
                    if (cc.languages.Count > 0)
                        nocc++;
            int npop = 0;
            foreach (cellclass cc in mapgridclass.map)
                if (cc != null)
                    npop += cc.population;
            memo("Total population: " + npop);
            memo("# cells occupied " + nocc);
            memo("known tech:" + techclass.knowntech);
            //memo("# roots " + wordclass.globalrootdict.Count);
            memo("==============================");
        }

        private void savebutton_Click(object sender, EventArgs e)
        {
            string runfolder = Form1.folder + @"output\"+FormGeography.region+" "+time+" yrs "+DateTime.Now.ToShortDateString()+@"\";
            if (!Directory.Exists(runfolder))
                Directory.CreateDirectory(runfolder);
            string fn = util.unusedfilename(runfolder+"parameters.txt");
            fn = fn.Replace("parameters", "swadesh");
            memo("Saving to " + fn);
            if (CB_Swadesh.Checked)
                swadeshclass.write_swadeshtable(fn, "");
            string fngram = fn.Replace("swadesh", "grammar");
            if (CBsavegrammar.Checked)
                grammarclass.write_grammartable(fngram, "");
            if (CB_CLDF.Checked)
            {
                memo("Saving CLDF");
                swadeshclass.write_CLDF(fn, "");
            }

            parameterclass.p.save(fn.Replace("swadesh", "parameters"));
            if (CBnexus.Checked)
            {
                memo("Saving nexus files");
                foreach (string src in langtreeclass.treedict.Keys)
                {
                    swadeshclass.write_nexus(fn, src);
                    swadeshclass.write_nexustree(fn, src);
                }
            }
            memo("Done saving");
        }

        private void soundchangestatbutton_Click(object sender, EventArgs e)
        {
            //foreach (int isound in soundchangedict.Keys)
            //{
            //    memo(segmentclass.segmentdict[isound].fullseg);
            //    foreach (int newsound in soundchangedict[isound].Keys)
            //        memo("\t" + segmentclass.segmentdict[newsound].fullseg + "\t" + soundchangedict[isound][newsound]);
            //}
            var q = from c in soundchangetodict orderby -c.Value select c;
            foreach (KeyValuePair<int,int> c in q)
                memo(segmentclass.segmentdict[c.Key].fullseg + "\t" + c.Value);
        }

        private void CB_CLDF_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CB_contact_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_contact.Checked)
                parameterclass.p.put("contactenabled", "true");
            else
                parameterclass.p.put("contactenabled", "false");
        }

        private void CB_semantics_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_semantics.Checked)
                parameterclass.p.put("semanticshiftenabled", "true");
            else
                parameterclass.p.put("semanticshiftenabled", "false");

        }

        private void Worddistbutton_Click(object sender, EventArgs e)
        {
            hbookclass samefamhist = new hbookclass("Same family");
            samefamhist.SetBins(0, 1, 20);
            hbookclass difffamhist = new hbookclass("Different family");
            difffamhist.SetBins(0, 1, 20);

            //if (LB_concepts.SelectedItem == null)
            //{
            //    memo("No concept selected");
            //    return;
            //}

            //string selectedconcept = LB_concepts.SelectedItem.ToString();

            //List<string> conceptlist = new List<string>();
            //if (selectedconcept == allstring)
            //{
            //    foreach (string ss in LB_concepts.Items)
            //        conceptlist.Add(ss);
            //}
            //else
            //{
            //    conceptlist.Add(selectedconcept);

            //}
            int nlang = 0;
            foreach (languageclass ll in languageclass.langdict.Values)
            {
                nlang++;
                if (nlang % 10 == 0)
                    memo(nlang + " " + ll.source + ll.id);
                foreach (languageclass ll2 in languageclass.langdict.Values)
                {
                    if (ll2.id <= ll.id)
                        continue;
                    bool samefam = ll.root == ll2.root;
                    foreach (int ic in ll.lexicon.concepts.Keys)
                    {
                        if (!ll2.lexicon.concepts.ContainsKey(ic))
                            continue;
                        foreach (int iw in ll.lexicon.concepts[ic])
                        {
                            wordclass wc = ll.lexicon.getword(iw);
                            foreach (int iw2 in ll2.lexicon.concepts[ic])
                            {
                                wordclass wc2 = ll2.lexicon.getword(iw2);
                                double dist = Colexification.Levenshtein.WeightedDistance(wc.codedform, wc2.codedform, segmentclass.segdistmatrix) 
                                    / (double)Math.Max(wc.codedform.Length,wc2.codedform.Length);
                                if (Double.IsNaN(dist))
                                    memo("IsNaN! " + wc.codedform + "|||" + wc2.codedform + "|||");
                                if (samefam)
                                    samefamhist.Add(dist);
                                else
                                {
                                    difffamhist.Add(dist);
                                }
                            }
                        }
                    }
                }
            }

            //foreach (string concept in conceptlist)
            //{

            //    //string[] codedforms = new string[q.Count()];
            //    //string[] formfamily = new string[q.Count()];
            //    //int ift = 0;
            //    //foreach (FormTable ft in q)
            //    //{
            //    //    codedforms[ift] = EncodeForm(ft);
            //    //    formfamily[ift] = ft.LanguageTable.Family;
            //    //    ift++;

            //    //}
            //    memo(DateTime.Now.ToString());
            //    //if (CB_weighteddist.Checked)
            //    {
            //        var q = from c in db.CodedFormTable
            //                where c.ConcepticonTable.Concepticon_Gloss == concept
            //                select c;
            //        memo(q.Count() + " forms");

            //        foreach (CodedFormTable cf1 in q)
            //        {
            //            if (String.IsNullOrEmpty(cf1.CodedForm))
            //                continue;
            //            foreach (CodedFormTable cf2 in q)
            //            {
            //                if (cf2.ID <= cf1.ID)
            //                    continue;
            //                if (String.IsNullOrEmpty(cf2.CodedForm))
            //                    continue;
            //                double dist = Colexification.Levenshtein.WeightedDistance(cf1.CodedForm, cf2.CodedForm, segmentclass.segdistmatrix) / (double)(cf1.CodedForm.Length + cf2.CodedForm.Length);
            //                if (Double.IsNaN(dist))
            //                    memo("IsNaN! " + cf1.CodedForm + "|||" + cf2.CodedForm + "|||");
            //                if (cf1.Family == cf2.Family)
            //                    samefamhist.Add(dist);
            //                else
            //                {
            //                    difffamhist.Add(dist);
            //                }
            //            }
            //        }
            //    }
            //    //else
            //    //{
            //    //    var q = from c in db.FormTable
            //    //            where c.ParameterTable.Concepticon_Gloss == concept
            //    //            select c;
            //    //    memo(concept + ": " + q.Count() + " forms");
            //    //    List<string> donelist = new List<string>();

            //    //    foreach (FormTable ft1 in q)
            //    //    {
            //    //        string form1 = cyrilliclist.Contains(ft1.LanguageTable.Name) ? ft1.AlternativeValue : ft1.Form;
            //    //        if (form1 == null)
            //    //            form1 = ft1.Clics_form;
            //    //        //memo(ft1.Form + "\t" + ft1.LanguageTable.Name);

            //    //        foreach (FormTable ft2 in q)
            //    //        {
            //    //            if (ft1.ID == ft2.ID)
            //    //                continue;
            //    //            if (donelist.Contains(ft2.ID))
            //    //                continue;
            //    //            string form2 = cyrilliclist.Contains(ft2.LanguageTable.Name) ? ft2.AlternativeValue : ft2.Form;
            //    //            if (form2 == null)
            //    //                form2 = ft2.Clics_form;
            //    //            double dist = (double)Levenshtein.EditDistance(form1, form2) / (double)(form1.Length + form2.Length);
            //    //            if (ft1.LanguageTable.Family == ft2.LanguageTable.Family)
            //    //                samefamhist.Add(dist);
            //    //            else
            //    //            {
            //    //                difffamhist.Add(dist);
            //    //                if (dist == 0)
            //    //                {
            //    //                    memo(ft1.Form + "\t" + ft1.LanguageTable.Name + "(" + ft1.LanguageTable.Family + ")\t" + ft2.LanguageTable.Name + "(" + ft2.LanguageTable.Family + ")");
            //    //                }
            //    //            }
            //    //        }
            //    //        donelist.Add(ft1.ID);
            //    //    }
            //    //}
            //    memo(DateTime.Now.ToString());
            //}
            memo(samefamhist.GetDHist());
            memo(difffamhist.GetDHist());

        }
    }
}
