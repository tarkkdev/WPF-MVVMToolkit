using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WpfMvvmProject.Interface;
using WpfMvvmProject.Model;

namespace WpfMvvmProject.ViewModel
{
    public class PlayerStatsViewModel : ViewModelBase
    {
        private readonly IPlayerStatsProvider _playerStatsProvider;

        public PlayerStatsViewModel(IPlayerStatsProvider playerStatsProvider)
        {
            _playerStatsProvider = playerStatsProvider;
        }

        public ObservableCollection<PlayerStats> PlayerStats { get; } = new();

        public override async Task LoadAsync()
        {
            if(PlayerStats.Any())
            {
                return;
            }

            var playerStats = await _playerStatsProvider.GetPlayerStatsAsync();
            if(playerStats is not null) 
            { 
                foreach(var stats in playerStats) 
                {
                    PlayerStats.Add(stats);
                }
            }
        }
    }
}
