using Ebenit.Requests.Api;
using Ebenit.Requests.Containers.Responses;
using System;
using UnityEngine;

namespace Ebenit.Requests
{
    /// <summary>
    /// Handles Order New request.
    /// </summary>
    [Serializable]
    public class OrderNewRequest : ARequest
    {
        /// <summary>
        /// ID or codename of payment type in Ebenit API. Defaults to "credit".
        /// </summary>
        public string payment_type = "credit";

        /// <summary>
        /// Order products.
        /// </summary>
        public Product[] products;
        /// <summary>
        /// Order discounts.
        /// </summary>
        public Discount[] discounts;

        public OrderNewRequest(uint request_number, string user_token) : base(request_number, user_token) { }

        protected override void handleResult(ApiRequestResult result) {
            if (result.pt_verified) {
                this.pt_response = JsonUtility.FromJson<OrderNewResponse>(result.pt_response_decoded);
            }
        }

        /// <summary>
        /// Order product sending container.
        /// </summary>
        [Serializable]
        public class Product
        {
            public uint id;
            public float num;

            public Product(uint id, float num) {
                this.id = id;
                this.num = num;
            }
        }

        /// <summary>
        /// Order discount sending container.
        /// </summary>
        [Serializable]
        public class Discount
        {
            public string name;
            public float percentage;

            public Discount(string name, float percentage) {
                this.name = name;
                this.percentage = percentage;
            }
        }
    }
}
