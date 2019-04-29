using System.Collections.Generic;

namespace Ebenit.Containers
{
    /// <summary>
    /// Information about current user.
    /// </summary>
    public class User
    {
        public uint pt_eid {
            get; protected set;
        }

        /// <summary>
        /// ID on current platform. Used mainly for platform login.
        /// </summary>
        public string pt_id {
            get; protected set;
        }

        /// <summary>
        /// Nickname on current platform.
        /// </summary>
        public string pt_nickname {
            get; protected set;
        }

        /// <summary>
        /// User token. Used to access some login required requests.
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

        public bool hasProduct(Product product) {
            return m_products.Contains(product);
        }

        public bool hasProduct(uint id) {
            foreach (Product product in m_products) {
                if (product.pt_id == id) {
                    return true;
                }
            }

            return false;
        }

        public HashSet<Product> getProducts() {
            return m_products;
        }

        public void setUserToken(string token) {
            this.pt_user_token = token;
        }

        public void setEid(uint id) {
            this.pt_eid = id;
        }
    }
}
