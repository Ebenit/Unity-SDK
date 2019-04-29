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
    public class HighscoreGetAllRequest : ARequest
    {
        public int platform_id;
        public string table_id;
        public bool reverse_order = false;
        public uint page = 1;
        public uint rows_max = 100;

        public HighscoreGetAllRequest(uint request_number, string user_token) : base(request_number, user_token) { }

        protected override void handleResult(ApiRequestResult result) {
            if (result.pt_verified) {
                this.pt_response = JsonUtility.FromJson<HighscoreGetResponse>(result.pt_response_decoded);
            }
        }
    }
}
