using System;

namespace Ebenit.Containers
{
    /// <summary>
    /// Order discount container.
    /// </summary>
    [Serializable]
    public class OrderDiscount
    {
        /// <summary>
        /// Name of the discount.
        /// </summary>
        public string pt_name {
            get; protected set;
        }
        /// <summary>
        /// Percentage value of the discount.
        /// </summary>
        public float pt_percentage {
            get; protected set;
        }

        public OrderDiscount(string name, float percentage) {
            this.pt_name = name;
            this.pt_percentage = percentage;
        }
    }
}
