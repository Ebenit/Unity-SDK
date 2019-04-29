using Ebenit.Containers;
using System.Collections;
using UnityEngine;

using System;
using System.Collections.Generic;
using Ebenit.Requests.Containers.Responses;

namespace Ebenit.Managers
{
    public class HighscoreManager : MonoBehaviour
    {
        protected static HighscoreManager t_instance;
        public static HighscoreManager getInstance() {
            if (t_instance == null)
                return new GameObject("Ebenit.Managers.HighscoreManager").AddComponent<HighscoreManager>();

            return t_instance;
        }

        private HashSet<HighscoreTableRowPlatform> m_platforms = new HashSet<HighscoreTableRowPlatform>();

        private RequestManager m_request_manager = null;

        void Awake() {
            if (t_instance != null) {
                Destroy(gameObject);
            } else {
                t_instance = this;
                DontDestroyOnLoad(this.gameObject);

                m_request_manager = RequestManager.getInstance();
            }
        }

        public void resetToDefault() {
            m_platforms.Clear();
        }

        public HighscoreSave uploadScore(string table_id, int score) {
            var highscore = new HighscoreSave(table_id, score);

            StartCoroutine(doUploadScore(highscore));

            return highscore;
        }

        private HighscoreTableRowPlatform getHighscoreTableRowPlatform(uint id, string name) {
            foreach (HighscoreTableRowPlatform platform in m_platforms) {
                if (platform.pt_id == id) {
                    return platform;
                }
            }

            HighscoreTableRowPlatform new_platform = new HighscoreTableRowPlatform(id, name);
            m_platforms.Add(new_platform);

            return new_platform;
        }

        private void fillInTableFromResponse(HighscoreTable table, HighscoreGetResponse response) {
            if (response != null && response.results != null && response.results.success && response.results.head != null) {
                table.pt_head.setTableRowsCurrentView(response.results.head.rows_count);
                if (response.results.head.table != null) {
                    table.pt_head.setTableRowsSum(response.results.head.table.rows_count);
                }

                table.p_rows.Clear();
                if (response.results.rows != null) {
                    foreach (var row in response.results.rows) {
                        User user = new User(row.user.id, row.user.nickname);
                        user.setEid(row.user.eid);

                        HighscoreTableRow tableRow = new HighscoreTableRow(user, getHighscoreTableRowPlatform(row.platform.id, row.platform.name), row.rank, row.score, DateTime.Parse(row.scored_at));
                        table.p_rows.Add(tableRow);
                    }
                }
            }
        }

        private IEnumerator doUploadScore(HighscoreSave highscore) {
            var request = m_request_manager.createHighscoreSaveRequest(highscore);

            yield return request.send();

            highscore.p_done = true;

            var result = request.pt_response as HighscoreSaveResponse;
            if (request != null && result.results != null) {
                highscore.p_success = result.results.success;

                try {
                    highscore.p_change = (HighscoreSave.HighscoreChange)Enum.ToObject(typeof(HighscoreSave.HighscoreChange), result.results.data);
                } catch (Exception) {
                    highscore.p_change = HighscoreSave.HighscoreChange.NOT_DEFINED;
                }
            }
        }

        public HighscoreTable getHighscoreAll(HighscoreTableHead table_head, bool reverse_order = false) {
            var table = new HighscoreTable(table_head);

            getHighscoreAll(table, reverse_order);

            return table;
        }

        public void getHighscoreAll(HighscoreTable table, bool reverse_order = false) {
            table.p_done = false;

            StartCoroutine(doGetHighscoreAll(table, reverse_order));
        }

        private IEnumerator doGetHighscoreAll(HighscoreTable table, bool reverse_order = false) {
            var request = m_request_manager.createHighscoreGetAllRequest(table.pt_head.pt_table_id == null ? table.pt_head.pt_table_name : table.pt_head.pt_table_id.ToString(), table.pt_head.pt_table_from_platform, table.pt_head.pt_table_rows_page_view, table.pt_head.pt_table_rows_max_view, reverse_order);

            yield return request.send();

            fillInTableFromResponse(table, request.pt_response as HighscoreGetResponse);

            table.p_done = true;
        }

        public HighscoreTable getHighscoreAroundUser(HighscoreTableHead table_head, bool platform_user = true) {
            var table = new HighscoreTable(table_head);

            getHighscoreAroundUser(table, platform_user);

            return table;
        }

        public void getHighscoreAroundUser(HighscoreTable table, bool platform_user = true) {
            table.p_done = false;

            StartCoroutine(doGetHighscoreAroundUser(table, platform_user));
        }

        private IEnumerator doGetHighscoreAroundUser(HighscoreTable table, bool platform_user = true) {
            var request = m_request_manager.createHighscoreGetAroundUserRequest(table.pt_head.pt_table_id == null ? table.pt_head.pt_table_name : table.pt_head.pt_table_id.ToString(), table.pt_head.pt_table_rows_max_view, platform_user);

            yield return request.send();

            fillInTableFromResponse(table, request.pt_response as HighscoreGetResponse);

            table.p_done = true;
        }
    }
}
