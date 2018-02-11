using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BenRuehl.UiResources.Controls
{
    [TemplatePart(Name = "PART_MenuExpander", Type = typeof(Button))]
    public class SplitButton : Button
    {
        static SplitButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButton), new FrameworkPropertyMetadata(typeof(SplitButton)));
        }

        public static readonly DependencyProperty SplitMenuProperty = DependencyProperty.Register("SplitMenu", typeof(ContextMenu), typeof(SplitButton), new PropertyMetadata(null));

        public ContextMenu SplitMenu
        {
            get => (ContextMenu)GetValue(SplitMenuProperty);
            set => SetValue(SplitMenuProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_MenuExpander") is Button menuExpanderButton)
                menuExpanderButton.PreviewMouseDown += OnMenuExpanderClick;
        }

        private void OnMenuExpanderClick(object sender, RoutedEventArgs e)
        {
            OpenSplitMenu();
        }

        private void OpenSplitMenu()
        {
            if (SplitMenu == null)
                return;

            SplitMenu.IsEnabled = true;
            SplitMenu.PlacementTarget = this;
            SplitMenu.Placement = PlacementMode.Bottom;
            SplitMenu.IsOpen = true;
        }
    }
}
