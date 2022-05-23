using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace PopStudio.Platform
{
    public partial class ActionSheet : Window
    {
        public ActionSheet()
        {
            InitializeComponent();
            LoadControl();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        void LoadControl()
        {
            list = this.Get<ListBox>("list");
            ok = this.Get<Button>("ok");
            cancel = this.Get<Button>("cancel");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public string result;
        string[] items;
        string cancelstring;
        string okstring;

        public ActionSheet(string title, string cancel, string ok, params string[] items)
        {
            InitializeComponent();
            LoadControl();
#if DEBUG
            this.AttachDevTools();
#endif
            int l = items.Length;
            Title = title;
            this.items = items;
            okstring = ok;
            cancelstring = cancel;
            this.ok.Content = ok.Replace("\0", string.Empty);
            this.cancel.Content = cancel.Replace("\0", string.Empty);
            List<SingleItem> items2 = new List<SingleItem>();
            for (int i = 0; i < l; i++)
            {
                items2.Add(new SingleItem(items[i]));
            }
            list.Items = items2;
            list.SelectedIndex = 0;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if (list.SelectedIndex < 0 || items.Length <= 0)
            {
                result = okstring;
            }
            else
            {
                result = items[list.SelectedIndex];
            }
            Close();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            result = cancelstring;
            Close();
        }

        private class SingleItem
        {
            public string ItemName { get; set; }

            public SingleItem()
            {

            }

            public SingleItem(string ItemName)
            {
                this.ItemName = ItemName;
            }
        }
    }
}
