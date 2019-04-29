using Ebenit.Containers;
using Ebenit.Requests.Containers.Responses;
using Ebenit.Requests.Containers.Results;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ebenit.Managers
{
    public class ProductManager : MonoBehaviour
    {
        protected static ProductManager t_instance;
        public static ProductManager getInstance() {
            if (t_instance == null)
                return new GameObject("Ebenit.Managers.ProductManager").AddComponent<ProductManager>();

            return t_instance;
        }

        public bool pt_products_fetched {
            get; protected set;
        }
        public bool pt_products_fetching {
            get; protected set;
        }

        private HashSet<Category> m_categories = new HashSet<Category>();
        private HashSet<Unit> m_units = new HashSet<Unit>();
        private List<Product> m_products = new List<Product>();

        private ApiManager m_api_manager = null;
        private CurrencyManager m_currency_manager = null;
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

        public void resetToDefault() {
            m_units.Clear();
            m_products.Clear();

            pt_products_fetched = false;
            pt_products_fetching = false;
        }

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

        public Product getProduct(uint id) {
            foreach (Product product in m_products) {
                if (product.pt_id == id) {
                    return product;
                }
            }

            return null;
        }

        public List<Product> getAllProducts() {
            return m_products;
        }

        public void fetchProducts(bool fetch_user_products = true, ProductSumResult[] user_products = null) {
            if (pt_products_fetching) {
                return;
            }

            pt_products_fetching = true;

            pt_products_fetched = false;
            StartCoroutine(doFetchProducts(fetch_user_products, user_products));
        }

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

        public Order buyProduct(uint id, float quantity) {
            return buyProduct(getProduct(id), quantity);
        }

        public Order buyProduct(Product product, float quantity) {
            if (product == null) {
                return null;
            }

            Order order = new Order();
            order.addProduct(product, quantity);

            sendOrder(order);

            return order;
        }

        public void sendOrder(Order order) {
            order.p_done = false;
            order.p_success = false;
            order.p_success_all = false;

            StartCoroutine(doSendOrder(order));
        }

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
    }
}
