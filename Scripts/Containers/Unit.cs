using System;

namespace Ebenit.Containers
{
    [Serializable]
    public class Unit
    {
        public uint pt_id {
            get; protected set;
        }
        public string pt_name {
            get; protected set;
        }

        public Unit(uint id, string name) {
            this.pt_id = id;
            this.pt_name = name;
        }
    }
}
