using System.Windows;

namespace Stein.Views
{
    public abstract class Dialog : Window
    {
        static Dialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Dialog), new FrameworkPropertyMetadata(typeof(Dialog)));
        }
        
        public object DialogButtons
        {
            get => GetValue(DialogButtonsProperty);
            set => SetValue(DialogButtonsProperty, value);
        }

        public static readonly DependencyProperty DialogButtonsProperty = DependencyProperty.Register(nameof(DialogButtons), typeof(object), typeof(Dialog), new PropertyMetadata(null));
    }
}
