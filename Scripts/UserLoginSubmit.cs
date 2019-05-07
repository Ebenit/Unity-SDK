using Ebenit.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Ebenit
{
    /// <summary>
    /// Template for login script in Ebenit API.
    /// </summary>
    public class UserLoginSubmit : MonoBehaviour
    {
#pragma warning disable 0649
        /// <summary>
        /// User e-mail input.
        /// </summary>
        public InputField p_email_input;
        /// <summary>
        /// User password input.
        /// </summary>
        public InputField p_password_input;
#pragma warning restore 0649

        /// <summary>
        /// Platform ID in Ebenit API. After calling the submit, this value will be set into ApiManager.
        /// </summary>
        protected uint t_platform_id;

        /// <summary>
        /// Virtual method to set the platform ID.
        /// 
        /// Defaultly the platform is set based on the directives:
        ///     EBENIT_API_LOGIN_STEAM = 1
        ///     EBENIT_API_LOGIN_GOOGLE_PLAY = 2
        ///     EBENIT_API_LOGIN_APP_STORE = 3
        ///     other or none = 5 (Ebenit)
        /// </summary>
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

        /// <summary>
        /// Submits the Login request. WARNING: This function does not wait until the request is finished.
        /// 
        /// If any of the inputs is not set this method will do nothing.
        /// This method calls the determinatePlatform() method before submiting the Login request.
        /// </summary>
        public virtual void submit() {
            if (p_email_input == null || p_password_input == null) {
                return;
            }

            determinatePlatform();

            ApiManager.getInstance().initializeApi(t_platform_id, p_email_input.text, p_password_input.text);
        }
    }
}
