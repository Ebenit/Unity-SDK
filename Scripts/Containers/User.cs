using System.Collections.Generic;

namespace Ebenit.Containers
{
    /// <summary>
    /// Information about user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// User ID in Ebenit API.
        /// </summary>
        public uint pt_eid {
            get; protected set;
        }

        /// <summary>
        /// User ID on current platform.
        /// </summary>
        public string pt_id {
            get; protected set;
        }

        /// <summary>
        /// User nickname on current platform.
        /// </summary>
        public string pt_nickname {
            get; protected set;
        }

        /// <summary>
        /// Current login token.
        /// </summary>
        public string pt_user_token {
            get; protected set;
        }

        /// <summary>
        /// Products that the user have bought.
        /// </summary>
        private HashSet<Product> m_products = new HashSet<Product>();

        public User(string id, string nickname) {
            pt_id = id;
            pt_nickname = nickname;
        }

        /// <summary>
        /// Sets product as bought and inserts into user products list.
        /// </summary>
        /// <param name="product"></param>
        public void addProduct(Product product) {
            product.setBought(true);

            m_products.Add(product);
        }

        /// <summary>
        /// Returns if the products is in the User's bought list.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool hasProduct(Product product) {
            return m_products.Contains(product);
        }

        /// <summary>
        /// Returns if the products is in the User's bought list.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool hasProduct(uint id) {
            foreach (Product product in m_products) {
                if (product.pt_id == id) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets product from User's list.
        /// </summary>
        /// <returns></returns>
        public HashSet<Product> getProducts() {
            return m_products;
        }

        /// <summary>
        /// Sets user login token.
        /// </summary>
        /// <param name="token"></param>
        public void setUserToken(string token) {
            this.pt_user_token = token;
        }

        /// <summary>
        /// Sets user ID in Ebenit API.
        /// </summary>
        /// <param name="id"></param>
        public void setEid(uint id) {
            this.pt_eid = id;
        }
    }
}
