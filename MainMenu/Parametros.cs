using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Reporting.WinForms;

namespace MainMenu
{
    class Parametros
    {
        public void Analisis_Venta_Desglose(int n_titulo, string fecha, string hora1, ref ReportParameter[] paramField)
        {

            int n1 = 10, n2 = 11;

            string[] Str_Titulo = new string[4] 
            {
              
              Variable.SYS_MSJ[126,Variable.idioma].ToUpper(), //"Analisis de venta por Grupo", //0
              Variable.SYS_MSJ[127,Variable.idioma].ToUpper(), //"Analisis de venta por Bascula",   //1
              Variable.SYS_MSJ[128,Variable.idioma].ToUpper(), //"Analisis de venta por Producto",  //2
              Variable.SYS_MSJ[129,Variable.idioma].ToUpper()  //
            
            }; //"Analisis de venta por Vendedor"}; //3



            paramField[0] = new ReportParameter("TITULO", Str_Titulo[n_titulo]);

            switch (n_titulo)
            {
                case 0: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[376, Variable.idioma]); break;  //Departamento
                case 1: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[164, Variable.idioma]); break;  //numero de serie
                case 2: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[145, Variable.idioma]); break; //codigo
                case 3: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[175, Variable.idioma]); break; //vendedor
            }

            switch (n_titulo)
            {
                case 0: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[156, Variable.idioma]); break;  //folio
                case 1: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[155, Variable.idioma]); break;  //corte
                case 2: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[146, Variable.idioma]); break;  //descripcion
                case 3: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[163, Variable.idioma]); break; //nombre          
            }

            switch (n_titulo)
            {
                case 0: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[215, Variable.idioma]); break;  //cantidad
                case 1: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[215, Variable.idioma]); break; //cantidad
                case 2: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[216, Variable.idioma]); break; //peso
                case 3: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[215, Variable.idioma]); break; //cantidad           
            }

            paramField[4] = new ReportParameter("SCABE4", Variable.SYS_MSJ[147, Variable.idioma]); //descuento
            paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[221, Variable.idioma]); //devolucion
            paramField[6] = new ReportParameter("SCABE6", Variable.SYS_MSJ[165, Variable.idioma]); //oferta
            paramField[7] = new ReportParameter("GranTotal", "GranTotal");
            paramField[8] = new ReportParameter("FECHA", fecha + "  " + hora1);
            paramField[9] = new ReportParameter("Impuesto", "Impuesto");

            if (n_titulo == 2)
            {
                paramField[10] = new ReportParameter("UNIMED", Variable.SYS_MSJ[219, Variable.idioma]); // "Piezas";
                paramField[11] = new ReportParameter("UNIPES", Variable.FOR_UM[Variable.unidad]);
                n1 = 12;
                n2 = 13;
            }



            else
            {

            }

            paramField[n1] = new ReportParameter("Subtotal", "Subtotal");
            paramField[n2] = new ReportParameter("GTOTAL", "GTOTAL");
            //paramField[n3] = new ReportParameter("Nombre", "Nombre");







            #region Trash
            //paramField[12] = new ReportParameter("GranTotal", "GranTotal");
            //paramField[13] = new ReportParameter("Impuesto", "Impuesto");


            //switch (n_titulo)
            //{
            //    case 0: paramField[9] = new ReportParameter("SubTotal", "Subtotal"); break; //subtotal
            //    case 1: paramField[9] = new ReportParameter("CampoRamiro", "Impuesto"); break;  //Impuesto
            //    case 2: paramField[9] = new ReportParameter("CampoRamiro", "Cantidad"); break; //Cantidad
            //}
            /*
            switch (n_titulo)
            {
                case 0: paramField[4] = new ReportParameter("SCABE4", Variable.SYS_MSJ[217, Variable.idioma]); break; //subtotal
                case 1: paramField[4] = new ReportParameter("SCABE4", Variable.SYS_MSJ[215, Variable.idioma]); break;  //cantidad
                case 2: paramField[4] = new ReportParameter("SCABE4", Variable.SYS_MSJ[166, Variable.idioma]); break; //precio
                case 3: paramField[4] = new ReportParameter("SCABE4", Variable.SYS_MSJ[215, Variable.idioma]); break; //Cantidad;
            }
            switch (n_titulo)
            {
                case 0: paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[147, Variable.idioma]); break; // descuento;
                case 1: paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[217, Variable.idioma]); break; //subtotal
                case 2: paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[217, Variable.idioma]); break;// subtotal
                case 3: paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[217, Variable.idioma]); break; //subtotal                   
            }
            switch (n_titulo)
            {
                case 0: paramField[6] = new ReportParameter("SCABE6", Variable.SYS_MSJ[148, Variable.idioma]); break; // Impuesto
                case 1: paramField[6] = new ReportParameter("SCABE6", Variable.SYS_MSJ[147, Variable.idioma]); break;  //descuento
                case 2: paramField[6] = new ReportParameter("SCABE6", Variable.SYS_MSJ[147, Variable.idioma]); break; // descuento
                case 3: paramField[6] = new ReportParameter("SCABE6", Variable.SYS_MSJ[147, Variable.idioma]); break;  //descuento                
            }*/
            #endregion






        }
        public void Analisis_Venta_Estadisticos_Desglose(int n_titulo, string fecha, string hora1, ref ReportParameter[] paramField)
        {



            string[] Str_Titulo = new string[4] 
            {
              
              Variable.SYS_MSJ[126,Variable.idioma].ToUpper(), //"Analisis de venta por Grupo", //0
              Variable.SYS_MSJ[127,Variable.idioma].ToUpper(), //"Analisis de venta por Bascula",   //1
              Variable.SYS_MSJ[128,Variable.idioma].ToUpper(), //"Analisis de venta por Producto",  //2
              Variable.SYS_MSJ[129,Variable.idioma].ToUpper()
            
            }; //"Analisis de venta por Vendedor"}; //3



            paramField[0] = new ReportParameter("TITULO", Str_Titulo[n_titulo]);

            switch (n_titulo)
            {
                case 0: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[376, Variable.idioma]); break;  //Departamento
                case 1: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[164, Variable.idioma]); break;  //numero de serie
                case 2: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[145, Variable.idioma]); break; //codigo
                case 3: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[175, Variable.idioma]); break; //vendedor
            }

            switch (n_titulo)
            {
                case 0: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[156, Variable.idioma]); break;  //folio
                case 1: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[155, Variable.idioma]); break;  //corte
                case 2: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[146, Variable.idioma]); break;  //descripcion
                case 3: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[163, Variable.idioma]); break; //nombre          
            }

            switch (n_titulo)
            {
                case 0: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[215, Variable.idioma]); break;  //cantidad
                case 1: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[215, Variable.idioma]); break; //cantidad
                case 2: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[216, Variable.idioma]); break; //peso
                case 3: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[215, Variable.idioma]); break; //cantidad           
            }


            paramField[4] = new ReportParameter("SCABE4", Variable.SYS_MSJ[147, Variable.idioma]); //descuento
            paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[221, Variable.idioma]); //devolucion
            paramField[6] = new ReportParameter("SCABE6", Variable.SYS_MSJ[165, Variable.idioma]); //oferta
            paramField[7] = new ReportParameter("GranTotal", "GranTotal");
            paramField[8] = new ReportParameter("FECHA", fecha + "  " + hora1);
            paramField[9] = new ReportParameter("Impuesto", "Impuesto");
            paramField[10] = new ReportParameter("Subtotal", "Subtotal");
            paramField[11] = new ReportParameter("GTOTAL", "GTOTAL");




        }
        public void Reportes_Venta_Desglose(int n_titulo, string fecha, string hora1, ref ReportParameter[] paramField)
        {
            string[] Str_Titulo = new string[6]
            { 
              Variable.SYS_MSJ[130,Variable.idioma].ToUpper(),  //"Reporte de Venta por Fecha",             //0
              Variable.SYS_MSJ[131,Variable.idioma].ToUpper(),  //  "Reporte de Venta por Grupo",        //1
              Variable.SYS_MSJ[132,Variable.idioma],  //  "Reporte de venta por Bascula",         //2
              Variable.SYS_MSJ[133,Variable.idioma],  //  "Reporte de venta por Corte de Ventas", //3 
              Variable.SYS_MSJ[134,Variable.idioma],  //  "Reporte de venta por Vendedor",        //4
              Variable.SYS_MSJ[135,Variable.idioma]};  //  "Reporte de venta por Producto" };      //5

            paramField[0] = new ReportParameter("TITULO", Str_Titulo[n_titulo]);
            paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[216, Variable.idioma]); //peso
            paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[165, Variable.idioma]); //Oferta
            paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[221, Variable.idioma]); //Devolucion
            paramField[4] = new ReportParameter("SCABE4", "TOTAL");   //Variable.SYS_MSJ[217, Variable.idioma]); //subtotal
            paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[148, Variable.idioma]); //Impuestos
            paramField[6] = new ReportParameter("SCABE6", Variable.SYS_MSJ[147, Variable.idioma]); //Descuentos

            switch (n_titulo)
            {
                case 0: { paramField[7] = new ReportParameter("CABE1", Variable.SYS_MSJ[154, Variable.idioma]); } break;  //Fecha
                case 1: { paramField[7] = new ReportParameter("CABE1", Variable.SYS_MSJ[223, Variable.idioma]); } break;  //No. Grupo                
                case 2: { paramField[7] = new ReportParameter("CABE1", Variable.SYS_MSJ[224, Variable.idioma]); } break; //No. Bascula
                case 3: { paramField[7] = new ReportParameter("CABE1", Variable.SYS_MSJ[155, Variable.idioma]); } break;  //coerte de caja
                case 4: { paramField[7] = new ReportParameter("CABE1", Variable.SYS_MSJ[225, Variable.idioma]); } break; //No.Vewndedor
                case 5: { paramField[7] = new ReportParameter("CABE1", Variable.SYS_MSJ[226, Variable.idioma]); } break;  //No. de Producto
            }
            paramField[8] = new ReportParameter("CABE2", Variable.SYS_MSJ[145, Variable.idioma]);  //codigo
            paramField[9] = new ReportParameter("CABE3", Variable.SYS_MSJ[163, Variable.idioma]);  //Nombre
            paramField[10] = new ReportParameter("CABE4", Variable.SYS_MSJ[166, Variable.idioma]); //Precio
            paramField[11] = new ReportParameter("GTOTAL", Variable.SYS_MSJ[222, Variable.idioma]);  //GRAN TOTAL
            paramField[12] = new ReportParameter("FECHA", fecha + "  " + hora1);
            paramField[13] = new ReportParameter("UNIMED", Variable.SYS_MSJ[219, Variable.idioma]); // "Piezas";
            paramField[14] = new ReportParameter("UNIPES", Variable.FOR_UM[Variable.unidad]);

            paramField[15] = new ReportParameter("Subtotal", "Subtotal");
            paramField[16] = new ReportParameter("Impuesto", "Impuesto");
            paramField[17] = new ReportParameter("GranTotal", "GranTotal");

        }
        public void Listados_Maestros_Desglose(int n_titulo, string fecha, string hora1, ref ReportParameter[] paramField)
        {
            string[] Str_Titulo = new string[6]
            { Variable.SYS_MSJ[119,Variable.idioma].ToUpper(),  //"Catalago de Basculas",       //0  166
              Variable.SYS_MSJ[123,Variable.idioma].ToUpper(), //  "Catalogo de Productos",    //1  165
              Variable.SYS_MSJ[120,Variable.idioma].ToUpper(),  //  "Catalogo de Info. Adicional",     //2  164
              Variable.SYS_MSJ[121,Variable.idioma].ToUpper(), //  "Catalogo de Mensajes",     //3
              Variable.SYS_MSJ[122,Variable.idioma].ToUpper(), //  "Catalogo de Ofertas",      //4
              Variable.SYS_MSJ[124,Variable.idioma].ToUpper()}; //  "Catalogo de Vendedores" };    //5

            // paramField = new ReportParameter[8];
            paramField[0] = new ReportParameter("TITULO", Str_Titulo[n_titulo]);

            switch (n_titulo)
            {
                case 0: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[214, Variable.idioma]); break;  //bascula
                case 1: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[145, Variable.idioma]); break;  //codigo
                case 2: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[149, Variable.idioma]); break; //Info adicional
                case 3: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[168, Variable.idioma]); break; //MEnsajes
                case 4: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[165, Variable.idioma]); break; //Ofertas
                case 5: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[175, Variable.idioma]); break; //Vendedores
            }
            switch (n_titulo)
            {
                case 0: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[146, Variable.idioma]); break;  //Descripcion
                case 1: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[162, Variable.idioma]); break;  //Num. PLU
                case 2: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[163, Variable.idioma]); break;  //Nombre
                case 3: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[174, Variable.idioma]); break; //Titulo
                case 4: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[163, Variable.idioma]); break; //Nombre
                case 5: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[163, Variable.idioma]); break;  //Nombre
            }
            switch (n_titulo)
            {
                case 0: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[164, Variable.idioma]); break;  //Num Serie
                case 1: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[146, Variable.idioma]); break; //Descripcion
                case 2: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[146, Variable.idioma]); break; //Descripcion
                case 3: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[168, Variable.idioma]); break; //Mensaje
                case 4: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[228, Variable.idioma]); break; //Fecha Inicial
                case 5: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[152, Variable.idioma]); break; //Mensaje Activo
            }
            if (n_titulo != 2 && n_titulo != 3)
            {
                switch (n_titulo)
                {
                    case 0: paramField[4] = new ReportParameter("SCABE4", Variable.SYS_MSJ[227, Variable.idioma]); break; //"Capacidad";
                    case 1: paramField[4] = new ReportParameter("SCABE4", Variable.SYS_MSJ[166, Variable.idioma]); break;  // "Precio";
                    case 4: paramField[4] = new ReportParameter("SCABE4", Variable.SYS_MSJ[229, Variable.idioma]); break; //Fecha Final
                    case 5: paramField[4] = new ReportParameter("SCABE4", Variable.SYS_MSJ[157, Variable.idioma]); break; // "Meta Activa";
                }
                switch (n_titulo)
                {
                    case 0: paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[230, Variable.idioma]); break; // "Div. Minima";
                    case 1: paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[143, Variable.idioma]); break; // "Caducidad";
                    case 4: paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[147, Variable.idioma]); break;// "Descuento";
                    case 5: paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[158, Variable.idioma]); break; // "Meta";                    
                }

                paramField[7] = new ReportParameter("Subtotal", "Subtotal");
                paramField[8] = new ReportParameter("Impuesto", "Impuesto");
                paramField[9] = new ReportParameter("GranTotal", "GranTotal");


                switch (n_titulo)
                {
                    case 0: paramField[6] = new ReportParameter("SCABE6", "IP"); break; // "Div. Minima";
                    case 1: paramField[6] = new ReportParameter("SCABE6", Variable.SYS_MSJ[173, Variable.idioma]); break;  //"Tipo";                   
                }

                if (n_titulo < 2) { paramField[7] = new ReportParameter("FECHA", fecha + "  " + hora1); }
                else { paramField[6] = new ReportParameter("FECHA", fecha + "  " + hora1); }
            }
            else
            {
                paramField[4] = new ReportParameter("FECHA", fecha + "  " + hora1);
            }
        }
        public void Analisis_Venta(int n_titulo, string fecha, string hora1, ref ReportParameter[] paramField)
        {

            string[] Str_Titulo = new string[4] 
            {
              
              Variable.SYS_MSJ[126,Variable.idioma].ToUpper(), //"Analisis de venta por Grupo", //0
              Variable.SYS_MSJ[127,Variable.idioma].ToUpper(), //"Analisis de venta por Bascula",   //1
              Variable.SYS_MSJ[128,Variable.idioma].ToUpper(), //"Analisis de venta por Producto",  //2
              Variable.SYS_MSJ[129,Variable.idioma].ToUpper()}; //"Analisis de venta por Vendedor"}; //3



            paramField[0] = new ReportParameter("TITULO", Str_Titulo[n_titulo]);

            switch (n_titulo)
            {
                case 0: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[376, Variable.idioma]); break;  //Departamento
                case 1: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[164, Variable.idioma]); break;  //numero de serie
                case 2: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[145, Variable.idioma]); break; //codigo
                case 3: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[175, Variable.idioma]); break; //vendedor
            }
            switch (n_titulo)
            {
                case 0: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[156, Variable.idioma]); break;  //folio
                case 1: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[155, Variable.idioma]); break;  //corte
                case 2: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[146, Variable.idioma]); break;  //descripcion
                case 3: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[163, Variable.idioma]); break; //nombre          
            }

            switch (n_titulo)
            {
                case 0: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[215, Variable.idioma]); break;  //cantidad
                case 1: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[215, Variable.idioma]); break; //cantidad
                case 2: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[216, Variable.idioma]); break; //peso
                case 3: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[215, Variable.idioma]); break; //cantidad           
            }
            paramField[4] = new ReportParameter("SCABE4", Variable.SYS_MSJ[147, Variable.idioma]); //descuento
            paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[221, Variable.idioma]); //devolucion
            paramField[6] = new ReportParameter("SCABE6", Variable.SYS_MSJ[165, Variable.idioma]); //oferta
            paramField[7] = new ReportParameter("GTOTAL", Variable.SYS_MSJ[222, Variable.idioma]);  //GRAN TOTAL
            paramField[8] = new ReportParameter("FECHA", fecha + "  " + hora1);

            if (n_titulo == 2)
            {
                paramField[9] = new ReportParameter("UNIMED", Variable.SYS_MSJ[219, Variable.idioma]); // "Piezas";
                paramField[10] = new ReportParameter("UNIPES", Variable.FOR_UM[Variable.unidad]);
            }
        }
        public void Reportes_Venta(int n_titulo, string fecha, string hora1, ref ReportParameter[] paramField)
        {
            string[] Str_Titulo = new string[6]
            { Variable.SYS_MSJ[130,Variable.idioma].ToUpper(),  //"Reporte de Venta por Fecha",             //0
              Variable.SYS_MSJ[131,Variable.idioma].ToUpper(),  //  "Reporte de Venta por Grupo",        //1
              Variable.SYS_MSJ[132,Variable.idioma],  //  "Reporte de venta por Bascula",         //2
              Variable.SYS_MSJ[133,Variable.idioma],  //  "Reporte de venta por Corte de Ventas", //3 
              Variable.SYS_MSJ[134,Variable.idioma],  //  "Reporte de venta por Vendedor",        //4
              Variable.SYS_MSJ[135,Variable.idioma]};  //  "Reporte de venta por Producto" };      //5

            paramField[0] = new ReportParameter("TITULO", Str_Titulo[n_titulo]);
            paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[216, Variable.idioma]); //peso
            paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[165, Variable.idioma]); //Oferta
            paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[221, Variable.idioma]); //Devolucion
            paramField[4] = new ReportParameter("SCABE4", "TOTAL");   //Variable.SYS_MSJ[217, Variable.idioma]); //subtotal
            paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[148, Variable.idioma]); //Impuestos
            paramField[6] = new ReportParameter("SCABE6", Variable.SYS_MSJ[147, Variable.idioma]); //Descuentos
            switch (n_titulo)
            {
                case 0: { paramField[7] = new ReportParameter("CABE1", Variable.SYS_MSJ[154, Variable.idioma]); } break;  //Fecha
                case 1: { paramField[7] = new ReportParameter("CABE1", Variable.SYS_MSJ[223, Variable.idioma]); } break;  //No. Grupo                
                case 2: { paramField[7] = new ReportParameter("CABE1", Variable.SYS_MSJ[224, Variable.idioma]); } break; //No. Bascula
                case 3: { paramField[7] = new ReportParameter("CABE1", Variable.SYS_MSJ[155, Variable.idioma]); } break;  //coerte de caja
                case 4: { paramField[7] = new ReportParameter("CABE1", Variable.SYS_MSJ[225, Variable.idioma]); } break; //No.Vewndedor
                case 5: { paramField[7] = new ReportParameter("CABE1", Variable.SYS_MSJ[226, Variable.idioma]); } break;  //No. de Producto
            }
            paramField[8] = new ReportParameter("CABE2", Variable.SYS_MSJ[145, Variable.idioma]);  //codigo
            paramField[9] = new ReportParameter("CABE3", Variable.SYS_MSJ[163, Variable.idioma]);  //Nombre
            paramField[10] = new ReportParameter("CABE4", Variable.SYS_MSJ[166, Variable.idioma]); //Precio
            paramField[11] = new ReportParameter("GTOTAL", Variable.SYS_MSJ[222, Variable.idioma]);  //GRAN TOTAL
            paramField[12] = new ReportParameter("FECHA", fecha + "  " + hora1);
            paramField[13] = new ReportParameter("UNIMED", Variable.SYS_MSJ[219, Variable.idioma]); // "Piezas";
            paramField[14] = new ReportParameter("UNIPES", Variable.FOR_UM[Variable.unidad]);
        }
        public void Listados_Maestros(int n_titulo, string fecha, string hora1, ref ReportParameter[] paramField)
        {
            string[] Str_Titulo = new string[6]
            { Variable.SYS_MSJ[119,Variable.idioma].ToUpper(),  //"Catalago de Basculas",       //0  166
              Variable.SYS_MSJ[123,Variable.idioma].ToUpper(), //  "Catalogo de Productos",    //1  165
              Variable.SYS_MSJ[120,Variable.idioma].ToUpper(),  //  "Catalogo de Info. Adicional",     //2  164
              Variable.SYS_MSJ[121,Variable.idioma].ToUpper(), //  "Catalogo de Mensajes",     //3
              Variable.SYS_MSJ[122,Variable.idioma].ToUpper(), //  "Catalogo de Ofertas",      //4
              Variable.SYS_MSJ[124,Variable.idioma].ToUpper()}; //  "Catalogo de Vendedores" };    //5


            paramField[0] = new ReportParameter("TITULO", Str_Titulo[n_titulo]);

            switch (n_titulo)
            {
                case 0: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[214, Variable.idioma]); break;  //bascula
                case 1: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[145, Variable.idioma]); break;  //codigo
                case 2: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[149, Variable.idioma]); break; //Info adicional
                case 3: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[168, Variable.idioma]); break; //MEnsajes
                case 4: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[165, Variable.idioma]); break; //Ofertas
                case 5: paramField[1] = new ReportParameter("SCABE1", Variable.SYS_MSJ[175, Variable.idioma]); break; //Vendedores
            }
            switch (n_titulo)
            {
                case 0: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[146, Variable.idioma]); break;  //Descripcion
                case 1: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[162, Variable.idioma]); break;  //Num. PLU
                case 2: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[163, Variable.idioma]); break;  //Nombre
                case 3: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[174, Variable.idioma]); break; //Titulo
                case 4: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[163, Variable.idioma]); break; //Nombre
                case 5: paramField[2] = new ReportParameter("SCABE2", Variable.SYS_MSJ[163, Variable.idioma]); break;  //Nombre
            }
            switch (n_titulo)
            {
                case 0: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[164, Variable.idioma]); break;  //Num Serie
                case 1: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[146, Variable.idioma]); break; //Descripcion
                case 2: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[146, Variable.idioma]); break; //Descripcion
                case 3: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[168, Variable.idioma]); break; //Mensaje
                case 4: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[228, Variable.idioma]); break; //Fecha Inicial
                case 5: paramField[3] = new ReportParameter("SCABE3", Variable.SYS_MSJ[152, Variable.idioma]); break; //Mensaje Activo
            }
            if (n_titulo != 2 && n_titulo != 3)
            {
                switch (n_titulo)
                {
                    case 0: paramField[4] = new ReportParameter("SCABE4", Variable.SYS_MSJ[227, Variable.idioma]); break; //"Capacidad";
                    case 1: paramField[4] = new ReportParameter("SCABE4", Variable.SYS_MSJ[166, Variable.idioma]); break;  // "Precio";
                    case 4: paramField[4] = new ReportParameter("SCABE4", Variable.SYS_MSJ[229, Variable.idioma]); break; //Fecha Final
                    case 5: paramField[4] = new ReportParameter("SCABE4", Variable.SYS_MSJ[157, Variable.idioma]); break; // "Meta Activa";
                }
                switch (n_titulo)
                {
                    case 0: paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[230, Variable.idioma]); break; // "Div. Minima";
                    case 1: paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[143, Variable.idioma]); break; // "Caducidad";
                    case 4: paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[147, Variable.idioma]); break;// "Descuento";
                    case 5: paramField[5] = new ReportParameter("SCABE5", Variable.SYS_MSJ[158, Variable.idioma]); break; // "Meta";                    
                }
                switch (n_titulo)
                {
                    case 0: paramField[6] = new ReportParameter("SCABE6", "IP"); break; // "Div. Minima";
                    case 1: paramField[6] = new ReportParameter("SCABE6", Variable.SYS_MSJ[173, Variable.idioma]); break;  //"Tipo";                   
                }

                if (n_titulo < 2) { paramField[7] = new ReportParameter("FECHA", fecha + "  " + hora1); }
                else { paramField[6] = new ReportParameter("FECHA", fecha + "  " + hora1); }
            }
            else
            {
                paramField[4] = new ReportParameter("FECHA", fecha + "  " + hora1);
            }
            if (paramField.Length > 8) { paramField[8] = new ReportParameter("Impuesto", "Impuesto"); }
        }
    }
}
