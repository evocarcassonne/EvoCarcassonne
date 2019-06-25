using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace EvoCarcassonne.ViewModels
{
    public class ApplicationViewModel : ObservableObject
    {
        private IViewModel _currentViewModel;
        private List<IViewModel> _viewModels;

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

        public List<IViewModel> ViewModels
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

        public ICommand StartNewGameCommand { get; set; }

        public ICommand ContinueGameCommand { get; set; }

        public ICommand GotoMenuCommand { get; set; }

        public ApplicationViewModel()
        {
            ViewModels = new List<IViewModel>();

            ViewModels.Add(new MenuViewModel());

            GotoMenu();

            StartNewGameCommand = new RelayCommand(StartNewGame);
            ContinueGameCommand = new RelayCommand(ContinueGame, CanContinueGame);
            GotoMenuCommand = new RelayCommand(GotoMenu);
        }

        private void ChangeViewModel(IViewModel viewModel)
        {
            if (!ViewModels.Contains(viewModel))
            {
                ViewModels.Add(viewModel);
            }

            CurrentViewModel = ViewModels.FirstOrDefault(vm => vm == viewModel);
        }

        private void StartNewGame()
        {
            ViewModels.RemoveAll(vm => vm.GetType().Name == nameof(MainController));
            ChangeViewModel(new MainController());
        }

        private void ContinueGame()
        {
            ChangeViewModel(ViewModels.First(vm => vm.GetType().Name == nameof(MainController)));
        }

        private bool CanContinueGame()
        {
            foreach (var viewModel in ViewModels)
            {
                if (viewModel.GetType().Name == nameof(MainController))
                {
                    return true;
                }
            }

            return false;
        }

        private void GotoMenu()
        {
            ChangeViewModel(ViewModels.First());
        }
    }
}
