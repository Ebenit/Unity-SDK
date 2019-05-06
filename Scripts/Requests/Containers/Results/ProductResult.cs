using System;

namespace Ebenit.Requests.Containers.Results
{
    /// <summary>
    /// Container of product request response (product row).
    /// </summary>
    [Serializable]
    public class ProductResult
    {
        public uint id = 0;
        public string name = null;
        public float price = 0;
        public LookupRowResult currency = null;
        public int vat = 0;
        public float price_vat = 0;
        public string description_small = null;
        public string description = null;
        public float quantity = 0;
        public bool hidden = false;
        public bool storable = false;
        public LookupRowResult unit = null;
        public LookupRowResult category = null;
    }
}
