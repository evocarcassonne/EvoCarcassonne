using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using EvoCarcassonne.Backend;
using EvoCarcassonne.ViewModel;

namespace EvoCarcassonne
{
    public class GameController : BaseViewModel
    {

        #region Public Properties

        /// <summary>
        /// The current player can put down this tile
        /// </summary>
        public Tile CurrentTile { get; set; }

        /// <summary>
        /// The current tile's ID
        /// </summary>
        public int CurrentTileID { get; set; } = 0;

        /// <summary>
        /// The current Player's ID
        /// </summary>
        public int CurrentPlayerID { get; set; } = 2;

        /// <summary>
        /// A flag that indicates if the round is started
        /// </summary>
        public bool IsRoundStarded { get; set; } = false;

        #endregion

        #region Commands

        /// <summary>
        /// The command to get a new Tile
        /// </summary>
        public ICommand GetNewTileCommand { get; set; }

        /// <summary>
        /// The command to rotate the CurrentTile left
        /// </summary>
        public ICommand RotateLeftCommand { get; set; }

        /// <summary>
        /// The command to rotate the CurrentTile right
        /// </summary>
        public ICommand RotateRightCommand { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="window"></param>
        public GameController(Window window)
        {
            // Create commands
            GetNewTileCommand = new RelayCommand(() => NextRound());
            RotateLeftCommand = new RelayCommand(() => CurrentTile.Rotate(-90));
            RotateRightCommand = new RelayCommand(() => CurrentTile.Rotate(90));

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// The current player gets a new Tile
        /// </summary>
        private void NextRound()
        {
            
            // If a round is started there is nothing to do
            if (IsRoundStarded)
                return;

            IsRoundStarded = true;

            // Set the current player's ID
            if (CurrentPlayerID ==  1)
                CurrentPlayerID = 2;
            else
                CurrentPlayerID = 1;



            // Get a new Tile
            GetNewTile();

            // TODO: Put the tile to the board

            // The round is finished
            IsRoundStarded = false;
            
        }

        /// <summary>
        /// Generate a new tile
        /// </summary>
        private void GetNewTile()
        {
            // TODO: Generate random tiles
            var list = new List<IDirection>();
            var speciality = Speciality.None;
            CurrentTile = new Tile(CurrentTileID++, list, speciality);
        }

        #endregion
    }
}
