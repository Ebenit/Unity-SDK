using System;

namespace Ebenit.Requests.Containers.Results
{
    /// <summary>
    /// Additional informations in result from Product Get By User request response.
    /// </summary>
    [Serializable]
    public class ProductByUserResult : StandardResult
    {
        public ProductSumResult[] products = null;
    }
}
