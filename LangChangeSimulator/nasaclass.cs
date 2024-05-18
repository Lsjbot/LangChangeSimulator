using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LangChangeSimulator
{
    public class nasaclass
    {
        public static Dictionary<int, double> historicaltempdict = new Dictionary<int, double>();
        public static double tempoffset = 0;
                
        public int landcover = -1; //Landcover code 1-17 http://eospso.nasa.gov/sites/default/files/atbd/atbd_mod12.pdf
        public int popdensity = -1; //people per square km
        public int temp_average = -999; //average across months and day-night
        public int temp_max = -999; //temp of hottest month
        public int month_max = -999; //hottest month (1-12)
        public int temp_min = -999; //temp of coldest month
        public int month_min = -999; //coldest month
        public int temp_daynight = -999; //average difference between day and night
        public int rainfall = -999; //mm per year
        public int rain_max = -999; //rain wettest month
        public int rain_month_max = -999; //wettest month (1-12)
        public int rain_min = 99999; //rain driest month
        public int rain_month_min = -999; //driest month
        public double rainfall_double = 0; //mm per year
        public int koppen = -1;
        public int[] month_temp_day = new int[13] { -999, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999 };
        public int[] month_temp_night = new int[13] { -999, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999 };
        public int[] month_rain = new int[13] { -999, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999 };

        public static void read_historicaltemperatures(string fn)
        {
            using (StreamReader sr = new StreamReader(fn))
            {
                sr.ReadLine();
                sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split('\t');
                    int time = -util.tryconvert(words[0]) * 1000;
                    double temp = util.tryconvertdouble(words[1]);
                    historicaltempdict.Add(time, temp);
                }
            }
            historicaltempdict.Add(0, 0);
        }

        public static void set_tempoffset(int year)
        {
            if (year > 0)
                tempoffset = 0;

            int yk = 1000 * (year / 1000);
            int yk2 = yk + 1000;
            double frac = 0.001*(year % 1000);
            tempoffset = historicaltempdict[yk] + frac * (historicaltempdict[yk2] - historicaltempdict[yk]);
            
        }
    }
}
