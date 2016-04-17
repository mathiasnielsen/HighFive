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

        private string name;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                Set(ref name, value);
                HighFiveCommand.RaiseCanExecuteChanged();
            }
        }

        public HighFiveViewModel()
        {
            Title = "High five!";

            HighFiveCommand = new RelayCommand(ExecuteHighFive, CanHighFiveExecute);

#if DEBUG
            name = "Ole Kirkegaard";
#endif
        }

        public RelayCommand HighFiveCommand { get; }

        private async void ExecuteHighFive()
        {
            IsLoading = true;
            var clientFactory = new HttpClientFactory();
            var executor = new HttpRequestExecutor(clientFactory);

            var result = await executor.Get<string>(string.Format("{0}/{1}", "http://highfiveapp.azurewebsites.net/api/highfive", Name));

            HighFiveMessage = result;

            IsLoading = false;
        }

        private bool CanHighFiveExecute()
        {
            return IsLoading == false && string.IsNullOrWhiteSpace(Name) == false;
        }
    }
}
