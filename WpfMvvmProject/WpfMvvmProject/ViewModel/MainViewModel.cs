using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Windows;

namespace WpfMvvmProject.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private ViewModelBase? _selectedViewModel;

        public MainViewModel(PlayersViewModel playersViewModel,
            PlayerStatsViewModel playerStatsViewModel)
        {
            PlayersViewModel = playersViewModel;
            PlayerStatsViewModel = playerStatsViewModel;
            SelectedViewModel = PlayersViewModel;            
        }        

        public ViewModelBase? SelectedViewModel
		{
			get =>_selectedViewModel;
			set 
			{
                SetProperty(ref _selectedViewModel, value);				
			}
		}
        public PlayersViewModel PlayersViewModel { get; }
        public PlayerStatsViewModel PlayerStatsViewModel { get; }

        public async override Task LoadAsync() 
        {
            if(SelectedViewModel is not null)
            {
                await SelectedViewModel.LoadAsync();
            }
        }

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

    }
}
