using System;

namespace Ebenit.Requests.Containers.Errors
{
    /// <summary>
    /// Container of standard errors returned by EbenitAPI.
    /// </summary>
    [Serializable]
    public class StandardErrors
    {
        public bool noInputData = false;
        public bool noSuchRequest = false;
        public bool dbConn = false;
        public bool auth = false;
        public bool authUser = false;
        public bool DBquery = false;

        public bool incorrectData = false;
        public bool platform = false;
        public bool user = false;
    }
}
