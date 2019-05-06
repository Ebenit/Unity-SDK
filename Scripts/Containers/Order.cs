using System;
using System.Collections.Generic;

namespace Ebenit.Containers
{
    /// <summary>
    /// Container for Order New request.
    /// </summary>
    [Serializable]
    public class Order
    {
        /// <summary>
        /// Order products.
        /// </summary>
        public List<OrderProduct> p_products = new List<OrderProduct>();
        /// <summary>
        /// Order discounts.
        /// </summary>
        public List<OrderDiscount> p_discounts = new List<OrderDiscount>();

        /// <summary>
        /// True if the request was finished.
        /// </summary>
        public bool p_done = false;
        /// <summary>
        /// True if the request was successful.
        /// </summary>
        public bool p_success = false;
        /// <summary>
        /// True if the request was successful with all products and discounts.
        /// </summary>
        public bool p_success_all = false;

        /// <summary>
        /// Adds product by quantity times into order. May be multiple of same product.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        public void addProduct(Product product, float quantity) {
            p_products.Add(new OrderProduct(product, quantity));
        }

        /// <summary>
        /// Adds discount to order.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="percentage">Discount in percentage value.</param>
        public void addDiscount(string name, float percentage) {
            p_discounts.Add(new OrderDiscount(name, percentage));
        }
    }
}
