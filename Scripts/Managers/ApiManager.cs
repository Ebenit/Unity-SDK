using Ebenit.Containers;
using System.Collections;
using UnityEngine;

using Ebenit.Requests.Containers.Responses;
using System.Text.RegularExpressions;

namespace Ebenit.Managers
{
    /// <summary>
    /// Main Manager of EbenitAPI communication.
    /// </summary>
    public class ApiManager : MonoBehaviour {
        protected static ApiManager t_instance;
        public static ApiManager getInstance() {
            if (t_instance == null)
                return new GameObject("Ebenit.Managers.ApiManager").AddComponent<ApiManager>();

            return t_instance;
        }

        public enum NewAliasResult
        {
            NOT_FINISHED = 0,
            SUCCESSFUL,
            UNKNOWN_ERROR,
            COMMUNICATION_ERROR,
            INCORRECT_DATA_ERROR,
            PLATFORM_ERROR,
            USER_ERROR,
            PASSWORD_ERROR,
        }

        public delegate void SetNewAliasResult(NewAliasResult result);

#pragma warning disable 0649
        [Header("Communication Settings")]
        /// <summary>
        /// ID of communication token.
        /// </summary>
        public string p_token_id;
        /// <summary>
        /// Communication token.
        /// </summary>
        public string p_token;
        /// <summary>
        /// EbenitAPI URL.
        /// </summary>
        public string p_api_url;

        [Header("Timeout Settings")]
        public int p_timeout_tries = 3;

        public int p_timeout_seconds = 5;

        [Header("CurrencyManager Settings")]
        public float p_currency_update_interval;

        /// <summary>
        /// Codenames of required currencies for correct run of game.
        /// </summary>
        public string[] p_required_currencies_names;

        [Header("ProductManager Settings")]
        public bool p_get_hidden_products = false;

        [Header("RequestManager Settings")]
        public float p_requests_clean_interval = 20.0f;
        public float p_requests_clean_exit_wait = 1.0f;
#pragma warning restore 0649

        public float pt_last_login_time {
            get; protected set;
        }

        public bool pt_login_done {
            get; protected set;
        }

        /// <summary>
        /// True if the communication with EbenitAPI was correctly established.
        /// </summary>
        public bool pt_online {
            get; protected set;
        }

        /// <summary>
        /// Current platform ID in EbenitAPI.
        /// </summary>
        public uint pt_platform_id {
            get; protected set;
        }

        /// <summary>
        /// Current User.
        /// </summary>
        public User pt_user {
            get; protected set;
        }

        public bool pt_timeout {
            get; protected set;
        }

        public float pt_last_timeout_time {
            get; protected set;
        }

        void Awake() {
            if (t_instance != null) {
                Destroy(gameObject);
            } else {
                t_instance = this;
                DontDestroyOnLoad(this.gameObject);

                if (!string.IsNullOrEmpty(p_token)) {
                    p_token = p_token.Trim();
                }
                if (!string.IsNullOrEmpty(p_token)) {
                    p_token_id = p_token_id.Trim();
                }
                if (!string.IsNullOrEmpty(p_token)) {
                    p_api_url = p_api_url.Trim();
                }

                pt_timeout = false;
                pt_online = false;
                
                pt_last_login_time = 0;
            }
        }

        public void setTimeout() {
            pt_last_timeout_time = Time.time;
            this.pt_timeout = true;
            this.pt_online = false;
        }

        public void logout() {
            this.pt_login_done = true;

            this.pt_timeout = false;
            this.pt_online = false;
            
            this.pt_user = null;
            this.pt_platform_id = 0;

            CurrencyManager.getInstance().resetToDefault();
            HighscoreManager.getInstance().resetToDefault();
            ProductManager.getInstance().resetToDefault();
        }

        public IEnumerator logoutWaitRequests() {
            if (pt_online && !pt_timeout) {
                RequestManager request_manager = RequestManager.getInstance();

                yield return request_manager.waitRequestsDone();
            }
            
            logout();
        }

        public bool userNewAlias(uint platform_id, string email, string nickname, string password, string password_check, SetNewAliasResult result_method) {
            this.pt_platform_id = platform_id;

            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, @"^[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$") || string.IsNullOrEmpty(nickname) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(password_check) ||
                !password.Equals(password_check)
            ) {
                result_method(NewAliasResult.INCORRECT_DATA_ERROR);
                return false;
            }

            result_method(NewAliasResult.NOT_FINISHED);

            StartCoroutine(doUserNewAlias(email, nickname, password, password_check, result_method));

            return true;
        }

        private IEnumerator doUserNewAlias(string email, string nickname, string password, string password_check, SetNewAliasResult result_method) {
            var request = RequestManager.getInstance().createUserNewAliasRequest(email, nickname, password, password_check);

            yield return request.send();

            var response = request.pt_response as UserNewAliasResponse;

            NewAliasResult alias_result = NewAliasResult.UNKNOWN_ERROR;

            if (response == null) {
                alias_result = NewAliasResult.UNKNOWN_ERROR;
            } else if (response.results != null && response.results.success) {
                alias_result = NewAliasResult.SUCCESSFUL;
            } else if (response.errors != null) {
                if (response.errors.incorrectData) {
                    alias_result = NewAliasResult.INCORRECT_DATA_ERROR;
                } else if (response.errors.platform) {
                    alias_result = NewAliasResult.PLATFORM_ERROR;
                } else if (response.errors.user) {
                    alias_result = NewAliasResult.USER_ERROR;
                } else if (response.errors.password) {
                    alias_result = NewAliasResult.PASSWORD_ERROR;
                } else {
                    alias_result = NewAliasResult.COMMUNICATION_ERROR;
                }
            } else {
                alias_result = NewAliasResult.UNKNOWN_ERROR;
            }

            result_method(alias_result);
        }

        public void initializeApi(string user_token, bool permanent_login = false) {
            this.pt_login_done = false;

            this.pt_timeout = false;
            this.pt_online = false;

            this.pt_user = null;
            this.pt_platform_id = 0;

            CurrencyManager.getInstance().resetToDefault();
            HighscoreManager.getInstance().resetToDefault();
            ProductManager.getInstance().resetToDefault();

            if (string.IsNullOrEmpty(user_token)) {
                this.pt_login_done = true;
                return;
            }

            StartCoroutine(doInitializeApi(user_token, permanent_login));
        }

        public void initializeApi(uint platform_id, string email, string password, bool permanent_login = false) {
            this.pt_login_done = false;

            this.pt_timeout = false;
            this.pt_online = false;

            this.pt_user = null;
            this.pt_platform_id = 0;

            CurrencyManager.getInstance().resetToDefault();
            HighscoreManager.getInstance().resetToDefault();
            ProductManager.getInstance().resetToDefault();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) {
                this.pt_login_done = true;
                return;
            }

            this.pt_platform_id = platform_id;

            StartCoroutine(doInitializeApi(email, password, permanent_login));
        }

        public void initializeApiPlatform(User user, uint platform_id) {
            this.pt_login_done = false;

            this.pt_timeout = false;
            this.pt_online = false;

            this.pt_user = null;
            this.pt_platform_id = 0;

            CurrencyManager.getInstance().resetToDefault();
            HighscoreManager.getInstance().resetToDefault();
            ProductManager.getInstance().resetToDefault();

            if (user == null || platform_id <= 0) {
                this.pt_login_done = true;
                return;
            }

            this.pt_user = user;
            this.pt_platform_id = platform_id;

            StartCoroutine(doInitializeApiPlatform());
        }

        private IEnumerator doInitializeApi(string user_token, bool permanent_login) {
            var request = RequestManager.getInstance().createUserLoginRequest(user_token, permanent_login);

            yield return request.send();

            var response = request.pt_response as UserLoginResponse;

            if (response != null && response.results != null && response.results.success) {
                this.pt_user = new User(null, response.results.nickname);
                this.pt_platform_id = response.results.platform_id;

                processResponse(response);
            } else {
                pt_online = false;
            }

            pt_login_done = true;
        }

        private IEnumerator doInitializeApi(string email, string password, bool permanent_login) {
            var request = RequestManager.getInstance().createUserLoginRequest(email, password, permanent_login);

            yield return request.send();

            var response = request.pt_response as UserLoginResponse;
            
            if (response != null && response.results != null && response.results.success) {
                this.pt_user = new User(null, response.results.nickname);

                processResponse(response);
            } else {
                pt_online = false;
            }

            pt_login_done = true;
        }

        private IEnumerator doInitializeApiPlatform() {
            var request = RequestManager.getInstance().createUserLoginPlatformRequest();

            yield return request.send();

            processResponse(request.pt_response as UserLoginResponse);

            pt_login_done = true;
        }

        private void processResponse(UserLoginResponse response) {
            if (response == null) {
                pt_online = false;
                return;
            }

            pt_last_login_time = Time.time;

            CurrencyManager currency_manager = CurrencyManager.getInstance();

            if (response.results != null && response.results.success) {
                this.pt_user.setUserToken(response.results.user_token);
                this.pt_user.setEid(response.results.eid);

                // set currencies
                currency_manager.setCurrencies(response.results.currencies);

                // currencies check (is all required currency present?)
                bool currency_not_found = false;
                foreach (var required_currency_name in p_required_currencies_names) {
                    if (!currency_manager.isCurrency(required_currency_name)) {
                        currency_not_found = true;
                        break;
                    }
                }

                pt_online = !currency_not_found;

                // set user products
                ProductManager.getInstance().setUserProducts(response.results.products);
            }
        }
    }
}
