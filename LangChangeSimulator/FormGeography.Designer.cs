namespace LangChangeSimulator
{
    partial class FormGeography
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
            this.BasemapButton = new System.Windows.Forms.Button();
            this.MapstatisticsButton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.CB_river = new System.Windows.Forms.CheckBox();
            this.CB_fillmissing = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RB_landcover = new System.Windows.Forms.RadioButton();
            this.RB_roughness = new System.Windows.Forms.RadioButton();
            this.RB_variance = new System.Windows.Forms.RadioButton();
            this.RB_terrain = new System.Windows.Forms.RadioButton();
            this.RB_climate = new System.Windows.Forms.RadioButton();
            this.LB_region = new System.Windows.Forms.ListBox();
            this.LB_startregion = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BasemapButton
            // 
            this.BasemapButton.Location = new System.Drawing.Point(449, 39);
            this.BasemapButton.Name = "BasemapButton";
            this.BasemapButton.Size = new System.Drawing.Size(145, 42);
            this.BasemapButton.TabIndex = 0;
            this.BasemapButton.Text = "Read mapfile";
            this.BasemapButton.UseVisualStyleBackColor = true;
            this.BasemapButton.Click += new System.EventHandler(this.BasemapButton_Click);
            // 
            // MapstatisticsButton
            // 
            this.MapstatisticsButton.Location = new System.Drawing.Point(449, 100);
            this.MapstatisticsButton.Name = "MapstatisticsButton";
            this.MapstatisticsButton.Size = new System.Drawing.Size(145, 42);
            this.MapstatisticsButton.TabIndex = 1;
            this.MapstatisticsButton.Text = "Map statistics";
            this.MapstatisticsButton.UseVisualStyleBackColor = true;
            this.MapstatisticsButton.Click += new System.EventHandler(this.MapstatisticsButton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(28, 29);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(371, 342);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // CB_river
            // 
            this.CB_river.AutoSize = true;
            this.CB_river.Location = new System.Drawing.Point(688, 193);
            this.CB_river.Name = "CB_river";
            this.CB_river.Size = new System.Drawing.Size(116, 17);
            this.CB_river.TabIndex = 4;
            this.CB_river.Text = "Show river squares";
            this.CB_river.UseVisualStyleBackColor = true;
            // 
            // CB_fillmissing
            // 
            this.CB_fillmissing.AutoSize = true;
            this.CB_fillmissing.Checked = true;
            this.CB_fillmissing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_fillmissing.Location = new System.Drawing.Point(688, 216);
            this.CB_fillmissing.Name = "CB_fillmissing";
            this.CB_fillmissing.Size = new System.Drawing.Size(120, 17);
            this.CB_fillmissing.TabIndex = 5;
            this.CB_fillmissing.Text = "Fill in missing values";
            this.CB_fillmissing.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RB_landcover);
            this.groupBox1.Controls.Add(this.RB_roughness);
            this.groupBox1.Controls.Add(this.RB_variance);
            this.groupBox1.Controls.Add(this.RB_terrain);
            this.groupBox1.Controls.Add(this.RB_climate);
            this.groupBox1.Location = new System.Drawing.Point(620, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 134);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // RB_landcover
            // 
            this.RB_landcover.AutoSize = true;
            this.RB_landcover.Location = new System.Drawing.Point(15, 61);
            this.RB_landcover.Name = "RB_landcover";
            this.RB_landcover.Size = new System.Drawing.Size(110, 17);
            this.RB_landcover.TabIndex = 4;
            this.RB_landcover.Text = "Map by landcover";
            this.RB_landcover.UseVisualStyleBackColor = true;
            this.RB_landcover.CheckedChanged += new System.EventHandler(this.RB_landcover_CheckedChanged);
            // 
            // RB_roughness
            // 
            this.RB_roughness.AutoSize = true;
            this.RB_roughness.Location = new System.Drawing.Point(15, 111);
            this.RB_roughness.Name = "RB_roughness";
            this.RB_roughness.Size = new System.Drawing.Size(112, 17);
            this.RB_roughness.TabIndex = 3;
            this.RB_roughness.Text = "Map by roughness";
            this.RB_roughness.UseVisualStyleBackColor = true;
            this.RB_roughness.CheckedChanged += new System.EventHandler(this.RB_roughness_CheckedChanged);
            // 
            // RB_variance
            // 
            this.RB_variance.AutoSize = true;
            this.RB_variance.Location = new System.Drawing.Point(15, 84);
            this.RB_variance.Name = "RB_variance";
            this.RB_variance.Size = new System.Drawing.Size(104, 17);
            this.RB_variance.TabIndex = 2;
            this.RB_variance.Text = "Map by variance";
            this.RB_variance.UseVisualStyleBackColor = true;
            this.RB_variance.CheckedChanged += new System.EventHandler(this.RB_variance_CheckedChanged);
            // 
            // RB_terrain
            // 
            this.RB_terrain.AutoSize = true;
            this.RB_terrain.Location = new System.Drawing.Point(15, 37);
            this.RB_terrain.Name = "RB_terrain";
            this.RB_terrain.Size = new System.Drawing.Size(92, 17);
            this.RB_terrain.TabIndex = 1;
            this.RB_terrain.Text = "Map by terrain";
            this.RB_terrain.UseVisualStyleBackColor = true;
            this.RB_terrain.CheckedChanged += new System.EventHandler(this.RB_terrain_CheckedChanged);
            // 
            // RB_climate
            // 
            this.RB_climate.AutoSize = true;
            this.RB_climate.Checked = true;
            this.RB_climate.Location = new System.Drawing.Point(15, 14);
            this.RB_climate.Name = "RB_climate";
            this.RB_climate.Size = new System.Drawing.Size(96, 17);
            this.RB_climate.TabIndex = 0;
            this.RB_climate.TabStop = true;
            this.RB_climate.Text = "Map by climate";
            this.RB_climate.UseVisualStyleBackColor = true;
            this.RB_climate.CheckedChanged += new System.EventHandler(this.RB_climate_CheckedChanged);
            // 
            // LB_region
            // 
            this.LB_region.FormattingEnabled = true;
            this.LB_region.Location = new System.Drawing.Point(416, 211);
            this.LB_region.Name = "LB_region";
            this.LB_region.Size = new System.Drawing.Size(120, 160);
            this.LB_region.TabIndex = 7;
            // 
            // LB_startregion
            // 
            this.LB_startregion.FormattingEnabled = true;
            this.LB_startregion.Location = new System.Drawing.Point(553, 211);
            this.LB_startregion.Name = "LB_startregion";
            this.LB_startregion.Size = new System.Drawing.Size(120, 160);
            this.LB_startregion.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(423, 189);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Map region";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(550, 189);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Start region";
            // 
            // FormGeography
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 412);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LB_startregion);
            this.Controls.Add(this.LB_region);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CB_fillmissing);
            this.Controls.Add(this.CB_river);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.MapstatisticsButton);
            this.Controls.Add(this.BasemapButton);
            this.Name = "FormGeography";
            this.Text = "FormGeography";
            this.ResizeEnd += new System.EventHandler(this.FormGeography_ResizeEnd);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BasemapButton;
        private System.Windows.Forms.Button MapstatisticsButton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox CB_river;
        private System.Windows.Forms.CheckBox CB_fillmissing;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RB_landcover;
        private System.Windows.Forms.RadioButton RB_roughness;
        private System.Windows.Forms.RadioButton RB_variance;
        private System.Windows.Forms.RadioButton RB_terrain;
        private System.Windows.Forms.RadioButton RB_climate;
        private System.Windows.Forms.ListBox LB_region;
        private System.Windows.Forms.ListBox LB_startregion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}