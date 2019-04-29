using System;

namespace Ebenit.Containers
{
    [Serializable]
    public class OrderDiscount
    {
        public string pt_name {
            get; protected set;
        }
        public float pt_percentage {
            get; protected set;
        }

        public OrderDiscount(string name, float percentage) {
            this.pt_name = name;
            this.pt_percentage = percentage;
        }
    }
}
