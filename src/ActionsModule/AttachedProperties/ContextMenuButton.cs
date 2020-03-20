using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace ActionsModule.AttachedProperties
{
    /// <summary>
    /// Attached property for buttons with a context menu to open it with left click and disable right click.
    /// </summary>
    public static class ContextMenuButton
    {
        public static bool GetEnable(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableProperty);
        }

        public static void SetEnable(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableProperty, value);
        }

        public static readonly DependencyProperty EnableProperty = DependencyProperty.RegisterAttached(
            "Enable",
            typeof(bool),
            typeof(ContextMenuButton),
            new UIPropertyMetadata(false, OnEnableChanged));

        private static void OnEnableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ButtonBase btn)
            {
                if ((bool)e.NewValue)
                {
                    btn.Click += OnMouseLeftButtonUp;
                    btn.PreviewMouseRightButtonDown += Handled;
                    btn.PreviewMouseRightButtonUp += Handled;
                }
                else
                {
                    btn.Click -= OnMouseLeftButtonUp;
                    btn.PreviewMouseRightButtonDown -= Handled;
                    btn.PreviewMouseRightButtonUp -= Handled;
                }
            }
        }

        private static void Handled(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        private static void OnMouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe)
            {
                if (fe.ContextMenu.DataContext == null)
                {
                    fe.ContextMenu.SetBinding(FrameworkElement.DataContextProperty, new Binding { Source = fe.DataContext });
                }

                fe.ContextMenu.IsOpen = true;
            }
        }

    }
}
