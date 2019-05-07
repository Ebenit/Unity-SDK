using Ebenit.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Ebenit
{
    /// <summary>
    /// Template for registration script in Ebenit API.
    /// </summary>
    public class UserNewAliasSubmit : MonoBehaviour
    {
#pragma warning disable 0649
        /// <summary>
        /// User e-mail input.
        /// </summary>
        public InputField p_email_input;
        /// <summary>
        /// User nickname input.
        /// </summary>
        public InputField p_nickname_input;
        /// <summary>
        /// User password input.
        /// </summary>
        public InputField p_password_input;
        /// <summary>
        /// User password (check) input. It is recommended to use different inputs to lesser the chance of user error.
        /// </summary>
        public InputField p_password_check_input;

        /// <summary>
        /// GDPR toggle.
        /// </summary>
        public Toggle p_gdpr_toggle;
#pragma warning restore 0649

        /// <summary>
        /// Result of the New Alias request.
        /// </summary>
        public ApiManager.NewAliasResult pt_result {
            get; protected set;
        }

        /// <summary>
        /// Platform ID in Ebenit API to which the New Alias request is sent. After calling the submit, this value will be set into ApiManager.
        /// (You cannot registrate to different platform than you are logged in.)
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
        /// Submits the New Alias request. WARNING: This function does not wait until the request is finished.
        /// 
        /// If any of the inputs is not set or the GDPR toggle is not on then this method will result in pt_result = ApiManager.NewAliasResult.INCORRECT_DATA_ERROR;.
        /// This method calls the determinatePlatform() method before submiting the New Alias request.
        /// </summary>
        public virtual void submit() {
            if (p_gdpr_toggle == null || !p_gdpr_toggle.isOn) {
                pt_result = ApiManager.NewAliasResult.INCORRECT_DATA_ERROR;
                return;
            }

            if (p_email_input == null || p_nickname_input == null || p_password_input == null || p_password_check_input == null) {
                pt_result = ApiManager.NewAliasResult.INCORRECT_DATA_ERROR;
                return;
            }

            determinatePlatform();

            ApiManager.getInstance().userNewAlias(t_platform_id, p_email_input.text, p_nickname_input.text, p_password_input.text, p_password_check_input.text, (ApiManager.NewAliasResult result) => { pt_result = result; });
        }
    }
}
