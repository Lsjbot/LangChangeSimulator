using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace LangChangeSimulator
{
    public partial class FormMap : Form
    {
        private Dictionary<int, Color> colordict = new Dictionary<int, Color>();
        private string prefix = "";
        public static string imagefolder;// = @"G:\Ling\LangChangeSimulator\output\images\";
        public FormMap()
        {
            InitializeComponent();
            colordict.Add(0, Color.Red);
            colordict.Add(1, Color.Violet);
            colordict.Add(2, Color.Yellow);
            colordict.Add(3, Color.Green);
            colordict.Add(4, Color.Orange);
            colordict.Add(5, Color.Black);
            colordict.Add(6, Color.Purple);
            colordict.Add(7, Color.LightBlue);
            colordict.Add(8, Color.Chartreuse);
            colordict.Add(9, Color.Chocolate);
            colordict.Add(10, Color.Gray);
            colordict.Add(11, Color.GreenYellow);
            colordict.Add(12, Color.HotPink);
            colordict.Add(13, Color.Brown);
            colordict.Add(14, Color.Turquoise);
            colordict.Add(15, Color.Olive);
            colordict.Add(16, Color.Pink);
            colordict.Add(17, Color.Lavender);
            colordict.Add(18, Color.Magenta);
            colordict.Add(19, Color.Maroon);
            colordict.Add(20, Color.Beige);
            colordict.Add(21, Color.Crimson);
            colordict.Add(22, Color.DarkGreen);
            colordict.Add(23, Color.Gold);
            colordict.Add(24, Color.AliceBlue);
            colordict.Add(25, Color.Bisque);
            colordict.Add(26, Color.BlueViolet);
            colordict.Add(27, Color.DarkSalmon);
            colordict.Add(28, Color.Salmon);
            colordict.Add(29, Color.Fuchsia);
            colordict.Add(30, Color.Goldenrod);
            colordict.Add(31, Color.LemonChiffon);
            colordict.Add(32, Color.MediumSeaGreen);
            colordict.Add(33, Color.PapayaWhip);
            colordict.Add(34, Color.PeachPuff);
            colordict.Add(35, Color.PowderBlue);
            colordict.Add(36, Color.RosyBrown);
            colordict.Add(37, Color.RoyalBlue);
            colordict.Add(38, Color.SandyBrown);
            colordict.Add(39, Color.Silver);
            colordict.Add(40, Color.Wheat);

            prefix = DateTime.Now.ToShortTimeString().Replace(":",".");

            imagefolder = Form1.folder + @"output\images\" + FormGeography.region + " " + FormSimulation.maxtime + " yrs " + DateTime.Now.ToShortDateString() + @"\";
            if (!Directory.Exists(imagefolder))
                Directory.CreateDirectory(imagefolder);
        }

        private void FormMap_ResizeEnd(object sender, EventArgs e)
        {
            pictureBox1.Width = this.Width - 30;
        }

        public void languagemap()
        {
            languagemap("byfamily",0,false);
        }

        public static Dictionary<string, Color> langcolor = new Dictionary<string, Color>();
        public static Dictionary<string, Color> subcolor = new Dictionary<string, Color>() {
            {"hunter-gatherer",Color.HotPink },
            {"horticulture",Color.Orange },
            {"agriculture", Color.Green },
            {"herding", Color.Yellow }
        };

        public void languagemap(string maptype, int year, bool savescreenshot)
        {
            //Console.WriteLine("Drawing language map");


            int imax = mapgridclass.basemap.GetLength(0);
            int jmax = mapgridclass.basemap.GetLength(1);
            int pitch = pictureBox1.Width / jmax;
            Bitmap b = new Bitmap(pitch * jmax, pitch * imax);
            Graphics g = Graphics.FromImage(b);
            g.FillRectangle(new SolidBrush(Color.Blue), 0, 0, pitch * jmax, pitch * imax);
            int icolor = langcolor.Count;
            if (maptype == "bysubsistence")
                icolor = subcolor.Count;
            foreach (cellclass mg in mapgridclass.map)
            {
                if (mg != null)
                {
                    Color c = Color.White;
                    int max = 0;
                    string srcmax = "";
                    string submax = "";
                    int ilmax = -1;
                    if (maptype == "bydensity")
                    {
                        int mcol = 240;
                        double maxpop = 10000;
                        if (mg.population > 0)
                        {
                            int popcol = (int)(mcol * mg.population / maxpop);
                            if (mcol-popcol < 0)
                                popcol = mcol;
                            c = Color.FromArgb(255, mcol - popcol, mcol - popcol, mcol - popcol);
                        }
                    }
                    else
                    {
                        if (mg.languages.Count > 0)
                        {
                            foreach (int ilang in mg.languages)
                            {
                                if (languageclass.langdict[ilang].speakers > max)
                                {
                                    max = languageclass.langdict[ilang].speakers;
                                    srcmax = languageclass.langdict[ilang].source;
                                    submax = languageclass.langdict[ilang].culture.subsistence.name;
                                    ilmax = ilang;
                                }
                            }
                            if (ilmax >= 0)
                            {
                                if (maptype == "bysubsistence")
                                {
                                    if (!subcolor.ContainsKey(submax))
                                    {
                                        subcolor.Add(submax, colordict[icolor]);
                                        icolor++;
                                        if (icolor >= colordict.Count)
                                            icolor = 0;
                                    }
                                    c = subcolor[submax];
                                }
                                else
                                {
                                    string src = srcmax;
                                    if (!langcolor.ContainsKey(src))
                                    {
                                        langcolor.Add(src, colordict[icolor]);
                                        icolor++;
                                        if (icolor >= colordict.Count)
                                            icolor = 0;
                                    }
                                    c = langcolor[src];
                                }
                            }
                        }
                    }
                    g.FillRectangle(new SolidBrush(c), mg.mg.ilon * pitch, (imax - mg.mg.ilat) * pitch, pitch, pitch);
                }
            }
            g.Flush();
            pictureBox1.Image = b;
            string fn = prefix + "-" + maptype + year.ToString("D5") + ".jpg";
            if (savescreenshot)
                pictureBox1.Image.Save(imagefolder + fn, ImageFormat.Jpeg);
            //this.Invalidate();
            this.Refresh();
            
        }
    }
}
