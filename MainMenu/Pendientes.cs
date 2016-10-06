using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainMenu
{
    public class Pendientes
    {
        /// <summary>
        /// 
        /// </summary>
        public static void CrearLLave()
        {

            if (StatusPendientes() == 2)
            {
                Microsoft.Win32.RegistryKey key;
                key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Pendientes");
                key.SetValue("Pendientes", "0");
                key.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static int StatusPendientes()
        {
            int res = -1;
            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Pendientes"))
            {
                if (key != null)
                {
                    res = Convert.ToInt32(key.GetValue("Pendientes"));
                }
                else
                {
                    res = 2;
                }
            }
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        public static void HabilitarPendientes()
        {
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Pendientes", "Pendientes", "1");//Actualiza la llave de registro 
        }
        /// <summary>
        /// 
        /// </summary>
        public static void DeshabilitarPendientes()
        {
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Pendientes", "Pendientes", "0");//Actualiza la llave de registro 
        }
    }
}
