using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LangChangeSimulator
{
    public partial class FormLanguageSetup : Form
    {
        public static Dictionary<string, Language> reallangdict = new Dictionary<string, Language>();
        public static Dictionary<string, Phonemeinventory> inventorydict = new Dictionary<string, Phonemeinventory>();
        public FormLanguageSetup()
        {
            InitializeComponent();

           
            
            foreach (string iso in fulldata_languages())
            {
                LB_lang.Items.Add(iso + "-" + reallangdict[iso].Name);
            }

            //using (StreamWriter sw = new StreamWriter(Form1.folder + @"fulldata_languages.txt"))
            //{
            //    for (int i = 0; i < LB_lang.Items.Count; i++)
            //        sw.WriteLine(LB_lang.Items[i].ToString());
            //}

                List<string> langs = parameterclass.p.get("languageset").Split('|').ToList();
            for (int i=0;i<LB_lang.Items.Count;i++)
            {
                if (langs.Contains(LB_lang.Items[i].ToString().Split('-')[0]))
                    LB_lang.SetItemChecked(i, true);
                else
                    LB_lang.SetItemChecked(i, false);
            }

            TB_nlang.Text = parameterclass.p.get("nlanguage");

            switch (parameterclass.p.get("languageorigin"))
            {
                case "scratch":
                    RB_scratch.Checked = true;
                    break;
                case "random":
                    RB_random.Checked = true;
                    break;
                case "checked":
                    RB_checked.Checked = true;
                    break;

            }
            switch (parameterclass.p.get("conceptset"))
            {
                case "swadesh100":
                    RB_swadesh100.Checked = true;
                    break;
                case "swadesh200":
                    RB_swadesh200.Checked = true;
                    break;
                case "clics3":
                    RB_clics3.Checked = true;
                    break;

            }

        }

        List<string> fulldata_languages()
        {
            if (reallangdict.Count > 0)
                return reallangdict.Keys.ToList();

            List<string> ls = new List<string>();
            var q = from c in Form1.dbclics3.LanguageTable2 select c;

            List<string> goodlang = new List<string>();
            using (StreamReader sr = new StreamReader(Form1.folder + @"fulldata_languages.txt"))
            {
                while (!sr.EndOfStream)
                {
                    goodlang.Add(sr.ReadLine().Split('-')[0]);
                }
            }

                int nseg = 0;
            int nnoseg = 0;
            int nboth = 0;
            foreach (LanguageTable2 lt in q)
            {
                string iso = lt.Iso;
                if (ls.Contains(iso))
                    continue;
                if (!goodlang.Contains(iso))
                    continue;

                //var qft = from c in Form1.dbclics3.CodedFormTable where c.Language == lt.ID select c;
                //int ns = (from c in qft where c.CodedForm.Length>1 select c).Count();
                //int nns = qft.Count() - ns;
                //if (ns > 0)
                //{
                //    if (nns > 0)
                //        nboth++;
                //    else
                //        nseg++;
                //}
                //else if (nns > 0)
                //{
                //    nnoseg++;
                //}
                //else
                //    memo("No forms for " + iso);
                //if (ns < 50)
                //    continue;

                Language ll = (from c in Form1.dblang.Language where c.Iso == iso select c).FirstOrDefault();
                if (ll == null)
                    continue;
                Phonemeinventory pp = (from c in Form1.dblang.Phonemeinventory where c.Language == iso select c).FirstOrDefault();
                if (pp == null)
                    continue;
                reallangdict.Add(iso, ll);
                inventorydict.Add(iso, pp);
                ls.Add(iso);
            }

            memo("nseg = " + nseg);
            memo("nnoseg = " + nnoseg);
            memo("nboth = " + nboth);
            return ls;
        }

        public void memo(string s)
        {
            richTextBox1.AppendText(s + "\n");
            richTextBox1.ScrollToCaret();
        }

        private void FormLanguageSetup_Load(object sender, EventArgs e)
        {

        }

        private void makebutton_Click(object sender, EventArgs e)
        {
            memo("Reading segments");
            segmentclass.init_segments(Form1.folder); // (@"G:\clics\");
            memo("Segments done");

            int nlang = util.tryconvert(TB_nlang.Text);
            if (nlang < 1)
                return;
            parameterclass.p.put("nlanguage", nlang.ToString());

            List<int> conceptlist = new List<int>();
            if (RB_swadesh100.Checked)
            {
                conceptlist = swadeshclass.conceptcodelist("swadesh100");
                parameterclass.p.put("conceptset", "swadesh100");
            }
            else if (RB_swadesh200.Checked)
            {
                conceptlist = swadeshclass.conceptcodelist("swadesh200");
                parameterclass.p.put("conceptset", "swadesh200");
            }
            else if (RB_clics3.Checked)
            {
                conceptlist = swadeshclass.conceptcodelist("clics3");
                parameterclass.p.put("conceptset", "clics3");
            }
            else if (RBnumbers.Checked)
            {
                conceptlist = swadeshclass.conceptcodelist("numbersonly");
                parameterclass.p.put("conceptset", "numbersonly");
            }



            if (RB_scratch.Checked)
            {
                for (int i = 0; i < nlang; i++)
                {
                    addlanguage("random",conceptlist);
                }
                parameterclass.p.put("languageorigin", "scratch");
                parameterclass.p.put("languageset", "");
            }
            else if (RB_random.Checked)
            {
                string langlist = "";
                Random rnd = new Random();
                int nmax = LB_lang.Items.Count;
                for (int i=0;i<nlang;i++)
                {
                    string iso = "";
                    bool used = false;
                    do
                    {
                        int jitem = rnd.Next(nmax);
                        iso = LB_lang.Items[jitem].ToString().Split('-')[0];
                        used = langtreeclass.treedict.ContainsKey(iso);
                    }
                    while (used);
                    addlanguage(iso, conceptlist);
                    langlist += (iso + "|");
                }
                parameterclass.p.put("languageorigin", "random");
                parameterclass.p.put("languageset", langlist.Trim('|'));
            }
            else if (RB_checked.Checked)
            {
                string langlist = "";
                foreach (string s in LB_lang.CheckedItems)
                {
                    string iso = s.Split('-')[0];
                    addlanguage(iso,conceptlist);
                    langlist += (iso + "|");
                }
                parameterclass.p.put("languageorigin", "checked");
                parameterclass.p.put("languageset", langlist.Trim('|'));

            }

            //defaultparams.pdict.Add("languageorigin", "checked");//checked, random, scratch
            //defaultparams.pdict.Add("languageset", "swe|eng");
            //defaultparams.pdict.Add("conceptset", "swadesh100");//swadesh100, swadesh200, clics3


            this.Close();
        }

        private void addlanguage(string source,List<int> conceptlist)
        {
            languageclass lc = new languageclass(source,conceptlist);
            int ilc = lc.id;
        }
    }
}
