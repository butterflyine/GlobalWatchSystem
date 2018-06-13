using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

namespace GPRSServer
{
    static class Program
    {

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "GRPSServer_123", out createdNew))
            {
                if (createdNew)
                {
                    Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                }
                else 
                {
                    return;

                }
            }
            
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            string path = Application.StartupPath + "\\Log";
            string fileName = path + "\\" + DateTime.Today.ToString("yyyy-MM-dd") + "_exception.log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (StreamWriter sw = new StreamWriter(fileName, true))
            {
                sw.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "(Not UI): " + ex.Message + ": " + ex.StackTrace);
                sw.WriteLine();
                sw.Write("Inner Exception: " + ex.InnerException.Message);
            }

            if (e.IsTerminating)
            {
                Application.Restart();
            }
            
        }
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            string path = Application.StartupPath + "\\Log";
            string fileName = path + "\\" + DateTime.Today.ToString("yyyy-MM-dd") + "_exception.log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (StreamWriter sw = new StreamWriter(fileName, true))
            {
                sw.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "(UI): " + e.Exception.Message + ": " + e.Exception.StackTrace);
                sw.WriteLine();
                sw.Write("Inner Exception: " + e.Exception.InnerException.Message);
            }
            Application.Restart();
        }
    }
}
