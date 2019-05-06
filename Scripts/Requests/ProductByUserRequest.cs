using Ebenit.Requests.Api;
using Ebenit.Requests.Containers.Responses;
using System;
using UnityEngine;

namespace Ebenit.Requests
{
    /// <summary>
    /// Handles Product Get By User request.
    /// </summary>
    [Serializable]
    public class ProductByUserRequest : ARequest
    {
        public ProductByUserRequest(uint request_number, string user_token) : base(request_number, user_token) { }

        protected override void handleResult(ApiRequestResult result) {
            if (result.pt_verified) {
                this.pt_response = JsonUtility.FromJson<ProductByUserResponse>(result.pt_response_decoded);

                var response = this.pt_response as ProductByUserResponse;

                if (response != null && response.results != null && response.results.success) {
                    Ebenit.Managers.ProductManager.getInstance().setUserProducts(response.results.products);
                }
            }
        }
    }
}
