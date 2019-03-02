using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramBot
{
    public class Logging
    {
        public static void log(string msg)
        {
            CheckFile();
            Console.WriteLine($"{DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss")} {msg}");
            try
            {
                using (StreamWriter sw = File.AppendText(Constants.logPath))
                {
                    sw.WriteLine($"{DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss")} {msg}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during log write: {e}");
            }
        }
        public static void timeStamp()
        {
            CheckFile();
            try
            {
                using (StreamWriter sw = File.AppendText(Constants.logPath))
                {
                    sw.WriteLine($"{Environment.NewLine}{DateTime.Now}{Environment.NewLine}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during timestamp write: {e}");
            }
        }
        public static void CheckFile()
        {
            try
            {
                if (!File.Exists(Constants.logPath))
                {
                    File.Create(Constants.logPath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during log creation: {e}");
            }
        }
    }
}
