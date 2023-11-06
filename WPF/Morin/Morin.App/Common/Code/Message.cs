using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morin.App.Common.Code
{
    public class Message
    {
        public void DebugOut(string t)
        {
            Debug.WriteLine(t);
        }

        public void ShowPopup(string text, bool success)
        {
            if (success)
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
            return messageBox.Resul;
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
            return messageBox.Resul;
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
            return messageBox.Resul;
        }
    }
}
