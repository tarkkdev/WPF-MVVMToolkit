using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMvvmProject.Command;

namespace WpfMvvmProject.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase? _selectedViewModel;

        public MainViewModel(PlayersViewModel playersViewModel,
            PlayerStatsViewModel playerStatsViewModel)
        {
            PlayersViewModel = playersViewModel;
            PlayerStatsViewModel = playerStatsViewModel;
            SelectedViewModel = PlayersViewModel;
            SelectViewModelCommand = new DelegateCommand(SelectViewModel);
        }
              

        public ViewModelBase? SelectedViewModel
		{
			get =>_selectedViewModel;
			set 
			{ 
				_selectedViewModel = value; 
				RaisePropertyChanged();
			}
		}
        public PlayersViewModel PlayersViewModel { get; }
        public PlayerStatsViewModel PlayerStatsViewModel { get; }

        public DelegateCommand SelectViewModelCommand { get; }

        public async override Task LoadAsync() 
        {
            if(SelectedViewModel is not null)
            {
                await SelectedViewModel.LoadAsync();
            }
        }

        private async void SelectViewModel(object? parameter)
        {
            SelectedViewModel = parameter as ViewModelBase;
            await LoadAsync();
        }

    }
}
