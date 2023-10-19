namespace Visualizer
{
    partial class MainForm
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
            this.LoadButton = new System.Windows.Forms.Button();
            this.ListName = new System.Windows.Forms.TextBox();
            this.DrawButton = new System.Windows.Forms.Button();
            this.Label = new System.Windows.Forms.Label();
            this.InsrumentPanel = new System.Windows.Forms.Panel();
            this.ResultGraphic = new System.Windows.Forms.PictureBox();
            this.InsrumentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultGraphic)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadButton
            // 
            this.LoadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.5F);
            this.LoadButton.Location = new System.Drawing.Point(3, 3);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(136, 37);
            this.LoadButton.TabIndex = 0;
            this.LoadButton.Text = "Load Excel File";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // ListName
            // 
            this.ListName.Location = new System.Drawing.Point(3, 63);
            this.ListName.Name = "ListName";
            this.ListName.Size = new System.Drawing.Size(136, 20);
            this.ListName.TabIndex = 1;
            // 
            // DrawButton
            // 
            this.DrawButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.5F);
            this.DrawButton.Location = new System.Drawing.Point(4, 89);
            this.DrawButton.Name = "DrawButton";
            this.DrawButton.Size = new System.Drawing.Size(136, 73);
            this.DrawButton.TabIndex = 2;
            this.DrawButton.Text = "Draw";
            this.DrawButton.UseVisualStyleBackColor = true;
            this.DrawButton.Click += new System.EventHandler(this.DrawButton_Click);
            // 
            // Label
            // 
            this.Label.AutoSize = true;
            this.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F);
            this.Label.Location = new System.Drawing.Point(3, 43);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(139, 17);
            this.Label.TabIndex = 1;
            this.Label.Text = "Enter Excel list name";
            // 
            // InsrumentPanel
            // 
            this.InsrumentPanel.Controls.Add(this.Label);
            this.InsrumentPanel.Controls.Add(this.DrawButton);
            this.InsrumentPanel.Controls.Add(this.ListName);
            this.InsrumentPanel.Controls.Add(this.LoadButton);
            this.InsrumentPanel.Location = new System.Drawing.Point(1018, 12);
            this.InsrumentPanel.Name = "InsrumentPanel";
            this.InsrumentPanel.Size = new System.Drawing.Size(143, 800);
            this.InsrumentPanel.TabIndex = 0;
            // 
            // ResultGraphic
            // 
            this.ResultGraphic.Location = new System.Drawing.Point(12, 12);
            this.ResultGraphic.Name = "ResultGraphic";
            this.ResultGraphic.Size = new System.Drawing.Size(1000, 800);
            this.ResultGraphic.TabIndex = 1;
            this.ResultGraphic.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 828);
            this.Controls.Add(this.ResultGraphic);
            this.Controls.Add(this.InsrumentPanel);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.InsrumentPanel.ResumeLayout(false);
            this.InsrumentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultGraphic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.TextBox ListName;
        private System.Windows.Forms.Button DrawButton;
        private System.Windows.Forms.Label Label;
        private System.Windows.Forms.Panel InsrumentPanel;
        private System.Windows.Forms.PictureBox ResultGraphic;
    }
}

