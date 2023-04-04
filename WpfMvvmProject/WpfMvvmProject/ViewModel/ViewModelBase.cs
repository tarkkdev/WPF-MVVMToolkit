using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace WpfMvvmProject.ViewModel
{
    public class ViewModelBase : ObservableObject
    {
        public virtual Task LoadAsync() => Task.CompletedTask;        
    }
}
