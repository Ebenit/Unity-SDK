using UnityEngine;

namespace Ebenit.Containers
{
    /// <summary>
    /// Currency container.
    /// </summary>
    public class Currency : Unit
    {
        /// <summary>
        /// Current currency value.
        /// </summary>
        public float pt_value {
            get; protected set;
        }

        /// <summary>
        /// Default currency value in EbenitAPI.
        /// </summary>
        public float pt_default_value {
            get; protected set;
        }

        /// <summary>
        /// Minimum possible value to set.
        /// </summary>
        public float pt_min_value {
            get; protected set;
        }

        /// <summary>
        /// Maximum possible value to set.
        /// </summary>
        public float pt_max_value {
            get; protected set;
        }

        /// <summary>
        /// Current value differential between in app value and server value. ("Current value transaction to send to server.")
        /// </summary>
        public float p_value_change = 0;

        /// <summary>
        /// True to refresh currency value from server.
        /// </summary>
        public bool pt_refresh {
            get; protected set;
        }

        public Currency(uint id, string name, float value, float default_value, float min_value, float max_value) : base(id, name) {
            this.pt_value = value;
            this.pt_default_value = default_value;
            this.pt_max_value = max_value;
            this.pt_min_value = min_value;

            this.pt_refresh = false;
        }

        /// <summary>
        /// Adds value to currency value.
        /// </summary>
        /// <param name="value"></param>
        public void addToValue(float value) {
            float real_change = Mathf.Floor(value * 1000.0f) / 1000.0f;

            if (pt_value + real_change > pt_max_value) {
                pt_value = pt_max_value;
                real_change = pt_max_value - pt_value;
            } else if (pt_value + real_change < pt_min_value) {
                pt_value = pt_min_value;
                real_change = pt_min_value - pt_value;
            } else {
                pt_value += real_change;
            }

            p_value_change += real_change;
        }

        /// <summary>
        /// Adds value to currency value, but this change is not reflected to p_value_change variable and thus is not sent to server as transaction.
        /// </summary>
        /// <param name="value"></param>
        public void addToValueWithoutChange(float value) {
            float real_change = Mathf.Floor(value * 1000.0f) / 1000.0f;

            pt_value += real_change;

            if (pt_value > pt_max_value) {
                pt_value = pt_max_value;
            } else if (pt_value < pt_min_value) {
                pt_value = pt_min_value;
            }
        }

        /// <summary>
        /// Sets the refresh property. Defaults to set to true. The currency is then subject of currency refresh from server.
        /// </summary>
        /// <param name="refresh"></param>
        public void setRefresh(bool refresh = true) {
            this.pt_refresh = refresh;
        }

        /// <summary>
        /// Sets the currency value after refresh.
        /// 
        /// If the currency is not in refresh, this method does nothing.
        /// </summary>
        /// <param name="value"></param>
        public void setValueRefresh(float value) {
            if (pt_refresh) {
                pt_value = value;
                if (pt_value > pt_max_value) {
                    pt_value = pt_max_value;
                } else if (pt_value < pt_min_value) {
                    pt_value = pt_min_value;
                }

                pt_refresh = false;
            }
        }
    }
}
