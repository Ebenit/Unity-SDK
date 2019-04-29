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

        private ApiManager m_api_manager = null;

        private List<ARequest> m_requests = null;

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

        private void cleanRequests() {
            m_last_requests_clean = Time.time;

            int i = 0;
            while (i < m_requests.Count) {
                if (m_requests[i].pt_done) {
                    // if the request is done remove it from list
                    m_requests.RemoveAt(i);
                } else {
                    // if the request is not done continue
                    i++;
                }
            }
        }

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
        /// Creates login platform request.
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

        public UserLoginRequest createUserLoginRequest(string email, string password, bool permanent_login = false) {
            var request = new UserLoginRequest(22, null);
            m_requests.Add(request);

            request.user.email = email;
            request.user.password = password;

            request.platform_id = m_api_manager.pt_platform_id;

            request.permanent_login = permanent_login;

            return request;
        }

        public UserLoginRequest createUserLoginRequest(string user_token, bool permanent_login = false) {
            var request = new UserLoginRequest(22, user_token);
            m_requests.Add(request);

            request.permanent_login = permanent_login;

            return request;
        }

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
        /// Creates currency transaction request.
        /// </summary>
        /// <param name="currencyId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public CurrencyTransactionRequest createCurrencyTransactionRequest(uint currencyId, float num, bool returnValue = false) {
            var request = new CurrencyTransactionRequest(19, m_api_manager.pt_user.pt_user_token);
            m_requests.Add(request);

            request.currency.id = currencyId;
            request.currency.num = num;

            request.return_value = returnValue;

            return request;
        }

        public ProductAllRequest createProductAllRequest(bool includeHidden = false) {
            var request = new ProductAllRequest(16, null);
            m_requests.Add(request);

            request.include_hidden = includeHidden;

            return request;
        }

        public ProductByUserRequest createProductByUserRequest() {
            var request = new ProductByUserRequest(17, m_api_manager.pt_user.pt_user_token);
            m_requests.Add(request);

            return request;
        }

        public OrderNewRequest createOrderNewRequest(List<OrderProduct> orderProducts, List<OrderDiscount> orderDiscounts) {
            var request = new OrderNewRequest(15, m_api_manager.pt_user.pt_user_token);
            m_requests.Add(request);

            int orderProductsCount = orderProducts == null ? 0 : orderProducts.Count;
            request.products = new OrderNewRequest.Product[orderProductsCount];
            for (int i = 0; i < orderProductsCount; i++) {
                request.products[i] = new OrderNewRequest.Product(orderProducts[i].pt_product.pt_id, orderProducts[i].pt_quantity);
            }

            int orderDiscountsCount = orderDiscounts == null ? 0 : orderDiscounts.Count;
            request.discounts = new OrderNewRequest.Discount[orderDiscountsCount];
            for (int i = 0; i < orderDiscountsCount; i++) {
                request.discounts[i] = new OrderNewRequest.Discount(orderDiscounts[i].pt_name, orderDiscounts[i].pt_percentage);
            }

            return request;
        }

        public HighscoreSaveRequest createHighscoreSaveRequest(HighscoreSave highscore) {
            var request = new HighscoreSaveRequest(12, m_api_manager.pt_user.pt_user_token);
            m_requests.Add(request);

            request.table_id = highscore.pt_table_id;
            request.score = highscore.p_score;

            return request;
        }

        public HighscoreGetAllRequest createHighscoreGetAllRequest(string tableId, bool fromRegion = true, uint page = 1, uint rowsMax = 100, bool reverseOrder = false) {
            var request = new HighscoreGetAllRequest(13, null);
            m_requests.Add(request);

            request.platform_id = fromRegion ? (int)m_api_manager.pt_platform_id : -1;
            request.table_id = tableId;
            
            request.page = page;
            request.rows_max = rowsMax;
            request.reverse_order = reverseOrder;

            return request;
        }

        public HighscoreGetAroundUserRequest createHighscoreGetAroundUserRequest(string tableId, uint rowsMax = 100, bool platformUser = true) {
            var request = new HighscoreGetAroundUserRequest(14, m_api_manager.pt_user.pt_user_token);
            m_requests.Add(request);

            request.table_id = tableId;
            request.rows_max = rowsMax;
            request.platform_user = platformUser;

            return request;
        }
    }
}
