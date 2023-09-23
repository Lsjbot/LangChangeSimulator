using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LangChangeSimulator
{
    class segmentclass
    {
        //public static List<segmentclass> segmentlist = new List<segmentclass>();
        public static Dictionary<int, segmentclass> segmentdict = new Dictionary<int, segmentclass>();
        public static double[,] segdistmatrix;
        public static double[,] changeprobmatrix;
        public static int totalseg = 0;
        public static double mindist = 0.2;

        public int segid = 0;
        public string goodseg = "";
        public string fullseg = "";
        public int nseg = 0;
        public string featurestring = "";
        public char soundtype = ' '; //'C' for consonants, 'V' vowels, 'T' tones, '!' clicks
        public double costsum = 0;
        public double probsum = 0;

         //if (s1.featurestring == s2.featurestring)
         //               changeprobmatrix[s1.segid, s2.segid] = 0;
         //           else if ((s2.featurestring == "no") || (s2.featurestring == "0"))
         //               changeprobmatrix[s1.segid, s2.segid] = 0;
         //           else if (s2.featurestring[0] != s1.featurestring[0]) //tones
         //           {
         //               changeprobmatrix[s1.segid, s2.segid] = 0;
         //           }
         //           else if (s2.featurestring[2] != s1.featurestring[2]) //syllabic
         //           {
         //               changeprobmatrix[s1.segid, s2.segid] = 0;
         //           }
         //           else if (s2.featurestring[36] != s1.featurestring[36]) //clicks
         //           {
         //               changeprobmatrix[s1.segid, s2.segid] = 0;
         //           }
         //           else if (s2.fullseg.Contains('/'))
         //           {
         //               changeprobmatrix[s1.segid, s2.segid] = 0;
         //           }
         //           else

        public segmentclass(string line)
        {
            string[] words = line.Split('\t');
            if (words.Length >= 4)
            {
                segid = totalseg;
                totalseg++;
                goodseg = words[0];
                fullseg = words[1];
                nseg = util.tryconvert(words[2]);
                featurestring = words[3];
                if (featurestring.Length >= 37)
                {
                    if (featurestring[0] == '+')
                        soundtype = 'T';
                    else if (featurestring[36] == '-')
                        soundtype = 'C';
                    else if (featurestring[36] == '+')
                        soundtype = 'K';
                    else
                        soundtype = 'V';
                }
            }
            else
                segid = -1;
        }

        public static void init_segments(string folder)
        {
            if (segmentdict.Count > 0)
                return;

            //memo("Read segment file");
            segmentdict = segmentclass.read_segmentfile(folder + "segments.txt");
            //memo("Build segment matrix");
            segmentclass.build_segdistmatrix(segmentdict.Values.ToList());
            segmentclass.build_changeprobmatrix(segmentdict.Values.ToList());
            //memo("Segment matrix done");

        }

        public static Dictionary<int, segmentclass> read_segmentfile(string fn)
        {
            //List<segmentclass> ls = new List<segmentclass>();
            Dictionary<int, segmentclass> ls = new Dictionary<int, segmentclass>();

            using (StreamReader sr = new StreamReader(fn))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    segmentclass sc = new segmentclass(line);
                    ls.Add(sc.segid, sc);
                }
            }
            return ls;
        }

        //public static List<segmentclass> read_segmentfile(string fn)
        //{
        //    List<segmentclass> ls = new List<segmentclass>();

        //    using (StreamReader sr = new StreamReader(fn))
        //    {
        //        while (!sr.EndOfStream)
        //        {
        //            string line = sr.ReadLine();
        //            ls.Add(new segmentclass(line));
        //        }
        //    }
        //    return ls;
        //}

        public static void build_segdistmatrix(List<segmentclass> lseg)
        {
            segdistmatrix = new double[totalseg, totalseg];
            for (int i = 0; i < totalseg; i++)
                for (int j = 0; j < totalseg; j++)
                    segdistmatrix[i, j] = 1;

            foreach (segmentclass s1 in lseg)
            {
                if (s1.segid < 0)
                    continue;

                if ((s1.featurestring == "no") || (s1.featurestring == "0"))
                    continue;
                foreach (segmentclass s2 in lseg)
                {
                    if (s1.featurestring == s2.featurestring)
                        segdistmatrix[s1.segid, s2.segid] = 0;
                    else if ((s2.featurestring == "no") || (s2.featurestring == "0"))
                        segdistmatrix[s1.segid, s2.segid] = 1;
                    else
                    {
                        double k = s1.featurestring.Length;
                        double eachdiff = (1 - mindist) / k;
                        double cost = mindist;
                        for (int i = 0; i < s1.featurestring.Length; i++)
                            if (s1.featurestring[i] != s2.featurestring[i])
                                cost += eachdiff;
                        segdistmatrix[s1.segid, s2.segid] = cost;
                    }
                    s1.costsum += segdistmatrix[s1.segid, s2.segid];
                }

            }
        }

        public static void build_changeprobmatrix(List<segmentclass> lseg)
        {
            changeprobmatrix = new double[totalseg, totalseg];
            for (int i = 0; i < totalseg; i++)
                for (int j = 0; j < totalseg; j++)
                    changeprobmatrix[i, j] = 0;

            foreach (segmentclass s1 in lseg)
            {
                if (s1.segid < 0)
                    continue;

                if ((s1.featurestring == "no") || (s1.featurestring == "0"))
                    continue;
                foreach (segmentclass s2 in lseg)
                {
                    if (s1.featurestring == s2.featurestring)
                        changeprobmatrix[s1.segid, s2.segid] = 0;
                    else if ((s2.featurestring == "no") || (s2.featurestring == "0"))
                        changeprobmatrix[s1.segid, s2.segid] = 0;
                    else if (s2.soundtype != s1.soundtype)
                    {
                        changeprobmatrix[s1.segid, s2.segid] = 0;
                    }
                    else if (s2.fullseg.Contains('/'))
                    {
                        changeprobmatrix[s1.segid, s2.segid] = 0;
                    }
                    else
                    {
                        double k = s1.featurestring.Length;
                        double eachdiff = (1 - mindist) / k;
                        double cost = mindist;
                        for (int i = 0; i < s1.featurestring.Length; i++)
                            if (s1.featurestring[i] != s2.featurestring[i])
                                cost += eachdiff;
                        double freq = Math.Log10(s2.nseg); //more likely to change into common sound
                        changeprobmatrix[s1.segid, s2.segid] = (1 - cost)*0.04*freq*freq; 
                    }
                    s1.probsum += changeprobmatrix[s1.segid, s2.segid];
                }

            }
        }

        public static double segdist(int s1,int s2)
        {
            return segdistmatrix[s1, s2];
        }

        public int get_changecandidate(Random rnd)
        {
            double x = rnd.NextDouble() * (this.probsum);
            double sum = 0;

            for (int i = 0; i < totalseg; i++)
            {
                sum += changeprobmatrix[this.segid, i];
                if (sum > x)
                    return i;
            }
            return -1;
        }

        public static string EncodeForm(FormTable ft)
        {
            return EncodeForm(ft.Segments);
        }
        public static string EncodeForm(string segstring)
        {
            StringBuilder sb = new StringBuilder("");
            string[] segs = segstring.Split();
            foreach (string ss in segs)
            {
                string s = ss.Trim(new char[] { ' ', '?', '+', '_' });
                if (String.IsNullOrEmpty(s))
                    continue;
                var qk = from c in segmentdict.Values where c.fullseg == s select c;
                if (qk.Count() > 0)
                {
                    int k = qk.First().segid;
                    sb.Append((char)k);
                }
            }
            return sb.ToString();

        }

        public static string getfullseg(char k)
        {
            var qk = from c in segmentdict.Values where c.segid == (int)k select c.fullseg;
            return qk.FirstOrDefault();
        }

        public static string DecodeForm(string codedform)
        {
            StringBuilder sb = new StringBuilder("");

            foreach (char k in codedform.ToCharArray())
            {
                //var qk = from c in segmentdict.Values where c.segid == (int)k select c;
                //if (qk.Count() > 0)
                //{
                //    sb.Append(qk.First().fullseg + " ");
                //}
                sb.Append(segmentdict[(int)k].fullseg + " ");
            }
            return sb.ToString();
        }

    }
}