using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMvvmProject.Model;

namespace WpfMvvmProject.ViewModel
{
    public class PlayerItemViewModel : ViewModelBase
    {
        private readonly Player _model;

        public PlayerItemViewModel(Player model)
        {
            _model = model;
        }

        public int Id => _model.Id;

        public string? FirstName
        {
            get => _model.FirstName;
            set 
            { 
                _model.FirstName = value;
                RaisePropertyChanged();
            }
        }

        
        public string? LastName
        {
            get => _model.LastName;
            set 
            { 
                _model.LastName = value;
                RaisePropertyChanged();
            }
        }

        public string? Position
        {
            get => _model.Position;
            set 
            {
                _model.Position = value;
                RaisePropertyChanged();
            }
        }

        public bool IsRetired
        { 
            get => _model.IsRetired;
            set
            {
                _model.IsRetired = value;
                RaisePropertyChanged();
            }
        }

    }
}
