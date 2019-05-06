using System;
using System.Collections.Generic;

namespace Ebenit.Containers
{
    /// <summary>
    /// Highscore table container.
    /// </summary>
    [Serializable]
    public class HighscoreTable
    {
        /// <summary>
        /// True if the highscore table fetching is done.
        /// </summary>
        public bool p_done = false;

        /// <summary>
        /// Summary informations about table.
        /// </summary>
        public HighscoreTableHead pt_head {
            get; protected set;
        }

        /// <summary>
        /// Individual rows of table.
        /// </summary>
        public List<HighscoreTableRow> p_rows = new List<HighscoreTableRow>();

        public HighscoreTable(HighscoreTableHead head) {
            this.pt_head = head;
        }
    }
}
