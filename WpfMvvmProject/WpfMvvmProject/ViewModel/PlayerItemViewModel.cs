using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMvvmProject.Model;

namespace WpfMvvmProject.ViewModel
{
    public partial class PlayerItemViewModel : ValidationViewModelBase
    {
        private readonly Player _model;

        private string? _firstName;

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
                SetProperty(ref _firstName, value);
                //_model.FirstName = value;
                //RaisePropertyChanged();
                if(string.IsNullOrEmpty(_model.FirstName)) 
                {
                    AddError("First Name is required");
                }
                else
                {
                    ClearError();  
                }
            }
        }

        
        public string? LastName
        {
            get => _model.LastName;
            set 
            { 
                _model.LastName = value;
                //RaisePropertyChanged();
                if (string.IsNullOrEmpty(_model.LastName))
                {
                    AddError("Last Name is required");
                }
                else
                {
                    ClearError();
                }
            }
        }

        public string? Position
        {
            get => _model.Position;
            set 
            {
                _model.Position = value;
                //RaisePropertyChanged();
                if (string.IsNullOrEmpty(_model.Position))
                {
                    AddError("Player postion is required");
                }
                else
                {
                    ClearError();
                }
            }
        }

        public bool IsRetired
        { 
            get => _model.IsRetired;
            set
            {
                _model.IsRetired = value;
                //RaisePropertyChanged();
            }
        }

    }
}
