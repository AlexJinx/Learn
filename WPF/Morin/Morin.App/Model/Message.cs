using Morin.App.Common.Page;

using System.Diagnostics;
using System.Windows;

namespace Morin.App.Model
{
    public class Message
    {
        public void DebugOut(string t)
        {
            Debug.WriteLine(t);
        }

        public void ShowPopup(string text, bool Success)
        {
            if (Success)
            {
                HandyControl.Controls.Growl.Success(
                new HandyControl.Data.GrowlInfo
                {
                    Message = text,
                    ShowDateTime = false,
                    WaitTime = 3,
                });
            }
            else
            {
                HandyControl.Controls.Growl.Warning(
                new HandyControl.Data.GrowlInfo
                {
                    Message = text,
                    ShowDateTime = false,
                    WaitTime = 5,
                    ShowCloseButton = true,
                });
            }
        }

        public bool ShowMessage(string text)
        {
            HandyControl.Controls.Growl.Info(
            new HandyControl.Data.GrowlInfo
            {
                Message = text,
                ShowDateTime = false,
                WaitTime = 3,
                ShowCloseButton = true,
            });
            return true;
        }

        public bool ShowMessageOK(string text)
        {
            ShowMessageBox messageBox = new ShowMessageBox();
            messageBox.text.Text = text;
            messageBox.Info.Text = "提示一下";
            messageBox.Button2.Visibility = Visibility.Visible;

            messageBox.ShowDialog();
            return messageBox.result;
        }

        public bool ShowMessageOK(string text, bool showRemember)
        {
            ShowMessageBox messageBox = new ShowMessageBox();
            messageBox.text.Text = text;
            messageBox.Info.Text = "提示一下";
            messageBox.Button2.Visibility = Visibility.Visible;
            if (showRemember) messageBox.Remember.Visibility = Visibility.Visible;
            messageBox.ShowDialog();
            return messageBox.result;
        }

        public bool ShowMessage(string text, string btnOK, string btnCancel)
        {
            ShowMessageBox messageBox = new ShowMessageBox();
            messageBox.text.Text = text;
            messageBox.Info.Text = "提示一下";
            messageBox.Button2.Visibility = Visibility.Visible;
            messageBox.ok1.Content = btnOK;
            messageBox.Cancel.Content = btnCancel;
            messageBox.ShowDialog();
            return messageBox.result;
        }

        public bool ShowMessageOK(string text, bool showRemember, string btnOK, string btnCancel, out bool Remember)
        {
            ShowMessageBox messageBox = new ShowMessageBox();
            messageBox.text.Text = text;
            messageBox.Info.Text = "提示一下";
            messageBox.Button2.Visibility = Visibility.Visible;
            messageBox.ok1.Content = btnOK;
            messageBox.Cancel.Content = btnCancel;
            if (showRemember) messageBox.Remember.Visibility = Visibility.Visible;
            messageBox.ShowDialog();
            Remember = (bool)!messageBox.Remember.IsChecked;
            return messageBox.result;
        }
    }
}
