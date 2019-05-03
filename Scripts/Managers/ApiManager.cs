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

        /// <summary>
        /// Enum with possible results of New Alias request.
        /// </summary>
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

        /// <summary>
        /// Delegate to function to set the result of New Alias request
        /// </summary>
        /// <param name="result"></param>
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
        /// <summary>
        /// Number of tries before timeout connection loss is set.
        /// </summary>
        public int p_timeout_tries = 3;

        /// <summary>
        /// Number of seconds before the request is considered as timeout.
        /// </summary>
        public int p_timeout_seconds = 5;

        [Header("CurrencyManager Settings")]
        /// <summary>
        /// Number of seconds between currency update intervals. If set to 0 or less, the update is possible to make every frame.
        /// </summary>
        public float p_currency_update_interval;

        /// <summary>
        /// Codenames of required currencies for correct run of game.
        /// </summary>
        public string[] p_required_currencies_names;

        [Header("ProductManager Settings")]
        /// <summary>
        /// Settings if the product manager should fetch hidden products in Products Get All request.
        /// </summary>
        public bool p_get_hidden_products = false;

        [Header("RequestManager Settings")]
        /// <summary>
        /// Number of seconds between deleting of finished requests.
        /// </summary>
        public float p_requests_clean_interval = 20.0f;

        /// <summary>
        /// Number of seconds between deleting of finished requests in the exit loop.
        /// </summary>
        public float p_requests_clean_exit_wait = 1.0f;
#pragma warning restore 0649

        /// <summary>
        /// The game time of the last successful login.
        /// </summary>
        public float pt_last_login_time {
            get; protected set;
        }

        /// <summary>
        /// True if the login try is finished. Regardless of the success.
        /// </summary>
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

        /// <summary>
        /// True if the timeout connection loss conditions were met.
        /// </summary>
        public bool pt_timeout {
            get; protected set;
        }

        /// <summary>
        /// The game time of the last timeout.
        /// </summary>
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

        /// <summary>
        /// Coroutine for login by the user token.
        /// </summary>
        /// <param name="user_token">User token previously obtained from Ebenit API.</param>
        /// <returns></returns>
        private IEnumerator doInitializeApi(string user_token) {
            var request = RequestManager.getInstance().createUserLoginRequest(user_token);

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

        /// <summary>
        /// Coroutine for login by the user credentials.
        /// </summary>
        /// <param name="email">User e-mail.</param>
        /// <param name="password">User password in plain text.</param>
        /// <param name="permanent_login">True to longer token validity.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Coroutine for login by pre-created User object.
        /// </summary>
        /// <returns></returns>
        private IEnumerator doInitializeApiPlatform() {
            var request = RequestManager.getInstance().createUserLoginPlatformRequest();

            yield return request.send();

            processResponse(request.pt_response as UserLoginResponse);

            pt_login_done = true;
        }

        /// <summary>
        /// Processes the login request response and sets all needed properties.
        /// </summary>
        /// <param name="response">Login request response from Ebenit API.</param>
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

        /// <summary>
        /// Coroutine to send the New Alias request and handle it.
        /// </summary>
        /// <param name="email">User e-mail.</param>
        /// <param name="nickname">User nickname.</param>
        /// <param name="password">User password in plain text.</param>
        /// <param name="password_check">User password in plain text. Recommended from other input box to verify the correctness of password.</param>
        /// <param name="result_method">Delegate method to store the request result.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Sets the timeout properties and sets the online property to false.
        /// </summary>
        public void setTimeout() {
            pt_last_timeout_time = Time.time;
            this.pt_timeout = true;
            this.pt_online = false;
        }

        /// <summary>
        /// Logs out and resets the managers.
        /// </summary>
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

        /// <summary>
        /// Coroutine to log out with waiting for current requests to finish.
        /// </summary>
        /// <returns></returns>
        public IEnumerator logoutWaitRequests() {
            if (pt_online && !pt_timeout) {
                RequestManager request_manager = RequestManager.getInstance();

                yield return request_manager.waitRequestsDone();
            }
            
            logout();
        }

        /// <summary>
        /// Starts the New Alias request.
        /// </summary>
        /// <param name="platform_id">ID of platform in Ebenit API to run the request to.</param>
        /// <param name="email">User e-mail.</param>
        /// <param name="nickname">User nickname.</param>
        /// <param name="password">User password in plain text.</param>
        /// <param name="password_check">User password in plain text. Recommended from other input box to verify the correctness of password.</param>
        /// <param name="result_method">Delegate method to store the request result.</param>
        /// <returns>True - if the request was successfuly started. False - otherwise.</returns>
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

        /// <summary>
        /// Starts the login coroutine where the login is done by the user token.
        /// </summary>
        /// <param name="user_token">User token previously obtained from Ebenit API.</param>
        public void initializeApi(string user_token) {
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

            StartCoroutine(doInitializeApi(user_token));
        }

        /// <summary>
        /// Starts the login coroutine where the login is done by the user credentials.
        /// </summary>
        /// <param name="platform_id">ID of platform in Ebenit API.</param>
        /// <param name="email">User e-mail.</param>
        /// <param name="password">User password in plain text.</param>
        /// <param name="permanent_login">True to longer token validity.</param>
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

        /// <summary>
        /// Start the login coroutine where the login is done by pre-created User object.
        /// If the user is not present in the Ebenit API, this request does create him.
        /// 
        /// We recommend using of this login only for logins from platform with its own authorization.
        /// </summary>
        /// <param name="user">User object.</param>
        /// <param name="platform_id">ID of platform in Ebenit API.</param>
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
    }
}
