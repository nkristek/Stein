using System.Windows;
using System.Windows.Controls;

namespace BenRuehl.UiResources.Controls
{
    public class RotatingButton
        : Button
    {
        static RotatingButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RotatingButton), new FrameworkPropertyMetadata(typeof(RotatingButton)));
        }

        public static readonly DependencyProperty IsRotatingProperty = DependencyProperty.Register(nameof(IsRotating), typeof(bool), typeof(RotatingButton), new PropertyMetadata(null));

        public bool IsRotating
        {
            get => (bool)GetValue(IsRotatingProperty);
            set => SetValue(IsRotatingProperty, value);
        }
    }
}
