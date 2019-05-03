using Ebenit.Containers;
using Ebenit.Requests.Containers.Responses;
using Ebenit.Requests.Containers.Results;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ebenit.Managers
{
    /// <summary>
    /// Manager to handle product requests.
    /// </summary>
    public class ProductManager : MonoBehaviour
    {
        protected static ProductManager t_instance;
        public static ProductManager getInstance() {
            if (t_instance == null)
                return new GameObject("Ebenit.Managers.ProductManager").AddComponent<ProductManager>();

            return t_instance;
        }

        /// <summary>
        /// True if all products were already fetched.
        /// </summary>
        public bool pt_products_fetched {
            get; protected set;
        }
        /// <summary>
        /// True if the fetching of all products is in progress.
        /// </summary>
        public bool pt_products_fetching {
            get; protected set;
        }

        /// <summary>
        /// Set of product categories. Only categories of fetched products are present.
        /// </summary>
        private HashSet<Category> m_categories = new HashSet<Category>();
        /// <summary>
        /// Units of products. Only units of fetched products are present.
        /// </summary>
        private HashSet<Unit> m_units = new HashSet<Unit>();
        /// <summary>
        /// All fetched products.
        /// </summary>
        private List<Product> m_products = new List<Product>();

        /// <summary>
        /// Instance of ApiManager.
        /// </summary>
        private ApiManager m_api_manager = null;
        /// <summary>
        /// Instance of CurrencyManager.
        /// </summary>
        private CurrencyManager m_currency_manager = null;
        /// <summary>
        /// Instance of RequestManager.
        /// </summary>
        private RequestManager m_request_manager = null;

        void Awake() {
            if (t_instance != null) {
                Destroy(gameObject);
            } else {
                t_instance = this;
                DontDestroyOnLoad(this.gameObject);

                m_api_manager = ApiManager.getInstance();
                m_currency_manager = CurrencyManager.getInstance();
                m_request_manager = RequestManager.getInstance();

                pt_products_fetched = false;
                pt_products_fetching = false;
            }
        }

        /// <summary>
        /// Coroutine to handle the Order New request.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private IEnumerator doSendOrder(Order order) {
            var request = m_request_manager.createOrderNewRequest(order.p_products, order.p_discounts);

            yield return request.send();

            OrderNewResponse result = request.pt_response as OrderNewResponse;
            if (result != null && result.results != null) {
                order.p_success_all = order.p_success = result.results.success;

                if (order.p_success && result.results.order != null) {
                    order.p_success_all = result.results.order.discounts.Length == order.p_discounts.Count;

                    uint currencyId = 0;
                    if (result.results.order.products != null) {
                        foreach (var order_product in result.results.order.products) {
                            if (order_product.currency != null) {
                                currencyId = order_product.currency.id;
                            }
                            Product product = getProduct(order_product.id);

                            if (product == null) {
                                order.p_success_all = false;
                            } else {
                                product.setBought(true, order_product.num);
                                m_api_manager.pt_user.addProduct(product);

                                if (order_product.unit != null) {
                                    m_currency_manager.addToCurrency(order_product.unit.id, order_product.quantity * order_product.num, false);
                                }
                            }
                        }
                    }

                    if (currencyId != 0) {
                        m_currency_manager.addToCurrency(currencyId, -result.results.order.total_price, false);
                    }
                }
            }

            order.p_done = true;
        }

        /// <summary>
        /// Coroutine to handle the Get All Products request (with possibility of Get User Products request).
        /// </summary>
        /// <param name="fetch_user_products"></param>
        /// <param name="user_products"></param>
        /// <returns></returns>
        private IEnumerator doFetchProducts(bool fetch_user_products = true, ProductSumResult[] user_products = null) {
            yield return m_request_manager.createProductAllRequest(m_api_manager.p_get_hidden_products).send();

            if (fetch_user_products) {
                yield return m_request_manager.createProductByUserRequest().send();
            }

            if (user_products != null) {
                setUserProducts(user_products);
            }

            pt_products_fetched = true;
            pt_products_fetching = false;
        }

        /// <summary>
        /// Resets the manager values to default.
        /// </summary>
        public void resetToDefault() {
            m_units.Clear();
            m_products.Clear();

            pt_products_fetched = false;
            pt_products_fetching = false;
        }

        /// <summary>
        /// Adds category if the category does not already exists.
        /// </summary>
        /// <param name="category"></param>
        /// <returns>True - if the addition was successful. False - otherwise.</returns>
        public bool addCategory(Category category) {
            if (category == null) {
                return false;
            }

            foreach (Category m_category in m_categories) {
                if (m_category.pt_id == category.pt_id) {
                    return false;
                }
            }

            m_categories.Add(category);
            return true;
        }

        /// <summary>
        /// Returns existing category.
        /// 
        /// If the name parameter is not null or empty and the category is not found. New category is created.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Category getCategory(uint id, string name = null) {
            foreach (Category category in m_categories) {
                if (category.pt_id == id) {
                    return category;
                }
            }

            if (!string.IsNullOrEmpty(name)) {
                Category category = new Category(id, name);
                m_categories.Add(category);
                return category;
            }

            return null;
        }

        /// <summary>
        /// Adds unit if the unit does not already exists.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool addUnit(Unit unit) {
            if (unit == null) {
                return false;
            }

            foreach (Unit m_unit in m_units) {
                if (m_unit.pt_id == unit.pt_id) {
                    return false;
                }
            }

            m_units.Add(unit);
            return true;
        }

        /// <summary>
        /// Returns existing unit.
        /// 
        /// If the name parameter is not null or empty and the unit is not found. New unit is created.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Unit getUnit(uint id, string name = null) {
            foreach (Unit unit in m_units) {
                if (unit.pt_id == id) {
                    return unit;
                }
            }

            if (!string.IsNullOrEmpty(name)) {
                Unit unit = new Unit(id, name);
                m_units.Add(unit);
                return unit;
            }

            return null;
        }

        /// <summary>
        /// Returns existing product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product getProduct(uint id) {
            foreach (Product product in m_products) {
                if (product.pt_id == id) {
                    return product;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns list of all products.
        /// </summary>
        /// <returns></returns>
        public List<Product> getAllProducts() {
            return m_products;
        }

        /// <summary>
        /// Starts the coroutine to fetch products. If the products are already fetching this method does nothing.
        /// </summary>
        /// <param name="fetch_user_products">True to also fetch user products. Defaults to true.</param>
        /// <param name="user_products">Already fetched user products to add to user. Defaults to null.</param>
        public void fetchProducts(bool fetch_user_products = true, ProductSumResult[] user_products = null) {
            if (pt_products_fetching) {
                return;
            }

            pt_products_fetching = true;

            pt_products_fetched = false;
            StartCoroutine(doFetchProducts(fetch_user_products, user_products));
        }

        /// <summary>
        /// Sets all products from Product Get All request.
        /// </summary>
        /// <param name="products"></param>
        public void setAllProducts(ProductResult[] products) {
            if (products == null) {
                return;
            }

            foreach (var product in products) {
                bool found = false;

                foreach (var product_existing in m_products) {
                    if (product.id == product_existing.pt_id) {
                        found = true;
                        break;
                    }
                }

                if (!found) {
                    Product new_product = new Product(product.id, product.name);
                    new_product.setPrice(product.price);
                    new_product.setCurrency(m_currency_manager.getCurrency(product.currency.id));
                    new_product.setVat(product.vat);
                    new_product.setPriceVat(product.price_vat);
                    new_product.setDescriptionSmall(product.description_small);
                    new_product.setDescription(product.description);
                    new_product.setQuantity(product.quantity);
                    new_product.setHidden(product.hidden);
                    new_product.setStorable(product.storable);
                    new_product.setUnit(getUnit(product.unit.id, product.unit.name));
                    new_product.setCategory(getCategory(product.category.id, product.category.name));

                    m_products.Add(new_product);
                }
            }
        }

        /// <summary>
        /// Sets user products from Product Get User request.
        /// </summary>
        /// <param name="user_products"></param>
        public void setUserProducts(ProductSumResult[] user_products) {
            foreach (var user_product in user_products) {
                if (user_product.product == null || user_product.sum <= 0) {
                    continue;
                }

                Product product = null;

                foreach (var product_existing in m_products) {
                    if (product_existing.pt_id == user_product.product.id) {
                        product = product_existing;
                        break;
                    }
                }

                if (product == null) {
                    product = new Product(user_product.product.id, user_product.product.name);
                    product.setPrice(user_product.product.price);
                    product.setCurrency(m_currency_manager.getCurrency(user_product.product.currency.id));
                    product.setVat(user_product.product.vat);
                    product.setPriceVat(user_product.product.price_vat);
                    product.setDescriptionSmall(user_product.product.description_small);
                    product.setDescription(user_product.product.description);
                    product.setQuantity(user_product.product.quantity);
                    product.setHidden(user_product.product.hidden);
                    product.setStorable(user_product.product.storable);
                    product.setUnit(getUnit(user_product.product.unit.id, user_product.product.unit.name));
                    product.setCategory(getCategory(user_product.product.category.id, user_product.product.category.name));

                    m_products.Add(product);
                }

                product.setBought(true, user_product.sum);
                m_api_manager.pt_user.addProduct(product);
            }
        }

        /// <summary>
        /// Creates order to buy one product of any quantity.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <returns>Order object.</returns>
        public Order buyProduct(uint id, float quantity) {
            return buyProduct(getProduct(id), quantity);
        }

        /// <summary>
        /// Creates order to buy one product of any quantity.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        /// <returns>Order object.</returns>
        public Order buyProduct(Product product, float quantity) {
            if (product == null) {
                return null;
            }

            Order order = new Order();
            order.addProduct(product, quantity);

            sendOrder(order);

            return order;
        }

        /// <summary>
        /// Starts the coroutine to send the order.
        /// </summary>
        /// <param name="order"></param>
        public void sendOrder(Order order) {
            order.p_done = false;
            order.p_success = false;
            order.p_success_all = false;

            StartCoroutine(doSendOrder(order));
        }
    }
}
