using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangChangeSimulator
{
    public class areaclass
    {
        Dictionary<int, int> sounddict = new Dictionary<int, int>(); //how many of each sound
        Dictionary<int, Dictionary<int, int>> conceptdict = new Dictionary<int, Dictionary<int, int>>();
        int nlang = 0;
        List<languageclass> langlist = new List<languageclass>();

        public void add(languageclass ll)
        {
            add(ll.inventory);
            add(ll.lexicon);
            langlist.Add(ll);
            nlang++;
        }
        public void add(soundsystemclass ss)
        {
            foreach (int i in ss.sounds)
            {
                add(i);
            }
        }

        public void add(lexiconclass lc)
        {
            foreach (wordclass w in lc.words)
            {
                add(w);
            }
        }

        public void add(int isound)
        {
            if (sounddict.ContainsKey(isound))
                sounddict[isound]++;
            else
                sounddict.Add(isound, 1);

        }

        public void add(areaclass aa2, int factor)
        {
            foreach (int i in aa2.sounddict.Keys)
            {
                if (this.sounddict.ContainsKey(i))
                    this.sounddict[i] += factor*aa2.sounddict[i];
                else
                    this.sounddict.Add(i, factor*aa2.sounddict[i]);
            }
            foreach (int cc in aa2.conceptdict.Keys)
            {
                if (!this.conceptdict.ContainsKey(cc))
                    this.conceptdict.Add(cc, new Dictionary<int, int>());
                foreach (int w in aa2.conceptdict[cc].Keys)
                {
                    if (this.conceptdict[cc].ContainsKey(w))
                        this.conceptdict[cc][w] += factor*aa2.conceptdict[cc][w];
                    else
                        this.conceptdict[cc].Add(w, factor*aa2.conceptdict[cc][w]);
                }
            }
            foreach (languageclass lc in aa2.langlist)
                this.langlist.Add(lc);
            this.nlang += factor * aa2.nlang;
        }

        public void add(wordclass w)
        {
            foreach (int cc in w.concepts)
            {
                add(w, cc);
            }
        }

        public void add(wordclass w,int cc) //add one sense to a word
        {
            if (!conceptdict.ContainsKey(cc))
                conceptdict.Add(cc, new Dictionary<int, int>());
            if (conceptdict[cc].ContainsKey(w.root))
                conceptdict[cc][w.root]++;
            else
                conceptdict[cc].Add(w.root, 1);
        }

        public void remove(wordclass w)
        {
            foreach (int cc in w.concepts)
            {
                remove(w, cc);
            }
        }

        public void remove(wordclass w, int cc)
        {
            if (conceptdict[cc][w.root] == 1)
                conceptdict[cc].Remove(w.root);
            else
                conceptdict[cc][w.root]--;
        }

        public void remove(int isound)
        {
            if (sounddict[isound] == 1)
                sounddict.Remove(isound);
            else
                sounddict[isound]--;
        }

        public void remove(lexiconclass lc)
        {
            foreach (wordclass w in lc.words)
            {
                remove(w);
            }
        }

        public void remove(soundsystemclass ss)
        {
            foreach (int i in ss.sounds)
            {
                remove(i);
            }
        }

        public void remove(languageclass ll)
        {
            remove(ll.lexicon);
            remove(ll.inventory);
            langlist.Remove(ll);
            nlang--;
        }

        public static areaclass sumregion(int ilat, int ilon, int radius, int centerbonus)
        {
            areaclass aa = new areaclass();
            for (int u=-radius;u<=radius;u++)
                for (int v=-radius;v<=radius;v++)
                {
                    if (u==0 && v==0)
                    {
                        aa.add(mapgridclass.map[ilat, ilon].aa, centerbonus);
                    }
                    else if (mapgridclass.inmap(ilat + u, ilon + v) && mapgridclass.basemap[ilat + u, ilon + v] != null)
                    {
                        aa.add(mapgridclass.map[ilat+u, ilon+v].aa, 1);
                    }
                }
            return aa;
        }

        public static int nlangregion(int ilat, int ilon, int radius, int centerbonus)
        {
            int nl = 0;
            for (int u = -radius; u <= radius; u++)
                for (int v = -radius; v <= radius; v++)
                {
                    if (u == 0 && v == 0)
                    {
                        nl += mapgridclass.map[ilat, ilon].aa.nlang * centerbonus;
                    }
                    else if (mapgridclass.inmap(ilat+u,ilon+v) && mapgridclass.basemap[ilat+u, ilon+v] != null)
                    {
                        nl += mapgridclass.map[ilat + u, ilon + v].aa.nlang;
                    }
                }


            return nl;
        }

        public wordclass findmajorityword(languageclass lc)
        {
            wordclass wc = null;

            foreach (int cc in conceptdict.Keys)
            {
                if (!lc.lexicon.concepts.ContainsKey(cc))
                {
                    foreach (int root in conceptdict[cc].Keys)
                    {
                        if (conceptdict[cc][root] > nlang/2)
                        {
                            foreach (languageclass la in langlist)
                            {
                                if (la.speakers <= 0)
                                {
                                    continue;
                                }
                                if (la.lexicon.concepts.ContainsKey(cc))
                                {
                                    wordclass wa = (from c in la.lexicon.words where c.root == root select c).FirstOrDefault();
                                    if (wa != null)
                                    {
                                        
                                        wc = new wordclass(wa, lc.id);
                                        return wc;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return wc;
        }
        public int findmajoritysound(languageclass lc)
        {
            wordclass wc = null;
            int ccmax = -1;
            int nlangmax = -1;

            foreach (int cc in sounddict.Keys)
            {
                if (!lc.inventory.sounds.Contains(cc))
                {
                    if (sounddict[cc] > nlangmax)
                    {
                        nlangmax = sounddict[cc];
                        ccmax = cc;
                    }
                }
            }

            if (nlangmax > nlang/2)
            {
                return ccmax;
            }

            return -1;
        }
    }
}
