using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace InstagramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Constants.verifyData())
            {
                Process.Start(Application.ExecutablePath);
                Environment.Exit(0);
            }
            Logging.timeStamp();
            API.buildApi();
            Control controller = new Control();
            if (Login.login())
            {
                Thread t = new Thread(new ThreadStart(controller.mainThread));
                t.Priority = ThreadPriority.Highest;
                t.Start();
            }
            else
            {
                Logging.log("Failed to login");
            }
            Console.ReadLine();
        }
    }
}
