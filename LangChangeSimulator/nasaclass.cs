using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LangChangeSimulator
{
    public class nasaclass
    {
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

    }
}
