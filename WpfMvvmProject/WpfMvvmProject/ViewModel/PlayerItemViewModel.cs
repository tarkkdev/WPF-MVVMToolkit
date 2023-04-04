using WpfMvvmProject.Model;

namespace WpfMvvmProject.ViewModel
{
    public class PlayerItemViewModel : ValidationViewModelBase
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
                SetProperty(_model.FirstName, value, _model, (p, n) => p.FirstName = n);
                if (string.IsNullOrEmpty(_model.FirstName))
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
                SetProperty(_model.LastName, value, _model, (p, l) => p.LastName = l);
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
                SetProperty(_model.Position, value, _model, (p, pst) => p.Position = pst);
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
            set => SetProperty(_model.IsRetired, value, _model, (p, r) => p.IsRetired = r);            
        }

    }
}
