using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfMvvmProject.Data;
using WpfMvvmProject.ViewModel;

namespace WpfMvvmProject.View
{
    /// <summary>
    /// Interaction logic for PlayersView.xaml
    /// </summary>
    public partial class PlayersView : UserControl
    {
        private PlayersViewModel _viewModel;

        public PlayersView()
        {
            InitializeComponent();
            _viewModel = new PlayersViewModel(new PlayerDataProvider());
            DataContext = _viewModel;
            Loaded += PlayersView_Loaded;
        }

        private async void PlayersView_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
    }
}
