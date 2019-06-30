using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using EvoCarcassonne.Models;
using Microsoft.Win32;
using Newtonsoft.Json;

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

        public ICommand LoadGameCommand { get; set; }

        public ICommand SaveGameCommand { get; set; }

        public ICommand SavePlayersCommand { get; set; }

        public ApplicationViewModel()
        {
            ViewModels = new List<IViewModel>();

            ViewModels.Add(new MenuViewModel());

            GotoMenu();

            StartNewGameCommand = new RelayCommand(StartNewGame);
            ContinueGameCommand = new RelayCommand(ContinueGame, CanContinueGame);
            GotoMenuCommand = new RelayCommand(GotoMenu);
            LoadGameCommand = new RelayCommand(LoadGame);
            SaveGameCommand = new RelayCommand(SaveGame, CanSaveGame);
            SavePlayersCommand = new RelayCommand<ObservableCollection<Player>>(SavePlayers);
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
            ChangeViewModel(new PlayerEditorViewModel());
        }

        private void ContinueGame()
        {
            ChangeViewModel(ViewModels.First(vm => vm.GetType().Name == nameof(MainController)));
        }

        private bool CanContinueGame()
        {
            return IsGameLoaded;
        }

        private bool IsGameLoaded
        {
            get
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
        }

        private void GotoMenu()
        {
            ViewModels.RemoveAll(vm => vm.GetType().Name == nameof(PlayerEditorViewModel));
            ChangeViewModel(ViewModels.First());
        }

        private void LoadGame()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Save file (*.json)|*.json",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var json = File.ReadAllText(openFileDialog.FileName);
                var gameVm = JsonConvert.DeserializeObject<MainController>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
                ViewModels.RemoveAll(vm => vm.GetType().Name == nameof(MainController));
                ChangeViewModel(gameVm);
            }
        }

        private void SaveGame()
        {
            var gameVm = ViewModels.First(vm => vm.GetType().Name == nameof(MainController)) as MainController;
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Save file (*.json)|*.json",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = $"EvoCarcassonne_Save_Round{gameVm.CurrentRound:D2}_{DateTime.Now:yyyyMMdd-HHmmss}.json"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                var json = JsonConvert.SerializeObject(gameVm, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
                File.WriteAllText(saveFileDialog.FileName, json);
            }
        }

        private bool CanSaveGame()
        {
            return IsGameLoaded;
        }

        private void SavePlayers(ObservableCollection<Player> players)
        {
            ViewModels.RemoveAll(vm => vm.GetType().Name == nameof(MainController));
            ChangeViewModel(new MainController(players));
        }
    }
}
