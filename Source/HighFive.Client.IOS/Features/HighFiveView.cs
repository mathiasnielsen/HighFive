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

        private UIView FlashView;

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

            PrepareFlashView();
            PrepareNameTextField();
            PrepareHighFiveButton();
            PrepareHighFiveMessage();

            tabGesture = new UITapGestureRecognizer(ViewTapped);
            View.AddGestureRecognizer(tabGesture);
        }

        private void PrepareFlashView()
        {
            FlashView = new UIView();
            FlashView.Frame = ContentView.Frame;
            FlashView.BackgroundColor = UIColor.White;
            FlashView.UserInteractionEnabled = false;
            FlashView.Hidden = true;

            ContentView.AddSubview(FlashView);
        }

        private void ViewTapped(UITapGestureRecognizer obj)
        {
            NameTextField.ResignFirstResponder();
        }

        private void PrepareNameTextField()
        {
            NameTextField = new UITextField();
            NameTextField.BackgroundColor = UIColor.White;

            NameTextField.Frame = new CGRect(margin, 40, View.Bounds.Width - margin * 2, 30);

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
            HighFiveMessageLabel.Frame = new CGRect(margin, 240, View.Frame.Width - doubleMargin, 100);

            HighFiveMessageLabel.TextAlignment = UITextAlignment.Center;
            HighFiveMessageLabel.Lines = 0;

            bindings.Add(this.SetBinding(
                () => ViewModel.HighFiveMessage,
                () => HighFiveMessageLabel.Text));

            HighFiveMessageLabel.Text = "No slaps yet.";
            NameTextField.ResignFirstResponder();

            ContentView.AddSubview(HighFiveMessageLabel);
        }

        private void PrepareHighFiveButton()
        {
            HighFiveButton = new UIButton();
            HighFiveButton.SetImage(UIImage.FromFile(string.Format("Assets/highfive_hand")), UIControlState.Normal);
            HighFiveButton.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

            HighFiveButton.Frame = new CGRect(0, 100, ContentView.Frame.Width, 150);

            ////HighFiveButton.SetCommand(nameof(HighFiveButton.TouchUpInside), ViewModel.HighFiveCommand);
            HighFiveButton.TouchUpInside += HighFiveButton_TouchUpInside;

            ContentView.AddSubview(HighFiveButton);
        }

        private void HighFiveButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.HighFiveCommand.CanExecute("Not empty string"))
            {

                FlashView.Hidden = false;
                FlashView.Alpha = 1;
                ContentView.BringSubviewToFront(FlashView);
                UIView.Animate(
                    0.4,
                   () =>
                   {
                       FlashView.Alpha = 0;
                   },
                   () =>
                   {
                       FlashView.Hidden = true;
                   });

                ViewModel.HighFiveCommand.Execute(null);
            }
        }
    }
}
