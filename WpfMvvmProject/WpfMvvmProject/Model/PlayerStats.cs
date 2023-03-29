using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMvvmProject.Model;

public class PlayerStats
{
    public string? Name { get; set; }
    public double PointsPerGameAvg { get; set; }

    public int CareerPoints { get; set; }

    public int NumberOfChampionships { get; set; }
}
