using Ebenit.Requests.Api;
using Ebenit.Requests.Containers.Responses;
using System;
using UnityEngine;

namespace Ebenit.Requests
{
    /// <summary>
    /// Handles Highscore Get All request.
    /// </summary>
    [Serializable]
    public class HighscoreGetAllRequest : ARequest
    {
        /// <summary>
        /// Platform ID in Ebenit API.
        /// </summary>
        public int platform_id;
        /// <summary>
        /// ID or name of highscore table in Ebenit API.
        /// </summary>
        public string table_id;
        /// <summary>
        /// True to fetch the rows in DESC order. Defaults to false.
        /// </summary>
        public bool reverse_order = false;
        /// <summary>
        /// Number of page to fetch. Defaults to 1.
        /// </summary>
        public uint page = 1;
        /// <summary>
        /// Maximum number of rows to fetch. Defaults to 100.
        /// </summary>
        public uint rows_max = 100;

        public HighscoreGetAllRequest(uint request_number, string user_token) : base(request_number, user_token) { }

        protected override void handleResult(ApiRequestResult result) {
            if (result.pt_verified) {
                this.pt_response = JsonUtility.FromJson<HighscoreGetResponse>(result.pt_response_decoded);
            }
        }
    }
}
