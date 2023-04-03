using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfMvvmProject.Command;
using WpfMvvmProject.Enum;
using WpfMvvmProject.Interface;
using WpfMvvmProject.Model;

namespace WpfMvvmProject.ViewModel
{    
    public partial class PlayersViewModel : ViewModelBase
    {
        private readonly IPlayerDataProvider _playerDataProvider;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsPlayerSelected))]
        [NotifyCanExecuteChangedFor(nameof(DeletePlayerCommand))]
        private PlayerItemViewModel? _selectedPlayer;
        [ObservableProperty]
        private NavigationOption _navigationOption;
        
        public PlayersViewModel(IPlayerDataProvider playerDataProvider)
        {
            _playerDataProvider = playerDataProvider;            
        }
                
        public ObservableCollection<PlayerItemViewModel> Players { get; } = new();                

        public bool IsPlayerSelected => SelectedPlayer is not null;        
                
        public async override Task LoadAsync()
        {
            if (Players.Any())
            {
                return;
            }

            var players = await _playerDataProvider.GetPlayerDataAsync();
            if (players is not null)
            {
                foreach (var player in players)
                {
                    Players.Add(new PlayerItemViewModel(player));
                }
            }
        }

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

        private bool CanDelete(object? parameter) => SelectedPlayer is not null;

        [RelayCommand(CanExecute = nameof(CanDelete))]
        private void DeletePlayer(object? parameter) 
        { 
            if(SelectedPlayer is not null)
            {
                Players.Remove(SelectedPlayer);
                SelectedPlayer = null;
            }
        }
    }
}
