using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramBot
{
    public static class Constants
    {
        public static readonly string botPath = "Bot";

        public static readonly string statePath = @"Bot\state.bin";

        public static readonly string passwordPath = @"Bot\creds.txt";

        public static readonly string logPath = @"Bot\log.txt";

        public static string username;

        public static string password;

        public static DateTime started;

        public static int reqCount;

        public static string devkey;

        public static bool verifyData()
        {
            started = DateTime.Now;
            try
            {
                bool flag = true;
                if (!Directory.Exists(botPath))
                {
                    Console.WriteLine("Bot path does not exist, creating");
                    Directory.CreateDirectory("Bot");
                    File.Create(passwordPath);
                    flag = false;
                }

                if (!File.Exists(passwordPath))
                {
                    Console.WriteLine("Password path does not exist, creating");
                    File.Create(passwordPath);
                    flag = false;
                }
                if (!File.Exists(logPath))
                {
                    Console.WriteLine("Log path does not exist, creating");
                    File.Create(logPath);
                    flag = false;
                }
                string line;
                int counter = 0;
                StreamReader file = new StreamReader(passwordPath);
                while ((line = file.ReadLine()) != null)
                {
                    if (counter == 0) username = line;
                    if (counter == 1) password = line;
                    if (counter == 2) devkey = line;
                    counter++;
                }
                Console.WriteLine("Loaded credentials");
                file.Close();
                return flag;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                return false;
            }
        }
    }
}
