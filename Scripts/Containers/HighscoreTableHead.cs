using System;

namespace Ebenit.Containers
{
    /// <summary>
    /// Summary informations about highscore table.
    /// </summary>
    [Serializable]
    public class HighscoreTableHead {
        /// <summary>
        /// ID of highscore table in Ebenit API. May be null if unknown. Is not changed by data fetch.
        /// </summary>
        public uint? pt_table_id {
            get; protected set;
        }

        /// <summary>
        /// Name of highscore table in Ebenit API. Is not change by data fetch.
        /// </summary>
        public string pt_table_name {
            get; protected set;
        }

        /// <summary>
        /// True to fetch data from user platform only. Defaults to true. Is not changed by data fetch.
        /// </summary>
        public bool pt_table_from_platform {
            get; protected set;
        }

        /// <summary>
        /// Number of all rows in highscore table (platfrom filter included). Is changed by data fetch.
        /// </summary>
        public uint pt_table_rows_sum {
            get; protected set;
        }

        /// <summary>
        /// Number of currently fetched rows. Is changed by data fetch.
        /// </summary>
        public uint pt_table_rows_current_view {
            get; protected set;
        }

        /// <summary>
        /// Maximum number of rows to fetch. Is not changed by data fetch.
        /// </summary>
        public uint pt_table_rows_max_view {
            get; protected set;
        }

        /// <summary>
        /// Highscore table page to fetch. Is not changed by data fetch.
        /// </summary>
        public uint pt_table_rows_page_view {
            get; protected set;
        }

        public HighscoreTableHead(uint? table_id, string table_name, uint table_rows_max, bool table_from_platform = true) {
            this.pt_table_id = table_id;
            this.pt_table_name = table_name;

            this.pt_table_rows_max_view = table_rows_max;

            this.pt_table_from_platform = table_from_platform;
        }

        /// <summary>
        /// Sets the number of all rows in highscore table.
        /// </summary>
        /// <param name="rows"></param>
        public void setTableRowsSum(uint rows) {
            this.pt_table_rows_sum = rows;
        }

        /// <summary>
        /// Sets the number of rows in current view. ("Number of currently fetched rows.")
        /// </summary>
        /// <param name="rows"></param>
        public void setTableRowsCurrentView(uint rows) {
            this.pt_table_rows_current_view = rows;
        }

        /// <summary>
        /// Sets the number of page to fetch.
        /// </summary>
        /// <param name="page"></param>
        public void setTableRowsPageView(uint page) {
            this.pt_table_rows_page_view = page;
        }
    }
}
