using System;

namespace Ebenit.Containers
{
    [Serializable]
    public class HighscoreSave
    {
        public enum HighscoreChange
        {
            NOT_DEFINED = 0,
            NOT_CHANGED,
            NEW_RECORD,
        }

        public HighscoreChange p_change = HighscoreChange.NOT_DEFINED;

        public bool p_done = false;
        public bool p_success = false;

        public int p_score;

        public string pt_table_id {
            get; protected set;
        }

        public HighscoreSave(string table_id, int score = 0) {
            this.pt_table_id = table_id;
            this.p_score = score;
        }
    }
}
