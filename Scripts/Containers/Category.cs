using System;

namespace Ebenit.Containers
{
    /// <summary>
    /// Category container.
    /// </summary>
    [Serializable]
    public class Category
    {
        /// <summary>
        /// Category ID in Ebenit API.
        /// </summary>
        public uint pt_id {
            get; protected set;
        }
        /// <summary>
        /// Category name in Ebenit API.
        /// </summary>
        public string pt_name {
            get; protected set;
        }

        public Category(uint id, string name) {
            this.pt_id = id;
            this.pt_name = name;
        }
    }
}
