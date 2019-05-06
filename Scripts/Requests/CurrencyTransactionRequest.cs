using Ebenit.Managers;
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
    public class CurrencyTransactionRequest : ARequest
    {
        /// <summary>
        /// Currency transaction informations.
        /// </summary>
        public CurrencyChange currency = new CurrencyChange();

        /// <summary>
        /// True to fetch current currency value from Ebenit API.
        /// </summary>
        public bool return_value = false;

        public CurrencyTransactionRequest(uint request_number, string user_token) : base(request_number, user_token) { }

        protected override void handleResult(ApiRequestResult result) {
            if (result.pt_verified) {
                this.pt_response = JsonUtility.FromJson<CurrencyTransactionResponse>(result.pt_response_decoded);

                if (return_value) {
                    CurrencyTransactionResponse response = pt_response as CurrencyTransactionResponse;

                    if (response != null && response.results != null && response.results.success) {
                        CurrencyManager.getInstance().setCurrencyValueAfterRefresh(currency.id, response.results.currency_num);
                    }
                }
            }
        }

        /// <summary>
        /// Container for additional informations about currency transaction.
        /// </summary>
        [Serializable]
        public class CurrencyChange
        {
            public uint id = 0;
            public float num = 0;
        }
    }
}
