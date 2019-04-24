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

        private void RotateRightButtonCLick(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            RotateRight(b);
        }

        private void RotateLeftButtonCLick(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            RotateLeft(b);
        }

        private void RotateLeft(Button button)
        {

            var vm = (MainController)this.DataContext;
            // If the tile is down we can't rotate
            if (vm.TileIsDown)
                return;

            vm.CurrentBoardTile.Angle -= 90;
        }

        private void RotateRight(Button button)
        {

            var vm = (MainController)this.DataContext;
            // If the tile is down we can't rotate
            if (vm.TileIsDown)
                return;

            vm.CurrentBoardTile.Angle += 90;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button b)
            {

                //ToggleCoordinates(b);

                // Put the current tile down
                var vm = (MainController)this.DataContext;
                vm.PutTile(sender as Button);                

            }
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
