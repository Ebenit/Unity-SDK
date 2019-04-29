using Ebenit.Requests.Api;
using Ebenit.Requests.Containers.Responses;
using System;
using UnityEngine;

namespace Ebenit.Requests
{
    /// <summary>
    /// Handles currency transaction request.
    /// </summary>
    [Serializable]
    public class HighscoreGetAroundUserRequest : ARequest
    {
        public string table_id;
        public uint rows_max = 100;
        public bool platform_user = true;

        public HighscoreGetAroundUserRequest(uint request_number, string user_token) : base(request_number, user_token) { }

        protected override void handleResult(ApiRequestResult result) {
            if (result.pt_verified) {
                this.pt_response = JsonUtility.FromJson<HighscoreGetResponse>(result.pt_response_decoded);
            }
        }
    }
}
