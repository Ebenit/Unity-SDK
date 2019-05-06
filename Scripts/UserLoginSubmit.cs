using Ebenit.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Ebenit
{
    public class UserLoginSubmit : MonoBehaviour
    {
#pragma warning disable 0649
        public InputField p_email_input;
        public InputField p_password_input;
#pragma warning restore 0649

        protected uint t_platform_id;

        protected virtual void determinatePlatform() {
            t_platform_id = 5; // ebenit platform

            // Steam
#if EBENIT_API_LOGIN_STEAM
            t_platform_id = 1;
#endif
            // GooglePlay
#if EBENIT_API_LOGIN_GOOGLE_PLAY
            t_platform_id = 2;
#endif
            // AppStore
#if EBENIT_API_LOGIN_APP_STORE
            t_platform_id = 3;
#endif
        }

        public virtual void submit() {
            if (p_email_input == null || p_password_input == null) {
                return;
            }

            determinatePlatform();

            ApiManager.getInstance().initializeApi(t_platform_id, p_email_input.text, p_password_input.text);
        }
    }
}
