using System.Windows;
using System.Windows.Controls;
using AdonisUI;
using AdonisUI.Extensions;
using NKristek.Wpf.Converters;

namespace Stein.Views
{
    public abstract class Dialog : Window
    {
        protected Dialog()
        {
            InitializeStyle();

            //Style = TryFindResource(typeof(Dialog)) as Style;
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(Dialog), new FrameworkPropertyMetadata(typeof(Dialog)));
        }
        
        public object DialogButtons
        {
            get => GetValue(DialogButtonsProperty);
            set => SetValue(DialogButtonsProperty, value);
        }

        public static readonly DependencyProperty DialogButtonsProperty = DependencyProperty.Register("DialogButtons", typeof(object), typeof(Dialog), new PropertyMetadata(null));

        private void InitializeStyle()
        {
            var template = new ControlTemplate(typeof(Dialog));

            var grid = new FrameworkElementFactory(typeof(Grid));
            grid.SetResourceReference(Grid.BackgroundProperty, Brushes.Layer0BackgroundBrush);
            var firstRowDefinition = new FrameworkElementFactory(typeof(RowDefinition));
            firstRowDefinition.SetValue(RowDefinition.HeightProperty, new GridLength(1.0, GridUnitType.Star));
            grid.AppendChild(firstRowDefinition);
            var secondRowDefinition = new FrameworkElementFactory(typeof(RowDefinition));
            secondRowDefinition.SetValue(RowDefinition.HeightProperty, GridLength.Auto);
            grid.AppendChild(secondRowDefinition);
            template.VisualTree = grid;

            // Dialog content

            var contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(Grid.RowProperty, 0);
            contentPresenter.SetValue(ContentPresenter.ContentProperty, new TemplateBindingExtension(ContentProperty));
            grid.AppendChild(contentPresenter);

            // Dialog buttons

            var buttonsGrid = new FrameworkElementFactory(typeof(Grid));
            buttonsGrid.SetValue(Grid.RowProperty, 1);
            buttonsGrid.SetResourceReference(Grid.BackgroundProperty, Brushes.Layer1BackgroundBrush);
            buttonsGrid.SetValue(Grid.VisibilityProperty, new TemplateBindingExtension(DialogButtonsProperty)
            {
                Converter = ValueNullToInverseVisibilityConverter.Instance
            });
            buttonsGrid.SetValue(LayerExtension.LayerProperty, 1);
            grid.AppendChild(buttonsGrid);

            var buttonsPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            buttonsPresenter.SetValue(ContentPresenter.ContentProperty, new TemplateBindingExtension(DialogButtonsProperty));
            buttonsGrid.AppendChild(buttonsPresenter);

            var style = new Style(typeof(Dialog), TryFindResource(typeof(Window)) as Style);
            var templateSetter = new Setter(TemplateProperty, template);
            style.Setters.Add(templateSetter);
            Style = style;
        }
    }
}
