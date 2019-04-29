using System;

namespace Ebenit.Containers
{
    [Serializable]
    public class HighscoreTableHead {
        public uint? pt_table_id {
            get; protected set;
        }

        public string pt_table_name {
            get; protected set;
        }

        public bool pt_table_from_platform {
            get; protected set;
        }

        public uint pt_table_rows_sum {
            get; protected set;
        }

        public uint pt_table_rows_current_view {
            get; protected set;
        }

        public uint pt_table_rows_max_view {
            get; protected set;
        }

        public uint pt_table_rows_page_view {
            get; protected set;
        }

        public HighscoreTableHead(uint? table_id, string table_name, uint table_rows_max, bool table_from_platform = true) {
            this.pt_table_id = table_id;
            this.pt_table_name = table_name;

            this.pt_table_rows_max_view = table_rows_max;

            this.pt_table_from_platform = table_from_platform;
        }

        public void setTableRowsSum(uint rows) {
            this.pt_table_rows_sum = rows;
        }

        public void setTableRowsCurrentView(uint rows) {
            this.pt_table_rows_current_view = rows;
        }

        public void setTableRowsPageView(uint page) {
            this.pt_table_rows_page_view = page;
        }
    }
}
