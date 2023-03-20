using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfMvvmProject.Interface;
using WpfMvvmProject.Model;

namespace WpfMvvmProject.ViewModel
{
    public class PlayersViewModel : INotifyPropertyChanged
    {
        private readonly IPlayerDataProvider _playerDataProvider;
        private Player? _selectedPlayer;

        public PlayersViewModel(IPlayerDataProvider playerDataProvider)
        {
            _playerDataProvider = playerDataProvider;
        }
        public ObservableCollection<Player> Players { get; } = new();

        public Player? SelectedPlayer 
        { 
            get => _selectedPlayer;
            set
            {
                _selectedPlayer = value;
                RaisePropertyChanged(/*nameof(SelectedPlayer)*/);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public async Task LoadAsync()
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
                    Players.Add(player);
                }
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
