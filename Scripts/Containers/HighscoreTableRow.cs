using System;

namespace Ebenit.Containers
{
    /// <summary>
    /// Information about one row in highscore table.
    /// </summary>
    [Serializable]
    public class HighscoreTableRow
    {
        /// <summary>
        /// User that achieved the score.
        /// </summary>
        public User pt_user {
            get; protected set;
        }

        /// <summary>
        /// Informations about platform where the score was achieved.
        /// </summary>
        public HighscoreTableRowPlatform pt_platform {
            get; protected set;
        }

        /// <summary>
        /// Score's global rank.
        /// </summary>
        public int pt_rank {
            get; protected set;
        }

        /// <summary>
        /// Score that was achieved.
        /// </summary>
        public int pt_score {
            get; protected set;
        }

        /// <summary>
        /// Timestamp when the score was achieved.
        /// </summary>
        public DateTime pt_scored_at {
            get; protected set;
        }

        public HighscoreTableRow(User user, HighscoreTableRowPlatform platform, int rank, int score, DateTime scored_at) {
            this.pt_user = user;
            this.pt_platform = platform;
            this.pt_rank = rank;
            this.pt_score = score;
            this.pt_scored_at = scored_at;
        }
    }
}
