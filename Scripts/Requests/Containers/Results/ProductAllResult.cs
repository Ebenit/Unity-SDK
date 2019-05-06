using System;

namespace Ebenit.Requests.Containers.Results
{
    /// <summary>
    /// Additional informations in result from Product Get All request response.
    /// </summary>
    [Serializable]
    public class ProductAllResult : StandardResult
    {
        public ProductResult[] products = null;
    }
}
