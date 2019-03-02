using InstagramApiSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramBot
{
    public class Cogs
    {
        public void doCogs()
        {
            #pragma warning disable CS4014
            proccFollowRequests();
            #pragma warning restore CS4014
        }

        async Task proccFollowRequests()
        {
            var currentFollowsers = await API.api.UserProcessor.GetCurrentUserFollowersAsync(PaginationParameters.Empty);
            foreach (var follower in currentFollowsers.Value)
            {
                var friendshipStatus = await API.api.UserProcessor.GetFriendshipStatusAsync(follower.Pk);
                if (!friendshipStatus.Value.Following && !friendshipStatus.Value.OutgoingRequest)
                {
                    var followUser = await API.api.UserProcessor.FollowUserAsync(follower.Pk);
                    string q = "\"";
                    await APIFunctions.sendDM(follower.UserName, $"AutoFollow: Automatically followed back. Reply with {q}help{q} for help.");

                    Logging.log("AutoFollowed back " + follower.UserName);
                }
            }
        }
    }
}
