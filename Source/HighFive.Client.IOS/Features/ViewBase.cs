using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using GalaSoft.MvvmLight.Helpers;
using HighFive.Client.Core.Features;
using Praeclarum.UI;
using UIKit;

namespace HighFive.Client.IOS.Features
{
    [Register("ViewBase")]
    public abstract class ViewBase<T> : UIViewController
        where T : ViewModelBase
    {
        private List<Binding> bindings = new List<Binding>();
        private UIActivityIndicatorView spinner;
        private T viewModel;

        public ViewBase()
        {
            View.BackgroundColor = UIColor.FromRGB(255, 216, 0);

            AutomaticallyAdjustsScrollViewInsets = false;
            EdgesForExtendedLayout = UIRectEdge.None;
        }

        public UIView ContentView { get; set; }

        public T ViewModel
        {
            get { return viewModel; }
            set { viewModel = value; }
        }

        public override void ViewDidLoad()
        {
            ViewModel = OnPrepareViewModel();
            base.ViewDidLoad();

            PrepareUIElements();
        }

        private void PrepareUIElements()
        {
            ContentView = new UIView();
            ContentView.Frame = new CoreGraphics.CGRect(View.Frame.Location, View.Frame.Size);
            View.AddSubview(ContentView);

            var margin = 20;
            View.ConstrainLayout(() =>
                ContentView.Frame.Top == View.Frame.Top + margin
                && ContentView.Frame.Left == View.Frame.Left + margin
                && ContentView.Frame.Right == View.Frame.Right - margin
                && ContentView.Frame.Bottom == View.Frame.Bottom - margin);

            OnPrepareUIElements();
        }

        protected abstract T OnPrepareViewModel();

        protected abstract void OnPrepareUIElements();
    }
}
