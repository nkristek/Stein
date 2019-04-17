using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using AdonisUI;
using AdonisUI.Extensions;
using NKristek.Wpf.Converters;

namespace Stein.Views
{
    [ContentProperty("DialogContent")]
    public abstract class Dialog : Window
    {
        protected Dialog()
        {
            SetDialogDefaults();
            BuildView();

            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BindingOperations.ClearBinding(this, TitleProperty);
            if (DataContext == null)
                return;

            var titleBinding = new Binding
            {
                Source = DataContext,
                Path = new PropertyPath(nameof(Title)),
                Mode = BindingMode.OneWay
            };
            SetBinding(TitleProperty, titleBinding);
        }

        private void SetDialogDefaults()
        {
            SizeToContent = SizeToContent.WidthAndHeight;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            Style = new Style(typeof(Window), (Style)FindResource(typeof(Window)));
        }

        private void BuildView()
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1.0, GridUnitType.Star)
            });
            grid.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });
            Content = grid;

            // Dialog content

            var contentPresenter = new ContentPresenter();
            contentPresenter.SetValue(Grid.RowProperty, 0);
            var contentBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(DialogContent)),
                Mode = BindingMode.OneWay
            };
            contentPresenter.SetBinding(ContentPresenter.ContentProperty, contentBinding);
            grid.Children.Add(contentPresenter);

            // Dialog buttons

            var buttonsGrid = new Grid();
            buttonsGrid.SetValue(Grid.RowProperty, 1);
            buttonsGrid.SetResourceReference(Grid.BackgroundProperty, Brushes.Layer1BackgroundBrush);
            var buttonsGridVisibilityBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(DialogButtons)),
                Mode = BindingMode.OneWay,
                Converter = ValueNullToInverseVisibilityConverter.Instance
            };
            buttonsGrid.SetBinding(VisibilityProperty, buttonsGridVisibilityBinding);
            grid.Children.Add(buttonsGrid);

            var buttonsPresenter = new ContentPresenter();
            var buttonsBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(DialogButtons)),
                Mode = BindingMode.OneWay
            };
            buttonsPresenter.SetBinding(ContentPresenter.ContentProperty, buttonsBinding);
            buttonsPresenter.SetValue(LayerExtension.LayerProperty, 1);
            buttonsGrid.Children.Add(buttonsPresenter);
        }

        public object DialogContent
        {
            get => GetValue(DialogContentProperty);
            set => SetValue(DialogContentProperty, value);
        }

        public static readonly DependencyProperty DialogContentProperty = DependencyProperty.Register("DialogContent", typeof(object), typeof(Dialog), new PropertyMetadata(null));

        public object DialogButtons
        {
            get => GetValue(DialogButtonsProperty);
            set => SetValue(DialogButtonsProperty, value);
        }

        public static readonly DependencyProperty DialogButtonsProperty = DependencyProperty.Register("DialogButtons", typeof(object), typeof(Dialog), new PropertyMetadata(null));
    }
}
