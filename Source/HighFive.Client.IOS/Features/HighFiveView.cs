using System;
using System.Collections.Generic;
using System.Text;
using CoreGraphics;
using GalaSoft.MvvmLight.Helpers;
using HighFive.Client.Core.Features;
using Praeclarum.UI;
using UIKit;

namespace HighFive.Client.IOS.Features
{
    public class HighFiveView : ViewBase<HighFiveViewModel>
    {
        private List<Binding> bindings = new List<Binding>();
        private UITapGestureRecognizer tabGesture;
        private UIView FlashView;
        private UIActivityIndicatorView spinner;

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
            PrepareSpinner();

            tabGesture = new UITapGestureRecognizer(ViewTapped);
            View.AddGestureRecognizer(tabGesture);

            SetupLayoutConstraints();
        }

        private void PrepareSpinner()
        {
            spinner = new UIActivityIndicatorView();
            spinner.Hidden = true;
            spinner.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
            spinner.Color = UIColor.Black;

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
        }

        private void PrepareFlashView()
        {
            FlashView = new UIView();
            FlashView.Frame = View.Frame;
            FlashView.BackgroundColor = UIColor.White;
            FlashView.UserInteractionEnabled = false;
            FlashView.Hidden = true;

            View.AddSubview(FlashView);
        }

        private void ViewTapped(UITapGestureRecognizer obj)
        {
            NameTextField.ResignFirstResponder();
        }

        private void PrepareNameTextField()
        {
            NameTextField = new UITextField();
            NameTextField.LeftView = new UIView(new CGRect(20, 20, 20, 20));
            NameTextField.LeftViewMode = UITextFieldViewMode.Always;
            NameTextField.BackgroundColor = UIColor.White;

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
            HighFiveButton = new UIButton();
            HighFiveButton.SetImage(UIImage.FromFile(string.Format("Assets/highfive_hand")), UIControlState.Normal);
            HighFiveButton.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

            HighFiveButton.TouchUpInside += HighFiveButton_TouchUpInside;

            ContentView.AddSubview(HighFiveButton);
        }

        private void HighFiveButton_TouchUpInside(object sender, EventArgs e)
        {
            NameTextField.ResignFirstResponder();
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

        private void SetupLayoutConstraints()
        {
            ContentView.ConstrainLayout(() =>

                NameTextField.Frame.Top == ContentView.Frame.Top
                && NameTextField.Frame.Left == ContentView.Frame.Left
                && NameTextField.Frame.Right == ContentView.Frame.Right
                && NameTextField.Frame.Height == 40

                && HighFiveButton.Frame.GetCenterY() == ContentView.Frame.GetCenterY()
                && HighFiveButton.Frame.Left == ContentView.Frame.Left
                && HighFiveButton.Frame.Right == ContentView.Frame.Right
                && HighFiveButton.Frame.Height == 300
                
                && spinner.Frame.GetCenterY() == ContentView.Frame.GetCenterY() + 60
                && spinner.Frame.GetCenterX() == ContentView.Frame.GetCenterX()
                && spinner.Frame.Height == 40
                && spinner.Frame.Width == 40

                && HighFiveMessageLabel.Frame.Bottom == ContentView.Frame.Bottom
                && HighFiveMessageLabel.Frame.Left == ContentView.Frame.Left
                && HighFiveMessageLabel.Frame.Right == ContentView.Frame.Right);
        }
    }
}
