using Ebenit.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Ebenit
{
    class UserNewAliasSubmit : MonoBehaviour
    {
#pragma warning disable 0649
        public InputField p_email_input;
        public InputField p_nickname_input;
        public InputField p_password_input;
        public InputField p_password_check_input;

        public Toggle p_gdpr_toggle;
#pragma warning restore 0649

        public ApiManager.NewAliasResult pt_result {
            get; protected set;
        }

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
