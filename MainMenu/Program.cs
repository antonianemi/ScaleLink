using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;

namespace MainMenu
{
    static class Program
    {
        
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Variable Glob = new Variable();
            Application.SetCompatibleTextRenderingDefault(true);
            string conect = new MainMenu.Properties.Settings().bdConexionString;
            //if (Thread.CurrentThread.CurrentCulture.Name == "es-MX") Variable.idioma = 0;
            //if (Thread.CurrentThread.CurrentCulture.Name == "en-US") Variable.idioma = 1;

            OleDbDataReader config = new ADOutil().Obtiene_Dato("Select IPLocal, UM, formato_fecha, idioma,formato_moneda,HabilitarPubli,pos_inter,ActivarFrecuencia from DatosGral", conect);
           
            if (config.Read())
            {
                Variable.unidad = config.GetInt16(1);
                Variable.ffecha = config.GetInt16(2);
                Variable.idioma = config.GetInt16(3);
                Variable.moneda = config.GetInt16(4);
                Variable.Habilitar_PubliWLSD = Convert.ToSByte(config.GetBoolean(5));
                Variable.pos_intervalo = config.GetInt16(6);
                Variable.Activar_Frecuencia = Convert.ToSByte(config.GetBoolean(7));
                if (Variable.moneda == 0) Variable.n_decimal = 2;
                else if (Variable.moneda == 1) Variable.n_decimal = 0;
                else if (Variable.moneda == 2) Variable.n_decimal = 1;
                else if (Variable.moneda == 3) Variable.n_decimal = 3;
                Variable.F_Decimal = Variable.FOR_MONEDA[Variable.moneda];
                Variable.idioma = 0;
            }
            else
            {
                //Configurar idioma y formato de fecha.
                //Configuracion de idioma para ingles
                //Variable.idioma = 0;
                //Variable.ffecha = 0;

                //Asi estaba antes esta version de codigo.
                if (Thread.CurrentThread.CurrentCulture.Name == "es-MX")
                {
                    Variable.idioma = 0;
                    Variable.ffecha = 0;
                }

                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                {
                    Variable.idioma = 1;
                    Variable.ffecha = 1;
                }

            }
            config.Close();

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Variable.FOR_IDIOMA[Variable.idioma]);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Variable.FOR_IDIOMA[Variable.idioma]);

            Glob.Cargar_Mensajes();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (IsExecutingApplication() == false)
                Application.Run(new Form1());
            else
                MessageBox.Show(Variable.SYS_MSJ[298, Variable.idioma]);  
            
        }
            
            /*if (IsExecutingApplication() == false)
                Application.Run(new Form1());
            else
                MessageBox.Show(Variable.SYS_MSJ[298, Variable.idioma]);  
            
        }*/

        private static bool IsExecutingApplication()
        {
            // Proceso actual
            System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();

            // Matriz de procesos
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();

            // Recorremos los procesos en ejecución
            foreach (System.Diagnostics.Process p in processes)
            {
                if (p.Id != currentProcess.Id)
                {
                    if (p.ProcessName == currentProcess.ProcessName)
                    {
                        return true;
                    }
                }
            }
            return false;
        } 
    
    }
}
