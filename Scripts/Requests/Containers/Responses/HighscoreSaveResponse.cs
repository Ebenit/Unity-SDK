using Ebenit.Requests.Containers.Errors;
using Ebenit.Requests.Containers.Results;
using System;

namespace Ebenit.Requests.Containers.Responses
{
    /// <summary>
    /// Container of product by user requests response.
    /// </summary>
    [Serializable]
    public class HighscoreSaveResponse : AResponse<HighscoreSaveResult, StandardErrors>
    {
    }
}
