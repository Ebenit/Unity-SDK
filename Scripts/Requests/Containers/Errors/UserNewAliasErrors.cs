using System;

namespace Ebenit.Requests.Containers.Errors
{
    /// <summary>
    /// Additional errors the User New Alias request may return.
    /// </summary>
    [Serializable]
    public class UserNewAliasErrors : StandardErrors
    {
        public bool password = false;
    }
}
