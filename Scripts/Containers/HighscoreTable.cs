using System;
using System.Collections.Generic;

namespace Ebenit.Containers
{
    /// <summary>
    /// Informations about product.
    /// </summary>
    [Serializable]
    public class HighscoreTable
    {
        public bool p_done = false;

        public HighscoreTableHead pt_head {
            get; protected set;
        }

        public List<HighscoreTableRow> p_rows = new List<HighscoreTableRow>();

        public HighscoreTable(HighscoreTableHead head) {
            this.pt_head = head;
        }
    }
}
