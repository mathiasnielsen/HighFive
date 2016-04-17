using System;
using System.Collections.Generic;
using System.Text;
using CoreGraphics;
using GalaSoft.MvvmLight.Helpers;
using HighFive.Client.Core.Features;
using UIKit;

namespace HighFive.Client.IOS.Features
{
    public class HighFiveView : ViewBase<HighFiveViewModel>
    {
        private List<Binding> bindings = new List<Binding>();
        private UITapGestureRecognizer tabGesture;

        private int margin = 40;

        private UITextField NameTextField { get; set; }

        private UIButton HighFiveButton { get; set; }

        private UILabel HighFiveMessageLabel { get; set; }

        protected override HighFiveViewModel OnPrepareViewModel()
        {
            return new HighFiveViewModel();
        }

        protected override void OnPrepareUIElements()
        {
            Title = ViewModel.Title;

            PrepareNameTextField();
            PrepareHighFiveButton();
            PrepareHighFiveMessage();

            tabGesture = new UITapGestureRecognizer(ViewTapped);
            View.AddGestureRecognizer(tabGesture);
        }

        private void ViewTapped(UITapGestureRecognizer obj)
        {
            NameTextField.ResignFirstResponder();
        }

        private void PrepareNameTextField()
        {
            NameTextField = new UITextField();

            NameTextField.Frame = new CGRect(margin, 40, View.Bounds.Width, 30);

            bindings.Add(this.SetBinding(
                () => ViewModel.Name,
                () => NameTextField.Text,
                BindingMode.TwoWay)
                .UpdateTargetTrigger(nameof(NameTextField.EditingChanged)));

            ContentView.AddSubview(NameTextField);
        }

        private void PrepareHighFiveMessage()
        {
            HighFiveMessageLabel = new UILabel();

            var doubleMargin = margin * 2;
            HighFiveMessageLabel.Frame = new CGRect(margin, 200, View.Frame.Width - doubleMargin, 100);

            HighFiveMessageLabel.TextAlignment = UITextAlignment.Center;
            HighFiveMessageLabel.Lines = 0;

            bindings.Add(this.SetBinding(
                () => ViewModel.HighFiveMessage,
                () => HighFiveMessageLabel.Text));

            HighFiveMessageLabel.Text = "No slaps yet.";

            ContentView.AddSubview(HighFiveMessageLabel);
        }

        private void PrepareHighFiveButton()
        {
            HighFiveButton = new UIButton(UIButtonType.System);
            HighFiveButton.Frame = new CGRect(margin, 100, View.Frame.Width, 30);
            HighFiveButton.SetTitle("Hit a High Five!", UIControlState.Normal);

            HighFiveButton.SetCommand(nameof(HighFiveButton.TouchUpInside), ViewModel.HighFiveCommand);

            ContentView.AddSubview(HighFiveButton);
        }
    }
}
