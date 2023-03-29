<<<<<<< HEAD
# WPF-MVVMToolkit
Repository to implements WPF MVVM pattern using MVVM Toolkit 
=======
# WPF-MVVM

### WPF-MVVM application to test Model-View-ViewModel (MVVM) design pattern

- Goal is to separate UI controls and program logic
- Implemetation doesn't use any toolkit or library such as MVVM Toolkit or Prism framework, plan to use these later and update the project

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

1. **View** - Added UI components  
  1.1 MainWindow - Window  
  1.2 PlayersView - User Control  
  1.3 HeaderControl - User Control
2. Added Data  
  2.1 PlayerDataProvider  
3. Added **Model**  
  3.1 Player  
  3.2 PlayerStats  
4. Added **ViewModel**  
   4.1 PlayersViewModel  
   > At PlayersViewModel class, created *ObservableCollection* of Player  
   > At PlayersViewModel class, created *SelectedPlayer* as Player  
   > At PlayersViewModel class, added async load function to load players data from PlayerDataProvider  
   > At PlayersView code behind file at constructor, initialized PlayerViewModel class and assign to *DataContext* of view    
   > At PlatersView code behind file at contructor, added *Loaded* event handler to load player data from view model asynchronously     
   > At PlayersView, bind ListControl *ItemSource* to Players  
   > At PlayersView, bind ListControl *SelectedItem* to SelectedPlayer  
   > At PlayersView, bind FirstName textbox *Text* to SelectedPlayer.FirstName  
   > At PlayersView, bind LastName textbox *Text* to SelectedPlayer.LastName  
   > At PlayersView, bind Position textbox *Text* to SelectedPlayer.Position  
   > At PlayersView, bind Player Retired checkbox *IsChecked* to SelectedPlayer.IsRetired
   4.2 PlayersViewModel  
   > At PlayersViewModel class, class implements *INotifyPropertyChanged* (System.ComponentModel)
     ```c#
       public class PlayersViewModel : INotifyPropertyChanged  
     ```  
   > At PlayersViewModel class, *PropertyChangedEventHandler* event implementation of *INotifyPropertyChanged*  
   > At PlayersViewModel class, added *RaisePropertyChanged* method  
     ```c#
       private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
       {
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
       }
     ``` 
   >  [CallerMethodName] (System.Runtime.CompilerServices) allows omiting Property Name when *RaisePropertyChanged* method is called
   ```c#
     public Player? SelectedPlayer 
     { 
        get => _selectedPlayer;
        set
        {
            _selectedPlayer = value;
            RaisePropertyChanged(/*nameof(SelectedPlayer)*/);
        }
     }
   ```  
5. Added **ViewModelBase** that implements INotifyPropertyChanged  
6. Changed PlayerViewModel to derive from ViewModelBase  
7. Added **PlayerItemViewModel** to separate *Player* class logic from *PlayersViewModel*  
   >  *PlayerItemViewModel* class also derived from *ViewModelBase*  
8. Added **NavigationOption** Enum  
9. Added **NavigationOptionToGridColumnConverter** class that implements IValueConverter (System.Windows.Data)  
   ```c#
     public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
     {
        var navigationOption = (NavigationOption)value;
        //return value for Grid Column
        return navigationOption == NavigationOption.Left
            ? 0 : 2;
     }
   ```  
10. Added **DelegateCommand** class that implements ICommand interface (System.Windows.Input)  
    > DelegateCommand class contructor expects Action delegate to call when Execute is called  
    > Constructor also expects Func delegate to call and has a return of bool when CanExecute is called, which is optional
    ```c#
    public class DelegateCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public DelegateCommand(Action<object?> execute, Func<object?,bool>? canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(execute);
            _execute = execute;
            _canExecute = canExecute;
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => _canExecute is null || _canExecute(parameter);
        
        public void Execute(object? parameter) => _execute(parameter);        
    }
    ```  
11. At PlayerView View, added Converter for Grid.Column property based on *NavigationOptionToGridColumnConverter*  
    ```xaml
    <UserControl.Resources>
        <converter:NavigationOptionToGridColumnConverter
            x:Key="NavOptionToGridColConv" />
    </UserControl.Resources>
    ```  
    > At Grid control switch Grid from left to right or vice versa  
    ```xaml
    <Grid Grid.Column="{Binding NavigationOption, Converter={StaticResource NavOptionToGridColConv}}">
    ```  
12. At PlayerView  
    >  bind AddPlayerCommand to Add button Command property  
    >  bind DeletePlayerCommand to Delete button Command property  
    >  bind SwitchNavigationCommand to Switch View button Command property  
13. At PlayersViewModel added delegates for button commands  
    ```c#
        public DelegateCommand AddPlayerCommand { get; }
        public DelegateCommand SwitchNavigationCommand { get; }
        public DelegateCommand DeletePlayerCommand { get; }
    ```  
14. At PlayersViewModel at constructor assigned functions to delegates
    ```c#
    public PlayersViewModel(IPlayerDataProvider playerDataProvider)
    {
        _playerDataProvider = playerDataProvider;

        AddPlayerCommand = new DelegateCommand(AddPlayer);
        SwitchNavigationCommand = new DelegateCommand(SwitchNavigation);
        DeletePlayerCommand = new DelegateCommand(DeletePlayer, CanDelete);
    }
    ```  
15. Methods to call from DelegateCommand
    ```c#
    private void AddPlayer(object? parameter)
    {
        var player = new Player { FirstName = "New entry..." };
        var playerItem = new PlayerItemViewModel(player);
        Players.Add(playerItem);
        SelectedPlayer = playerItem;
    }

    private void SwitchNavigation(object? parameter)
    {
        NavigationOption = NavigationOption == NavigationOption.Left
            ? NavigationOption.Right
            : NavigationOption.Left;
    }

    private bool CanDelete(object? parameter) => SelectedPlayer is not null;        

    private void DeletePlayer(object? parameter) 
    { 
        if(SelectedPlayer is not null)
        {
            Players.Remove(SelectedPlayer);
            SelectedPlayer = null;
        }
    }
    ```
16. At SelectedPlayer property, raise CanExecute for Delete button (to enable or disable the delete button)
    ```c#
    public PlayerItemViewModel? SelectedPlayer 
    { 
        get => _selectedPlayer;
        set
        {
            _selectedPlayer = value;
            RaisePropertyChanged();
            DeletePlayerCommand?.RaiseCanExecuteChanged();
        }
    }
    ```  
17. Added Resources directory for global location for all resource files  
18. Added Converter resource for Boolean to Visibility  
    ```xaml
    <ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
       <BooleanToVisibilityConverter x:Key="BooleanToVisibilityConv"/>
    </ResourceDictionary>
    ```  
19. Added Brushes resource
    ```xaml
    <ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
        <SolidColorBrush x:Key="HeaderBackgroundBrush" Color="CornflowerBlue"/>
        <SolidColorBrush x:Key="HeaderForegroundBrush" Color="White"/>    
    </ResourceDictionary>
    ```
19. At PlayersView added *BooleanToVisibilityConverter*
    ```xaml
      <StackPanel Grid.Column="1" Margin="10 30 10 10"
                    Visibility="{Binding IsPlayerSelected, Converter={StaticResource BooleanToVisibilityConv}}">
    ```  
20. At HeaderControl changed brushes to use from global resources  
21. At App.xaml created Resource Dictionary and merged both Brushes and Converters resources
    ```xaml
       <Application.Resources>
            <ResourceDictionary>
                <ResourceDictionary.MergedDictionaries>
                    <ResourceDictionary Source="/Resources/Brushes.xaml"/>
                    <ResourceDictionary Source="/Resources/Converters.xaml"/>
                </ResourceDictionary.MergedDictionaries>
            </ResourceDictionary>
       </Application.Resources>
    ```  
22. Added *PlayerStatsView*  
23. Added *PlayerStatsViewModel*  
24. Added *MainViewModel* class to handle switching between Player View and Player Stats View  
    >  Constructor expects both View Models - **PlayersViewModel** and **PlayerStatsViewModel**  
    ```c#
       public MainViewModel(PlayersViewModel playersViewModel,
            PlayerStatsViewModel playerStatsViewModel)
        {
            PlayersViewModel = playersViewModel;
            PlayerStatsViewModel = playerStatsViewModel;
            SelectedViewModel = PlayersViewModel;
            SelectViewModelCommand = new DelegateCommand(SelectViewModel);
        }
    ```  
    > SelectedViewModel property set to PlayersViewModel but will change from MainWindow  
    > *SelectedViewModelCommand* will be called based on Menu selection Command and CommandParameters  
    > *SelectViewModel* method passed to SelectedViewModelCommand of DelegateCommand as Action parameter  
    ```c#
        private async void SelectViewModel(object? parameter)
        {
            SelectedViewModel = parameter as ViewModelBase;
            await LoadAsync();
        }
    ```  
25. At *MainWindow.xaml* created DataTemplates for each View based on selected ViewModel
    ```xaml
        <Window.Resources>
            <DataTemplate DataType="{x:Type viewModel:PlayersViewModel}">
                <view:PlayersView/>
            </DataTemplate>
            <DataTemplate DataType="{x:Type viewModel:PlayerStatsViewModel}">
                <view:PlayerStatsView/>
            </DataTemplate>
        </Window.Resources>
    ```  
    > Menu commands to switch between Players and Player Stats views
    ```xaml
        <MenuItem Header="_Select View">
                <MenuItem Header="_Players"
                          Command="{Binding SelectViewModelCommand}"
                          CommandParameter="{Binding PlayersViewModel}"/>
                <MenuItem Header="_Stats"
                          Command="{Binding SelectViewModelCommand}"
                          CommandParameter="{Binding PlayerStatsViewModel}"/>
        </MenuItem>
    ```  
    > Changed to ContentControl to show Selected View from Menu selection
    ```xaml
        <ContentControl Grid.Row="2" Content="{Binding SelectedViewModel}"/>
    ```  
26. Dependency Injection
    > At Dependencies, Manage Nuget Packages - install **Microsoft.Extensions.DependencyInjection** package  
    > At App.xaml - remove the StartupUri  
    > At App.xaml.cs code behind file override OnStartup event  
    > At App.xaml.cs add a constructor and intialize ServiceCollection  
    ```c#
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
    ```  
27. Data Validation  
    > Added new **ValidationViewModelBase** class that implements *INotifyDataErrorInfo (System.ComponentModel)* and derived from *ViewModelBase*    
    > Added a new dictionary to store each error by property name
    ```c#
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new();
    ```  
    > If there are any errors *HasError* will return true:  
    ```c#
        public bool HasErrors => _errorsByPropertyName.Any();
    ```  
    > *GetErrors* returns the error based on property  
    ```c#
        public IEnumerable GetErrors(string? propertyName)
        {
            return propertyName is not null && _errorsByPropertyName.ContainsKey(propertyName)
                ? _errorsByPropertyName[propertyName] : Enumerable.Empty<string>();
        }
    ```  
    > Added virtual *OnErrorsChanged* method that will be called when there is an error
    ```c#
        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs args) 
        {
            ErrorsChanged?.Invoke(this, args);
        }
    ```  
    > Added AddError to add error to dictionary and ClearError to remove error from dictionarty and to invoke *OnErrorsChanged* and Raise *HasError*  
    ```c#
        protected void AddError(string error, [CallerMemberName] string? propertyName = null)
        {
            if (propertyName is null) return;


            if (!_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName[propertyName] = new List<string>();
            }

            if (!_errorsByPropertyName[propertyName].Contains(error)) 
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
                RaisePropertyChanged(nameof(HasErrors));
            }
        }

        protected void ClearError([CallerMemberName] string? propertyName = null)
        {
            if(propertyName is null) return;

            if(_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
                RaisePropertyChanged(nameof(HasErrors));
            }
        }
    ```  
    > Update **PlayerItemViewModel** class to derive from *ValidationViewModelBase*  
    > At properties added Error conditions and call *AddError* and *ClearError* methods from base Validation class:
      ```c#
        public string? FirstName
        {
            get => _model.FirstName;
            set 
            { 
                _model.FirstName = value;
                RaisePropertyChanged();
                if(string.IsNullOrEmpty(_model.FirstName)) 
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
    > At Styles.xaml for TextBox Controls added Trigger for *Validation.HasError* property and if set to True  
    > Set Background to Red  
    > Set Margin for below of Textbox to enable space for error message  
    > Set ToolTip for Error Message  
    > Set *Validation.ErrorTemplate* to show Error Message below the TextBox  
    ```xaml
        <ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
            <Style TargetType="ToolTip">
                <Setter Property="FontSize" Value="20"/>
            </Style>
            <Style TargetType="TextBox" x:Key="TextBoxBaseStyle">
                <Setter Property="Padding" Value="5"/>
            </Style>
            <Style TargetType="TextBox" BasedOn="{StaticResource TextBoxBaseStyle}">
                <Setter Property="Background" Value="#555555"/>
                <Setter Property="Foreground" Value="White"/>
                <Setter Property="Validation.ErrorTemplate">
                    <Setter.Value>
                        <ControlTemplate>
                            <StackPanel>
                                <AdornedElementPlaceholder x:Name="placeholder"/>
                                <TextBlock Foreground="Red" 
                                           Text="{Binding ElementName=placeholder,
                                                 Path=AdornedElement.(Validation.Errors)[0].ErrorContent}" 
                                           Margin="3 0 0 0"/>
                            </StackPanel>
                        </ControlTemplate>
                    </Setter.Value>
                </Setter>
                <Style.Triggers>
                    <Trigger Property="IsMouseOver" Value="True">
                        <Setter Property="Background" Value="DimGray"/>
                    </Trigger>
                    <Trigger Property="IsFocused" Value="True">
                        <Setter Property="Background" Value="White"/>
                        <Setter Property="Foreground" Value="Black"/>
                    </Trigger>
                    <Trigger Property="Validation.HasError" Value="True">
                        <Setter Property="Background" Value="Red" />
                        <Setter Property="Margin" Value="0 0 0 20" />
                        <Setter Property="ToolTip"
                                Value="{Binding RelativeSource={RelativeSource Self},Path=(Validation.Errors)[0].ErrorContent}"/>
                    </Trigger>
                </Style.Triggers>
            </Style>    
        </ResourceDictionary>
    ```
    > At PlayersView.xaml to not show the errors at list box text boxes, set *ValidationsOnNotifyDataErrors* to false  
      ```xaml
         <UserControl.Resources>
            <converter:NavigationOptionToGridColumnConverter x:Key="NavOptionToGridColConv" />
            <DataTemplate x:Key="PlayerListDataTemplate">
                <StackPanel Orientation="Horizontal">
                    <TextBlock Text="{Binding FirstName, ValidatesOnNotifyDataErrors=False}" FontWeight="Bold" />
                    <TextBlock Text="{Binding LastName, ValidatesOnNotifyDataErrors=False}" Margin="5 0 0 0" />
                </StackPanel>
            </DataTemplate>
        </UserControl.Resources>
      ```
        

   
>>>>>>> 88e36f3aa0bafadde27dae1ae05279c24e642dfe
