using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Client.Core.Features
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        private bool isLoading;

        public bool IsLoading
        {
            get { return isLoading; }
            set { Set(ref isLoading, value); }
        }

        public string Title { get; set; }
    }
}
