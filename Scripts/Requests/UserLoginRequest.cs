using Ebenit.Managers;
using Ebenit.Requests.Api;
using Ebenit.Requests.Containers.Responses;
using System;
using System.Collections;
using UnityEngine;

namespace Ebenit.Requests
{
    [Serializable]
    public class UserLoginRequest : ARequest
    {
        public UserLogin user = new UserLogin();
        public uint platform_id;

        public bool permanent_login = false;

        public UserLoginRequest(uint request_number, string user_token) : base(request_number, user_token) {
            t_require_online = false;
        }

        protected override void handleResult(ApiRequestResult result) {
            if (result.pt_verified) {
                this.pt_response = JsonUtility.FromJson<UserLoginResponse>(result.pt_response_decoded);
            }
        }

        [Serializable]
        public class UserLogin
        {
            public string email;
            public string password;
        }
    }
}
