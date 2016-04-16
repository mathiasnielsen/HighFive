using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using HighFive.Client.Core.Features;
using UIKit;

namespace HighFive.Client.IOS.Features
{
    [Register("ViewBase")]
    public abstract class ViewBase<T> : UIViewController
        where T : ViewModelBase
    {
        private T viewModel;

        public ViewBase()
        {
            View.BackgroundColor = UIColor.White;

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
            ContentView.BackgroundColor = UIColor.White;

            View.AddSubview(ContentView);

            OnPrepareUIElements();
        }

        protected abstract T OnPrepareViewModel();

        protected abstract void OnPrepareUIElements();
    }
}
