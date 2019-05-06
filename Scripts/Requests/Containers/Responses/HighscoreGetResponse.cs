using Ebenit.Requests.Containers.Errors;
using Ebenit.Requests.Containers.Results;
using System;

namespace Ebenit.Requests.Containers.Responses
{
    /// <summary>
    /// Container for highscore get requests responses.
    /// </summary>
    [Serializable]
    public class HighscoreGetResponse : AResponse<HighscoreGetResult, StandardErrors>
    {
    }
}
