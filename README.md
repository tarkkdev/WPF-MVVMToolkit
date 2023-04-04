# WPF-MVVMToolkit
Repository to implement WPF MVVM pattern using MVVM Toolkit 
=======
# WPF-MVVM

### WPF-MVVM application to test Model-View-ViewModel (MVVM) design pattern using MVVM Toolkit

- Implemetation uses MVVM toolkit
- [MVVM Toolkit Documentation](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)  

#### Notes

* To enable xaml binding errors at Visual Studio 2022:  
  > Debug -> Windows -> XAML Binding Failures  

* To Access the View Model properties from View:  
  > xmlns:viewModel="clr-namespace:ProjectName.ViewModel"  
  ```xaml
    xmlns:viewModel="clr-namespace:WpfMvvmProject.ViewModel"
  ```  
  > d:DataContext="\{d:DesignInstance Type=viewModel:ViewModelName\}"
  ```xaml
    d:DataContext="{d:DesignInstance Type=viewModel:PlayersViewModel}"
  ```  

#### Progress  

1. Install *CommunityTookkit.Mvvm* from Nuget Package manager  
2. At **ViewModelBase** derive from *ObservableObject*  
   - *ObservableObject* is base class for objects that are observable by implementing *INotifyPropertyChanged* and *INotifyPropertyChanging* interfaces  
   ```c#
        public class ViewModelBase : ObservableObject
        {
            public virtual Task LoadAsync() => Task.CompletedTask;        
        }
   ```  
   - All code to implement *INotifyPropertyChanged* removed and handled by *ObservableObject*  
3. Delete *DelegateCommand* class - use RelayCommand from toolkit  
4. At **MainViewModel** class
   - Change class to **partial** class  
   - Remove all DelegateCommand references  
   - Add *[RelayCommand]* attiribute to both *SelectViewModel* and *ExitApplication* button event handlers  
   ```c#
        [RelayCommand]
        private async void SelectViewModel(object? parameter)
        {
            SelectedViewModel = parameter as ViewModelBase;
            await LoadAsync();
        }

        [RelayCommand]
        private void ExitApplication(object? obj)
        {
            Application.Current.Shutdown();
        }
   ```  
   - Update *SelectedViewModel* set to use *SetProperty*  
   ```c#
        public ViewModelBase? SelectedViewModel
		{
			get =>_selectedViewModel;
			set 
			{
                SetProperty(ref _selectedViewModel, value);				
			}
		}
   ```  
5. At *PlayerItemViewModel* class, remove *RaisePropertyChanged* and update properties to use *SetProperty*  
   ```c#
        public string? FirstName
        {
            get => _model.FirstName;
            set
            {
                SetProperty(_model.FirstName, value, _model, (p, n) => p.FirstName = n);
                if (string.IsNullOrEmpty(_model.FirstName))
                {
                    AddError("First Name is required");
                }
                else
                {
                    ClearError();
                }
            }            
        }
   ```  
6. At *ValidationViewModelBase* class, remove *RaisePropertyChanged*  
7. At *PlayersViewModel* class  
   - Remove Properties  
   - Remove DelegateCommand references
   - Add *partial* to class  
   - *_selectedPlayer* private field  
      - Add *[ObservableProperty]*  
      - Add *[NotifyPropertyChangedFor(nameof(IsPlayerSelected))]*  - to notify when player is selected
      - Add *[NotifyCanExecuteChangedFor(nameof(DeletePlayerCommand))]*  - to notify DeletePlayerCommmand CanExecute
   ```c#
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsPlayerSelected))]
        [NotifyCanExecuteChangedFor(nameof(DeletePlayerCommand))]
        private PlayerItemViewModel? _selectedPlayer;
   ```  
   - Add *[ObservableProperty]* to *NavigationOption* field  
   ```c#
        [ObservableProperty]
        private NavigationOption _navigationOption;
   ```
   - Add *[RelayCommand]* to *SwitchNavigation* and *AddPlayer* button event handlers  
   ```c#
        [RelayCommand]
        private void AddPlayer(object? parameter)
        {
            var player = new Player { FirstName = "New entry..." };
            var playerItem = new PlayerItemViewModel(player);
            Players.Add(playerItem);
            SelectedPlayer = playerItem;
        }
        [RelayCommand]
        private void SwitchNavigation(object? parameter)
        {
            NavigationOption = NavigationOption == NavigationOption.Left
                ? NavigationOption.Right
                : NavigationOption.Left;
        }
   ```  
   - Add *[RelayCommand(CanExecute = nameof(CanDelete))]* to *DeletePlayer* button event handler  
      - Set *CanExecute* to *CanDelete*  
   ```c#
        [RelayCommand(CanExecute = nameof(CanDelete))]
        private void DeletePlayer(object? parameter) 
        { 
            if(SelectedPlayer is not null)
            {
                Players.Remove(SelectedPlayer);
                SelectedPlayer = null;
            }
        }
   ```  
8.  At Add.xaml update and streamline Dependency Injection (DI) implementation  
    ```c#
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
    ```

