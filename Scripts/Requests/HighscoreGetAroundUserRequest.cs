using Ebenit.Requests.Api;
using Ebenit.Requests.Containers.Responses;
using System;
using UnityEngine;

namespace Ebenit.Requests
{
    /// <summary>
    /// Handles Highscore Get Around User request.
    /// </summary>
    [Serializable]
    public class HighscoreGetAroundUserRequest : ARequest
    {
        /// <summary>
        /// ID or name of highscore table in Ebenit API.
        /// </summary>
        public string table_id;
        /// <summary>
        /// Maximum number of rows to fetch. Defaults to 100.
        /// </summary>
        public uint rows_max = 100;
        /// <summary>
        /// True to fetch highscores only from user's platform. Defaults to true.
        /// </summary>
        public bool platform_user = true;

        public HighscoreGetAroundUserRequest(uint request_number, string user_token) : base(request_number, user_token) { }

        protected override void handleResult(ApiRequestResult result) {
            if (result.pt_verified) {
                this.pt_response = JsonUtility.FromJson<HighscoreGetResponse>(result.pt_response_decoded);
            }
        }
    }
}
