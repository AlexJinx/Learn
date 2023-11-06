using Hangfire;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hangfire_WPF_Demo;
public class TaskJob
{
    public async Task Test()
    {
        await Application.Current.Dispatcher.Invoke(async () =>
        {
            var _mainWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            await _mainWindow.TestAsync();
        });
    }
}
