using Ebenit.Requests.Containers.Errors;
using System;

namespace Ebenit.Requests.Containers.Responses
{
    [Serializable]
    public abstract class AResponse<R, E>
    {
        public R results;
        public E errors;

        public string id;
        public string signature;
    }
}
