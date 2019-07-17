using System.Collections.ObjectModel;
using System.Linq;
using EvoCarcassonne.Models;

namespace EvoCarcassonne.ViewModels
{
    public class GameOverViewModel : IViewModel
    {
        public MainController MainController { get; set; }

        public GameOverViewModel(MainController vm)
        {
            MainController = vm;

            MainController.Players = new ObservableCollection<Player>(MainController.Players.OrderByDescending(o => o.BackendOwner.Points));

            for (int i = 0; i < MainController.Players.Count; i++)
            {
                MainController.Players[i].Rank = i + 1;
            }
        }
    }
}
