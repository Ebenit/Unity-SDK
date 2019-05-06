using System;
using System.Collections;
using System.Globalization;
using Ebenit.Managers;
using Ebenit.Requests.Api;
using UnityEngine;

namespace Ebenit.Requests
{
    /// <summary>
    /// Abstract class of general request behaviour.
    /// </summary>
    [Serializable]
    public abstract class ARequest
    {
        /// <summary>
        /// URL where the request is send.
        /// </summary>
        [NonSerialized]
        protected string t_url;

        /// <summary>
        /// Message ID
        /// </summary>
        [SerializeField]
        protected string id = null;
        public string pt_id {
            get { return id; }
            protected set { id = value; }
        }

        /// <summary>
        /// Token identifying user in requests.
        /// </summary>
        public string user_token = null;

        /// <summary>
        /// True if the request was completed (does not matter if successfuly or not).
        /// </summary>
        public bool pt_done {
            get; protected set;
        }

        /// <summary>
        /// Objectified server response.
        /// </summary>
        public object pt_response {
            get; protected set;
        }

        /// <summary>
        /// ApiManager instance.
        /// </summary>
        private ApiManager m_api_manager = null;

        /// <summary>
        /// Number of request send tries.
        /// </summary>
        private int m_tries = 0;

        /// <summary>
        /// True if the request requires already intialized and online ApiManager.
        /// </summary>
        protected bool t_require_online = true;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="request_number">Number of Ebenit API request. Last part of URL.</param>
        /// <param name="user_token">User login token.</param>
        public ARequest(uint request_number, string user_token) {
            this.user_token = user_token;

            m_api_manager = ApiManager.getInstance();
            t_url = m_api_manager.p_api_url;

            if (t_url.EndsWith("/")) {
                t_url += "?" + request_number;
            } else if (t_url.EndsWith("?")) {
                t_url += request_number;
            } else {
                t_url += "/?" + request_number;
            }

            pt_id = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) + "_" + UnityEngine.Random.Range(0, 10).ToString();
        }

        /// <summary>
        /// Coroutine which handles the request sending.
        /// </summary>
        /// <returns></returns>
        public IEnumerator send() {
            if (t_require_online && !m_api_manager.pt_online) {
                while (!m_api_manager.pt_login_done) {
                    yield return null;
                }

                if (!string.IsNullOrEmpty(this.user_token) && m_api_manager.pt_user != null) {
                    this.user_token = m_api_manager.pt_user.pt_user_token;
                }
            }

            SEND_API_REQUEST:
            if (!t_require_online || m_api_manager.pt_online) {
                ApiRequestResult result = new ApiRequestResult();

                yield return RequestManager.getInstance().pt_api_request.send(t_url, this, ref result);
                m_tries++;

                if (result.pt_www_error || result.pt_auth_error || result.pt_auth_user_error) {
                    // request was not successful
                    if (m_tries > m_api_manager.p_timeout_tries) {
                        // if out of number of tries logout
                        m_api_manager.setTimeout();
                    } else {
                        // if number of tries less than or equal to timeout tries, try again
                        goto SEND_API_REQUEST;
                    }
                } else {
                    // if everything ok process response
                    handleResult(result);
                }
            }

            pt_done = true;
        }

        /// <summary>
        /// Abstract method for result handling.
        /// </summary>
        /// <param name="result"></param>
        protected abstract void handleResult(ApiRequestResult result);
    }
}
