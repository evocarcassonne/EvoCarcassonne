using System.Windows;
using System.Windows.Input;

namespace EvoCarcassonne.ViewModels
{
    public class MenuViewModel : ObservableObject, IViewModel
    {
        public ICommand ExitApplicationCommand { get; set; }

        public MenuViewModel()
        {
            ExitApplicationCommand = new RelayCommand(Application.Current.Shutdown);
        }
    }
}
