using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMvvmProject.Interface;
using WpfMvvmProject.Model;

namespace WpfMvvmProject.Data
{
    public class PlayerStatsProvider : IPlayerStatsProvider
    {
        public async Task<IEnumerable<PlayerStats>?> GetPlayerStatsAsync()
        {
            //simulate long running task
            await Task.Delay(100);

            return new List<PlayerStats>()
            {
                new PlayerStats{Name="Michael Jordan", CareerPoints=32292, NumberOfChampionships=6, PointsPerGameAvg=30.1 },
                new PlayerStats{Name="Shaquille O'Neal", CareerPoints=28596, NumberOfChampionships=4, PointsPerGameAvg=23.7 },
                new PlayerStats{Name="LeBron James", CareerPoints=38450, NumberOfChampionships=4, PointsPerGameAvg=27.2 },
                new PlayerStats{Name="Kevin Durant", CareerPoints=26764, NumberOfChampionships=2, PointsPerGameAvg=27.3 },
                new PlayerStats{Name="Magic Johson", CareerPoints=17707, NumberOfChampionships=5, PointsPerGameAvg=19.5 }
            };
        }
    }
}
