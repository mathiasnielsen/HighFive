using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Client.Core.Features
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        public string Title { get; set; }
    }
}
