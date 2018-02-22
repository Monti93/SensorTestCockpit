namespace SensorTestCockpit
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
            this.kinectPanel = new System.Windows.Forms.Panel();
            this.myoPanel = new System.Windows.Forms.Panel();
            this.leapPanel = new System.Windows.Forms.Panel();
            this.statusPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // kinectPanel
            // 
            this.kinectPanel.Location = new System.Drawing.Point(0, 0);
            this.kinectPanel.Name = "kinectPanel";
            this.kinectPanel.Size = new System.Drawing.Size(600, 600);
            this.kinectPanel.TabIndex = 0;
            this.kinectPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.OnKinectPaint);
            // 
            // myoPanel
            // 
            this.myoPanel.Location = new System.Drawing.Point(606, 0);
            this.myoPanel.Name = "myoPanel";
            this.myoPanel.Size = new System.Drawing.Size(600, 300);
            this.myoPanel.TabIndex = 1;
            this.myoPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.OnMyoPaint);
            // 
            // leapPanel
            // 
            this.leapPanel.Location = new System.Drawing.Point(606, 306);
            this.leapPanel.Name = "leapPanel";
            this.leapPanel.Size = new System.Drawing.Size(800, 600);
            this.leapPanel.TabIndex = 2;
            this.leapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaintLeap);
            // 
            // statusPanel
            // 
            this.statusPanel.Location = new System.Drawing.Point(3, 606);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Size = new System.Drawing.Size(597, 173);
            this.statusPanel.TabIndex = 3;
            this.statusPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.OnStatusPaint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1486, 782);
            this.Controls.Add(this.statusPanel);
            this.Controls.Add(this.leapPanel);
            this.Controls.Add(this.myoPanel);
            this.Controls.Add(this.kinectPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel kinectPanel;
        private System.Windows.Forms.Panel myoPanel;
        private System.Windows.Forms.Panel leapPanel;
        private System.Windows.Forms.Panel statusPanel;
    }
}

