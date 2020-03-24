using OpenCvSharp;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using OpenCvSharp.Extensions;
using MenuModule.Events;

namespace MenuModule.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        private readonly IEventAggregator _ea;

        public DelegateCommand OpenCommand { get; set; }

        public MenuViewModel(IEventAggregator ea)
        {
            _ea = ea;
            OpenCommand = new DelegateCommand(() =>
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Filter = "Images (.bmp;.jpg;.jpeg;.png;.gif)|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                dlg.Multiselect = true;

                if (dlg.ShowDialog() == true
                 && dlg.FileNames.Any())
                {
                    _ea.GetEvent<OpenImageEvent>().Publish(dlg.FileNames);
                }
            });
        }
    }
}
