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
   
