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
    public partial class FormShowLanguage : Form
    {
        public FormShowLanguage()
        {
            InitializeComponent();
            foreach (int ilc in languageclass.langdict.Keys)
            {
                if (languageclass.langdict[ilc].speakers > 0)
                    LB_lang.Items.Add(ilc + "| " + languageclass.langdict[ilc].source);
            }
            memo(languageclass.langdict.Count + " languages");

            LB_stock.Items.Add("ALL");
            foreach (string src in langtreeclass.treedict.Keys)
            {
                LB_stock.Items.Add(src);
            }
        }

        public void memo(string s)
        {
            richTextBox1.AppendText(s + "\n");
            richTextBox1.ScrollToCaret();
        }



        private void Closebutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LB_lang_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ilc = util.tryconvert(LB_lang.SelectedItem.ToString().Split('|')[0]);
            languageclass lc = languageclass.langdict[ilc];
            memo("=============================");
            memo("=   " + ilc);
            memo("Source: "+lc.source);
            memo("Lat,lon: " + mapgridclass.basemap[lc.ilat, lc.ilon].lat + " " + mapgridclass.basemap[lc.ilat, lc.ilon].lon);
            memo(lc.inventory.ToString());

            foreach (wordclass w in lc.lexicon.words)
            {
                StringBuilder sb = new StringBuilder(w.getform() + "\t");
                foreach (string ss in wordclass.getmeanings(w))
                    sb.Append(ss + "\t");
                memo(sb.ToString());
            }

            foreach (int ic in lc.lexicon.concepts.Keys)
            {
                StringBuilder sb = new StringBuilder(swadeshclass.codeconceptdict[ic] + "\t");
                foreach (int w in lc.lexicon.concepts[ic])
                    sb.Append(lc.lexicon.getword(w).getform() + "\t");
                memo(sb.ToString());
            }

            List<string> seglist = lc.lexicon.segmentsused();
            //memo(lc.inventory.compare_segmentlist(seglist));
        }

        private void LB_stock_SelectedIndexChanged(object sender, EventArgs e)
        {
            string src = LB_stock.SelectedItem.ToString();
            memo(src);
            if (!langtreeclass.treedict.ContainsKey(src))
            {
                memo("Not found!");
                return;
            }

            memo(langtreeclass.treedict[src].ToString());
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void swadeshbutton_Click(object sender, EventArgs e)
        {
            string src = "";
            if (LB_stock.SelectedItem != null)
                src = LB_stock.SelectedItem.ToString();
            if (src == "ALL")
                src = "";
            memo(src);
            if (!String.IsNullOrEmpty(src))
                if (!langtreeclass.treedict.ContainsKey(src))
                {
                    memo("Not found!");
                    return;
                }

            string fn = util.unusedfilename(Form1.folder+@"output\swadesh-"+src+DateTime.Now.ToShortDateString()+".txt");

            memo("Writing to " + fn);
            swadeshclass.write_swadeshtable(fn, src);
            memo("Done");
        }
    }

}
