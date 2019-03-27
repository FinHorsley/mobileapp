﻿using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using MvvmCross.Plugin.Color.Platforms.Ios;
using Toggl.Daneel.Extensions;
using Toggl.Daneel.Extensions.Reactive;
using Toggl.Daneel.Presentation.Attributes;
using Toggl.Foundation;
using Toggl.Foundation.MvvmCross.ViewModels;
using Toggl.Multivac.Extensions;
using UIKit;

namespace Toggl.Daneel.ViewControllers
{
    [ModalCardPresentation]
    public sealed partial class EditProjectViewController : ReactiveViewController<EditProjectViewModel>, IDismissableViewController
    {
        private const double desiredIpadHeight = 360;
        private static readonly nfloat nameAlreadyTakenHeight = 16;

        public EditProjectViewController()
            : base(nameof(EditProjectViewController))
        {
        }

        public Task<bool> Dismiss()
        {
            ViewModel.Close.Execute();
            return Task.FromResult(true);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
            {
                PreferredContentSize = new CGSize(0, desiredIpadHeight);
            }

            TitleLabel.Text = Resources.NewProject;
            NameTextField.Placeholder = Resources.ProjectName;
            NameTakenErrorLabel.Text = Resources.NameTakenError;
            DoneButton.SetTitle(Resources.Create, UIControlState.Normal);

            // Name
            NameTextField.Rx().Text()
                .Subscribe(ViewModel.Name.Accept)
                .DisposedBy(DisposeBag);

            ViewModel.Name
                .Subscribe(NameTextField.Rx().TextObserver())
                .DisposedBy(DisposeBag);

            // Color
            ColorPickerOpeningView.Rx()
                .BindAction(ViewModel.PickColor)
                .DisposedBy(DisposeBag);

            ViewModel.Color
                .Select(color => color.ToNativeColor())
                .Subscribe(ColorCircleView.Rx().BackgroundColor())
                .DisposedBy(DisposeBag);

            // Error
            ViewModel.NameIsAlreadyTaken
                .Select(nameIsTaken => nameIsTaken ? nameAlreadyTakenHeight : 0)
                .Subscribe(ProjectNameUsedErrorTextHeight.Rx().Constant())
                .DisposedBy(DisposeBag);

            // Workspace
            WorkspaceLabel.Rx()
                .BindAction(ViewModel.PickWorkspace)
                .DisposedBy(DisposeBag);

            ViewModel.WorkspaceName
                .Subscribe(WorkspaceLabel.Rx().Text())
                .DisposedBy(DisposeBag);

            // Client
            ClientLabel.Rx()
                .BindAction(ViewModel.PickClient)
                .DisposedBy(DisposeBag);

            var emptyText = Resources.AddClient.PrependWithAddIcon(ClientLabel.Font.CapHeight);
            ViewModel.ClientName
                .Select(attributedClientName)
                .Subscribe(ClientLabel.Rx().AttributedText())
                .DisposedBy(DisposeBag);

            // Is Private
            PrivateProjectSwitchContainer.Rx().Tap()
                .Select(_ => PrivateProjectSwitch.On)
                .Subscribe(ViewModel.IsPrivate.Accept)
                .DisposedBy(DisposeBag);

            ViewModel.IsPrivate
                .Subscribe(PrivateProjectSwitch.Rx().On())
                .DisposedBy(DisposeBag);

            // Save
            DoneButton.Rx()
                .BindAction(ViewModel.Save)
                .DisposedBy(DisposeBag);

            CloseButton.Rx()
                .BindAction(ViewModel.Close)
                .DisposedBy(DisposeBag);

            NSAttributedString attributedClientName(string clientName)
            {
                if (string.IsNullOrEmpty(clientName))
                    return emptyText;

                return new NSAttributedString(clientName);
            }
        }
    }
}

