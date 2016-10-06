namespace MainMenu
{
    partial class ChooseMode
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
            this.CHK_WLABEL = new System.Windows.Forms.CheckBox();
            this.CHK_WLS = new System.Windows.Forms.CheckBox();
            this.CHK_WLSD = new System.Windows.Forms.CheckBox();
            this.BTN_ENTRAR = new System.Windows.Forms.Button();
            this.BTN_CANCELAR = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CHK_WLABEL
            // 
            this.CHK_WLABEL.AutoSize = true;
            this.CHK_WLABEL.BackColor = System.Drawing.Color.White;
            this.CHK_WLABEL.Location = new System.Drawing.Point(84, 224);
            this.CHK_WLABEL.Name = "CHK_WLABEL";
            this.CHK_WLABEL.Size = new System.Drawing.Size(70, 17);
            this.CHK_WLABEL.TabIndex = 3;
            this.CHK_WLABEL.Text = "WLABEL";
            this.CHK_WLABEL.UseVisualStyleBackColor = false;
            this.CHK_WLABEL.CheckedChanged += new System.EventHandler(this.CHK_WLABEL_CheckedChanged);
            // 
            // CHK_WLS
            // 
            this.CHK_WLS.AutoSize = true;
            this.CHK_WLS.BackColor = System.Drawing.Color.White;
            this.CHK_WLS.Location = new System.Drawing.Point(304, 224);
            this.CHK_WLS.Name = "CHK_WLS";
            this.CHK_WLS.Size = new System.Drawing.Size(50, 17);
            this.CHK_WLS.TabIndex = 4;
            this.CHK_WLS.Text = "WLS";
            this.CHK_WLS.UseVisualStyleBackColor = false;
            // 
            // CHK_WLSD
            // 
            this.CHK_WLSD.AutoSize = true;
            this.CHK_WLSD.BackColor = System.Drawing.Color.White;
            this.CHK_WLSD.Location = new System.Drawing.Point(510, 224);
            this.CHK_WLSD.Name = "CHK_WLSD";
            this.CHK_WLSD.Size = new System.Drawing.Size(61, 17);
            this.CHK_WLSD.TabIndex = 5;
            this.CHK_WLSD.Text = "WLS-D";
            this.CHK_WLSD.UseVisualStyleBackColor = false;
            // 
            // BTN_ENTRAR
            // 
            this.BTN_ENTRAR.BackColor = System.Drawing.Color.Transparent;
            this.BTN_ENTRAR.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BTN_ENTRAR.FlatAppearance.BorderSize = 0;
            this.BTN_ENTRAR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_ENTRAR.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.BTN_ENTRAR.Image = global::MainMenu.Properties.Resources.aceptar;
            this.BTN_ENTRAR.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BTN_ENTRAR.Location = new System.Drawing.Point(231, 247);
            this.BTN_ENTRAR.Name = "BTN_ENTRAR";
            this.BTN_ENTRAR.Size = new System.Drawing.Size(60, 85);
            this.BTN_ENTRAR.TabIndex = 8;
            this.BTN_ENTRAR.UseVisualStyleBackColor = false;
            this.BTN_ENTRAR.Click += new System.EventHandler(this.BTN_ENTRAR_Click);
            // 
            // BTN_CANCELAR
            // 
            this.BTN_CANCELAR.BackColor = System.Drawing.Color.Transparent;
            this.BTN_CANCELAR.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BTN_CANCELAR.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BTN_CANCELAR.FlatAppearance.BorderSize = 0;
            this.BTN_CANCELAR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CANCELAR.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.BTN_CANCELAR.Image = global::MainMenu.Properties.Resources.salir;
            this.BTN_CANCELAR.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BTN_CANCELAR.Location = new System.Drawing.Point(348, 250);
            this.BTN_CANCELAR.Name = "BTN_CANCELAR";
            this.BTN_CANCELAR.Size = new System.Drawing.Size(49, 79);
            this.BTN_CANCELAR.TabIndex = 9;
            this.BTN_CANCELAR.UseVisualStyleBackColor = false;
            this.BTN_CANCELAR.Click += new System.EventHandler(this.BTN_CANCELAR_Click);
            // 
            // ChooseMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MainMenu.Properties.Resources.fondoWizard1;
            this.ClientSize = new System.Drawing.Size(650, 332);
            this.ControlBox = false;
            this.Controls.Add(this.BTN_CANCELAR);
            this.Controls.Add(this.BTN_ENTRAR);
            this.Controls.Add(this.CHK_WLSD);
            this.Controls.Add(this.CHK_WLS);
            this.Controls.Add(this.CHK_WLABEL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChooseMode";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChooseMode";
            this.Load += new System.EventHandler(this.ChooseMode_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox CHK_WLABEL;
        private System.Windows.Forms.CheckBox CHK_WLS;
        private System.Windows.Forms.CheckBox CHK_WLSD;
        private System.Windows.Forms.Button BTN_ENTRAR;
        private System.Windows.Forms.Button BTN_CANCELAR;
    }
}