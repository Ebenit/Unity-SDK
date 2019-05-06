using Ebenit.Containers;
using Ebenit.Requests.Containers.Results;
using System.Collections.Generic;
using UnityEngine;

namespace Ebenit.Managers
{
    /// <summary>
    /// Handles all about currency.
    /// </summary>
    public class CurrencyManager : MonoBehaviour
    {
        protected static CurrencyManager t_instance;
        public static CurrencyManager getInstance() {
            if (t_instance == null)
                return new GameObject("Ebenit.Managers.CurrencyManager").AddComponent<CurrencyManager>();

            return t_instance;
        }

        /// <summary>
        /// Set of currencies.
        /// </summary>
        private HashSet<Currency> m_currencies = new HashSet<Currency>();

        /// <summary>
        /// True if update is in due.
        /// </summary>
        private bool m_do_update = false;

        /// <summary>
        /// Game time of last update;
        /// </summary>
        private float m_last_update_time = float.MinValue;

        /// <summary>
        /// Instance of ApiManager
        /// </summary>
        private ApiManager m_api_manager = null;
        /// <summary>
        /// Instance of RequestManager
        /// </summary>
        private RequestManager m_request_manager = null;

        void Awake() {
            if (t_instance != null) {
                Destroy(gameObject);
            } else {
                t_instance = this;
                DontDestroyOnLoad(this.gameObject);

                m_api_manager = ApiManager.getInstance();
                m_request_manager = RequestManager.getInstance();
            }
        }

        void Update() {
            if (m_api_manager.pt_online && m_do_update) {
                if (Time.time - m_last_update_time < m_api_manager.p_currency_update_interval) {
                    return;
                }

                m_do_update = false;

                updateNow();
            }
        }

        /// <summary>
        /// Starts the currency transaction coroutines (if online).
        /// </summary>
        public void updateNow() {
            if (m_api_manager.pt_online) {
                m_last_update_time = Time.time;

                foreach (var currency in m_currencies) {
                    if ((Mathf.Abs(currency.p_value_change) >= 0.001 || currency.pt_refresh)) {
                        StartCoroutine(m_request_manager.createCurrencyTransactionRequest(currency.pt_id, currency.p_value_change, currency.pt_refresh).send());

                        currency.p_value_change = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether all currency changes are up to date.
        /// </summary>
        /// <returns>True - if all currencies are up to date. False - otherwise</returns>
        public bool isCurrenciesUpdated() {
            foreach (var currency in m_currencies) {
                if (Mathf.Abs(currency.p_value_change) >= 0.001 || currency.pt_refresh) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Resets the manager values to default.
        /// </summary>
        public void resetToDefault() {
            m_currencies.Clear();
            m_do_update = false;
            m_last_update_time = float.MinValue;
        }

        /// <summary>
        /// Fills-in currencies from LogingResponse.
        /// </summary>
        /// <param name="currencies"></param>
        public void setCurrencies(UserLoginResult.Currency[] currencies) {
            if (currencies == null) {
                return;
            }

            m_currencies.Clear();

            ProductManager product_manager = ProductManager.getInstance();

            foreach (var currency in currencies) {
                Currency m_currency = new Currency(currency.id, currency.name, currency.num, currency.default_value, currency.min_value, currency.max_value);
                m_currencies.Add(m_currency);

                product_manager.addUnit(m_currency);
            }
        }

        /// <summary>
        /// Gets currency value based on its ID in EbenitAPI.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public float getCurrencyValue(uint id) {
            foreach (Currency currency in m_currencies) {
                if (currency.pt_id == id) {
                    return currency.pt_value;
                }
            }

            return 0;
        }

        /// <summary>
        /// Gets currency value based on its name in EbenitAPI.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public float getCurrencyValue(string name) {
            if (string.IsNullOrEmpty(name)) {
                return 0;
            }

            foreach (Currency currency in m_currencies) {
                if (name.Equals(currency.pt_name)) {
                    return currency.pt_value;
                }
            }

            return 0;
        }

        /// <summary>
        /// Checks whether currency with this ID exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Currency getCurrency(uint id) {
            foreach (Currency currency in m_currencies) {
                if (currency.pt_id == id) {
                    return currency;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks whether currency with this name exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Currency getCurrency(string name) {
            if (string.IsNullOrEmpty(name)) {
                return null;
            }

            foreach (Currency currency in m_currencies) {
                if (name.Equals(currency.pt_name)) {
                    return currency;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks whether currency with this ID exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool isCurrency(uint id) {
            foreach (Currency currency in m_currencies) {
                if (currency.pt_id == id) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks whether currency with this name exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool isCurrency(string name) {
            if (string.IsNullOrEmpty(name)) {
                return false;
            }

            foreach (Currency currency in m_currencies) {
                if (name.Equals(currency.pt_name)) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds value to currency based on its ID in EbenitAPI.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="update"></param>
        /// <returns>True if addition was successful.</returns>
        public bool addToCurrency(uint id, float value, bool update = true) {
            foreach (Currency currency in m_currencies) {
                if (currency.pt_id == id) {
                    if (update) {
                        m_do_update = true;
                        currency.addToValue(value);
                    } else {
                        currency.addToValueWithoutChange(value);
                    }
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds value to currency based on its name in EbenitAPI.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="update"></param>
        /// <returns>True if addition was successful.</returns>
        public bool addToCurrency(string name, float value, bool update = true) {
            if (string.IsNullOrEmpty(name)) {
                return false;
            }

            foreach (Currency currency in m_currencies) {
                if (name.Equals(currency.pt_name)) {
                    if (update) {
                        m_do_update = true;
                        currency.addToValue(value);
                    } else {
                        currency.addToValueWithoutChange(value);
                    }
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds value to currency.
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="value"></param>
        /// <param name="update"></param>
        /// <returns>True if addition was successful.</returns>
        public bool addToCurrency(Currency currency, float value, bool update = true) {
            if (currency == null) {
                return false;
            }

            if (m_currencies.Contains(currency)) {
                if (update) {
                    m_do_update = true;
                    currency.addToValue(value);
                } else {
                    currency.addToValueWithoutChange(value);
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the currency for refresh request.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True - if the currency was set to refresh. False - otherwise.</returns>
        public bool refreshCurrency(uint id) {
            foreach (Currency currency in m_currencies) {
                if (currency.pt_id == id) {
                    m_do_update = true;
                    currency.setRefresh();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the currency for refresh request.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True - if the currency was set to refresh. False - otherwise.</returns>
        public bool refreshCurrency(string name) {
            if (string.IsNullOrEmpty(name)) {
                return false;
            }

            foreach (Currency currency in m_currencies) {
                if (name.Equals(currency.pt_name)) {
                    m_do_update = true;
                    currency.setRefresh();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the currency for refresh request.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool refreshCurrency(Currency currency) {
            if (currency == null) {
                return false;
            }

            if (m_currencies.Contains(currency)) {
                m_do_update = true;
                currency.setRefresh();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reset currency with ID to the default value.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if the currency was reseted.</returns>
        public bool resetCurrencyToDefault(uint id) {
            foreach (Currency currency in m_currencies) {
                if (currency.pt_id == id) {
                    currency.addToValue(currency.pt_default_value - currency.pt_value);
                    m_do_update = true;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Reset currency with name to the default value.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True if the currency was reseted.</returns>
        public bool resetCurrencyToDefault(string name) {
            if (string.IsNullOrEmpty(name)) {
                return false;
            }

            foreach (Currency currency in m_currencies) {
                if (name.Equals(currency.pt_name)) {
                    currency.addToValue(currency.pt_default_value - currency.pt_value);
                    m_do_update = true;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Reset currency to the default value.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True if the currency was reseted.</returns>
        public bool resetCurrencyToDefault(Currency currency) {
            if (currency == null) {
                return false;
            }

            if (m_currencies.Contains(currency)) {
                currency.addToValue(currency.pt_default_value - currency.pt_value);
                m_do_update = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the currency value after refresh.
        /// 
        /// The currency needs to be in refresh mode to be able to use this method.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns>True - if the currency value was refreshed. False - otherwise.</returns>
        public bool setCurrencyValueAfterRefresh(uint id, float value) {
            foreach (Currency currency in m_currencies) {
                if (currency.pt_id == id) {
                    if (currency.pt_refresh) {
                        currency.setValueRefresh(value);

                        return true;
                    } else {
                        return false;
                    }
                }
            }

            return false;
        }
    }
}
