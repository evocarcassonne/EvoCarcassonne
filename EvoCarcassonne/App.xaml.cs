using System.Windows;
using EvoCarcassonne.ViewModels;
using EvoCarcassonne.Views;

namespace EvoCarcassonne
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Overrides and extends the base <see cref="Application.OnStartup"/> method with our <see cref="OnStartup()"/> method
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            OnStartup();
        }

        /// <summary>
        /// Starts the <see cref="ApplicationView"/>, sets the data context <see cref="ApplicationViewModel"/> and shows it to user
        /// </summary>
        private static void OnStartup()
        {
            var app = new ApplicationView();
            var context = new ApplicationViewModel();
            app.DataContext = context;
            app.Show();
        }
    }
}
