using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media;
using EvoCarcassonne.Models;

namespace EvoCarcassonne.ViewModels
{
    public class PlayerEditorViewModel : ObservableObject, IViewModel
    {
        public List<PropertyInfo> Brushes { get; set; } = typeof(Brushes).GetProperties().ToList();

        private PropertyInfo _selectedBrush;

        public PropertyInfo SelectedBrush
        {
            get { return _selectedBrush; }
            set
            {
                if (_selectedBrush != value)
                {
                    _selectedBrush = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _playerName;

        public string PlayerName
        {
            get { return _playerName; }
            set
            {
                if (_playerName != value)
                {
                    _playerName = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();

        private Player _selectedPlayer;

        public Player SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                if (_selectedPlayer != value)
                {
                    _selectedPlayer = value;
                    OnPropertyChanged();
                }
            }
        }

        public PlayerEditorViewModel()
        {
            var random = new Random();
            SelectedBrush = Brushes[random.Next(Brushes.Count)];

            AddPlayerCommand = new RelayCommand(AddPlayer, CanAddPlayer);
            RemovePlayerCommand = new RelayCommand(RemovePlayer, CanRemovePlayer);
        }
        private void AddPlayer()
        {
            var color = SelectedBrush.GetValue(Brushes) as Brush;

            string playerName;
            if (string.IsNullOrWhiteSpace(PlayerName))
            {
                playerName = $"Player{Players.Count + 1}";
            }
            else
            {
                playerName = PlayerName;
            }
            var player = new Player(playerName, color);
            
            var random = new Random();

            Brushes.Remove(SelectedBrush);
            SelectedBrush = Brushes[random.Next(Brushes.Count)];
            PlayerName = string.Empty;

            Players.Add(player);
            OnPropertyChanged("CanSavePlayers");
        }

        private bool CanAddPlayer()
        {
            return SelectedBrush != null;
        }

        private void RemovePlayer()
        {
            Players.Remove(SelectedPlayer);
            OnPropertyChanged("CanSavePlayers");
        }

        private bool CanRemovePlayer()
        {
            return SelectedPlayer != null;
        }

        public bool CanSavePlayers
        {
            get
            {
                return Players.Count > 0;
            }
        }

        public ICommand AddPlayerCommand { get; set; }
        public ICommand RemovePlayerCommand { get; set; }

    }
}
