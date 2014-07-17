namespace UnofficalFretlightSDK
{
    partial class FretlightDemo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FretlightDemo));
            this.chkGuitarCom = new System.Windows.Forms.CheckBox();
            this.btnOff = new System.Windows.Forms.Button();
            this.btnOn = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnFootSw = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtDisplay = new System.Windows.Forms.TextBox();
            this.picFn = new System.Windows.Forms.PictureBox();
            this.picFb = new System.Windows.Forms.PictureBox();
            this.pnlFb = new System.Windows.Forms.Panel();
            this.ledBulbStatus = new cntlLedBulb.LedBulb();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFb)).BeginInit();
            this.pnlFb.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkGuitarCom
            // 
            this.chkGuitarCom.AutoSize = true;
            this.chkGuitarCom.Checked = true;
            this.chkGuitarCom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGuitarCom.Location = new System.Drawing.Point(45, 183);
            this.chkGuitarCom.Name = "chkGuitarCom";
            this.chkGuitarCom.Size = new System.Drawing.Size(94, 17);
            this.chkGuitarCom.TabIndex = 19;
            this.chkGuitarCom.Text = "Send to Guitar";
            this.chkGuitarCom.UseVisualStyleBackColor = true;
            // 
            // btnOff
            // 
            this.btnOff.Location = new System.Drawing.Point(316, 180);
            this.btnOff.Name = "btnOff";
            this.btnOff.Size = new System.Drawing.Size(66, 23);
            this.btnOff.TabIndex = 20;
            this.btnOff.Text = "All Off";
            this.btnOff.UseVisualStyleBackColor = true;
            this.btnOff.Click += new System.EventHandler(this.btnOff_Click);
            // 
            // btnOn
            // 
            this.btnOn.Location = new System.Drawing.Point(388, 180);
            this.btnOn.Name = "btnOn";
            this.btnOn.Size = new System.Drawing.Size(66, 23);
            this.btnOn.TabIndex = 23;
            this.btnOn.Text = "All On";
            this.btnOn.UseVisualStyleBackColor = true;
            this.btnOn.Click += new System.EventHandler(this.btnOn_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(613, 182);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(109, 19);
            this.lblStatus.TabIndex = 24;
            this.lblStatus.Text = "Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(460, 180);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(66, 23);
            this.btnTest.TabIndex = 25;
            this.btnTest.Text = "Led Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnFootSw
            // 
            this.btnFootSw.Location = new System.Drawing.Point(532, 180);
            this.btnFootSw.Name = "btnFootSw";
            this.btnFootSw.Size = new System.Drawing.Size(75, 23);
            this.btnFootSw.TabIndex = 26;
            this.btnFootSw.Text = "FootSw Test";
            this.btnFootSw.UseVisualStyleBackColor = true;
            this.btnFootSw.Click += new System.EventHandler(this.btnFootSw_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(734, 24);
            this.menuStrip1.TabIndex = 31;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // txtDisplay
            // 
            this.txtDisplay.Location = new System.Drawing.Point(147, 181);
            this.txtDisplay.Name = "txtDisplay";
            this.txtDisplay.Size = new System.Drawing.Size(147, 20);
            this.txtDisplay.TabIndex = 32;
            this.txtDisplay.Text = "Fretboard Display";
            // 
            // picFn
            // 
            this.picFn.Location = new System.Drawing.Point(9, 8);
            this.picFn.Name = "picFn";
            this.picFn.Size = new System.Drawing.Size(696, 20);
            this.picFn.TabIndex = 28;
            this.picFn.TabStop = false;
            // 
            // picFb
            // 
            this.picFb.Location = new System.Drawing.Point(9, 34);
            this.picFb.Name = "picFb";
            this.picFb.Size = new System.Drawing.Size(696, 99);
            this.picFb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picFb.TabIndex = 29;
            this.picFb.TabStop = false;
            this.picFb.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picFb_MouseMove);
            // 
            // pnlFb
            // 
            this.pnlFb.Controls.Add(this.picFn);
            this.pnlFb.Controls.Add(this.picFb);
            this.pnlFb.Location = new System.Drawing.Point(8, 27);
            this.pnlFb.Name = "pnlFb";
            this.pnlFb.Size = new System.Drawing.Size(714, 140);
            this.pnlFb.TabIndex = 35;
            // 
            // ledBulbStatus
            // 
            this.ledBulbStatus.Color = System.Drawing.Color.LimeGreen;
            this.ledBulbStatus.ColorDark = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ledBulbStatus.ColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(230)))), ((int)(((byte)(153)))));
            this.ledBulbStatus.Fancy = true;
            this.ledBulbStatus.Location = new System.Drawing.Point(8, 177);
            this.ledBulbStatus.Name = "ledBulbStatus";
            this.ledBulbStatus.On = false;
            this.ledBulbStatus.Size = new System.Drawing.Size(24, 24);
            this.ledBulbStatus.TabIndex = 36;
            this.ledBulbStatus.Text = "ledBulb1";
            // 
            // FretlightDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 223);
            this.Controls.Add(this.ledBulbStatus);
            this.Controls.Add(this.pnlFb);
            this.Controls.Add(this.txtDisplay);
            this.Controls.Add(this.btnFootSw);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnOn);
            this.Controls.Add(this.btnOff);
            this.Controls.Add(this.chkGuitarCom);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FretlightDemo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Unoffical Fretlight SDK Demo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FretLight_FormClosed);
            this.Load += new System.EventHandler(this.FretLight_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FretlightDemo_MouseMove);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFb)).EndInit();
            this.pnlFb.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckBox chkGuitarCom;
        private System.Windows.Forms.Button btnOff;
        private System.Windows.Forms.Button btnOn;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnFootSw;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.TextBox txtDisplay;
        private System.Windows.Forms.PictureBox picFn;
        private System.Windows.Forms.PictureBox picFb;
        private System.Windows.Forms.Panel pnlFb;
        private cntlLedBulb.LedBulb ledBulbStatus;
    }
}

