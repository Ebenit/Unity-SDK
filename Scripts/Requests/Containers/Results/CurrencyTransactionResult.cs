using System;

namespace Ebenit.Requests.Containers.Results
{
    [Serializable]
    public class CurrencyTransactionResult : StandardResult
    {
        public float currency_num = 0;
    }
}
