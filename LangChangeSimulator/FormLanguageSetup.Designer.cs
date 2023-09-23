namespace LangChangeSimulator
{
    partial class FormLanguageSetup
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
            this.LB_lang = new System.Windows.Forms.CheckedListBox();
            this.TB_nlang = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RB_checked = new System.Windows.Forms.RadioButton();
            this.RB_random = new System.Windows.Forms.RadioButton();
            this.RB_scratch = new System.Windows.Forms.RadioButton();
            this.makebutton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RBnumbers = new System.Windows.Forms.RadioButton();
            this.RB_clics3 = new System.Windows.Forms.RadioButton();
            this.RB_swadesh200 = new System.Windows.Forms.RadioButton();
            this.RB_swadesh100 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // LB_lang
            // 
            this.LB_lang.FormattingEnabled = true;
            this.LB_lang.Location = new System.Drawing.Point(44, 58);
            this.LB_lang.Name = "LB_lang";
            this.LB_lang.Size = new System.Drawing.Size(138, 334);
            this.LB_lang.Sorted = true;
            this.LB_lang.TabIndex = 0;
            // 
            // TB_nlang
            // 
            this.TB_nlang.Location = new System.Drawing.Point(331, 33);
            this.TB_nlang.Name = "TB_nlang";
            this.TB_nlang.Size = new System.Drawing.Size(55, 20);
            this.TB_nlang.TabIndex = 1;
            this.TB_nlang.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(218, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "# starting languages";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RB_checked);
            this.groupBox1.Controls.Add(this.RB_random);
            this.groupBox1.Controls.Add(this.RB_scratch);
            this.groupBox1.Location = new System.Drawing.Point(222, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Language set";
            // 
            // RB_checked
            // 
            this.RB_checked.AutoSize = true;
            this.RB_checked.Checked = true;
            this.RB_checked.Location = new System.Drawing.Point(20, 62);
            this.RB_checked.Name = "RB_checked";
            this.RB_checked.Size = new System.Drawing.Size(94, 17);
            this.RB_checked.TabIndex = 2;
            this.RB_checked.TabStop = true;
            this.RB_checked.Text = "Checked in list";
            this.RB_checked.UseVisualStyleBackColor = true;
            // 
            // RB_random
            // 
            this.RB_random.AutoSize = true;
            this.RB_random.Location = new System.Drawing.Point(20, 39);
            this.RB_random.Name = "RB_random";
            this.RB_random.Size = new System.Drawing.Size(103, 17);
            this.RB_random.TabIndex = 1;
            this.RB_random.Text = "Random from list";
            this.RB_random.UseVisualStyleBackColor = true;
            // 
            // RB_scratch
            // 
            this.RB_scratch.AutoSize = true;
            this.RB_scratch.Enabled = false;
            this.RB_scratch.Location = new System.Drawing.Point(20, 16);
            this.RB_scratch.Name = "RB_scratch";
            this.RB_scratch.Size = new System.Drawing.Size(117, 17);
            this.RB_scratch.TabIndex = 0;
            this.RB_scratch.Text = "Create from scratch";
            this.RB_scratch.UseVisualStyleBackColor = true;
            // 
            // makebutton
            // 
            this.makebutton.Location = new System.Drawing.Point(632, 350);
            this.makebutton.Name = "makebutton";
            this.makebutton.Size = new System.Drawing.Size(145, 59);
            this.makebutton.TabIndex = 4;
            this.makebutton.Text = "Make languages!";
            this.makebutton.UseVisualStyleBackColor = true;
            this.makebutton.Click += new System.EventHandler(this.makebutton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(243, 228);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(242, 164);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.RBnumbers);
            this.groupBox2.Controls.Add(this.RB_clics3);
            this.groupBox2.Controls.Add(this.RB_swadesh200);
            this.groupBox2.Controls.Add(this.RB_swadesh100);
            this.groupBox2.Location = new System.Drawing.Point(456, 63);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 118);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Concept set";
            // 
            // RBnumbers
            // 
            this.RBnumbers.AutoSize = true;
            this.RBnumbers.Location = new System.Drawing.Point(13, 86);
            this.RBnumbers.Name = "RBnumbers";
            this.RBnumbers.Size = new System.Drawing.Size(113, 17);
            this.RBnumbers.TabIndex = 3;
            this.RBnumbers.TabStop = true;
            this.RBnumbers.Text = "Numbers 1-10 only";
            this.RBnumbers.UseVisualStyleBackColor = true;
            // 
            // RB_clics3
            // 
            this.RB_clics3.AutoSize = true;
            this.RB_clics3.Location = new System.Drawing.Point(13, 63);
            this.RB_clics3.Name = "RB_clics3";
            this.RB_clics3.Size = new System.Drawing.Size(97, 17);
            this.RB_clics3.TabIndex = 2;
            this.RB_clics3.Text = "Full CLICS3 set";
            this.RB_clics3.UseVisualStyleBackColor = true;
            // 
            // RB_swadesh200
            // 
            this.RB_swadesh200.AutoSize = true;
            this.RB_swadesh200.Location = new System.Drawing.Point(13, 41);
            this.RB_swadesh200.Name = "RB_swadesh200";
            this.RB_swadesh200.Size = new System.Drawing.Size(90, 17);
            this.RB_swadesh200.TabIndex = 1;
            this.RB_swadesh200.Text = "Swadesh 200";
            this.RB_swadesh200.UseVisualStyleBackColor = true;
            // 
            // RB_swadesh100
            // 
            this.RB_swadesh100.AutoSize = true;
            this.RB_swadesh100.Checked = true;
            this.RB_swadesh100.Location = new System.Drawing.Point(13, 18);
            this.RB_swadesh100.Name = "RB_swadesh100";
            this.RB_swadesh100.Size = new System.Drawing.Size(90, 17);
            this.RB_swadesh100.TabIndex = 0;
            this.RB_swadesh100.TabStop = true;
            this.RB_swadesh100.Text = "Swadesh 100";
            this.RB_swadesh100.UseVisualStyleBackColor = true;
            // 
            // FormLanguageSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.makebutton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TB_nlang);
            this.Controls.Add(this.LB_lang);
            this.Name = "FormLanguageSetup";
            this.Text = "FormLanguageSetup";
            this.Load += new System.EventHandler(this.FormLanguageSetup_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox LB_lang;
        private System.Windows.Forms.TextBox TB_nlang;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RB_checked;
        private System.Windows.Forms.RadioButton RB_random;
        private System.Windows.Forms.RadioButton RB_scratch;
        private System.Windows.Forms.Button makebutton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton RB_clics3;
        private System.Windows.Forms.RadioButton RB_swadesh200;
        private System.Windows.Forms.RadioButton RB_swadesh100;
        private System.Windows.Forms.RadioButton RBnumbers;
    }
}