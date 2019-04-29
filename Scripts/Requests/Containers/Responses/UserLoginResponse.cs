using Ebenit.Requests.Containers.Errors;
using Ebenit.Requests.Containers.Results;
using System;

namespace Ebenit.Requests.Containers.Responses
{
    /// <summary>
    /// Container login request response.
    /// </summary>
    [Serializable]
    public class UserLoginResponse : AResponse<UserLoginResult, StandardErrors>
    {
    }
}
