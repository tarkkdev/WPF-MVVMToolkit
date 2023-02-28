using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMvvmProject.Model;

namespace WpfMvvmProject.Interface;

public interface IPlayerDataProvider
{
    Task<IEnumerable<Player>?> GetPlayerDataAsync();
}
