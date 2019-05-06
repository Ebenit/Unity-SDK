using System;

namespace Ebenit.Requests.Containers.Results
{
    /// <summary>
    /// Container for result of LookUp table in EbenitAPI.
    /// </summary>
    [Serializable]
    public class LookupRowResult
    {
        public uint id = 0;
        public string name = null;
        public string code_name = null;
    }
}
