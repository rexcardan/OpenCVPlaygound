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
                dlg.DefaultExt = ".png"; // Default file extension 
                //dlg.Filter = "Bitmaps (.bmp)|*.bmp | JPEG Files (*.jpeg) | *.jpeg | PNG Files(*.png) | *.png | JPG Files(*.jpg) | *.jpg | GIF Files(*.gif) | *.gif"; // Filter files by extension 

                // Show open file dialog box 
                Nullable<bool> result = dlg.ShowDialog();

                // Process open file dialog box results 
                if (result == true)
                {
                    // Open document 
                    string filename = dlg.FileName;
                    _ea.GetEvent<OpenImageEvent>().Publish(filename);
                }
            });
        }
    }
}
