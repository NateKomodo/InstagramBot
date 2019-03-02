using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Logger;
using System;

namespace InstagramBot
{
    public class API
    {
        public static IInstaApi api;

        public static void buildApi()
        {
            var data = new UserSessionData
            {
                UserName = Constants.username,
                Password = Constants.password
            };

            api = InstaApiBuilder.CreateBuilder()
                .SetUser(data)
                .UseLogger(new DebugLogger(LogLevel.Exceptions))
                .Build();
            Logging.log("API built");
        }
    }
}
