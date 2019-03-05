using Android.Support.V7.Widget;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Toggl.Giskard.Adapters
{
    public sealed partial class UniversalAdapter : RecyclerView.Adapter
    {
        private UniversalAdapterConfigurationBase adapterConfiguration;
        private IImmutableList<VisualNode> items = ImmutableArray<VisualNode>.Empty;

        public UniversalAdapter(UniversalAdapterConfigurationBase adapterConfiguration)
        {
            this.adapterConfiguration = adapterConfiguration;
            HasStableIds = false;
        }

        public void UpdateItems(IReadOnlyList<object> items)
        {
            // Normalize
            var sw = Stopwatch.StartNew();
            var normalizedData = adapterConfiguration.Normalize(items);
            sw.Stop();
            System.Diagnostics.Debug.WriteLine($"NORMALIZE :: {sw.ElapsedTicks / (double)Stopwatch.Frequency}");

            // Flatten
            sw = Stopwatch.StartNew();
            var flattenedVisualNodes = flatten(normalizedData);
            sw.Stop();
            System.Diagnostics.Debug.WriteLine($"FLATTEN ({flattenedVisualNodes.Length}):: {sw.ElapsedTicks / (double)Stopwatch.Frequency}");

            // Update
            sw = Stopwatch.StartNew();
            updateItems(flattenedVisualNodes);
            sw.Stop();
            System.Diagnostics.Debug.WriteLine($"UPDATE ({flattenedVisualNodes.Length}):: {sw.ElapsedTicks / (double)Stopwatch.Frequency}");
        }

        public override int ItemCount
            => items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = items[position].UnderlyingItem;
            adapterConfiguration.OnBindViewHolder(item, holder);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.FromContext(parent.Context);
            return adapterConfiguration.OnCreateViewHolder(viewType, parent, inflater);
        }

        public override long GetItemId(int position)
            => base.GetItemId(position);

        public override int GetItemViewType(int position)
        {
            var item = items[position].UnderlyingItem;
            return adapterConfiguration.GetCellTypeForItem(item);
        }
    }

}
