using System;

namespace Ebenit.Requests.Containers.Results
{
    /// <summary>
    /// Additional informations in result from Currency Transaction request response.
    /// </summary>
    [Serializable]
    public class CurrencyTransactionResult : StandardResult
    {
        public float currency_num = 0;
    }
}
