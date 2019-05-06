using System;

namespace Ebenit.Containers
{
    /// <summary>
    /// Container for Highscore Save request.
    /// </summary>
    [Serializable]
    public class HighscoreSave
    {
        /// <summary>
        /// Possible request results.
        /// </summary>
        public enum HighscoreChange
        {
            NOT_DEFINED = 0,
            NOT_CHANGED,
            NEW_RECORD,
        }

        /// <summary>
        /// Request result.
        /// </summary>
        public HighscoreChange p_change = HighscoreChange.NOT_DEFINED;

        /// <summary>
        /// True if the request was proccessed.
        /// </summary>
        public bool p_done = false;
        /// <summary>
        /// True if the request was successful.
        /// </summary>
        public bool p_success = false;

        /// <summary>
        /// Score to submit.
        /// </summary>
        public int p_score;

        /// <summary>
        /// ID or name of highscore table in Ebenit API.
        /// </summary>
        public string pt_table_id {
            get; protected set;
        }

        public HighscoreSave(string table_id, int score = 0) {
            this.pt_table_id = table_id;
            this.p_score = score;
        }
    }
}
