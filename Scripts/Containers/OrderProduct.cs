using System;

namespace Ebenit.Containers
{
    /// <summary>
    /// Order product container.
    /// </summary>
    [Serializable]
    public class OrderProduct
    {
        /// <summary>
        /// Instance of Product to buy.
        /// </summary>
        public Product pt_product {
            get; protected set;
        }
        /// <summary>
        /// Number of Product to buy.
        /// </summary>
        public float pt_quantity {
            get; protected set;
        }

        public OrderProduct(Product product, float quantity) {
            this.pt_product = product;
            this.pt_quantity = quantity;
        }
    }
}
