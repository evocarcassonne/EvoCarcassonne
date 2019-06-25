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

            // TODO: add view models to the collection

            CurrentViewModel = ViewModels.First();

            ChangeViewModelCommand = new RelayCommand<string>(ChangeViewModel);
        }

        private void ChangeViewModel(string viewModelName)
        {
            ChangeViewModel(ViewModels.First(vm => vm.Title.Contains(viewModelName)));
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
