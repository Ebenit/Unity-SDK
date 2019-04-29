using System;

namespace Ebenit.Requests.Containers.Errors
{
    /// <summary>
    /// Container of standard errors returned by EbenitAPI.
    /// </summary>
    [Serializable]
    public class UserNewAliasErrors : StandardErrors
    {
        public bool password = false;
    }
}
