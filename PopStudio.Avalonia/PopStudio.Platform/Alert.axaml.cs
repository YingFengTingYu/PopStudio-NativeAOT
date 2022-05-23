using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PopStudio.Platform
{
    public partial class Alert : Window
    {
        public Alert()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
