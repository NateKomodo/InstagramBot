using System;
using System.Threading.Tasks;

namespace InstagramBot
{
    public class APIFunctions
    {
        public static async Task sendDM(string desireUsername, string content)
        {
            var user = await API.api.UserProcessor.GetUserAsync(desireUsername);
            var userId = user.Value.Pk.ToString();
            var directText = await API.api.MessagingProcessor.SendDirectTextAsync(userId, null, content);
            Logging.log($"Sending message to {desireUsername}: {content}");
        }

        public static async Task sendDMGroup(string id, string content)
        {
            var directText = await API.api.MessagingProcessor.SendDirectTextAsync(null, id, content);
            Logging.log($"Sending message to group with ID {id}: {content}");
        }
    }
}
