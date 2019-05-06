using System;

namespace Ebenit.Requests.Containers.Results
{
    /// <summary>
    /// Container for user bought product in Ebenit API responses.
    /// </summary>
    [Serializable]
    public class ProductSumResult
    {
        public float sum = 0;
        public ProductResult product;
    }
}
