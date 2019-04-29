using System;

namespace Ebenit.Requests.Containers.Results
{
    [Serializable]
    public class ProductAllResult : StandardResult
    {
        public ProductResult[] products = null;
    }
}
