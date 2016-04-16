using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;

namespace HighFive.Client.Core.Features
{
    public class HighFiveViewModel : ViewModelBase
    {
        private string highFiveMessage;

        public string HighFiveMessage
        {
            get { return highFiveMessage; }
            set { Set(ref highFiveMessage, value); }
        }

        public HighFiveViewModel()
        {
            Title = "High five!";

            HighFiveCommand = new RelayCommand(ExecuteHighFive);
        }

        public RelayCommand HighFiveCommand { get; }

        private void ExecuteHighFive()
        {
            HighFiveMessage = $"You hit me! Time: {DateTime.Now.ToString("yyyy MMM dd HH:mm")}";
        }
    }
}
