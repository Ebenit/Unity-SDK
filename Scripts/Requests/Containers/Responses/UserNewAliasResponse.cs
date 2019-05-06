using Ebenit.Requests.Containers.Errors;
using Ebenit.Requests.Containers.Results;
using System;

namespace Ebenit.Requests.Containers.Responses
{
    /// <summary>
    /// Container of User New Alias request response.
    /// </summary>
    [Serializable]
    public class UserNewAliasResponse : AResponse<UserNewAliasResult, UserNewAliasErrors>
    {
    }
}
