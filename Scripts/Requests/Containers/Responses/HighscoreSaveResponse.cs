using Ebenit.Requests.Containers.Errors;
using Ebenit.Requests.Containers.Results;
using System;

namespace Ebenit.Requests.Containers.Responses
{
    /// <summary>
    /// Container of Highscore Save request response.
    /// </summary>
    [Serializable]
    public class HighscoreSaveResponse : AResponse<HighscoreSaveResult, StandardErrors>
    {
    }
}
