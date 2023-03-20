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
   
   
