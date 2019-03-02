using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramBot
{
    public class Login
    {
        public static bool login()
        {
            string stateFile = Constants.statePath;

            try
            {
                if (File.Exists(stateFile))
                {
                    Logging.log("Loading state from file");
                    using (var fs = File.OpenRead(stateFile))
                    {
                        API.api.LoadStateDataFromStream(fs);
                    }
                }

                if (!API.api.IsUserAuthenticated)
                {
                    Logging.log("Logging in with creds");
                    var logInResult = API.api.LoginAsync().Result;
                    if (!logInResult.Succeeded)
                    {
                        Logging.log($"Unable to login: {logInResult.Info.Message}");
                        return false;
                    }
                }
                var state = API.api.GetStateDataAsStream();
                using (var fileStream = File.Create(stateFile))
                {
                    state.Seek(0, SeekOrigin.Begin);
                    state.CopyTo(fileStream);
                }
                Logging.log("Logged in");
                return true;
            } 
            catch (Exception e)
            {
                Logging.log($"Error during login: {e}");
                return false;
            }
        }
    }
}
