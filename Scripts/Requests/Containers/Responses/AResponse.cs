using System;

namespace Ebenit.Requests.Containers.Responses
{
    /// <summary>
    /// Standard response template.
    /// </summary>
    /// <typeparam name="R">Type of response result.</typeparam>
    /// <typeparam name="E">Type of errors.</typeparam>
    [Serializable]
    public abstract class AResponse<R, E>
    {
        /// <summary>
        /// Message results.
        /// </summary>
        public R results;
        /// <summary>
        /// Message errors returned by Ebenit API.
        /// </summary>
        public E errors;

        /// <summary>
        /// Message ID from Ebenit API.
        /// </summary>
        public string id;
        /// <summary>
        /// Message signature.
        /// </summary>
        public string signature;
    }
}
