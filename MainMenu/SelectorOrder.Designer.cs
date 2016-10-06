namespace MainMenu
{
    partial class SelectorOrder
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
            this.btn_wlabel = new System.Windows.Forms.Button();
            this.btn_wls = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_wlabel
            // 
            this.btn_wlabel.Location = new System.Drawing.Point(12, 22);
            this.btn_wlabel.Name = "btn_wlabel";
            this.btn_wlabel.Size = new System.Drawing.Size(164, 137);
            this.btn_wlabel.TabIndex = 0;
            this.btn_wlabel.UseVisualStyleBackColor = true;
            this.btn_wlabel.Click += new System.EventHandler(this.btn_wlabel_Click);
            // 
            // btn_wls
            // 
            this.btn_wls.Location = new System.Drawing.Point(193, 22);
            this.btn_wls.Name = "btn_wls";
            this.btn_wls.Size = new System.Drawing.Size(164, 137);
            this.btn_wls.TabIndex = 1;
            this.btn_wls.UseVisualStyleBackColor = true;
            this.btn_wls.Click += new System.EventHandler(this.btn_wls_Click);
            // 
            // SelectorOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 181);
            this.Controls.Add(this.btn_wls);
            this.Controls.Add(this.btn_wlabel);
            this.Name = "SelectorOrder";
            this.Text = "ChooseMode";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_wlabel;
        private System.Windows.Forms.Button btn_wls;

    }
}