using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;


namespace PtoVenta
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Puente puent;
            
            if (IsExecutingApplication() == false)
              puent = new Puente();
            else
                MessageBox.Show("La app se encuentra en ejecución");
        }
       
        private static bool IsExecutingApplication()
        {
            // Proceso actual
            Process currentProcess = Process.GetCurrentProcess();

            // Matriz de procesos
            Process[] processes = Process.GetProcesses();

            // Recorremos los procesos en ejecución
            foreach (Process p in processes)
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
