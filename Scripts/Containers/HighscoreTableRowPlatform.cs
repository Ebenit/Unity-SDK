using System;

namespace Ebenit.Containers
{
    [Serializable]
    public class HighscoreTableRowPlatform
    {
        public uint pt_id {
            get; protected set;
        }

        public string pt_name {
            get; protected set;
        }

        public HighscoreTableRowPlatform(uint id, string name) {
            this.pt_id = id;
            this.pt_name = name;
        }
    }
}
