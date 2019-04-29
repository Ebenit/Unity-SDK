using System;

namespace Ebenit.Containers
{
    [Serializable]
    public class OrderProduct
    {
        public Product pt_product {
            get; protected set;
        }
        public float pt_quantity {
            get; protected set;
        }

        public OrderProduct(Product product, float quantity) {
            this.pt_product = product;
            this.pt_quantity = quantity;
        }
    }
}
