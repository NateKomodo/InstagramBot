using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramBot
{
    public class Control
    {

        public void mainThread()
        {
            #pragma warning disable CS4014
            loop();
            #pragma warning restore CS4014
        }

        async Task loop()
        {
            CommandReciver recv = new CommandReciver();
            Cogs cogs = new Cogs();
            while (true)
            {
                try
                {
                    new Thread(new ThreadStart(cogs.doCogs)).Start();
                    await recv.recvCommands();
                    await Task.Delay(1500);
                }
                catch (Exception ex)
                {
                    Logging.log($"Error: {ex.Message} {ex.InnerException} {ex.StackTrace}");
                }
            }
        }
    }
}
