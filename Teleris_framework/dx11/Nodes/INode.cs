using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teleris.Entities;

namespace Teleris.Nodes.Nodes
{
    public class INode
    {
        /// <summary>
        /// The entity whose components are included in the node.
        /// </summary>
        public Entity Entity { get; set; }

        /// <summary>
        /// Used by the NodeList class. The previous node in a node list.
        /// </summary>
        public INode Previous { get; set; }

        /// <summary>
        /// Used by the NodeList class. The next node in a node list.
        /// </summary>
        public INode Next { get; set; }

        public object GetProperty(string propertyName)
        {

            return GetType().GetProperty(propertyName).GetValue(this, null);

        }

        public void SetProperty(string propertyName, object value)
        {
            GetType().GetProperty(propertyName).SetValue(this, value, null);
        }
    }
}
