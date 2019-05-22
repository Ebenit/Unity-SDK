using Ebenit.Containers;
using UnityEngine;
using Ebenit.Managers;
using System.Collections;

#if (EBENIT_API_LOGIN_STEAM && !DISABLESTEAMWORKS)
using Steamworks;
#endif

#if EBENIT_API_LOGIN_GOOGLE_PLAY
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

#if EBENIT_API_LOGIN_APP_STORE
using UnityEngine.SocialPlatforms.GameCenter;
#endif

namespace Ebenit
{
    /// <summary>
    /// Template for login with platform specific authentication.
    /// </summary>
    public class UserLoginPlatformStartup : MonoBehaviour
    {
        /// <summary>
        /// User credentials.
        /// </summary>
        protected User t_user = null;
        /// <summary>
        /// Platform ID in Ebenit API.
        /// </summary>
        protected uint t_platform_id = 0;

        /// <summary>
        /// Instance of ApiManager.
        /// </summary>
        protected ApiManager t_api_manager = null;

        protected virtual IEnumerator Start() {
            t_api_manager = ApiManager.getInstance();

            if (t_api_manager.pt_online) {
                // do not try login if already logged in
                yield break;
            }

            yield return initializeUser();

            t_api_manager.initializeApiPlatform(t_user, t_platform_id);
        }

        /// <summary>
        /// Virtual coroutine to set the User credentials and determinate the platform ID.
        /// 
        /// Defaultly the action is based on the directives:
        ///     EBENIT_API_LOGIN_STEAM
        ///         t_platform_id = 1
        ///         User ID = SteamUser.GetSteamID().m_SteamID.ToString()
        ///         User nickname = SteamFriends.GetPersonaName()
        ///     EBENIT_API_LOGIN_GOOGLE_PLAY
        ///         t_platform_id = 2
        ///         User ID = Social.localUser.id
        ///         User nickname = Social.localUser.userName
        ///     EBENIT_API_LOGIN_APP_STORE
        ///         t_platform_id = 3
        ///         User ID = Social.localUser.id
        ///         User nickname = Social.localUser.userName
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator initializeUser() {
            // Steam
#if EBENIT_API_LOGIN_STEAM
            t_user = new User(SteamUser.GetSteamID().m_SteamID.ToString(), SteamFriends.GetPersonaName());
            t_platform_id = 1;
#endif

            // GooglePlay + AppStore user
#if (EBENIT_API_LOGIN_GOOGLE_PLAY || EBENIT_API_LOGIN_APP_STORE)
            if (!Social.localUser.authenticated) {
                bool done = false;

                Social.localUser.Authenticate((bool success) =>
                {
                    done = true;
                });

                while (!done) {
                    yield return null;
                }
            }

            if (Social.localUser.authenticated) {
                t_user = new User(Social.localUser.id, Social.localUser.userName);
            }
#endif
            // GooglePlay platform
#if EBENIT_API_LOGIN_GOOGLE_PLAY
            t_platform_id = 2;
#endif
            // AppStore platform
#if EBENIT_API_LOGIN_APP_STORE
            t_platform_id = 3;
#endif

            yield return null;
        }
    }
}
