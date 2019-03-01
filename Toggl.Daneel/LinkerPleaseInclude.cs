﻿using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using Foundation;
using Google.SignIn;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Target;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.IoC;
using MvvmCross.UI;
using MvvmCross.Plugin.Color;
using MvvmCross.Plugin.Color.Platforms.Ios;
using MvvmCross.Plugin.Visibility;
using Newtonsoft.Json.Converters;
using UIKit;
using MvvmCross.ViewModels;
using Toggl.Daneel.Views;

namespace Toggl.Daneel
{
    // This class is never actually executed, but when Xamarin linking is enabled it does ensure types and properties
    // are preserved in the deployed app
    [Preserve(AllMembers = true)]
    public sealed class LinkerPleaseInclude
    {
        public void Include(MvxTaskBasedBindingContext c)
        {
            c.Dispose();
            var c2 = new MvxTaskBasedBindingContext();
            c2.Dispose();
        }

        public void Include(UIButton uiButton)
        {
            uiButton.TouchUpInside += (s, e) =>
                                      uiButton.SetTitle(uiButton.Title(UIControlState.Normal), UIControlState.Normal);
        }

        public void Include(SignInButton button)
        {
            button = new SignInButton();
            button = new SignInButton(null);
        }

        public void Include(UIBarButtonItem barButton)
        {
            barButton.Clicked += (s, e) =>
                                 barButton.Title = barButton.Title + "";
        }

        public void Include(UIView view)
        {
            view.Hidden = true;
        }

        public void Include(UITextField textField)
        {
            textField.Hidden = true;
            textField.TextColor = UIColor.White;
            textField.Text = textField.Text + "";
            textField.EditingChanged += (sender, args) => { textField.Text = ""; };
            textField.EditingDidEnd += (sender, args) => { textField.Text = ""; };
        }

        public void Include(UITextView textView)
        {
            textView.Hidden = true;
            textView.Text = textView.Text + "";
            textView.Changed += (sender, args) => { textView.Text = ""; };
            textView.TextStorage.DidProcessEditing += (sender, e) => textView.Text = "";
        }

        public void Include(UILabel label)
        {
            label.Hidden = true;
            label.Text = label.Text + "";
            label.TextColor = UIColor.White;
            label.AttributedText = new NSAttributedString(label.AttributedText.ToString() + "");
        }

        public void Include(UIImageView imageView)
        {
            imageView.Hidden = true;
            imageView.Image = new UIImage(imageView.Image.CGImage);
        }

        public void Include(UIDatePicker date)
        {
            date.Date = date.Date.AddSeconds(1);
            date.ValueChanged += (sender, args) => { date.Date = NSDate.DistantFuture; };
        }

        public void Include(UISlider slider)
        {
            slider.Value = slider.Value + 1;
            slider.ValueChanged += (sender, args) => { slider.Value = 1; };
        }

        public void Include(UIProgressView progress)
        {
            progress.Progress = progress.Progress + 1;
        }

        public void Include(UISwitch sw)
        {
            sw.On = !sw.On;
            sw.ValueChanged += (sender, args) => { sw.On = false; };
        }

        public void Include(MvxViewController vc)
        {
            vc.Title = vc.Title + "";
        }

        public void Include(UIStepper s)
        {
            s.Value = s.Value + 1;
            s.ValueChanged += (sender, args) => { s.Value = 0; };
        }

        public void Include(UIPageControl s)
        {
            s.Pages = s.Pages + 1;
            s.ValueChanged += (sender, args) => { s.Pages = 0; };
        }

        public void Include(INotifyCollectionChanged changed)
        {
            changed.CollectionChanged += (s, e) => { var test = $"{e.Action}{e.NewItems}{e.NewStartingIndex}{e.OldItems}{e.OldStartingIndex}"; };
        }

        public void Include(ICommand command)
        {
            command.CanExecuteChanged += (s, e) => { if (command.CanExecute(null)) command.Execute(null); };
        }

        public void Include(MvxPropertyInjector injector)
        {
            injector = new MvxPropertyInjector();
        }

        public void Include(MvxUIPageControlCurrentPageTargetBinding binding)
        {
            binding = new MvxUIPageControlCurrentPageTargetBinding(null, null);
        }

        public void Include(MvxUIViewVisibilityTargetBinding binding)
        {
            binding = new MvxUIViewVisibilityTargetBinding(null);
        }

        public void Include(MvxUISwitchOnTargetBinding binding)
        {
            binding = new MvxUISwitchOnTargetBinding(null);
        }

        public void Include(MvxVisibilityValueConverter converter)
        {
            converter.Convert(null, null, null, null);
            converter.ConvertBack(null, null, null, null);
        }

        public void Include(MvxInvertedVisibilityValueConverter converter)
        {
            converter.Convert(null, null, null, null);
            converter.ConvertBack(null, null, null, null);
        }

        public void Include(MvxViewModelViewTypeFinder typeFinder)
        {
            typeFinder = new MvxViewModelViewTypeFinder(null, null);
        }

        public void Include(MvxNativeColorValueConverter converter)
        {
            converter.Convert(null, null, null, null);
            converter.ConvertBack(null, null, null, null);
        }

        public void Include(MvxColor color)
        {
            color.ToNativeColor();
        }

        public void Include(MvxUIDatePickerDateTargetBinding binding)
        {
            binding = new MvxUIDatePickerDateTargetBinding(null, null);
        }

        public void Include(MvxUIDatePickerMinMaxTargetBinding binding)
        {
            binding = new MvxUIDatePickerMinMaxTargetBinding(null, null);
        }

        public void Include(MvxUISliderValueTargetBinding binding)
        {
            binding = new MvxUISliderValueTargetBinding(null, null);
        }

        public void Include(StringEnumConverter converter)
        {
            converter = new StringEnumConverter(true);
        }

        public void Include(ConsoleColor color)
        {
            Console.Write("");
            Console.WriteLine("");
            color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.ForegroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }

        public void Include(TextViewWithPlaceholder textView)
        {
            textView.TextColor = textView.TextColor;
        }
    }
}
