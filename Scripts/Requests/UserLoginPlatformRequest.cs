using Ebenit.Managers;
using Ebenit.Requests.Api;
using Ebenit.Requests.Containers.Responses;
using System;
using System.Collections;
using UnityEngine;

namespace Ebenit.Requests
{
    /// <summary>
    /// Handles login platform request.
    /// </summary>
    [Serializable]
    public class UserLoginPlatformRequest : ARequest
    {
        /// <summary>
        /// Current user credentials.
        /// </summary>
        public UserLoginPlatform user = new UserLoginPlatform();
        /// <summary>
        /// Current platform ID in EbenitAPI.
        /// </summary>
        public uint platform_id;

        public UserLoginPlatformRequest(uint request_number, string user_token) : base(request_number, user_token) {
            t_require_online = false;
        }

        protected override void handleResult(ApiRequestResult result) {
            if (result.pt_verified) {
                this.pt_response = JsonUtility.FromJson<UserLoginResponse>(result.pt_response_decoded);
            }
        }

        [Serializable]
        public class UserLoginPlatform
        {
            public string id;
            public string nickname;
        }
    }
}
