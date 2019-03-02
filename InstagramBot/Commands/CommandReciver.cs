using InstagramApiSharp;
using InstagramApiSharp.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramBot
{
    public class CommandReciver
    {
        public async Task recvCommands()
        {
            var inbox = await API.api.MessagingProcessor.GetDirectInboxAsync(PaginationParameters.Empty);
            if (inbox.Value == null)
            {
                Logging.log("Get inbox failed!");
                return;
            }
            foreach (var thread in inbox.Value.Inbox.Threads.Where(t => t.HasUnreadMessage))
            {
                thread.HasUnreadMessage = false;
                await doCommand(thread);
            }
        }

        public async Task doCommand(InstaDirectInboxThread thread)
        {
            if (thread.Items.Last().UserId == API.api.GetCurrentUserAsync().Result.Value.Pk)
            {
                return;
            }
            if (thread.Items.Last().ItemType != InstaDirectThreadItemType.Text)
            {
                return;
            }
            if (thread.IsGroup)
            {
                Logging.log(thread.Title + ": " + thread.Items.Last().Text);
                await APIFunctions.sendDMID(thread.ThreadId, CommandProcess.Proccess(thread.Items.Last(), thread));
            }
            else
            {
                Logging.log(thread.Users.FirstOrDefault().UserName + ": " + thread.Items.Last().Text);
                await APIFunctions.sendDM(thread.Users.FirstOrDefault().UserName, CommandProcess.Proccess(thread.Items.Last(), thread));
            }
        }
    }
}
