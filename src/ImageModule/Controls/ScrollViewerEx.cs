using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImageModule.Controls
{
    public class ScrollViewerEx : ScrollViewer
    {
        public static DependencyProperty ZoomCommandProperty = DependencyProperty.Register("ZoomCommand", typeof(ICommand), typeof(ScrollViewerEx));

        private bool isDragging = false;
        private Point lastMousePosition;

        public ICommand ZoomCommand
        {
            get => (ICommand)this.GetValue(ZoomCommandProperty);
            set => this.SetValue(ZoomCommandProperty, value);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.CaptureMouse();
            this.lastMousePosition = e.GetPosition(this);
            this.isDragging = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            this.isDragging = false;
            this.ReleaseMouseCapture();
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            this.ZoomCommand?.Execute(new ZoomCommandArgs(e.Delta));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!this.isDragging)
                return;

            var pos = e.GetPosition(this);
            var delta = pos - this.lastMousePosition;
            this.lastMousePosition = pos;

            this.ScrollToHorizontalOffset(this.HorizontalOffset - delta.X);
            this.ScrollToVerticalOffset(this.VerticalOffset - delta.Y);
        }
    }

    public class ZoomCommandArgs
    {
        public ZoomCommandArgs(int delta)
        {
            this.Delta = delta;
        }

        public int Delta { get; }
    }
}
