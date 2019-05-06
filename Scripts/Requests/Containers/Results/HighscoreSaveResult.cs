using System;

namespace Ebenit.Requests.Containers.Results
{
    /// <summary>
    /// Additional informations in result from Highscore Save request response.
    /// </summary>
    [Serializable]
    public class HighscoreSaveResult : StandardResult
    {
        public int data = 0;
    }
}
