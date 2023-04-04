using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WpfMvvmProject.Data;
using WpfMvvmProject.Interface;
using WpfMvvmProject.ViewModel;

namespace WpfMvvmProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public sealed partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = App.Current.Services.GetService<MainWindow>();
            mainWindow?.Show();
        }
        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<MainWindow>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<PlayersViewModel>();
            services.AddTransient<PlayerStatsViewModel>();
            services.AddSingleton<IPlayerDataProvider, PlayerDataProvider>();
            services.AddSingleton<IPlayerStatsProvider, PlayerStatsProvider>();

            return services.BuildServiceProvider();
        }        
    }
}
