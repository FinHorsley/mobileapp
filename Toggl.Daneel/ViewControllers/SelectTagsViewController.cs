﻿using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Toggl.Daneel.Presentation.Attributes;
using Toggl.Foundation.MvvmCross.ViewModels;
using UIKit;
using Toggl.Foundation.MvvmCross.Helper;
using System.Threading.Tasks;
using Toggl.Daneel.Extensions;
using Toggl.Daneel.Extensions.Reactive;
using Toggl.Daneel.Views.Tag;
using Toggl.Daneel.ViewSources;
using Toggl.Multivac.Extensions;
using System.Reactive;
using System.Linq;

namespace Toggl.Daneel.ViewControllers
{
    [ModalCardPresentation]
    public sealed partial class SelectTagsViewController : KeyboardAwareViewController<SelectTagsViewModel>, IDismissableViewController
    {
        private const double headerHeight = 100;

        public SelectTagsViewController()
            : base(nameof(SelectTagsViewController))
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var tableViewSource = new SelectTagsTableViewSource(TagsTableView);

            tableViewSource.Rx().ModelSelected()
                .Subscribe(ViewModel.SelectTag.Inputs)
                .DisposedBy(DisposeBag);

            var tagsReplay = ViewModel.Tags.Replay();

            tagsReplay
                .Subscribe(TagsTableView.Rx().ReloadItems(tableViewSource))
                .DisposedBy(DisposeBag);

            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
            {
                tagsReplay
                    .Select((tags) =>
                    {
                        return new CoreGraphics.CGSize(0, (tags.ToList().Count() * SelectTagsTableViewSource.RowHeight) + headerHeight);
                    })
                    .Subscribe(this.Rx().PreferredContentSize())
                    .DisposedBy(DisposeBag);
            }

            tagsReplay.Connect();

            ViewModel.IsEmpty
                .Subscribe(EmptyStateImage.Rx().IsVisible())
                .DisposedBy(DisposeBag);

            ViewModel.IsEmpty
                .Subscribe(EmptyStateLabel.Rx().IsVisible())
                .DisposedBy(DisposeBag);

            ViewModel.FilterText
                .Subscribe(TextField.Rx().TextObserver())
                .DisposedBy(DisposeBag);

            CloseButton.Rx()
                .BindAction(ViewModel.Close)
                .DisposedBy(DisposeBag);

            SaveButton.Rx()
                .BindAction(ViewModel.Save)
                .DisposedBy(DisposeBag);

            TextField.Rx().Text()
                .Subscribe(ViewModel.FilterText)
                .DisposedBy(DisposeBag);

            BottomConstraint.Active |= UIDevice.CurrentDevice.UserInterfaceIdiom != UIUserInterfaceIdiom.Pad;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            TextField.BecomeFirstResponder();
        }

        public async Task<bool> Dismiss()
        {
            await ViewModel.Close.Execute();
            return true;
        }

        protected override void KeyboardWillShow(object sender, UIKeyboardEventArgs e)
        {
            BottomConstraint.Constant = e.FrameEnd.Height;
            UIView.Animate(Animation.Timings.EnterTiming, () => View.LayoutIfNeeded());
        }

        protected override void KeyboardWillHide(object sender, UIKeyboardEventArgs e)
        {
            BottomConstraint.Constant = 0;
            UIView.Animate(Animation.Timings.EnterTiming, () => View.LayoutIfNeeded());
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            View.ClipsToBounds |= UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            View.ClipsToBounds |= UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;
        }
    }
}
