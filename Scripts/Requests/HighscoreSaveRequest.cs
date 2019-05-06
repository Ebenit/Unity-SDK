using Ebenit.Requests.Api;
using Ebenit.Requests.Containers.Responses;
using System;
using UnityEngine;

namespace Ebenit.Requests
{
    /// <summary>
    /// Handles Highscore Save request.
    /// </summary>
    [Serializable]
    public class HighscoreSaveRequest : ARequest
    {
        /// <summary>
        /// ID or name of highscore table in Ebenit API.
        /// </summary>
        public string table_id;
        /// <summary>
        /// Achieved score.
        /// </summary>
        public int score;

        public HighscoreSaveRequest(uint request_number, string user_token) : base(request_number, user_token) { }

        protected override void handleResult(ApiRequestResult result) {
            if (result.pt_verified) {
                this.pt_response = JsonUtility.FromJson<HighscoreSaveResponse>(result.pt_response_decoded);
            }
        }
    }
}
