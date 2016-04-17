using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using GalaSoft.MvvmLight.Helpers;
using HighFive.Client.Core.Features;
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
            ContentView.BackgroundColor = UIColor.FromRGB(255, 216, 0);

            spinner = new UIActivityIndicatorView();
            spinner.Hidden = true;
            spinner.Frame = new CoreGraphics.CGRect(0, 0, 40, 40);
            spinner.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
            spinner.Color = UIColor.Green;
            bindings.Add(this.SetBinding(
                () => ViewModel.IsLoading).WhenSourceChanges(
                () =>
                {
                    if (ViewModel.IsLoading)
                    {
                        spinner.StartAnimating();
                        spinner.Hidden = false;
                    }
                    else
                    {
                        spinner.StopAnimating();
                        spinner.Hidden = true;
                    }
                }));

            ContentView.AddSubview(spinner);

            View.AddSubview(ContentView);

            OnPrepareUIElements();
        }

        protected abstract T OnPrepareViewModel();

        protected abstract void OnPrepareUIElements();
    }
}
