<<<<<<< HEAD
# WPF-MVVMToolkit
Repository to implement WPF MVVM pattern using MVVM Toolkit 
=======
# WPF-MVVM

### WPF-MVVM application to test Model-View-ViewModel (MVVM) design pattern using MVVM Toolkit

- Implemetation uses MVVM toolkit

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


