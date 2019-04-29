using Ebenit.Requests.Containers.Errors;
using Ebenit.Requests.Containers.Results;
using System;

namespace Ebenit.Requests.Containers.Responses
{
    /// <summary>
    /// Container of currency transaction request response.
    /// </summary>
    [Serializable]
    public class CurrencyTransactionResponse : AResponse<CurrencyTransactionResult, StandardErrors>
    {
    }
}
