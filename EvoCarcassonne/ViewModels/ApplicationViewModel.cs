using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EvoCarcassonne.ViewModels
{
    public class ApplicationViewModel : ObservableObject
    {
        private IViewModel _currentViewModel;
        private ObservableCollection<IViewModel> _viewModels;

        public IViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<IViewModel> ViewModels
        {
            get => _viewModels;
            set
            {
                if (_viewModels != value)
                {
                    _viewModels = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ChangeViewModelCommand { get; set; }

        public ApplicationViewModel()
        {
            ViewModels = new ObservableCollection<IViewModel>();

            ViewModels.Add(new MenuViewModel());
            ViewModels.Add(new MainController());

            CurrentViewModel = ViewModels.First();

            ChangeViewModelCommand = new RelayCommand<string>(ChangeViewModel);
        }

        private void ChangeViewModel(string viewModelName)
        {
            switch (viewModelName)
            {
                case "menu":
                    ChangeViewModel(ViewModels[0]);
                    break;
                case "game":
                    ChangeViewModel(ViewModels[1]);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void ChangeViewModel(IViewModel viewModel)
        {
            if (!ViewModels.Contains(viewModel))
            {
                ViewModels.Add(viewModel);
            }

            CurrentViewModel = ViewModels.FirstOrDefault(vm => vm == viewModel);
        }
    }
}
