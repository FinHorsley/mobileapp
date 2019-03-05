using Android.Support.V7.Util;
using Android.Support.V7.Widget;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading.Tasks;
using Handler = Android.OS.Handler;

namespace Toggl.Giskard.Adapters
{
    public sealed partial class UniversalAdapter
    {
        private readonly object updateLock = new object();

        private bool isUpdateRunning;
        private IImmutableList<VisualNode> nextUpdate;

        private void updateItems(IImmutableList<VisualNode> newItems)
        {
            lock (updateLock)
            {
                if (!isUpdateRunning)
                {
                    isUpdateRunning = true;
                    processUpdate(newItems);
                }
                else
                {
                    nextUpdate = newItems;
                }
            }
        }

        private void processUpdate(IImmutableList<VisualNode> newItems)
        {
            var oldItems = items;
            var handler = new Handler();
            Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                var diffResult = DiffUtil.CalculateDiff(new BaseDiffCallBack(oldItems, newItems, adapterConfiguration));
                sw.Stop();
                System.Diagnostics.Debug.WriteLine($"DIFF :: {sw.ElapsedTicks / (double)Stopwatch.Frequency}");

                handler.Post(() =>
                {
                    dispatchUpdates(newItems, diffResult);
                });
            });
        }

        private void dispatchUpdates(IImmutableList<VisualNode> newItems, DiffUtil.DiffResult diffResult)
        {
            items = newItems;
            diffResult.DispatchUpdatesTo(this);
            lock (updateLock)
            {
                if (nextUpdate != null)
                {
                    processUpdate(nextUpdate);
                    nextUpdate = null;
                }
                else
                {
                    isUpdateRunning = false;
                }
            }
        }

        private sealed class BaseDiffCallBack : DiffUtil.Callback
        {
            private IImmutableList<VisualNode> oldNodes;
            private IImmutableList<VisualNode> newNodes;
            private UniversalAdapterConfigurationBase adapterConfiguration;

            public BaseDiffCallBack(
                IImmutableList<VisualNode> oldNodes,
                IImmutableList<VisualNode> newNodes,
                UniversalAdapterConfigurationBase adapterConfiguration)
            {
                this.oldNodes = oldNodes;
                this.newNodes = newNodes;
                this.adapterConfiguration = adapterConfiguration;
            }

            public override bool AreContentsTheSame(int oldItemPosition, int newItemPosition)
            {
                var oldItem = oldNodes[oldItemPosition].UnderlyingItem;
                var newItem = newNodes[newItemPosition].UnderlyingItem;

                return adapterConfiguration.AreContentsTheSame(oldItem, newItem);
            }

            public override bool AreItemsTheSame(int oldItemPosition, int newItemPosition)
            {
                var oldItem = oldNodes[oldItemPosition].UnderlyingItem;
                var newItem = newNodes[newItemPosition].UnderlyingItem;

                return adapterConfiguration.AreItemsTheSame(oldItem, newItem);
            }

            public override int NewListSize => newNodes.Count;
            public override int OldListSize => oldNodes.Count;
        }
    }
}
