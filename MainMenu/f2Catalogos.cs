using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace MainMenu
{
    public partial class f2Catalogos: MdiChildForm
    {
        public f2Catalogos()
        {            
            InitializeComponent();
            if (Variable.TypeScale == Variable.TipoBascula.WLABEL) { funcionWLABEL(); }

            loadProductos();
        }




        void funcionWLABEL()
        {
            this.toolStrip2.Items.Remove(this.ribPublicidad);//Remover lo de publicidad
        }

        void f2Catalogos_Activated(object sender, EventArgs e)
        {
            for (int i = 0; i < Variable.privilegio.Length; i++)
            {
                if (Variable.privilegio.Substring(i, 1) == "1")
                {
                    if (i == 4) { this.ribProductos.Enabled = true; } //modulo de productos
                    if (i == 5) { this.ribIngredientes.Enabled = true; }  //modulo de ingrediente    
                    if (i == 6) { this.ribOfertas.Enabled = true; } //modulo de info adicional
                    if (i == 7) { this.ribPublicidad.Enabled = true; }  //modulo de mensajes de publicidad 
                    if (i == 9) { this.ribVendedores.Enabled = true; } //modulo de vendedores                
                }
                else
                {
                    if (i == 4) { this.ribProductos.Enabled = false; } //modulo de productos
                    if (i == 5) { this.ribIngredientes.Enabled = false; }  //modulo de ingrediente    
                    if (i == 6) { this.ribOfertas.Enabled = false; } //modulo de info adicional
                    if (i == 7) { this.ribPublicidad.Enabled = false; }  //modulo de mensajes de publicidad 
                    if (i == 9) { this.ribVendedores.Enabled = false; } //modulo de vendedores                   
                }
            }            
        }

        private void f2Catalogos_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            this.BringToFront();
        }
        #region BotonesTap Catalogos

        private void iniciar_control()
        {
            this.Activate();
            this.panel1.Controls.Clear();
            ToolStripManager.RevertMerge(toolStrip2);      
            ribProductos.Checked = false;
            ribOfertas.Checked = false;
            ribIngredientes.Checked = false;
            ribPublicidad.Checked = false;
            ribVendedores.Checked = false;
        }



        void loadProductos()
        {
            Form1.btnEdicion = ESTADO.botonesEdicionEnum.PKPRODUCTOS;
            iniciar_control();
            ribProductos.Checked = true;
            UserProductos UsProd = new UserProductos();
            UsProd.Dock = DockStyle.Fill;
            if (ToolStripManager.FindToolStrip("toolStrip1").Items.Count > 0)
                ToolStripManager.Merge(UsProd.toolStrip1, toolStrip2);
            this.panel1.Controls.Add(UsProd);
        }





        private void ribProductos_Click(object sender, EventArgs e){loadProductos();}



        private void ribTablaNutri_Click(object sender, EventArgs e)
        {
            /*
             * Form1.btnEdicion = ESTADO.botonesEdicionEnum.PKINGREDIENTE;
            iniciar_control();
            ribIngredientes.Checked = true;
            UserIngrediente UsIngre = new UserIngrediente();
            UsIngre.Dock = DockStyle.Fill;
            if (ToolStripManager.FindToolStrip("toolStrip1").Items.Count > 0)
                ToolStripManager.Merge(UsIngre.toolStrip1, toolStrip2);   
            this.panel1.Controls.Add(UsIngre);
             */            
        }

        private void ribIngredientes_Click(object sender, EventArgs e)
        {
            Form1.btnEdicion = ESTADO.botonesEdicionEnum.PKINGREDIENTE;
            iniciar_control();
            ribIngredientes.Checked = true;
            UserIngrediente UsIngre = new UserIngrediente();
            UsIngre.Dock = DockStyle.Fill;
            if (ToolStripManager.FindToolStrip("toolStrip1").Items.Count > 0)
                ToolStripManager.Merge(UsIngre.toolStrip1, toolStrip2);   
            this.panel1.Controls.Add(UsIngre);
        }

        private void ribOfertas_Click(object sender, EventArgs e)
        {
            Form1.btnEdicion = ESTADO.botonesEdicionEnum.PKOFERTA;
            iniciar_control();
            ribOfertas.Checked = true;
            UserOfertas UsOfert = new UserOfertas();
            UsOfert.Dock = DockStyle.Fill;
            if (ToolStripManager.FindToolStrip("toolStrip1").Items.Count > 0)
                ToolStripManager.Merge(UsOfert.toolStrip1, toolStrip2);   
            this.panel1.Controls.Add(UsOfert);
        }

        private void ribPublicidad_Click(object sender, EventArgs e)
        {
            Form1.btnEdicion = ESTADO.botonesEdicionEnum.PKPUBLICIDAD;
            iniciar_control();
            ribPublicidad.Checked = true;
            UserPublicidad UsPublic = new UserPublicidad();            
            UsPublic.Dock = DockStyle.Fill;
            if (ToolStripManager.FindToolStrip("toolStrip1").Items.Count > 0)
                ToolStripManager.Merge(UsPublic.toolStrip1, toolStrip2);     
            this.panel1.Controls.Add(UsPublic);
        }

        private void ribVendedor_Click(object sender, EventArgs e)
        {
            Form1.btnEdicion = ESTADO.botonesEdicionEnum.PKVENDEDORES;
            iniciar_control();
            ribVendedores.Checked = true;
            UserVendedores UsVend = new UserVendedores();
            UsVend.Dock = DockStyle.Fill;
            if (ToolStripManager.FindToolStrip("toolStrip1").Items.Count > 0)
                ToolStripManager.Merge(UsVend.toolStrip1, toolStrip2);           
            this.panel1.Controls.Add(UsVend);
        }
        #endregion
               
    }
}
