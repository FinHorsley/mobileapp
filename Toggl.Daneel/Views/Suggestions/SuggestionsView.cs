﻿using System.Collections.Generic;
using System.Reactive.Subjects;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Plugin.Color.Platforms.Ios;
using Toggl.Foundation;
using Toggl.Foundation.MvvmCross.Converters;
using Toggl.Foundation.MvvmCross.Helper;
using Toggl.Foundation.MvvmCross.ViewModels;
using Toggl.Foundation.Suggestions;
using Toggl.Multivac.Extensions;
using UIKit;
using static Toggl.Daneel.Extensions.ViewBindingExtensions;

namespace Toggl.Daneel.Suggestions
{
    public sealed class SuggestionsView: UIView
    {
        private const float titleSize = 12;
        private const float sideMargin = 16;
        private const float suggestionHeightCompact = 64;
        private const float suggestionHeightRegular = 48;
        private const float distanceAboveTitleLabel = 20;
        private const float distanceBelowTitleLabel = 16;
        private const float distanceBetweenSuggestions = 12;

        private readonly UILabel titleLabel = new UILabel();

        private NSLayoutConstraint heightConstraint;
        private float suggestionHeight;

        public ISubject<Suggestion> SuggestionTapped { get; } = new Subject<Suggestion>();

        public SuggestionsView()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
            BackgroundColor = UIColor.White;
            ClipsToBounds = true;

            heightConstraint = HeightAnchor.ConstraintEqualTo(1);
        }

        public override void MovedToSuperview()
        {
            base.MovedToSuperview();

            TopAnchor.ConstraintEqualTo(Superview.TopAnchor).Active = true;
            WidthAnchor.ConstraintEqualTo(Superview.WidthAnchor).Active = true;
            CenterXAnchor.ConstraintEqualTo(Superview.CenterXAnchor).Active = true;
            //Actual value is set with bindings a few lines below
            heightConstraint.Active = true;

            prepareTitleLabel();

            LayoutIfNeeded();
        }

        public void OnSuggestions(Suggestion[] suggestions)
        {
            foreach (UIView view in Subviews)
            {
                if (view is SuggestionView)
                {
                    view.RemoveFromSuperview();
                }
            }

            suggestionHeight = TraitCollection.VerticalSizeClass == UIUserInterfaceSizeClass.Regular
                               && TraitCollection.HorizontalSizeClass == UIUserInterfaceSizeClass.Regular
                               ? suggestionHeightRegular
                               : suggestionHeightCompact;

            for (int i = 0; i < suggestions.Length; i++)
            {
                var suggestionView = SuggestionView.Create();
                suggestionView.Suggestion = suggestions[i];
                AddSubview(suggestionView);
                suggestionView.TranslatesAutoresizingMaskIntoConstraints = false;
                suggestionView.HeightAnchor.ConstraintEqualTo(suggestionHeight).Active = true;
                suggestionView.CenterXAnchor.ConstraintEqualTo(Superview.CenterXAnchor).Active = true;
                suggestionView.WidthAnchor.ConstraintEqualTo(Superview.WidthAnchor, 1, -2 * sideMargin).Active = true;
                suggestionView.TopAnchor.ConstraintEqualTo(titleLabel.BottomAnchor, distanceFromTitleLabel(i)).Active = true;

                suggestionView.AddGestureRecognizer(new UITapGestureRecognizer(() =>
                {
                    SuggestionTapped.OnNext(suggestionView.Suggestion);
                }));
            }
            heightConstraint.Constant = heightForSuggestionCount(suggestions.Length);
            heightConstraint.Active = true;
            SetNeedsLayout();
        }

        private void prepareTitleLabel()
        {
            AddSubview(titleLabel);
            titleLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            titleLabel.Text = Resources.SuggestionsHeader;
            titleLabel.Font = UIFont.SystemFontOfSize(titleSize, UIFontWeight.Medium);
            titleLabel.TextColor = Color.Main.SuggestionsTitle.ToNativeColor();
            titleLabel.TopAnchor.ConstraintEqualTo(Superview.TopAnchor, distanceAboveTitleLabel).Active = true;
            titleLabel.LeadingAnchor.ConstraintEqualTo(Superview.LeadingAnchor, sideMargin).Active = true;
        }

        private float distanceFromTitleLabel(int index)
            => distanceBelowTitleLabel
               + index * distanceBetweenSuggestions
               + index * suggestionHeight;

        private float heightForSuggestionCount(int count)
        {
            if (count == 0)
            {
                return 0;
            }
            return count * (suggestionHeight + distanceBetweenSuggestions) + distanceAboveTitleLabel
                                                                           + distanceBelowTitleLabel
                                                                           + (float) titleLabel.Frame.Height;
        }
    }
}
