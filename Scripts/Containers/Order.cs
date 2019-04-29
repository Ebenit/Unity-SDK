using System;
using System.Collections.Generic;

namespace Ebenit.Containers
{
    [Serializable]
    public class Order
    {
        public List<OrderProduct> p_products = new List<OrderProduct>();
        public List<OrderDiscount> p_discounts = new List<OrderDiscount>();

        public bool p_done = false;
        public bool p_success = false;
        public bool p_success_all = false;

        public void addProduct(Product product, float quantity) {
            p_products.Add(new OrderProduct(product, quantity));
        }

        public void addDiscount(string name, float percentage) {
            p_discounts.Add(new OrderDiscount(name, percentage));
        }
    }
}
