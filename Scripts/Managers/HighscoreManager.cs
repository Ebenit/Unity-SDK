using Ebenit.Containers;
using System.Collections;
using UnityEngine;

using System;
using System.Collections.Generic;
using Ebenit.Requests.Containers.Responses;

namespace Ebenit.Managers
{
    /// <summary>
    /// Handles all highscores requests.
    /// </summary>
    public class HighscoreManager : MonoBehaviour
    {
        protected static HighscoreManager t_instance;
        public static HighscoreManager getInstance() {
            if (t_instance == null)
                return new GameObject("Ebenit.Managers.HighscoreManager").AddComponent<HighscoreManager>();

            return t_instance;
        }

        /// <summary>
        /// Set with all fetched platforms. Platforms are fetched with the get requests (not every single platform from Ebenit API is fetched).
        /// </summary>
        private HashSet<HighscoreTableRowPlatform> m_platforms = new HashSet<HighscoreTableRowPlatform>();

        /// <summary>
        /// Instance of RequestManager
        /// </summary>
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

        /// <summary>
        /// Returns platform or sets it if the platform is not present yet.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Fills the table head and rows from Highscore Get request.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="response"></param>
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

        /// <summary>
        /// Coroutine which handles the Highscore Save request.
        /// </summary>
        /// <param name="highscore"></param>
        /// <returns></returns>
        private IEnumerator doUploadScore(HighscoreSave highscore) {
            var request = m_request_manager.createHighscoreSaveRequest(highscore);

            yield return request.send();

            var result = request.pt_response as HighscoreSaveResponse;
            if (request != null && result.results != null) {
                highscore.p_success = result.results.success;

                try {
                    highscore.p_change = (HighscoreSave.HighscoreChange)Enum.ToObject(typeof(HighscoreSave.HighscoreChange), result.results.data);
                } catch (Exception) {
                    highscore.p_change = HighscoreSave.HighscoreChange.NOT_DEFINED;
                }
            }

            highscore.p_done = true;
        }

        /// <summary>
        /// Coroutine which handles the Highscore Get All request.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="reverse_order"></param>
        /// <returns></returns>
        private IEnumerator doGetHighscoreAll(HighscoreTable table, bool reverse_order = false) {
            var request = m_request_manager.createHighscoreGetAllRequest(table.pt_head.pt_table_id == null ? table.pt_head.pt_table_name : table.pt_head.pt_table_id.ToString(), table.pt_head.pt_table_from_platform, table.pt_head.pt_table_rows_page_view, table.pt_head.pt_table_rows_max_view, reverse_order);

            yield return request.send();

            fillInTableFromResponse(table, request.pt_response as HighscoreGetResponse);

            table.p_done = true;
        }

        /// <summary>
        /// Coroutine which handles the Highscore Get Around User request.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="platform_user"></param>
        /// <returns></returns>
        private IEnumerator doGetHighscoreAroundUser(HighscoreTable table, bool platform_user = true) {
            var request = m_request_manager.createHighscoreGetAroundUserRequest(table.pt_head.pt_table_id == null ? table.pt_head.pt_table_name : table.pt_head.pt_table_id.ToString(), table.pt_head.pt_table_rows_max_view, platform_user);

            yield return request.send();

            fillInTableFromResponse(table, request.pt_response as HighscoreGetResponse);

            table.p_done = true;
        }

        /// <summary>
        /// Resets the manager values to default.
        /// </summary>
        public void resetToDefault() {
            m_platforms.Clear();
        }

        /// <summary>
        /// Starts the coroutine to upload highscore.
        /// </summary>
        /// <param name="table_id">ID or name of the highscore table.</param>
        /// <param name="score">New score to upload.</param>
        /// <returns>HighscoreSave instance.</returns>
        public HighscoreSave uploadScore(string table_id, int score) {
            var highscore = new HighscoreSave(table_id, score);

            StartCoroutine(doUploadScore(highscore));

            return highscore;
        }

        /// <summary>
        /// Starts the coroutine to get all highscores.
        /// </summary>
        /// <param name="table_head">HighscoreTableHead object of table to fetch.</param>
        /// <param name="reverse_order">True to fetch in DESC order. Defaults to false.</param>
        /// <returns>HighscoreTable object.</returns>
        public HighscoreTable getHighscoreAll(HighscoreTableHead table_head, bool reverse_order = false) {
            var table = new HighscoreTable(table_head);

            getHighscoreAll(table, reverse_order);

            return table;
        }

        /// <summary>
        /// Starts the coroutine to get all highscores.
        /// </summary>
        /// <param name="table">HighscoreTable object of the table to fetch.</param>
        /// <param name="reverse_order">True to fetch in DESC order. Defaults to false.</param>
        public void getHighscoreAll(HighscoreTable table, bool reverse_order = false) {
            table.p_done = false;

            StartCoroutine(doGetHighscoreAll(table, reverse_order));
        }

        /// <summary>
        /// Starts the coroutine to get highscores around user.
        /// </summary>
        /// <param name="table_head">HighscoreTableHead object of table to fetch.</param>
        /// <param name="platform_user">True to fetch highscores only from user platform. Defaults to true.</param>
        /// <returns>HighscoreTable object.</returns>
        public HighscoreTable getHighscoreAroundUser(HighscoreTableHead table_head, bool platform_user = true) {
            var table = new HighscoreTable(table_head);

            getHighscoreAroundUser(table, platform_user);

            return table;
        }

        /// <summary>
        /// Starts the coroutine to get highscores around user.
        /// </summary>
        /// <param name="table">HighscoreTable object of the table to fetch.</param>
        /// <param name="platform_user">True to fetch highscores only from user platform. Defaults to true.</param>
        public void getHighscoreAroundUser(HighscoreTable table, bool platform_user = true) {
            table.p_done = false;

            StartCoroutine(doGetHighscoreAroundUser(table, platform_user));
        }
    }
}
