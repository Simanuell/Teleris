using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teleris.Entities;
using Teleris.Components;

namespace Teleris.Components
{
    public sealed class NumberComponent : IComponent
    {

        private uint _number;

        public uint Number
        {
            get { return _number; }

            set { _number = value; }
        }

    }
}