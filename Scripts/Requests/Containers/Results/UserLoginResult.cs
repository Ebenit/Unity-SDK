using System;

namespace Ebenit.Requests.Containers.Results
{
    [Serializable]
    public class UserLoginResult : StandardResult
    {
        public string user_token = null;
        public uint eid = 0;
        public string nickname = null;
        public uint platform_id = 0;
        public Currency[] currencies = null;
        public ProductSumResult[] products = null;

        /// <summary>
        /// Container of currency loging request result.
        /// </summary>
        [Serializable]
        public class Currency
        {
            public uint id = 0;
            public string name = null;
            public float num = 0;
            public float default_value = 0;
            public float min_value = 0;
            public float max_value = 0;
        }
    }
}
