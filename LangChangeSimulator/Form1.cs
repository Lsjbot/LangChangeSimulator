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
    public partial class Form1 : Form
    {
        public static LangdbV1 dblang;
        public static CLICS3 dbclics3;
        static string connectionstringlang = "Data Source=DESKTOP-JOB29A9;Initial Catalog=\"langdb v1\";Integrated Security=True";
        static string connectionstringclics3 = "Data Source=DESKTOP-JOB29A9;Initial Catalog=\"CLICS3\";Integrated Security=True";
        public static string folder = @"G:\Ling\LangChangeSimulator\";
        public FormMap fm = new FormMap();
        

        public Form1()
        {
            InitializeComponent();
            dblang = new LangdbV1(connectionstringlang);
            dbclics3 = new CLICS3(connectionstringclics3);

            parameterclass.InitDefaults();
            swadeshclass.fillconceptdicts();
            techclass.init_tech(folder);
            walsclass.initWALS();
            gramfeatureclass.fill_gramfeatures();
        }

        private void GeographyButton_Click(object sender, EventArgs e)
        {
            fm.Show();
            FormGeography fg = new FormGeography(fm);
            fg.Show();
            LanguageSetupButton.Enabled = true;
        }

        private void LanguageSetupButton_Click(object sender, EventArgs e)
        {
            FormLanguageSetup fl = new FormLanguageSetup();
            fl.ShowDialog();
            LanguageViewButton.Enabled = true;
        }

        public void memo(string s)
        {
            richTextBox1.AppendText(s + "\n");
            richTextBox1.ScrollToCaret();
        }

        private void LanguageViewButton_Click(object sender, EventArgs e)
        {
            FormShowLanguage fs = new FormShowLanguage();
            fs.Show();
        }

        private void SimulationButton_Click(object sender, EventArgs e)
        {
            FormSimulation fs = new FormSimulation();
            fs.Show();
        }

        private void quitbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Parameterbutton_Click(object sender, EventArgs e)
        {
            parameterclass.p.load();
        }
    }
}
