using Hangfire;
using Hangfire.Redis;
using Hangfire.Storage.SQLite;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Hangfire_WPF_Demo;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public ServiceProvider ServiceProvider { get; private set; }
    public BackgroundJobServer _backgroundJobServer { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();
        var mainView = ServiceProvider.GetRequiredService<MainWindow>();
        mainView.ShowDialog();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _backgroundJobServer.Dispose();

        base.OnExit(e);
    }

    private async void ConfigureServices(ServiceCollection services)
    {
        GlobalConfiguration.Configuration.UseSQLiteStorage();
        _backgroundJobServer = new BackgroundJobServer();

        RecurringJob.RemoveIfExists("Test");
        RecurringJob.AddOrUpdate<TaskJob>("Test", (job) => job.Test(), Cron.MinuteInterval(1), TimeZoneInfo.Local);
        services.AddTransient(typeof(MainWindow));
    }


}

