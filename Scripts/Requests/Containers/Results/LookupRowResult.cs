using System;

namespace Ebenit.Requests.Containers.Results
{
    /// <summary>
    /// Container for standard result of LookUp table in EbenitAPI.
    /// </summary>
    [Serializable]
    public class LookupRowResult : StandardResult
    {
        public uint id = 0;
        public string name = null;
        public string code_name = null;
    }
}
