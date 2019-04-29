using System;

namespace Ebenit.Requests.Containers.Results
{
    [Serializable]
    public class ProductByUserResult : StandardResult
    {
        public ProductSumResult[] products = null;
    }
}
