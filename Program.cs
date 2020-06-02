using System;
using System.Windows.Forms;

namespace tcpServer
{
    static class Program
    {
        public static Server form1;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(form1 = new Server());


        }

    }
}
