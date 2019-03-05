using Android.Support.V7.Widget;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Toggl.Giskard.Adapters
{
    public sealed partial class UniversalAdapter : RecyclerView.Adapter
    {
        private ImmutableArray<VisualNode> flatten(IReadOnlyList<Node> nodes)
        {
            var builder = ImmutableArray.CreateBuilder<VisualNode>();

            foreach (var rootNode in nodes)
                flattenRecursively(rootNode, builder);

            return builder.ToImmutable();
        }

        private void flattenRecursively(Node node, ImmutableArray<VisualNode>.Builder builder)
        {
            if (node is VisualNode visualNode)
                builder.Add(visualNode);

            foreach (var childNode in node.Items)
                flattenRecursively(childNode, builder);
        }
    }
}
