using System;

namespace Ebenit.Requests.Containers.Results
{
    /// <summary>
    /// Additional informations in result from User New Alias request response.
    /// </summary>
    [Serializable]
    public class UserNewAliasResult : StandardResult
    {
        public uint eid = 0;
    }
}
