using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMvvmProject.Interface;
using WpfMvvmProject.Model;

namespace WpfMvvmProject.ViewModel
{
    public class PlayersViewModel
    {
        private readonly IPlayerDataProvider _playerDataProvider;

        public PlayersViewModel(IPlayerDataProvider playerDataProvider)
        {
            _playerDataProvider = playerDataProvider;
        }
        public ObservableCollection<Player> Players { get; } = new();

        public Player? SelectedPlayer { get; set; }

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
    }
}
