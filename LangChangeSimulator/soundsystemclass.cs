using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangChangeSimulator
{
    [Serializable()]
    public class soundsystemclass
    {

        public static Dictionary<int,Sound> sounddict = new Dictionary<int, Sound>(); //All sounds from 

        public bool phoibleinventory = false;
        public List<int> sounds = new List<int>(); //index to sounddict or to segmentdict

        public soundsystemclass(string source)
        {
            //if "source" is valid iso-code, use inventory from that language
            //if "source" == "random", generate random inventory
            initsounds();

            if (FormLanguageSetup.inventorydict.ContainsKey(source))
            {
                int ipi = FormLanguageSetup.inventorydict[source].Id;
                var q = from c in Form1.dblang.Soundininventory where c.Inventory == ipi select c;

                foreach (Soundininventory sii in q)
                    this.sounds.Add(sii.Sound);
                phoibleinventory = true;
            }
            else if (source == "random")
            {

            }
        }

        public soundsystemclass(lexiconclass lc)
        {
            sounds = lc.segidsused();
            phoibleinventory = false;
        }

        public int soundtype(char stype,Random rnd)
        {
            //get a random sound of type stype ('C', 'V')
            switch (stype)
            {
                case 'C':
                case 'V':
                //case 'T':
                //case 'K':
                    var qv = from c in sounds where segmentclass.segmentdict[c].soundtype == stype select c;
                    int r = rnd.Next(qv.Count());
                    return qv.ElementAt(r);
                    break;
                default:
                    return sounds.First();
                    break;
            }
        }

        public static void initsounds()
        {
            if (sounddict.Count == 0)
                foreach (Sound ss in from c in Form1.dblang.Sound select c)
                    sounddict.Add(ss.Id, ss);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("");
            if (phoibleinventory)
                foreach (int ss in this.sounds)
                    sb.Append(sounddict[ss].IPA + " ");
            else
            {
                foreach (int ss in this.sounds)
                    sb.Append(segmentclass.segmentdict[ss].fullseg + " ");
            }
            return sb.ToString();
        }

        public string compare_segmentlist(List<string> seglist)
        {
            StringBuilder sbmis1 = new StringBuilder("");
            StringBuilder sbmis2 = new StringBuilder("");
            StringBuilder sbgood = new StringBuilder("");
            foreach (string seg in seglist)
            {
                List<int> ql;
                if (phoibleinventory)
                    ql = (from c in this.sounds where sounddict[c].IPA == seg select c).ToList();
                else
                    ql = (from c in this.sounds where segmentclass.segmentdict[c].fullseg == seg select c).ToList();

                if (ql.Count() == 0)
                {
                    sbmis1.Append(seg + " used but missing from inventory\n");
                }
                else
                {
                    sbgood.Append(seg +" ok\n");
                }
            }
            foreach (int ss in sounds)
            {
                if (phoibleinventory)
                {
                    if (!seglist.Contains(sounddict[ss].IPA))
                        sbmis2.Append(sounddict[ss].IPA + " in inventory but not used\n");
                }
                else
                {
                    if (!seglist.Contains(segmentclass.segmentdict[ss].fullseg))
                        sbmis2.Append(sounddict[ss].IPA + " in inventory but not used\n");

                }
            }

            return sbgood.ToString() + sbmis1.ToString() + sbmis2.ToString();
        }
    }
}
