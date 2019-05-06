using Ebenit.Requests.Api;
using Ebenit.Requests.Containers.Responses;
using System;
using UnityEngine;

namespace Ebenit.Requests
{
    /// <summary>
    /// Handles Product Get All request.
    /// </summary>
    [Serializable]
    public class ProductAllRequest : ARequest
    {
        /// <summary>
        /// True to fetch hidden product. Defaults to false.
        /// </summary>
        public bool include_hidden = false;

        public ProductAllRequest(uint request_number, string user_token) : base(request_number, user_token) { }

        protected override void handleResult(ApiRequestResult result) {
            if (result.pt_verified) {
                this.pt_response = JsonUtility.FromJson<ProductAllResponse>(result.pt_response_decoded);

                var response = this.pt_response as ProductAllResponse;

                if (response != null && response.results != null && response.results.success) {
                    Ebenit.Managers.ProductManager.getInstance().setAllProducts(response.results.products);
                }
            }
        }
    }
}
