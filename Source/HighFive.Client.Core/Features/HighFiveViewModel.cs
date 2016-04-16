using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using HighFive.Client.Core.Http;

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

        private async void ExecuteHighFive()
        {
            var clientFactory = new HttpClientFactory();
            var executor = new HttpRequestExecutor(clientFactory);

            var name = "Ole Kirkegaard";
            var result = await executor.Get<string>(string.Format("{0}/{1}", "http://highfiveapp.azurewebsites.net/api/highfive", name));

            HighFiveMessage = result;
        }
    }
}
