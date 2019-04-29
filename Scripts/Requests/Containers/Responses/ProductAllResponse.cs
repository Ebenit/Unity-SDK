using Ebenit.Requests.Containers.Errors;
using Ebenit.Requests.Containers.Results;
using System;

namespace Ebenit.Requests.Containers.Responses
{
    /// <summary>
    /// Container of product all requests response.
    /// </summary>
    [Serializable]
    public class ProductAllResponse : AResponse<ProductAllResult, StandardErrors>
    {
    }
}
