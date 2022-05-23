using Avalonia.Controls;
using PopStudio.Avalonia;

namespace PopStudio.Platform
{
    internal static class PopupDialog
    {
        public static Task<string> DisplayActionSheet(this UserControl page, string title, string cancel, string ok, params string[] items)
        {
            var actionbox = new ActionSheet(title, cancel, ok, items);
            var tcs = new TaskCompletionSource<string>();
            actionbox.Closed += (_, _) => tcs.SetResult(actionbox.result);
            actionbox.ShowDialog(MainWindow.Singleten);
            return tcs.Task;
        }

        public static Task DisplayAlert(this UserControl page, string title, string message, string cancel)
        {
            var msgbox = new Alert()
            {
                Title = title
            };
            msgbox.FindControl<TextBlock>("Text").Text = message;
            var buttonPanel = msgbox.FindControl<StackPanel>("Buttons");
            var btn = new Button { Content = cancel.Replace("\0", string.Empty) };
            btn.Click += (_, _) => msgbox.Close();
            buttonPanel.Children.Add(btn);
            var tcs = new TaskCompletionSource();
            msgbox.Closed += (_, _) => tcs.SetResult();
            msgbox.ShowDialog(MainWindow.Singleten);
            return tcs.Task;
        }

        public static Task<bool> DisplayAlert(this UserControl page, string title, string message, string accept, string cancel)
        {
            var msgbox = new Alert()
            {
                Title = title
            };
            msgbox.FindControl<TextBlock>("Text").Text = message;
            bool ans = false;
            var buttonPanel = msgbox.FindControl<StackPanel>("Buttons");
            var btn_accept = new Button { Content = accept.Replace("\0", string.Empty) };
            btn_accept.Click += (_, _) =>
            {
                ans = true;
                msgbox.Close();
            };
            buttonPanel.Children.Add(btn_accept);
            var btn_close = new Button { Content = cancel.Replace("\0", string.Empty) };
            btn_close.Click += (_, _) =>
            {
                ans = false;
                msgbox.Close();
            };
            buttonPanel.Children.Add(btn_close);
            var tcs = new TaskCompletionSource<bool>();
            msgbox.Closed += (_, _) => tcs.SetResult(ans);
            msgbox.ShowDialog(MainWindow.Singleten);
            return tcs.Task;
        }

        public static Task<string> DisplayPromptAsync(this UserControl page, string title, string message, string accept = "OK", string cancel = "Cancel", string initialValue = "")
        {
            var msgbox = new Alert()
            {
                Title = title
            };
            msgbox.FindControl<TextBlock>("Text").Text = message;
            var input = msgbox.FindControl<TextBox>("Input");
            input.IsVisible = true;
            input.Text = initialValue;
            string ans = null;
            var buttonPanel = msgbox.FindControl<StackPanel>("Buttons");
            var btn_accept = new Button { Content = accept.Replace("\0", string.Empty) };
            btn_accept.Click += (_, _) =>
            {
                ans = input.Text;
                msgbox.Close();
            };
            buttonPanel.Children.Add(btn_accept);
            var btn_close = new Button { Content = cancel.Replace("\0", string.Empty) };
            btn_close.Click += (_, _) =>
            {
                ans = null;
                msgbox.Close();
            };
            buttonPanel.Children.Add(btn_close);
            var tcs = new TaskCompletionSource<string>();
            msgbox.Closed += (_, _) => tcs.SetResult(ans);
            msgbox.ShowDialog(MainWindow.Singleten);
            return tcs.Task;
        }

        //public static async Task DisplayPicture(string title, BitmapImage img, string cancel = "OK", Action action = null, bool TouchLeave = false)
        //{
        //    //MainWindow.Singleten.OpenPictureDialog(title, img, cancel, action, TouchLeave);
        //    //while (MainWindow.Singleten.Result == null)
        //    //{
        //    //    await Task.Delay(100);
        //    //}
        //}
    }
}
