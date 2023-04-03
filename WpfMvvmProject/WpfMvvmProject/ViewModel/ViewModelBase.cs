using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfMvvmProject.ViewModel
{    
    public partial class ViewModelBase : ObservableObject
    {
        //public event PropertyChangedEventHandler? PropertyChanged;

        //protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public virtual Task LoadAsync() => Task.CompletedTask;
        
    }
}
