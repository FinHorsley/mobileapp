using Android.Support.V7.Widget;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Toggl.Giskard.Adapters
{
    public abstract class UniversalAdapterConfigurationBase
    {
        public abstract ImmutableList<Node> Normalize(IReadOnlyList<object> data);
        public abstract bool AreItemsTheSame(object oldItem, object newItem);
        public abstract bool AreContentsTheSame(object oldItem, object newItem);
        public abstract RecyclerView.ViewHolder OnCreateViewHolder(int type, ViewGroup parent, LayoutInflater inflater);
        public abstract void OnBindViewHolder(object item, RecyclerView.ViewHolder viewHolder);
        public abstract int GetCellTypeForItem(object item);
    }
}
