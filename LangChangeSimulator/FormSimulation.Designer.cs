namespace LangChangeSimulator
{
    partial class FormSimulation
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
            this.QuitButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TB_timestep = new System.Windows.Forms.TextBox();
            this.RunButton = new System.Windows.Forms.Button();
            this.CB_migration = new System.Windows.Forms.CheckBox();
            this.CB_wordborrowing = new System.Windows.Forms.CheckBox();
            this.CB_soundchange = new System.Windows.Forms.CheckBox();
            this.CB_unconditional = new System.Windows.Forms.CheckBox();
            this.CB_conditional = new System.Windows.Forms.CheckBox();
            this.CB_soundborrowing = new System.Windows.Forms.CheckBox();
            this.RefreshMapButton = new System.Windows.Forms.Button();
            this.statbutton = new System.Windows.Forms.Button();
            this.TB_maxtime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CB_contact = new System.Windows.Forms.CheckBox();
            this.CB_techborrowing = new System.Windows.Forms.CheckBox();
            this.CB_longrange = new System.Windows.Forms.CheckBox();
            this.CB_semantics = new System.Windows.Forms.CheckBox();
            this.savebutton = new System.Windows.Forms.Button();
            this.soundchangestatbutton = new System.Windows.Forms.Button();
            this.CBnexus = new System.Windows.Forms.CheckBox();
            this.CB_Swadesh = new System.Windows.Forms.CheckBox();
            this.CB_CLDF = new System.Windows.Forms.CheckBox();
            this.CB_areal = new System.Windows.Forms.CheckBox();
            this.CB_arealword = new System.Windows.Forms.CheckBox();
            this.CBscreenshot = new System.Windows.Forms.CheckBox();
            this.TB_memlimit = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(307, 599);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // QuitButton
            // 
            this.QuitButton.Location = new System.Drawing.Point(665, 704);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new System.Drawing.Size(123, 38);
            this.QuitButton.TabIndex = 1;
            this.QuitButton.Text = "Close";
            this.QuitButton.UseVisualStyleBackColor = true;
            this.QuitButton.Click += new System.EventHandler(this.QuitButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(607, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Time step (years)";
            // 
            // TB_timestep
            // 
            this.TB_timestep.Location = new System.Drawing.Point(700, 19);
            this.TB_timestep.Name = "TB_timestep";
            this.TB_timestep.Size = new System.Drawing.Size(46, 20);
            this.TB_timestep.TabIndex = 3;
            // 
            // RunButton
            // 
            this.RunButton.Location = new System.Drawing.Point(665, 621);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(123, 61);
            this.RunButton.TabIndex = 4;
            this.RunButton.Text = "Run simulation!";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // CB_migration
            // 
            this.CB_migration.AutoSize = true;
            this.CB_migration.Checked = true;
            this.CB_migration.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_migration.Location = new System.Drawing.Point(637, 82);
            this.CB_migration.Name = "CB_migration";
            this.CB_migration.Size = new System.Drawing.Size(69, 17);
            this.CB_migration.TabIndex = 5;
            this.CB_migration.Text = "Migration";
            this.CB_migration.UseVisualStyleBackColor = true;
            // 
            // CB_wordborrowing
            // 
            this.CB_wordborrowing.AutoSize = true;
            this.CB_wordborrowing.Checked = true;
            this.CB_wordborrowing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_wordborrowing.Location = new System.Drawing.Point(653, 135);
            this.CB_wordborrowing.Name = "CB_wordborrowing";
            this.CB_wordborrowing.Size = new System.Drawing.Size(101, 17);
            this.CB_wordborrowing.TabIndex = 6;
            this.CB_wordborrowing.Text = "Word borrowing";
            this.CB_wordborrowing.UseVisualStyleBackColor = true;
            // 
            // CB_soundchange
            // 
            this.CB_soundchange.AutoSize = true;
            this.CB_soundchange.Checked = true;
            this.CB_soundchange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_soundchange.Location = new System.Drawing.Point(637, 287);
            this.CB_soundchange.Name = "CB_soundchange";
            this.CB_soundchange.Size = new System.Drawing.Size(96, 17);
            this.CB_soundchange.TabIndex = 7;
            this.CB_soundchange.Text = "Sound change";
            this.CB_soundchange.UseVisualStyleBackColor = true;
            this.CB_soundchange.CheckedChanged += new System.EventHandler(this.CB_soundchange_CheckedChanged);
            // 
            // CB_unconditional
            // 
            this.CB_unconditional.AutoSize = true;
            this.CB_unconditional.Checked = true;
            this.CB_unconditional.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_unconditional.Location = new System.Drawing.Point(653, 310);
            this.CB_unconditional.Name = "CB_unconditional";
            this.CB_unconditional.Size = new System.Drawing.Size(130, 17);
            this.CB_unconditional.TabIndex = 8;
            this.CB_unconditional.Text = "Unconditional change";
            this.CB_unconditional.UseVisualStyleBackColor = true;
            // 
            // CB_conditional
            // 
            this.CB_conditional.AutoSize = true;
            this.CB_conditional.Checked = true;
            this.CB_conditional.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_conditional.Location = new System.Drawing.Point(653, 333);
            this.CB_conditional.Name = "CB_conditional";
            this.CB_conditional.Size = new System.Drawing.Size(117, 17);
            this.CB_conditional.TabIndex = 9;
            this.CB_conditional.Text = "Conditional change";
            this.CB_conditional.UseVisualStyleBackColor = true;
            // 
            // CB_soundborrowing
            // 
            this.CB_soundborrowing.AutoSize = true;
            this.CB_soundborrowing.Checked = true;
            this.CB_soundborrowing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_soundborrowing.Location = new System.Drawing.Point(677, 230);
            this.CB_soundborrowing.Name = "CB_soundborrowing";
            this.CB_soundborrowing.Size = new System.Drawing.Size(106, 17);
            this.CB_soundborrowing.TabIndex = 10;
            this.CB_soundborrowing.Text = "Sound borrowing";
            this.CB_soundborrowing.UseVisualStyleBackColor = true;
            // 
            // RefreshMapButton
            // 
            this.RefreshMapButton.Location = new System.Drawing.Point(665, 472);
            this.RefreshMapButton.Name = "RefreshMapButton";
            this.RefreshMapButton.Size = new System.Drawing.Size(123, 39);
            this.RefreshMapButton.TabIndex = 11;
            this.RefreshMapButton.Text = "Refresh language map";
            this.RefreshMapButton.UseVisualStyleBackColor = true;
            this.RefreshMapButton.Click += new System.EventHandler(this.RefreshMapButton_Click);
            // 
            // statbutton
            // 
            this.statbutton.Location = new System.Drawing.Point(667, 393);
            this.statbutton.Name = "statbutton";
            this.statbutton.Size = new System.Drawing.Size(121, 23);
            this.statbutton.TabIndex = 12;
            this.statbutton.Text = "Language statistics";
            this.statbutton.UseVisualStyleBackColor = true;
            this.statbutton.Click += new System.EventHandler(this.statbutton_Click);
            // 
            // TB_maxtime
            // 
            this.TB_maxtime.Location = new System.Drawing.Point(700, 45);
            this.TB_maxtime.Name = "TB_maxtime";
            this.TB_maxtime.Size = new System.Drawing.Size(46, 20);
            this.TB_maxtime.TabIndex = 13;
            this.TB_maxtime.Text = "4000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(624, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Max time";
            // 
            // CB_contact
            // 
            this.CB_contact.AutoSize = true;
            this.CB_contact.Checked = true;
            this.CB_contact.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_contact.Location = new System.Drawing.Point(637, 112);
            this.CB_contact.Name = "CB_contact";
            this.CB_contact.Size = new System.Drawing.Size(114, 17);
            this.CB_contact.TabIndex = 15;
            this.CB_contact.Text = "Contact processes";
            this.CB_contact.UseVisualStyleBackColor = true;
            this.CB_contact.CheckedChanged += new System.EventHandler(this.CB_contact_CheckedChanged);
            // 
            // CB_techborrowing
            // 
            this.CB_techborrowing.AutoSize = true;
            this.CB_techborrowing.Checked = true;
            this.CB_techborrowing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_techborrowing.Location = new System.Drawing.Point(653, 158);
            this.CB_techborrowing.Name = "CB_techborrowing";
            this.CB_techborrowing.Size = new System.Drawing.Size(100, 17);
            this.CB_techborrowing.TabIndex = 16;
            this.CB_techborrowing.Text = "Tech borrowing";
            this.CB_techborrowing.UseVisualStyleBackColor = true;
            // 
            // CB_longrange
            // 
            this.CB_longrange.AutoSize = true;
            this.CB_longrange.Checked = true;
            this.CB_longrange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_longrange.Location = new System.Drawing.Point(653, 182);
            this.CB_longrange.Name = "CB_longrange";
            this.CB_longrange.Size = new System.Drawing.Size(119, 17);
            this.CB_longrange.TabIndex = 17;
            this.CB_longrange.Text = "Long range contact";
            this.CB_longrange.UseVisualStyleBackColor = true;
            // 
            // CB_semantics
            // 
            this.CB_semantics.AutoSize = true;
            this.CB_semantics.Checked = true;
            this.CB_semantics.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_semantics.Location = new System.Drawing.Point(636, 356);
            this.CB_semantics.Name = "CB_semantics";
            this.CB_semantics.Size = new System.Drawing.Size(97, 17);
            this.CB_semantics.TabIndex = 18;
            this.CB_semantics.Text = "Semantic shifts";
            this.CB_semantics.UseVisualStyleBackColor = true;
            this.CB_semantics.CheckedChanged += new System.EventHandler(this.CB_semantics_CheckedChanged);
            // 
            // savebutton
            // 
            this.savebutton.Location = new System.Drawing.Point(667, 533);
            this.savebutton.Name = "savebutton";
            this.savebutton.Size = new System.Drawing.Size(121, 63);
            this.savebutton.TabIndex = 19;
            this.savebutton.Text = "Save output";
            this.savebutton.UseVisualStyleBackColor = true;
            this.savebutton.Click += new System.EventHandler(this.savebutton_Click);
            // 
            // soundchangestatbutton
            // 
            this.soundchangestatbutton.Location = new System.Drawing.Point(667, 422);
            this.soundchangestatbutton.Name = "soundchangestatbutton";
            this.soundchangestatbutton.Size = new System.Drawing.Size(121, 44);
            this.soundchangestatbutton.TabIndex = 20;
            this.soundchangestatbutton.Text = "Sound change statistics";
            this.soundchangestatbutton.UseVisualStyleBackColor = true;
            this.soundchangestatbutton.Click += new System.EventHandler(this.soundchangestatbutton_Click);
            // 
            // CBnexus
            // 
            this.CBnexus.AutoSize = true;
            this.CBnexus.Checked = true;
            this.CBnexus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBnexus.Location = new System.Drawing.Point(537, 579);
            this.CBnexus.Name = "CBnexus";
            this.CBnexus.Size = new System.Drawing.Size(107, 17);
            this.CBnexus.TabIndex = 21;
            this.CBnexus.Text = "Save NEXUS file";
            this.CBnexus.UseVisualStyleBackColor = true;
            // 
            // CB_Swadesh
            // 
            this.CB_Swadesh.AutoSize = true;
            this.CB_Swadesh.Checked = true;
            this.CB_Swadesh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_Swadesh.Location = new System.Drawing.Point(537, 533);
            this.CB_Swadesh.Name = "CB_Swadesh";
            this.CB_Swadesh.Size = new System.Drawing.Size(124, 17);
            this.CB_Swadesh.TabIndex = 22;
            this.CB_Swadesh.Text = "Save Swadesh table";
            this.CB_Swadesh.UseVisualStyleBackColor = true;
            // 
            // CB_CLDF
            // 
            this.CB_CLDF.AutoSize = true;
            this.CB_CLDF.Checked = true;
            this.CB_CLDF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_CLDF.Location = new System.Drawing.Point(537, 557);
            this.CB_CLDF.Name = "CB_CLDF";
            this.CB_CLDF.Size = new System.Drawing.Size(107, 17);
            this.CB_CLDF.TabIndex = 23;
            this.CB_CLDF.Text = "Save CLDFormat";
            this.CB_CLDF.UseVisualStyleBackColor = true;
            this.CB_CLDF.CheckedChanged += new System.EventHandler(this.CB_CLDF_CheckedChanged);
            // 
            // CB_areal
            // 
            this.CB_areal.AutoSize = true;
            this.CB_areal.Checked = true;
            this.CB_areal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_areal.Location = new System.Drawing.Point(653, 205);
            this.CB_areal.Name = "CB_areal";
            this.CB_areal.Size = new System.Drawing.Size(85, 17);
            this.CB_areal.TabIndex = 24;
            this.CB_areal.Text = "Areal effects";
            this.CB_areal.UseVisualStyleBackColor = true;
            // 
            // CB_arealword
            // 
            this.CB_arealword.AutoSize = true;
            this.CB_arealword.Checked = true;
            this.CB_arealword.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_arealword.Location = new System.Drawing.Point(677, 253);
            this.CB_arealword.Name = "CB_arealword";
            this.CB_arealword.Size = new System.Drawing.Size(125, 17);
            this.CB_arealword.TabIndex = 25;
            this.CB_arealword.Text = "Areal word borrowing";
            this.CB_arealword.UseVisualStyleBackColor = true;
            // 
            // CBscreenshot
            // 
            this.CBscreenshot.AutoSize = true;
            this.CBscreenshot.Location = new System.Drawing.Point(528, 646);
            this.CBscreenshot.Name = "CBscreenshot";
            this.CBscreenshot.Size = new System.Drawing.Size(111, 17);
            this.CBscreenshot.TabIndex = 26;
            this.CBscreenshot.Text = "Save screenshots";
            this.CBscreenshot.UseVisualStyleBackColor = true;
            // 
            // TB_memlimit
            // 
            this.TB_memlimit.Location = new System.Drawing.Point(571, 669);
            this.TB_memlimit.Name = "TB_memlimit";
            this.TB_memlimit.Size = new System.Drawing.Size(68, 20);
            this.TB_memlimit.TabIndex = 27;
            this.TB_memlimit.Text = "10000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(498, 672);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Memory limit:";
            // 
            // FormSimulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 754);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TB_memlimit);
            this.Controls.Add(this.CBscreenshot);
            this.Controls.Add(this.CB_arealword);
            this.Controls.Add(this.CB_areal);
            this.Controls.Add(this.CB_CLDF);
            this.Controls.Add(this.CB_Swadesh);
            this.Controls.Add(this.CBnexus);
            this.Controls.Add(this.soundchangestatbutton);
            this.Controls.Add(this.savebutton);
            this.Controls.Add(this.CB_semantics);
            this.Controls.Add(this.CB_longrange);
            this.Controls.Add(this.CB_techborrowing);
            this.Controls.Add(this.CB_contact);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TB_maxtime);
            this.Controls.Add(this.statbutton);
            this.Controls.Add(this.RefreshMapButton);
            this.Controls.Add(this.CB_soundborrowing);
            this.Controls.Add(this.CB_conditional);
            this.Controls.Add(this.CB_unconditional);
            this.Controls.Add(this.CB_soundchange);
            this.Controls.Add(this.CB_wordborrowing);
            this.Controls.Add(this.CB_migration);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.TB_timestep);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.QuitButton);
            this.Controls.Add(this.richTextBox1);
            this.Name = "FormSimulation";
            this.Text = "FormSimulation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button QuitButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TB_timestep;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.CheckBox CB_migration;
        private System.Windows.Forms.CheckBox CB_wordborrowing;
        private System.Windows.Forms.CheckBox CB_soundchange;
        private System.Windows.Forms.CheckBox CB_unconditional;
        private System.Windows.Forms.CheckBox CB_conditional;
        private System.Windows.Forms.CheckBox CB_soundborrowing;
        private System.Windows.Forms.Button RefreshMapButton;
        private System.Windows.Forms.Button statbutton;
        private System.Windows.Forms.TextBox TB_maxtime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox CB_contact;
        private System.Windows.Forms.CheckBox CB_techborrowing;
        private System.Windows.Forms.CheckBox CB_longrange;
        private System.Windows.Forms.CheckBox CB_semantics;
        private System.Windows.Forms.Button savebutton;
        private System.Windows.Forms.Button soundchangestatbutton;
        private System.Windows.Forms.CheckBox CBnexus;
        private System.Windows.Forms.CheckBox CB_Swadesh;
        private System.Windows.Forms.CheckBox CB_CLDF;
        private System.Windows.Forms.CheckBox CB_areal;
        private System.Windows.Forms.CheckBox CB_arealword;
        private System.Windows.Forms.CheckBox CBscreenshot;
        private System.Windows.Forms.TextBox TB_memlimit;
        private System.Windows.Forms.Label label3;
    }
}