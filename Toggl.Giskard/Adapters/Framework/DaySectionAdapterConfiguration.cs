using System;
using System.Collections.Immutable;
using System.Linq;
using Toggl.Foundation.MvvmCross.Collections;

namespace Toggl.Giskard.Adapters
{
    using TimeEntryGroup = CollectionSection<NGroup, NTimeEntry>;
    using DaySection = CollectionSection<NSummaryDayVM, CollectionSection<NGroup, NTimeEntry>>;
    using Android.Support.V7.Widget;
    using Android.Views;
    using Android.Widget;
    using Android.Runtime;
    using Toggl.Giskard.ViewHolders;
    using System.Collections.Generic;

    public class DaySectionAdapterConfiguration : UniversalAdapterConfigurationBase
    {
        public override ImmutableList<Node> Normalize(IReadOnlyList<object> data)
            => data.OfType<DaySection>().Select(convert).ToImmutableList();

        private Node convert(DaySection section)
            => new VisualNode(section, section.Items.Select(convert));

        private Node convert(TimeEntryGroup group)
            => new VisualNode(group, group.Items.Select(convert));

        private Node convert(NTimeEntry timeEntry)
            => new VisualNode(timeEntry);

        public override RecyclerView.ViewHolder OnCreateViewHolder(int viewType, ViewGroup parent, LayoutInflater inflater)
        {
            switch (viewType)
            {
                case Resource.Layout.NodeTypeACell:
                    var summaryCell = inflater.Inflate(Resource.Layout.NodeTypeACell, parent, false);
                    return new DescriptionBasedViewHolder<NSummaryDayVM>(summaryCell, summary => summary.DateTime.ToString("yyyy-MM-dd"));

                case Resource.Layout.NodeTypeBCell:
                    var groupCell = inflater.Inflate(Resource.Layout.NodeTypeBCell, parent, false);
                    return new DescriptionBasedViewHolder<NGroup>(groupCell, group => group.Title);

                case Resource.Layout.NodeTypeCCell:
                    var timeEntryCell = inflater.Inflate(Resource.Layout.NodeTypeCCell, parent, false);
                    return new DescriptionBasedViewHolder<NTimeEntry>(timeEntryCell, timeEntry => timeEntry.Description);
            }

            throw new InvalidOperationException("Recycler item type not supported.");
        }

        public override void OnBindViewHolder(object item, RecyclerView.ViewHolder viewHolder)
        {
            switch (item)
            {
                case DaySection summaryItem:
                    ((DescriptionBasedViewHolder<NSummaryDayVM>)viewHolder).Item = summaryItem.Header;
                    return;

                case TimeEntryGroup groupItem:
                    ((DescriptionBasedViewHolder<NGroup>)viewHolder).Item = groupItem.Header;
                    return;

                case NTimeEntry timeEntryItem:
                    ((DescriptionBasedViewHolder<NTimeEntry>)viewHolder).Item = timeEntryItem;
                    return;
            }

            throw new InvalidOperationException("Recycler item type not supported.");
        }

        public override int GetCellTypeForItem(object item)
        {
            if (item is DaySection)
                return Resource.Layout.NodeTypeACell;

            if (item is TimeEntryGroup)
                return Resource.Layout.NodeTypeBCell;

            if (item is NTimeEntry)
                return Resource.Layout.NodeTypeCCell;

            throw new InvalidOperationException("Recycler item type not supported.");
        }

        public override bool AreItemsTheSame(object oldItem, object newItem)
        {
            if (oldItem.GetType() != newItem.GetType())
                return false;

            switch (oldItem)
            {
                case DaySection _:
                    return ((DaySection)oldItem).Header.DateTime == ((DaySection)newItem).Header.DateTime;

                case TimeEntryGroup _:
                    return ((TimeEntryGroup)oldItem).Header.ID == ((TimeEntryGroup)newItem).Header.ID;

                case NTimeEntry _:
                    return ((NTimeEntry)oldItem).ID == ((NTimeEntry)newItem).ID;
            }

            throw new InvalidOperationException("Recycler item type not supported.");
        }

        public override bool AreContentsTheSame(object oldItem, object newItem)
        {
            return AreItemsTheSame(oldItem, newItem);
        }
    }
}
