using System;
using System.Collections.Generic;

namespace Ebenit.Containers
{
    [Serializable]
    public class HighscoreTableRow
    {
        public User pt_user {
            get; protected set;
        }

        public HighscoreTableRowPlatform pt_platform {
            get; protected set;
        }

        public int pt_rank {
            get; protected set;
        }

        public int pt_score {
            get; protected set;
        }

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
