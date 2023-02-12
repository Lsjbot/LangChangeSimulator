using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LangChangeSimulator
{
    public class terrainclass
    {
        public double altitude = 0; //mean altitude in square
        public double variance = 0; //altitude variance whole square
        public double roughness = 0; //variance between adjacent pixels
        //public string slope = "";
        public string terraintype = "flat";
        public double landfraction = 0;
        public Dictionary<char, bool> coastdict = new Dictionary<char, bool>() {
            { 'S', true },
            {'N',true },
            {'E',true },
            {'W',true } }; //coastdict['S'] is true if entire south edge is ocean

        public string type0()
        {
            return this.terraintype.Split('|')[0];
        }
    }
}
