using ActionsModule.Events;
using OpenCvSharp;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using OpenCvSharp.Extensions;
using MenuModule.Events;
using Prism.Mvvm;
using System.Windows.Input;
using Prism.Commands;
using ImageModule.Controls;
using System.Collections.ObjectModel;
using ActionsModule.Actions;

namespace ImageModule.ViewModels
{
    public class ImageViewModel : BindableBase, IDisposable
    {
        private readonly IEventAggregator _ea;
        private IEnumerable<ImageAction> actions;

        public ImageViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _ea.GetEvent<OperateOnImageEvent>().Subscribe((actions) =>
            {
                this.actions = actions;
                this.Compute();
            });

            _ea.GetEvent<OpenImageEvent>().Subscribe((filename) =>
            {
                var images = CachedImage.FromFiles(filename);
                this.Images.AddRange(images);
                this.SelectedImage = images.First();
            });

            SetUnityZoomCommand = new DelegateCommand(() => this.ZoomLevel = 1.0);
            ZoomCommand = new DelegateCommand<ZoomCommandArgs>(args => this.ZoomLevel = Math.Max(0.1, Math.Min(this.ZoomLevel + args.Delta / 1200.0, 3.0)));
        }

        public Mat OperatedImage { get; set; }

        public ObservableCollection<CachedImage> Images { get; } = new ObservableCollection<CachedImage>();
        
        private CachedImage selectedImage = null;
        public CachedImage SelectedImage
        {
            get { return this.selectedImage; }
            set
            {
                SetProperty(ref this.selectedImage, value);
                this.OnSelectedImageChanged(value);
            }
        }

        private BitmapSource image = null;
        public BitmapSource Image
        {
            get { return image; }
            set { SetProperty(ref image, value); }
        }

        private double zoomLevel = 1.0;
        public double ZoomLevel
        {
            get { return zoomLevel; }
            set { SetProperty(ref zoomLevel, value); }
        }

        public ICommand SetUnityZoomCommand { get; }
        
        public ICommand ZoomCommand { get; }

        private void OnSelectedImageChanged(CachedImage value)
        {
            this.Compute();
        }

        private void Compute()
        {
            if (this.SelectedImage == null)
                return;

            if (this.actions == null)
            {
                Image = this.SelectedImage.GetCopy().ToBitmapSource();
                return;
            }

            OperatedImage = this.SelectedImage.GetCopy();
            foreach (var act in this.actions)
            {
                try
                {
                    OperatedImage = act.Action(OperatedImage);

                    act.ErrorMessage = string.Empty;
                    act.HasError = false;
                }
                catch (Exception ex)
                {
                    act.ErrorMessage = $"{ex.GetType().Name}: {ex.Message}";
                    act.HasError = true;
                }

                act.HasChanged = false;
            }
            Image = OperatedImage.ToBitmapSource();
        }

        public void Dispose()
        {
            // Todo: Dispose is not called. Check if DI container is disposed correctly.
            foreach (var cachedImage in this.Images)
                cachedImage.Dispose();
        }
    }
}
