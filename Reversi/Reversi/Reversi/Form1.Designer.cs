namespace Reversi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbkolommen = new System.Windows.Forms.TextBox();
            this.tbrijen = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(423, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "kolommen:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(984, 24);
            this.label2.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 37);
            this.label2.TabIndex = 1;
            this.label2.Text = "rijen:";
            // 
            // tbkolommen
            // 
            this.tbkolommen.Location = new System.Drawing.Point(626, 24);
            this.tbkolommen.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.tbkolommen.Name = "tbkolommen";
            this.tbkolommen.Size = new System.Drawing.Size(308, 44);
            this.tbkolommen.TabIndex = 2;
            this.tbkolommen.TextChanged += new System.EventHandler(this.tbkolommen_TextChanged);
            // 
            // tbrijen
            // 
            this.tbrijen.Location = new System.Drawing.Point(1094, 24);
            this.tbrijen.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.tbrijen.Name = "tbrijen";
            this.tbrijen.Size = new System.Drawing.Size(308, 44);
            this.tbrijen.TabIndex = 3;
            this.tbrijen.TextChanged += new System.EventHandler(this.tbrijen_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(19F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2166, 1881);
            this.Controls.Add(this.tbrijen);
            this.Controls.Add(this.tbkolommen);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbkolommen;
        private System.Windows.Forms.TextBox tbrijen;
    }
}

