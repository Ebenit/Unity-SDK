using Ebenit.Requests.Containers.Errors;
using Ebenit.Requests.Containers.Responses;
using Ebenit.Requests.Containers.Results;

namespace Ebenit.Requests.Api
{
    /// <summary>
    /// Simple container used for retrieving information for message verification.
    /// </summary>
    public class ApiRequestResponse: AResponse<StandardResult, StandardErrors>
    {
    }
}
