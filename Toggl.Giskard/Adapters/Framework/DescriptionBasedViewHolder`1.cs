using System;
using System.Linq;
using Toggl.Foundation.MvvmCross.Collections;

namespace Toggl.Giskard.Adapters
{
    using Android.Runtime;
    using Android.Views;
    using Android.Widget;
    using Toggl.Giskard.ViewHolders;
    public sealed class DescriptionBasedViewHolder<T> : BaseRecyclerViewHolder<T>
    {
        private TextView textView;
        private Func<T, string> transformFunction;

        public DescriptionBasedViewHolder(View itemView, Func<T, string> transformFunction)
            : base(itemView)
        {
            this.transformFunction = transformFunction;
        }

        public DescriptionBasedViewHolder(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override void InitializeViews()
        {
            textView = ItemView.FindViewById<TextView>(Resource.Id.Description);
        }

        protected override void UpdateView()
        {
            textView.Text = transformFunction(Item);
        }
    }
}
