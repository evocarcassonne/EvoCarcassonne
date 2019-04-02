using System.Windows;
using System.Windows.Controls;

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
                b.Content = b.Tag.ToString();
                // var coords = b.Tag.ToString().Split(';').Select(int.Parse).ToArray();
            }
        }
    }
}
