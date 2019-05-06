using Ebenit.Requests.Containers.Errors;
using Ebenit.Requests.Containers.Results;
using System;

namespace Ebenit.Requests.Containers.Responses
{
    /// <summary>
    /// Container of Order New request response.
    /// </summary>
    [Serializable]
    public class OrderNewResponse : AResponse<OrderNewResult, StandardErrors>
    {
    }
}
