using System;

namespace Ebenit.Containers
{
    /// <summary>
    /// Platform for highscore table container.
    /// </summary>
    [Serializable]
    public class HighscoreTableRowPlatform
    {
        /// <summary>
        /// Platform ID in Ebenit API.
        /// </summary>
        public uint pt_id {
            get; protected set;
        }

        /// <summary>
        /// Platform name in Ebenit API.
        /// </summary>
        public string pt_name {
            get; protected set;
        }

        public HighscoreTableRowPlatform(uint id, string name) {
            this.pt_id = id;
            this.pt_name = name;
        }
    }
}
