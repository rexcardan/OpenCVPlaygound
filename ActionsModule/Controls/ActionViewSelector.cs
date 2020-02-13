using ActionsModule.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ActionsModule.Attributes;
using System.Reflection;
using System.Windows.Data;
using Xceed.Wpf.Toolkit;

namespace ActionsModule.Controls
{
    public class ActionViewSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var dts = new DataTemplates();
            if (item is ImageAction)
            {
                var ia = item as ImageAction;
                if (ia.IsEditMode)
                {
                    var defaultDt = (dts.FindResource("default") as DataTemplate);
                    FrameworkElementFactory factory = new FrameworkElementFactory(typeof(ContentPresenter));
                    factory.SetValue(ContentPresenter.ContentTemplateProperty, defaultDt);
                    var conv = new BooleanToVisibilityConverter();
                    var mainPanel = new FrameworkElementFactory(typeof(StackPanel));
                    mainPanel.AppendChild(factory);
                    var sp = new FrameworkElementFactory(typeof(StackPanel));
                    sp.SetBinding(StackPanel.VisibilityProperty, new Binding("IsEditMode") { Converter = conv });

                    foreach (var prop in ia.GetType().GetProperties())
                    {
                        object[] attrs = prop.GetCustomAttributes(true);
                        if (attrs == null || attrs.Length == 0)
                            continue;

                        foreach (Attribute attr in attrs)
                        {
                            if (attr is LabelAttribute)
                            {
                                var label = GenerateLabel(prop);
                                sp.AppendChild(GenerateLabel(prop));
                                label.SetBinding(TextBlock.TextProperty, new Binding(prop.Name));
                                sp.AppendChild(label);
                                sp.AppendChild(new FrameworkElementFactory(typeof(Separator)));
                            }
                            
                            if (attr is CheckBoxAttribute)
                            {
                                var checkbox = new FrameworkElementFactory(typeof(CheckBox));
                                checkbox.SetValue(CheckBox.ContentProperty, prop.Name);
                                checkbox.SetValue(CheckBox.MarginProperty, new Thickness(5));
                                checkbox.SetBinding(CheckBox.IsCheckedProperty, new Binding(prop.Name));
                                sp.AppendChild(checkbox);
                                sp.AppendChild(new FrameworkElementFactory(typeof(Separator)));
                            }

                            if (attr is SliderAttribute)
                            {
                                sp.AppendChild(GenerateLabel(prop));
                                var sl = attr as SliderAttribute;
                                var slider = new FrameworkElementFactory(typeof(Slider));
                                slider.SetValue(Slider.MaximumProperty, sl.MaxVal);
                                slider.SetValue(Slider.MinimumProperty, sl.MinVal);
                                if (sl.IsIntegerType)
                                {
                                    slider.SetValue(Slider.IsSnapToTickEnabledProperty, true);
                                    slider.SetValue(Slider.TickFrequencyProperty, sl.Increment);
                                    slider.SetValue(Slider.SmallChangeProperty, sl.Increment);
                                }

                                slider.SetValue(Slider.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                                slider.SetValue(Slider.MarginProperty, new Thickness(5));
                                slider.SetBinding(Slider.ValueProperty, new Binding(prop.Name));

                                var tb = new FrameworkElementFactory(typeof(TextBlock));
                                tb.SetValue(TextBlock.PaddingProperty, new Thickness(5));
                                tb.SetBinding(TextBlock.TextProperty, new Binding(prop.Name) { StringFormat = sl.IsIntegerType ? "N1" : "N2" });
                                sp.AppendChild(tb);
                                sp.AppendChild(slider);
                                sp.AppendChild(new FrameworkElementFactory(typeof(Separator)));
                            }

                            if (attr is EnumAttribute)
                            {
                                sp.AppendChild(GenerateLabel(prop));
                                var en = attr as EnumAttribute;
                                var combo = new FrameworkElementFactory(typeof(ComboBox));

                                combo.SetValue(ComboBox.ItemsSourceProperty, Enum.GetValues(en.EnumClass));

                                combo.SetValue(ComboBox.SelectedItemProperty, new Binding(prop.Name));

                                sp.AppendChild(combo);
                                sp.AppendChild(new FrameworkElementFactory(typeof(Separator)));
                            }

                            if (attr is RGBColorAttribute)
                            {
                                sp.AppendChild(GenerateLabel(prop));
                                var rgb = attr as RGBColorAttribute;
                                var cp = new FrameworkElementFactory(typeof(ColorCanvas));
                                cp.SetBinding(ColorCanvas.SelectedColorProperty, new Binding(prop.Name));
                                cp.SetValue(ColorCanvas.UsingAlphaChannelProperty, false);
                                sp.AppendChild(cp);
                                sp.AppendChild(new FrameworkElementFactory(typeof(Separator)));
                            }
                        }
                    }

                    mainPanel.AppendChild(sp);
                    return new DataTemplate() { VisualTree = mainPanel };
                }
                //Non edit mode
                return dts.FindResource("default") as DataTemplate;
            }
            return null;
        }

        private FrameworkElementFactory GenerateLabel(PropertyInfo prop)
        {
            var label = new FrameworkElementFactory(typeof(TextBlock));
            label.SetValue(TextBlock.TextProperty, prop.Name);
            label.SetValue(TextBlock.PaddingProperty, new Thickness(5));
            return label;
        }
    }
}