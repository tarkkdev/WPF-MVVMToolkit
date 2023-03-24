using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfMvvmProject.Data;
using WpfMvvmProject.Interface;
using WpfMvvmProject.ViewModel;

namespace WpfMvvmProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }        

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow?.Show(); 
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<MainWindow>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<PlayersViewModel>();
            services.AddTransient<PlayerStatsViewModel>();  
            services.AddTransient<IPlayerDataProvider, PlayerDataProvider>();
            services.AddTransient<IPlayerStatsProvider, PlayerStatsProvider>();
        }
    }
}
