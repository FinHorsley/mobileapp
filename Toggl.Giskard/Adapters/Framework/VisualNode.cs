using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Toggl.Giskard.Adapters
{
    public class VisualNode : Node
    {
        public VisualNode(object item, IEnumerable<Node> items = null)
            : base(items)
        {
            UnderlyingItem = item;
        }

        public object UnderlyingItem { get; private set; }
    }
}
