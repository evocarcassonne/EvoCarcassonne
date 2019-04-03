using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EvoCarcassonne.Controller;

namespace EvoCarcassonne.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button b)
            {
                //ToggleCoordinates(b);
                RotateRight(b);
            }
        }

        // for testing purposes
        private void RotateRight(Button button)
        {
            var coords = button.Tag.ToString().Split(';').Select(int.Parse).ToArray();
            MainController.GetTile(coords).Angle += 90.0;
        }

        // for testing purposes
        private void ToggleCoordinates(ContentControl button)
        {
            if (button.Content == null)
            {
                var coords = button.Tag.ToString().Split(';').Select(int.Parse).ToArray();
                button.Content = MainController.GetTile(coords).Coordinates;
            }
            else
            {
                button.Content = null;
            }
        }
    }
}
