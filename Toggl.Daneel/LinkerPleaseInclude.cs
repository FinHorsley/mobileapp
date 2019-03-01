using System;
using Foundation;
using MvvmCross.IoC;
using MvvmCross.Plugin.Color;
using MvvmCross.ViewModels;
using Toggl.Daneel.Views;

namespace Toggl.Daneel
{
    // This class is never actually executed, but when Xamarin linking is enabled it does ensure types and properties
    // are preserved in the deployed app
    [Preserve(AllMembers = true)]
    public sealed class LinkerPleaseInclude
    {
        public void Include(MvxPropertyInjector injector)
        {
            injector = new MvxPropertyInjector();
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
