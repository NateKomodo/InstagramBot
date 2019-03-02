using System;
using System.Linq;
using InstagramApiSharp;
using InstagramApiSharp.Classes.Models;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace InstagramBot
{
    public class CommandProcess
    {
        public static string Proccess(InstaDirectInboxItem item, InstaDirectInboxThread thread)
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
                if (posts.FirstOrDefault() == null)
                {
                    return "You dont have any posts or you have not accepted the follow request.";
                }
                var like = API.api.MediaProcessor.LikeMediaAsync(posts.FirstOrDefault().Pk);
                return "NGL thats pretty desperate if your getting a bot to like your posts";
            }
            if (cmd == "stats")
            {
                var uptime = DateTime.Now.Subtract(Constants.started);
                return $"Uptime: {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s" + Environment.NewLine
                    + $"Total requests served: {Constants.reqCount}";
            }
            if (cmd.StartsWith("whois"))
            {
                var parts = cmd.Split(' ');

                if (parts.Length == 1) return $"Please provide a user parameter. E.g. {q}whois ktechsys{q}";

                var profile = API.api.UserProcessor.GetUserInfoByUsernameAsync(parts[1]);

                if (profile.Result.Value == null) return "User does not exist.";

                var nl = Environment.NewLine;

                var data = profile.Result.Value;
                var json = new JavaScriptSerializer().Serialize(data);

                var requester = API.api.UserProcessor.GetUserInfoByIdAsync(item.UserId).Result.Value.Username;
                APIFunctions.sendDM(profile.Result.Value.Username, $"A whois lookup was requested on your account by user {requester}");

                var ID = Pastebin(json, Constants.devkey).Split(new[] { ".com/" }, StringSplitOptions.None)[1];

                var link = $"https://pastebin.com/raw/{ID}";

                API.api.MessagingProcessor.SendDirectLinkAsync("Click the link below to view results", link, thread.ThreadId);

                return "Please be aware they have been informed of the lookup";     
            }
            if (cmd == "source")
            {
                API.api.MessagingProcessor.SendDirectLinkAsync("Click the link below to go to github", "https://github.com/NateKomodo/InstagramBot", thread.ThreadId);
                return null;
            }
            if (cmd == "help")
            {
                return "Komosys help: " + Environment.NewLine
                    + $"{q}ping{q} - gets message response times" + Environment.NewLine
                    + $"{q}like{q} - likes your most recent post" + Environment.NewLine
                    + $"{q}stats{q} - gets stats about current bot session" + Environment.NewLine
                    + $"{q}whois <name>{q} - performs instagram lookup on a person" + Environment.NewLine
                    + $"{q}source{q} - sends link to source code" + Environment.NewLine
                    + $"{q}help{q} - displays this" + Environment.NewLine
                    + "NOTE: Due to API response times, commands may take up to 5s to process";
            }
            return $"Unknown command, type {q}help{q} for help";
        }
        static string Pastebin(string code, string devkey)
        {
            if (string.IsNullOrEmpty(devkey))
                return null;
            var req = System.Net.WebRequest.Create("http://pastebin.com/api/api_post.php");
            req.Timeout = 20000;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            string postString = $"api_dev_key={devkey}&api_option=paste&api_paste_code={code}";
            using (StreamWriter reqStream = new StreamWriter(req.GetRequestStream()))
            {
                reqStream.Write(postString);
            }
            try
            {
                using (var resp = req.GetResponse())
                using (var rdr = new StreamReader(resp.GetResponseStream()))
                {
                    string rslt = rdr.ReadToEnd();
                    return rslt;
                }
            }
            catch { return null; }
        }
    }
}
