namespace LangChangeSimulator
{
    partial class FormShowLanguage
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.LB_lang = new System.Windows.Forms.ListBox();
            this.Closebutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LB_stock = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.swadeshbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(22, 21);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(688, 685);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // LB_lang
            // 
            this.LB_lang.FormattingEnabled = true;
            this.LB_lang.Location = new System.Drawing.Point(763, 36);
            this.LB_lang.Name = "LB_lang";
            this.LB_lang.Size = new System.Drawing.Size(144, 225);
            this.LB_lang.TabIndex = 1;
            this.LB_lang.SelectedIndexChanged += new System.EventHandler(this.LB_lang_SelectedIndexChanged);
            // 
            // Closebutton
            // 
            this.Closebutton.Location = new System.Drawing.Point(807, 694);
            this.Closebutton.Name = "Closebutton";
            this.Closebutton.Size = new System.Drawing.Size(75, 48);
            this.Closebutton.TabIndex = 2;
            this.Closebutton.Text = "Close";
            this.Closebutton.UseVisualStyleBackColor = true;
            this.Closebutton.Click += new System.EventHandler(this.Closebutton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(760, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Languages";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // LB_stock
            // 
            this.LB_stock.FormattingEnabled = true;
            this.LB_stock.Location = new System.Drawing.Point(763, 341);
            this.LB_stock.Name = "LB_stock";
            this.LB_stock.Size = new System.Drawing.Size(144, 225);
            this.LB_stock.TabIndex = 4;
            this.LB_stock.SelectedIndexChanged += new System.EventHandler(this.LB_stock_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(760, 325);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Stocks";
            // 
            // swadeshbutton
            // 
            this.swadeshbutton.Location = new System.Drawing.Point(775, 618);
            this.swadeshbutton.Name = "swadeshbutton";
            this.swadeshbutton.Size = new System.Drawing.Size(132, 32);
            this.swadeshbutton.TabIndex = 6;
            this.swadeshbutton.Text = "Make Swadesh table";
            this.swadeshbutton.UseVisualStyleBackColor = true;
            this.swadeshbutton.Click += new System.EventHandler(this.swadeshbutton_Click);
            // 
            // FormShowLanguage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 754);
            this.Controls.Add(this.swadeshbutton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LB_stock);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Closebutton);
            this.Controls.Add(this.LB_lang);
            this.Controls.Add(this.richTextBox1);
            this.Name = "FormShowLanguage";
            this.Text = "FormShowLanguage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ListBox LB_lang;
        private System.Windows.Forms.Button Closebutton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox LB_stock;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button swadeshbutton;
    }
}