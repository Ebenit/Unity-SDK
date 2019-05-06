using Ebenit.Requests.Api;
using Ebenit.Requests.Containers.Responses;
using System;
using UnityEngine;

namespace Ebenit.Requests
{
    /// <summary>
    /// Handles User New Alias request.
    /// </summary>
    [Serializable]
    public class UserNewAliasRequest : ARequest
    {
        /// <summary>
        /// User's credentials.
        /// </summary>
        public UserNewAlias user = new UserNewAlias();
        /// <summary>
        /// Platform ID in EbenitAPI.
        /// </summary>
        public uint platform_id;

        public UserNewAliasRequest(uint request_number, string user_token) : base(request_number, user_token) {
            t_require_online = false;
        }

        protected override void handleResult(ApiRequestResult result) {
            if (result.pt_verified) {
                this.pt_response = JsonUtility.FromJson<UserNewAliasResponse>(result.pt_response_decoded);
            }
        }

        /// <summary>
        /// User's credentials sending container.
        /// </summary>
        [Serializable]
        public class UserNewAlias
        {
            public string email;
            public string nickname;
            public string password;
            public string password_check;
        }
    }
}
