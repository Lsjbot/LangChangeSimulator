using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LangChangeSimulator
{
    public partial class FormGeography : Form
    {
        private FormMap fm;
        public FormGeography(FormMap fmpar)
        {
            InitializeComponent();
            fm = fmpar;
            LB_region.Items.Add("World");
            LB_region.Items.Add("Australia");
            LB_region.Items.Add("Africa");
            LB_region.Items.Add("New Guinea");
            LB_region.Items.Add("South America");
            foreach (string s in LB_region.Items)
                LB_startregion.Items.Add(s);
        }

        private void BasemapButton_Click(object sender, EventArgs e)
        {
            string region = "World";
            if (LB_region.SelectedItem != null)
            {
                region = LB_region.SelectedItem.ToString();
            }
            string startregion = "World";
            if (LB_startregion.SelectedItem != null)
            {
                startregion = LB_startregion.SelectedItem.ToString();
            }
            mapgridclass.read_basemap(region,startregion);
            memo("Done reading mapfile");
            if (mapgridclass.km < 0)
                return;
            if (CB_fillmissing.Checked)
                mapgridclass.fill_missing_climate();
            drawbasemap();
            mapgridclass.make_mainmap();
        }

        public void drawbasemap()
        {
            int imax = mapgridclass.basemap.GetLength(0);
            int jmax = mapgridclass.basemap.GetLength(1);
            int pitch = fm.pictureBox1.Width / jmax;
            memo("pitch = " + pitch);
            memo("imax = " + jmax);
            memo("pb.Width = " + fm.pictureBox1.Width);
            Bitmap b = new Bitmap(pitch * jmax, pitch * imax);
            Graphics g = Graphics.FromImage(b);
            g.FillRectangle(new SolidBrush(Color.Blue), 0, 0, pitch * jmax, pitch * imax);

            if (RB_climate.Checked)
            {
                Dictionary<string, Color> colordict = new Dictionary<string, Color>();
                colordict.Add("", Color.Gray);
                colordict.Add("arctic", Color.White);
                colordict.Add("boreal", Color.DarkGreen);
                colordict.Add("continental", Color.DarkOliveGreen);
                colordict.Add("continental subarctic", Color.AntiqueWhite);
                colordict.Add("desert", Color.SandyBrown);
                colordict.Add("desert cold", Color.Tan);
                colordict.Add("desert hot", Color.Yellow);
                colordict.Add("hemiboreal", Color.ForestGreen);
                colordict.Add("humid subtropical", Color.Green);
                colordict.Add("mediterranean", Color.Olive);
                colordict.Add("monsoon", Color.DarkTurquoise);
                colordict.Add("oceanic", Color.LightGreen);
                colordict.Add("rainforest", Color.MediumSeaGreen);
                colordict.Add("savanna", Color.LimeGreen);
                colordict.Add("steppe", Color.Lime);
                colordict.Add("steppe cold", Color.LightYellow);
                colordict.Add("steppe hot", Color.GreenYellow);
                colordict.Add("temperate", Color.LawnGreen);
                colordict.Add("tropical", Color.LightBlue);
                colordict.Add("tropical highland", Color.Maroon);
                colordict.Add("tundra", Color.LightGray);
                colordict.Add("unknown", Color.Gray);
                colordict.Add("river", Color.BlueViolet);

                foreach (mapgridclass mg in mapgridclass.basemap)
                {
                    if (mg != null)
                    {
                        string clim = mg.climatestring.Split('|')[0];
                        if (CB_river.Checked && !String.IsNullOrEmpty(mg.river))
                            clim = "river";
                        g.FillRectangle(new SolidBrush(colordict[clim]), mg.ilon * pitch, (imax - mg.ilat) * pitch, pitch, pitch);
                    }
                }
            }
            else if (RB_terrain.Checked)
            {
                Dictionary<string, Color> colordict = new Dictionary<string, Color>();
                colordict.Add("flat", Color.Yellow);
                colordict.Add("flat high", Color.Orange);
                colordict.Add("very flat high", Color.OrangeRed);
                colordict.Add("high-mountains", Color.Purple);
                colordict.Add("hilly", Color.Green);
                colordict.Add("hilly high", Color.DarkOliveGreen);
                colordict.Add("low-mountains", Color.SandyBrown);
                colordict.Add("medium-mountains", Color.Brown);
                colordict.Add("somewhat hilly", Color.Lime);
                colordict.Add("somewhat hilly high", Color.Olive);
                colordict.Add("very flat", Color.LightYellow);
                foreach (mapgridclass mg in mapgridclass.basemap)
                {
                    if (mg != null)
                    {
                        string clim = mg.terrain.terraintype.Split('|')[0];
                        g.FillRectangle(new SolidBrush(colordict[clim]), mg.ilon * pitch, (imax - mg.ilat) * pitch, pitch, pitch);
                    }
                }

            }
            else if (RB_landcover.Checked)
            {
                Dictionary<int, Color> colordict = new Dictionary<int, Color>();
                colordict.Add(-1, Color.Black); //missing data
                colordict.Add(2, Color.DarkGreen); //evergreen needleleaf forest.
                colordict.Add(3, Color.Green); //evergreen broadleaf forest.
                colordict.Add(4, Color.LightGreen); //deciduous needleleaf forest.
                colordict.Add(5, Color.LawnGreen); //deciduous broadleaf forest.
                colordict.Add(6, Color.ForestGreen); //mixed forest.
                colordict.Add(7, Color.DarkOliveGreen); //closed shrublands.
                colordict.Add(8, Color.Olive); //open shrublands.
                colordict.Add(9, Color.Lime); //woody savanna.
                colordict.Add(10, Color.Yellow); //savanna.
                colordict.Add(11, Color.GreenYellow); //grasslands.
                colordict.Add(12, Color.Brown); //permanent wetlands.
                colordict.Add(13, Color.Violet); //croplands.
                colordict.Add(14, Color.Red); //urban and built-up.
                colordict.Add(15, Color.MediumPurple); //cropland/natural vegetation.
                colordict.Add(16, Color.White); //permanent snow and ice.
                colordict.Add(17, Color.Gray); //barren or sparsely vegetated.
                foreach (mapgridclass mg in mapgridclass.basemap)
                {
                    if (mg != null)
                    {
                        int lc = mg.climate.landcover;
                        g.FillRectangle(new SolidBrush(colordict[lc]), mg.ilon * pitch, (imax - mg.ilat) * pitch, pitch, pitch);
                    }
                }

            }
            else if (RB_variance.Checked || RB_roughness.Checked)
            {
                Dictionary<int, Color> colordict = new Dictionary<int, Color>();
                colordict.Add(0, Color.Black);
                colordict.Add(1, Color.Violet);
                colordict.Add(2, Color.Lavender);
                colordict.Add(3, Color.Turquoise);
                colordict.Add(4, Color.DarkGreen);
                colordict.Add(5, Color.Green);
                colordict.Add(6, Color.LimeGreen);
                colordict.Add(7, Color.Yellow);
                colordict.Add(8, Color.Orange);
                colordict.Add(9, Color.OrangeRed);
                colordict.Add(10, Color.Red);
                foreach (mapgridclass mg in mapgridclass.basemap)
                {
                    if (mg != null)
                    {
                        int k = 0;
                        if (RB_roughness.Checked)
                        {
                            k = (int)Math.Round(Math.Log10(mg.terrain.roughness)/0.7);
                        }
                        else if (RB_variance.Checked)
                        {
                            k = (int)Math.Round(Math.Log10(mg.terrain.variance) / 0.7);
                        }
                        if (k < 0)
                            k = 0;
                        g.FillRectangle(new SolidBrush(colordict[k]), mg.ilon * pitch, (imax - mg.ilat) * pitch, pitch, pitch);
                    }
                }
            }
            g.Flush();
            fm.pictureBox1.Image = b;


        }

        public void memo(string s)
        {
            richTextBox1.AppendText(s + "\n");
            richTextBox1.ScrollToCaret();
        }

        private void MapstatisticsButton_Click(object sender, EventArgs e)
        {
            hbookclass coverhist = new hbookclass("Landcover");
            hbookclass climatehist = new hbookclass("Climate");
            hbookclass temphist = new hbookclass("Temperature");
            hbookclass tempminhist = new hbookclass("Coldest Temperature");
            hbookclass rainhist = new hbookclass("Rainfall");
            hbookclass koppenhist = new hbookclass("Koppen");
            hbookclass terrainhist = new hbookclass("Terrain");
            hbookclass badclimatelathist = new hbookclass("Bad by latitude");
            hbookclass badcoverlathist = new hbookclass("Bad by latitude");
            badclimatelathist.SetBins(-60, 70, 26);
            badcoverlathist.SetBins(-60, 70, 26);
            hbookclass variancehist = new hbookclass("Altitude variance");
            variancehist.SetBins(0, 10, 40);
            hbookclass roughnesshist = new hbookclass("Altitude roughness");
            roughnesshist.SetBins(0, 20, 40);

            foreach (mapgridclass mg in mapgridclass.basemap)
            {
                if (mg != null)
                {
                    coverhist.Add(mg.climate.landcover);
                    climatehist.Add(mg.climatestring);
                    temphist.Add(mg.climate.temp_average);
                    tempminhist.Add(mg.climate.temp_min);
                    rainhist.Add(mg.climate.rainfall);
                    koppenhist.Add(mg.climate.koppen);
                    terrainhist.Add(mg.terrain.terraintype.Split('|')[0]);

                    if (String.IsNullOrEmpty(mg.climatestring))
                        badclimatelathist.Add(mg.lat);
                    if (mg.climate.landcover < 0)
                        badcoverlathist.Add(mg.lat);

                    variancehist.Add(Math.Log10(mg.terrain.variance));
                    roughnesshist.Add(Math.Log10(mg.terrain.roughness));
                }
            }

            memo(coverhist.GetIHist());
            memo(climatehist.GetSHist());
            memo(temphist.GetIHist());
            memo(tempminhist.GetIHist());
            memo(rainhist.GetIHist());
            memo(koppenhist.GetIHist());
            memo(terrainhist.GetSHist());
            memo(badclimatelathist.GetDHist());
            memo(badcoverlathist.GetDHist());
            memo(variancehist.GetDHist());
            memo(roughnesshist.GetDHist());
        }

        private void RB_climate_CheckedChanged(object sender, EventArgs e)
        {
            drawbasemap();
        }

        private void RB_terrain_CheckedChanged(object sender, EventArgs e)
        {
            drawbasemap();
        }

        private void RB_landcover_CheckedChanged(object sender, EventArgs e)
        {
            drawbasemap();
        }

        private void RB_variance_CheckedChanged(object sender, EventArgs e)
        {
            drawbasemap();
        }

        private void RB_roughness_CheckedChanged(object sender, EventArgs e)
        {
            drawbasemap();
        }

        private void FormGeography_ResizeEnd(object sender, EventArgs e)
        {
            //pictureBox1.Width = this.Width - 30;
        }
    }
}
