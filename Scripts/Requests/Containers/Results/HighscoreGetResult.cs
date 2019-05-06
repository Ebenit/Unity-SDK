using System;

namespace Ebenit.Requests.Containers.Results
{
    /// <summary>
    /// Additional informations in result from Highscore Get (All/Around User) requests response.
    /// </summary>
    [Serializable]
    public class HighscoreGetResult : StandardResult
    {
        public Head head;
        public Row[] rows;

        [Serializable]
        public class Head
        {
            public HeadTable table;
            public uint rows_count;
        }

        [Serializable]
        public class HeadTable
        {
            public uint id;
            public string name;
            public uint rows_count;
        }

        [Serializable]
        public class Row
        {
            public UserRow user;
            public LookupRowResult platform;
            public int rank;
            public int score;
            public string scored_at;
        }

        [Serializable]
        public class UserRow
        {
            public uint eid;
            public string id;
            public string nickname;
        }
    }
}
