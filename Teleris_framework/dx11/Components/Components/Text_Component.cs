using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teleris.Entities;
using Teleris.Components;

namespace Teleris.Components
{
    public sealed class TextComponent : IComponent
    {

        private string _text;

        public string Text
        {
            get { return _text; }

            set { _text = value; }
        }

    }
}