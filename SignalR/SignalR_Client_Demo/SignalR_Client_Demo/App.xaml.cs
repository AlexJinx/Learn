using Hangfire;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SignalR_Client_Demo;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public ServiceProvider ServiceProvider { get; private set; }

    public BackgroundJobServer BackgroundJobServer { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();
        var mainView = ServiceProvider.GetRequiredService<MainWindow>();
        mainView.ShowDialog();
        base.OnStartup(e);
    }

    private void ConfigureServices(IServiceCollection services)
    {
        #region Hangfire

        //GlobalConfiguration.Configuration.UseInMemoryStorage();
        //BackgroundJobServer = new BackgroundJobServer();
        //RecurringJob.AddOrUpdate<TaskJob>("InitTimer", job => job.InitToDayAsync(), Cron.Daily(1, 30), TimeZoneInfo.Local);

        #endregion

        //AddApiClient(services);
        //AddFileClient(services);
        services.AddHttpClient();
        //services.AddTransient<SqliteHelper>();
        services.AddTransient(typeof(MainWindow));
    }

}
