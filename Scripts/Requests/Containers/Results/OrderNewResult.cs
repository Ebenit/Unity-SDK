using System;

namespace Ebenit.Requests.Containers.Results
{
    [Serializable]
    public class OrderNewResult : StandardResult
    {
        public Order order = null;

        [Serializable]
        public class Order
        {
            public uint id = 0;
            public LookupRowResult payment_type;
            public string date_created;
            public string date_due;
            public string date_taxable;
            public float total_price;
            public Product[] products;
            public Discount[] discounts;
        }

        [Serializable]
        public class Product
        {
            public uint id;
            public string name;
            public float quantity;
            public float num;
            public float price;
            public LookupRowResult currency = null;
            public LookupRowResult unit = null;
            public float price_vat;
        }

        [Serializable]
        public class Discount
        {
            public string name;
            public float price;
        }
    }
}
