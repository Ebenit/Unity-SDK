using System;

namespace Ebenit.Containers
{
    /// <summary>
    /// Unit container.
    /// </summary>
    [Serializable]
    public class Unit
    {
        /// <summary>
        /// Unit ID in Ebenit API.
        /// </summary>
        public uint pt_id {
            get; protected set;
        }
        /// <summary>
        /// Unit name in Ebenit API.
        /// </summary>
        public string pt_name {
            get; protected set;
        }

        public Unit(uint id, string name) {
            this.pt_id = id;
            this.pt_name = name;
        }
    }
}
