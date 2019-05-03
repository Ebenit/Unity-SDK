using Ebenit.Containers;
using Ebenit.Requests;
using Ebenit.Requests.Api;
using Ebenit.Requests.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ebenit.Managers
{
    /// <summary>
    /// Prepares EbenitAPI requests.
    /// </summary>
    public class RequestManager : MonoBehaviour
    {
        protected static RequestManager t_instance;
        public static RequestManager getInstance() {
            if (t_instance == null)
                return new GameObject("Ebenit.Managers.RequestManager").AddComponent<RequestManager>();

            return t_instance;
        }

        /// <summary>
        /// Instance which is used for actual request sending.
        /// </summary>
        public ApiRequest pt_api_request {
            get; protected set;
        }

        /// <summary>
        /// Instance of ApiManager.
        /// </summary>
        private ApiManager m_api_manager = null;
        /// <summary>
        /// List of all running requests.
        /// </summary>
        private List<ARequest> m_requests = null;

        /// <summary>
        /// Game time of last cleaning of finished requests.
        /// </summary>
        private float m_last_requests_clean = 0;

        void Awake() {
            if (t_instance != null) {
                Destroy(gameObject);
            } else {
                t_instance = this;
                DontDestroyOnLoad(this.gameObject);

                m_api_manager = ApiManager.getInstance();

                pt_api_request = new ApiRequest(m_api_manager.p_token_id, m_api_manager.p_token, m_api_manager.p_timeout_seconds);

                m_requests = new List<ARequest>();
            }
        }

        private void Update() {
            if (Time.time - m_last_requests_clean < m_api_manager.p_requests_clean_interval) {
                return;
            }

            cleanRequests();
        }

        /// <summary>
        /// Removes finished requests from list of running requests.
        /// </summary>
        private void cleanRequests() {
            m_last_requests_clean = Time.time;

            int i = 0;
            while (i < m_requests.Count) {
                if (m_requests[i].pt_done) {
                    // if the request is done remove it from list
                    m_requests.RemoveAt(i);

                    // do not add to i, since the request was removed (count of running request is one less and all requests after the currently removed are shifted by one index)
                } else {
                    // if the request is not done continue
                    i++;
                }
            }
        }

        /// <summary>
        /// Coroutine to wait for all requests to finish.
        /// </summary>
        /// <returns></returns>
        public IEnumerator waitRequestsDone() {
            while (true) {
                cleanRequests();

                if (m_requests.Count == 0) {
                    // all request were done
                    break;
                }

                if (m_api_manager.p_requests_clean_exit_wait > 0) {
                    yield return new WaitForSeconds(m_api_manager.p_requests_clean_exit_wait);
                }
            }
        }

        /// <summary>
        /// Prepares login platform request.
        /// </summary>
        /// <returns></returns>
        public UserLoginPlatformRequest createUserLoginPlatformRequest() {
            var request = new UserLoginPlatformRequest(18, null);
            m_requests.Add(request);

            request.user.id = m_api_manager.pt_user.pt_id;
            request.user.nickname = m_api_manager.pt_user.pt_nickname;

            request.platform_id = m_api_manager.pt_platform_id;

            return request;
        }

        /// <summary>
        /// Prepares Login request.
        /// </summary>
        /// <param name="email">User e-mail.</param>
        /// <param name="password">User password in plain text.</param>
        /// <param name="permanent_login">True to longer login validity.</param>
        /// <returns></returns>
        public UserLoginRequest createUserLoginRequest(string email, string password, bool permanent_login = false) {
            var request = new UserLoginRequest(22, null);
            m_requests.Add(request);

            request.user.email = email;
            request.user.password = password;

            request.platform_id = m_api_manager.pt_platform_id;

            request.permanent_login = permanent_login;

            return request;
        }

        /// <summary>
        /// Prepares Login request.
        /// </summary>
        /// <param name="user_token">Valid user token fetched from Ebenit API.</param>
        /// <returns></returns>
        public UserLoginRequest createUserLoginRequest(string user_token) {
            var request = new UserLoginRequest(22, user_token);
            m_requests.Add(request);

            request.permanent_login = false;

            return request;
        }

        /// <summary>
        /// Prepares User New Alias request.
        /// </summary>
        /// <param name="email">User e-mail.</param>
        /// <param name="nickname">User nickname.</param>
        /// <param name="password">User password in plain text.</param>
        /// <param name="password_check">User password in plain text. Used to check possible mishaps in password.</param>
        /// <returns></returns>
        public UserNewAliasRequest createUserNewAliasRequest(string email, string nickname, string password, string password_check) {
            var request = new UserNewAliasRequest(23, null);
            m_requests.Add(request);

            request.user.email = email;
            request.user.nickname = nickname;
            request.user.password = password;
            request.user.password_check = password;

            request.platform_id = m_api_manager.pt_platform_id;

            return request;
        }

        /// <summary>
        /// Prepares currency transaction request.
        /// </summary>
        /// <param name="currency_id">Currency ID in Ebenit API.</param>
        /// <param name="num">The change of currency.</param>
        /// <param name="return_value">True to return the currency value in Ebenit API (server side). Defaults to false.</param>
        /// <returns></returns>
        public CurrencyTransactionRequest createCurrencyTransactionRequest(uint currency_id, float num, bool return_value = false) {
            var request = new CurrencyTransactionRequest(19, m_api_manager.pt_user.pt_user_token);
            m_requests.Add(request);

            request.currency.id = currency_id;
            request.currency.num = num;

            request.return_value = return_value;

            return request;
        }

        /// <summary>
        /// Prepares the get all products request.
        /// </summary>
        /// <param name="includeHidden">True to also fetch hidden products.</param>
        /// <returns></returns>
        public ProductAllRequest createProductAllRequest(bool includeHidden = false) {
            var request = new ProductAllRequest(16, null);
            m_requests.Add(request);

            request.include_hidden = includeHidden;

            return request;
        }

        /// <summary>
        /// Prepares the get user products request.
        /// </summary>
        /// <returns></returns>
        public ProductByUserRequest createProductByUserRequest() {
            var request = new ProductByUserRequest(17, m_api_manager.pt_user.pt_user_token);
            m_requests.Add(request);

            return request;
        }

        /// <summary>
        /// Prepares the new order request.
        /// </summary>
        /// <param name="order_products">List of product included in order.</param>
        /// <param name="order_discounts">List of discounts included in order.</param>
        /// <returns></returns>
        public OrderNewRequest createOrderNewRequest(List<OrderProduct> order_products, List<OrderDiscount> order_discounts) {
            var request = new OrderNewRequest(15, m_api_manager.pt_user.pt_user_token);
            m_requests.Add(request);

            int orderProductsCount = order_products == null ? 0 : order_products.Count;
            request.products = new OrderNewRequest.Product[orderProductsCount];
            for (int i = 0; i < orderProductsCount; i++) {
                request.products[i] = new OrderNewRequest.Product(order_products[i].pt_product.pt_id, order_products[i].pt_quantity);
            }

            int orderDiscountsCount = order_discounts == null ? 0 : order_discounts.Count;
            request.discounts = new OrderNewRequest.Discount[orderDiscountsCount];
            for (int i = 0; i < orderDiscountsCount; i++) {
                request.discounts[i] = new OrderNewRequest.Discount(order_discounts[i].pt_name, order_discounts[i].pt_percentage);
            }

            return request;
        }

        /// <summary>
        /// Prepares the highscore save request.
        /// </summary>
        /// <param name="highscore">HighscoreSave object.</param>
        /// <returns></returns>
        public HighscoreSaveRequest createHighscoreSaveRequest(HighscoreSave highscore) {
            var request = new HighscoreSaveRequest(12, m_api_manager.pt_user.pt_user_token);
            m_requests.Add(request);

            request.table_id = highscore.pt_table_id;
            request.score = highscore.p_score;

            return request;
        }

        /// <summary>
        /// Prepares the highscore get all request.
        /// </summary>
        /// <param name="table_id">ID or name of highscore table in Ebenit API.</param>
        /// <param name="from_region">True to fetch highscore only from player region. Default to true.</param>
        /// <param name="page">Number of page to fetch. Defaults to 1 (first page).</param>
        /// <param name="rows_max">Maximum number of rows to fetch. Defaults to 100.</param>
        /// <param name="reverse_order">True to fetch the scores in DESC order. Defaults to false.</param>
        /// <returns></returns>
        public HighscoreGetAllRequest createHighscoreGetAllRequest(string table_id, bool from_region = true, uint page = 1, uint rows_max = 100, bool reverse_order = false) {
            var request = new HighscoreGetAllRequest(13, null);
            m_requests.Add(request);

            request.platform_id = from_region ? (int)m_api_manager.pt_platform_id : -1;
            request.table_id = table_id;
            
            request.page = page;
            request.rows_max = rows_max;
            request.reverse_order = reverse_order;

            return request;
        }

        /// <summary>
        /// Prepares the highscore around user request.
        /// </summary>
        /// <param name="table_id">ID or name of highscore table in Ebenit API.</param>
        /// <param name="rows_max">Maximum number of rows to fetch. Defaults to 100.</param>
        /// <param name="platform_user">True to fetch highscore only from player region. Default to true.</param>
        /// <returns></returns>
        public HighscoreGetAroundUserRequest createHighscoreGetAroundUserRequest(string table_id, uint rows_max = 100, bool platform_user = true) {
            var request = new HighscoreGetAroundUserRequest(14, m_api_manager.pt_user.pt_user_token);
            m_requests.Add(request);

            request.table_id = table_id;
            request.rows_max = rows_max;
            request.platform_user = platform_user;

            return request;
        }
    }
}
