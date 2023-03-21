using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfMvvmProject.Command;
using WpfMvvmProject.Enum;
using WpfMvvmProject.Interface;
using WpfMvvmProject.Model;

namespace WpfMvvmProject.ViewModel
{
    public class PlayersViewModel : ViewModelBase
    {
        private readonly IPlayerDataProvider _playerDataProvider;

        

        private PlayerItemViewModel? _selectedPlayer;
        private NavigationOption _navigationOption;

        public PlayersViewModel(IPlayerDataProvider playerDataProvider)
        {
            _playerDataProvider = playerDataProvider;

            AddPlayerCommand = new DelegateCommand(AddPlayer);
            SwitchNavigationCommand = new DelegateCommand(SwitchNavigation);
            DeletePlayerCommand = new DelegateCommand(DeletePlayer, CanDelete);
        }
                
        public ObservableCollection<PlayerItemViewModel> Players { get; } = new();

        public PlayerItemViewModel? SelectedPlayer 
        { 
            get => _selectedPlayer;
            set
            {
                _selectedPlayer = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsPlayerSelected)); 
                DeletePlayerCommand?.RaiseCanExecuteChanged();
            }
        }

        public bool IsPlayerSelected => SelectedPlayer is not null;
        public NavigationOption NavigationOption 
        { 
            get => _navigationOption;
            set
            {
                _navigationOption = value;
                RaisePropertyChanged();
            } 
        }
        public DelegateCommand AddPlayerCommand { get; }
        public DelegateCommand SwitchNavigationCommand { get; }
        public DelegateCommand DeletePlayerCommand { get; }
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
    }
}
