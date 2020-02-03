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
        public static DependencyProperty DragCommandProperty = DependencyProperty.Register("DragCommand", typeof(ICommand), typeof(ScrollViewerEx));

        private bool isDragging = false;
        private Point lastMousePosition;

        public ICommand DragCommand
        {
            get => (ICommand)this.GetValue(DragCommandProperty);
            set => this.SetValue(DragCommandProperty, value);
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
}
