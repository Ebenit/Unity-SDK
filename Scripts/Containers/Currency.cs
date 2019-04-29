using UnityEngine;

namespace Ebenit.Containers
{
    public class Currency : Unit
    {
        public float pt_value {
            get; protected set;
        }

        public float pt_default_value {
            get; protected set;
        }

        public float pt_min_value {
            get; protected set;
        }

        public float pt_max_value {
            get; protected set;
        }

        public float p_value_change = 0;

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

        public void addToValue(float value) {
            float real_change = Mathf.Floor(value * 1000.0f) / 1000.0f;

            pt_value += real_change;
            p_value_change += real_change;
        }

        public void addToValueWithoutChange(float value) {
            float real_change = Mathf.Floor(value * 1000.0f) / 1000.0f;

            pt_value += real_change;
        }

        public void setRefresh(bool refresh = true) {
            this.pt_refresh = refresh;
        }

        public void setValueRefresh(float value) {
            if (pt_refresh) {
                pt_value = value;
                pt_refresh = false;
            }
        }
    }
}
