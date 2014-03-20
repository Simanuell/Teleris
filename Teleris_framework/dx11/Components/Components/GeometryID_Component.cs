using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teleris.Components.Components
{

    public sealed class GeometryIDComponent : IComponent
    {

        private string _GeometryID;

        public string GeometryID
        {
            get { return _GeometryID; }

            set { _GeometryID = value; }
        }

    }    

}
