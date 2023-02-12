namespace LangChangeSimulator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GeographyButton = new System.Windows.Forms.Button();
            this.LanguageSetupButton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.LanguageViewButton = new System.Windows.Forms.Button();
            this.SimulationButton = new System.Windows.Forms.Button();
            this.quitbutton = new System.Windows.Forms.Button();
            this.Parameterbutton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // GeographyButton
            // 
            this.GeographyButton.Location = new System.Drawing.Point(655, 157);
            this.GeographyButton.Name = "GeographyButton";
            this.GeographyButton.Size = new System.Drawing.Size(118, 43);
            this.GeographyButton.TabIndex = 0;
            this.GeographyButton.Text = "Geography";
            this.GeographyButton.UseVisualStyleBackColor = true;
            this.GeographyButton.Click += new System.EventHandler(this.GeographyButton_Click);
            // 
            // LanguageSetupButton
            // 
            this.LanguageSetupButton.Enabled = false;
            this.LanguageSetupButton.Location = new System.Drawing.Point(655, 217);
            this.LanguageSetupButton.Name = "LanguageSetupButton";
            this.LanguageSetupButton.Size = new System.Drawing.Size(118, 41);
            this.LanguageSetupButton.TabIndex = 1;
            this.LanguageSetupButton.Text = "Language setup";
            this.LanguageSetupButton.UseVisualStyleBackColor = true;
            this.LanguageSetupButton.Click += new System.EventHandler(this.LanguageSetupButton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(35, 48);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(293, 291);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // LanguageViewButton
            // 
            this.LanguageViewButton.Enabled = false;
            this.LanguageViewButton.Location = new System.Drawing.Point(655, 264);
            this.LanguageViewButton.Name = "LanguageViewButton";
            this.LanguageViewButton.Size = new System.Drawing.Size(118, 39);
            this.LanguageViewButton.TabIndex = 3;
            this.LanguageViewButton.Text = "View languages";
            this.LanguageViewButton.UseVisualStyleBackColor = true;
            this.LanguageViewButton.Click += new System.EventHandler(this.LanguageViewButton_Click);
            // 
            // SimulationButton
            // 
            this.SimulationButton.Location = new System.Drawing.Point(655, 330);
            this.SimulationButton.Name = "SimulationButton";
            this.SimulationButton.Size = new System.Drawing.Size(118, 48);
            this.SimulationButton.TabIndex = 4;
            this.SimulationButton.Text = "Simulation setup";
            this.SimulationButton.UseVisualStyleBackColor = true;
            this.SimulationButton.Click += new System.EventHandler(this.SimulationButton_Click);
            // 
            // quitbutton
            // 
            this.quitbutton.Location = new System.Drawing.Point(655, 396);
            this.quitbutton.Name = "quitbutton";
            this.quitbutton.Size = new System.Drawing.Size(118, 42);
            this.quitbutton.TabIndex = 5;
            this.quitbutton.Text = "Quit";
            this.quitbutton.UseVisualStyleBackColor = true;
            this.quitbutton.Click += new System.EventHandler(this.quitbutton_Click);
            // 
            // Parameterbutton
            // 
            this.Parameterbutton.Location = new System.Drawing.Point(655, 106);
            this.Parameterbutton.Name = "Parameterbutton";
            this.Parameterbutton.Size = new System.Drawing.Size(118, 36);
            this.Parameterbutton.TabIndex = 6;
            this.Parameterbutton.Text = "Read parameter file";
            this.Parameterbutton.UseVisualStyleBackColor = true;
            this.Parameterbutton.Click += new System.EventHandler(this.Parameterbutton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Parameterbutton);
            this.Controls.Add(this.quitbutton);
            this.Controls.Add(this.SimulationButton);
            this.Controls.Add(this.LanguageViewButton);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.LanguageSetupButton);
            this.Controls.Add(this.GeographyButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button GeographyButton;
        private System.Windows.Forms.Button LanguageSetupButton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button LanguageViewButton;
        private System.Windows.Forms.Button SimulationButton;
        private System.Windows.Forms.Button quitbutton;
        private System.Windows.Forms.Button Parameterbutton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

