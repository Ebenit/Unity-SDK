using System;

namespace Ebenit.Requests.Containers.Results
{
    /// <summary>
    /// Container of standard product request response.
    /// </summary>
    [Serializable]
    public class ProductSumResult : StandardResult
    {
        public float sum = 0;
        public ProductResult product;
    }
}
