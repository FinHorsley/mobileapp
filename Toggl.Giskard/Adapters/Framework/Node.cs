using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Toggl.Giskard.Adapters
{
    public class Node
    {
        public Node(IEnumerable<Node> items = null)
        {
            Items = items?.ToImmutableList() ?? ImmutableList<Node>.Empty;
        }

        public ImmutableList<Node> Items { get; protected set; }
    }
}
