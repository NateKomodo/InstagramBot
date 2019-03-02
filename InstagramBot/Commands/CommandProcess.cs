using System;
using System.Linq;
using InstagramApiSharp;
using InstagramApiSharp.Classes.Models;

namespace InstagramBot
{
    public class CommandProcess
    {
        public static string Proccess(InstaDirectInboxItem item)
        {
            Constants.reqCount++;
            string cmd = item.Text.ToLower();
            string q = "\"";
            if (cmd == "ping")
            {
                var now = DateTime.Now;
                var timetook = now.Subtract(item.TimeStamp);
                return $"Command sent at {item.TimeStamp.TimeOfDay} and recieved at {now.TimeOfDay.ToString("hh\\:mm\\:ss")}" + Environment.NewLine 
                    + $"API Latency: {(int)timetook.TotalMilliseconds}ms" + Environment.NewLine
                    + $"TTP including latency: {(int)timetook.TotalMilliseconds * 2}ms";
            }
            if (cmd == "like")
            {
                var posts = API.api.UserProcessor.GetUserMediaByIdAsync(item.UserId, PaginationParameters.Empty).Result.Value;
                var like = API.api.MediaProcessor.LikeMediaAsync(posts.FirstOrDefault().Pk);
                return "NGL thats pretty desperate if your getting a bot to like your posts";
            }
            if (cmd == "stats")
            {
                var uptime = DateTime.Now.Subtract(Constants.started);
                return $"Uptime: {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s" + Environment.NewLine
                    + $"Total requests served: {Constants.reqCount}";
            }
            if (cmd == "help")
            {
                return "Komosys help: " + Environment.NewLine
                    + $"{q}ping{q} - gets message response times" + Environment.NewLine
                    + $"{q}like{q} - likes your most recent post" + Environment.NewLine
                    + $"{q}stats{q} - gets stats about current bot session" + Environment.NewLine
                    + $"{q}help{q} - displays this" + Environment.NewLine
                    + "NOTE: Due to API response times, commands may take up to 5s to process";
            }
            return $"Unknown command, type {q}help{q} for help";
        }
    }
}
