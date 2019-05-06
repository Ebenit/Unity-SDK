using Ebenit.Requests.Api;
using Ebenit.Requests.Containers.Responses;
using System;
using UnityEngine;

namespace Ebenit.Requests
{
    /// <summary>
    /// Handles Login request.
    /// </summary>
    [Serializable]
    public class UserLoginRequest : ARequest
    {
        /// <summary>
        /// User's credentials.
        /// </summary>
        public UserLogin user = new UserLogin();
        /// <summary>
        /// Platform ID in Ebenit API.
        /// </summary>
        public uint platform_id;

        /// <summary>
        /// True to longer validity of token.
        /// </summary>
        public bool permanent_login = false;

        public UserLoginRequest(uint request_number, string user_token) : base(request_number, user_token) {
            t_require_online = false;
        }

        protected override void handleResult(ApiRequestResult result) {
            if (result.pt_verified) {
                this.pt_response = JsonUtility.FromJson<UserLoginResponse>(result.pt_response_decoded);
            }
        }

        /// <summary>
        /// User's credentials sending container.
        /// </summary>
        [Serializable]
        public class UserLogin
        {
            public string email;
            public string password;
        }
    }
}
