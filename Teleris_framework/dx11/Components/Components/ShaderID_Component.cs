using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teleris.Components.Components
{

    public sealed class ShaderIDComponent : IComponent
    {

        private string _ShaderID;

        public string ShaderID
        {
            get { return _ShaderID; }

            set { _ShaderID = value; }
        }

    }    

}
