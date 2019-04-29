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
    class UserLoginPlatformStartup : MonoBehaviour
    {
        protected static UserLoginPlatformStartup t_instance;
        public static UserLoginPlatformStartup GetInstance() {
            if (t_instance == null)
                return new GameObject("Ebenit.LoginPlatformStartup").AddComponent<UserLoginPlatformStartup>();

            return t_instance;
        }

        protected User t_user = null;
        protected uint t_platform_id = 0;

        protected ApiManager t_api_manager = null;

        protected virtual void Start() {
            if (t_instance != null) {
                Destroy(this.gameObject);
                return;
            }
            t_instance = this;
            DontDestroyOnLoad(this.gameObject);

            t_api_manager = ApiManager.getInstance();

            if (t_api_manager.pt_online) {
                // if badly used, do not try login if already logged in
                return;
            }

            StartCoroutine(doInitilization());
        }

        protected virtual IEnumerator doInitilization() {
            yield return initializeUser();

            t_api_manager.initializeApiPlatform(t_user, t_platform_id);
        }

        protected virtual IEnumerator initializeUser() {
            // Steam
#if EBENIT_API_LOGIN_STEAM
            t_user = new User(SteamUser.GetSteamID().m_SteamID.ToString(), SteamFriends.GetPersonaName());
            t_platform_id = 1;
#endif

            // GooglePlay + AppStore user
#if (EBENIT_API_LOGIN_GOOGLE_PLAY || EBENIT_API_LOGIN_APP_STORE)
            if (!Social.localUser.authenticated) {
                Social.localUser.Authenticate((bool success) =>
                {
                });
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
