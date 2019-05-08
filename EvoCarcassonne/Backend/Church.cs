﻿using System.Collections.Generic;
using EvoCarcassonne.Controller;
using EvoCarcassonne.Model;

namespace EvoCarcassonne.Backend
{
    public class Church : ITile, ILandscape
    {
        public int TileID { get; set; }
        public List<IDirection> Directions { get; set; }
        public List<Speciality> Speciality { get; set; }
        private bool IsColostor = false;

        public Church(int tileId, List<IDirection> directions, List<Speciality> speciality)
        {
            TileID = tileId;
            Directions = directions;
            Speciality = speciality;
        }

        public void Rotate(int direction)
        {
            throw new System.NotImplementedException();
        }

        public IDirection getTileSideByCardinalDirection(CardinalDirection side)
        {

            return null;

        }

        public int calculate(BoardTile currentTile, CardinalDirection whereToGo, bool firstCall)
        {
            return 0;
        }

        public bool checkCurrentSpeciality(BoardTile currentTile)
        {

            if (currentTile.BackendTile.Speciality.Contains(Backend.Speciality.Colostor))
            {
                return IsColostor = true;
            }

            else
            {
                return IsColostor = false;
            }
        }
        public int checkScore(BoardTile currentTile)
        {
            int score = 0;

            if (checkCurrentSpeciality(currentTile) == true)
            {

                score = isThereEightNeighbor(currentTile);

            }
            else
            {

            }
            return score;

        }
        public int isThereEightNeighbor(BoardTile currentTile)
        {
            int temp = 0;
            int score = 0;

            foreach (var neighborTile in MainController.PlacedBoardTiles)
            {


                if (currentTile.Coordinates.X + 10 == neighborTile.Coordinates.X &&
                    currentTile.Coordinates.Y == neighborTile.Coordinates.Y && neighborTile.BackendTile.TileID != 0)
                {
                    if (checkCurrentSpeciality(neighborTile) == true)
                    {

                        score += isThereEightNeighbor(neighborTile);
                    }
                    temp++;


                }

                if (currentTile.Coordinates.Y + 10 == neighborTile.Coordinates.Y &&
                    currentTile.Coordinates.X == neighborTile.Coordinates.X && neighborTile.BackendTile.TileID != 0)
                {
                    if (checkCurrentSpeciality(neighborTile) == true)
                    {

                        score += isThereEightNeighbor(neighborTile);
                    }
                    temp++;
                }


                if (currentTile.Coordinates.X - 10 == neighborTile.Coordinates.X &&
                    currentTile.Coordinates.Y == neighborTile.Coordinates.Y && neighborTile.BackendTile.TileID != 0)
                {
                    if (checkCurrentSpeciality(neighborTile) == true)
                    {

                        score += isThereEightNeighbor(neighborTile);
                    }
                    temp++;

                }


                if (currentTile.Coordinates.Y - 10 == neighborTile.Coordinates.Y &&
                    currentTile.Coordinates.X == neighborTile.Coordinates.X && neighborTile.BackendTile.TileID != 0)
                {
                    if (checkCurrentSpeciality(neighborTile) == true)
                    {

                        score += isThereEightNeighbor(neighborTile);
                    }
                    temp++;

                }
                if (currentTile.Coordinates.Y - 10 == neighborTile.Coordinates.Y &&
                    currentTile.Coordinates.X - 10 == neighborTile.Coordinates.X && neighborTile.BackendTile.TileID != 0)
                {
                    if (checkCurrentSpeciality(neighborTile) == true)
                    {

                        score += isThereEightNeighbor(neighborTile);
                    }
                    temp++;

                }
                if (currentTile.Coordinates.Y + 10 == neighborTile.Coordinates.Y &&
                    currentTile.Coordinates.X - 10 == neighborTile.Coordinates.X && neighborTile.BackendTile.TileID != 0)
                {
                    if (checkCurrentSpeciality(neighborTile) == true)
                    {

                        score += isThereEightNeighbor(neighborTile);
                    }
                    temp++;

                }
                if (currentTile.Coordinates.Y - 10 == neighborTile.Coordinates.Y &&
                    currentTile.Coordinates.X + 10 == neighborTile.Coordinates.X && neighborTile.BackendTile.TileID != 0)
                {
                    if (checkCurrentSpeciality(neighborTile) == true)
                    {

                        score += isThereEightNeighbor(neighborTile);
                    }
                    temp++;

                }
                if (currentTile.Coordinates.Y + 10 == neighborTile.Coordinates.Y &&
                    currentTile.Coordinates.X + 10 == neighborTile.Coordinates.X && neighborTile.BackendTile.TileID != 0)
                {
                    if (checkCurrentSpeciality(neighborTile) == true)
                    {

                        score += isThereEightNeighbor(neighborTile);
                    }
                    temp++;

                }

            }
            if (temp == 8)
            {
                score++;
            }

            return score;
        }


        public int checkFinalScore(BoardTile currentTile)
        {
            int FinalScore = 0;
            if (checkCurrentSpeciality(currentTile) == true)
            {

                FinalScore = isThereFinalEightNeighbor(currentTile);

            }
            else
            {





            }
            return FinalScore;
        }

        public int isThereFinalEightNeighbor(BoardTile currentTile)
        {
            int temp = 0;
            int score = 0;

            foreach (var neighborTile in MainController.PlacedBoardTiles)
            {


                if (currentTile.Coordinates.X + 10 == neighborTile.Coordinates.X &&
                    currentTile.Coordinates.Y == neighborTile.Coordinates.Y && neighborTile.BackendTile.TileID != 0)
                {

                    temp++;


                }

                if (currentTile.Coordinates.Y + 10 == neighborTile.Coordinates.Y &&
                    currentTile.Coordinates.X == neighborTile.Coordinates.X && neighborTile.BackendTile.TileID != 0)
                {

                    temp++;
                }


                if (currentTile.Coordinates.X - 10 == neighborTile.Coordinates.X &&
                    currentTile.Coordinates.Y == neighborTile.Coordinates.Y && neighborTile.BackendTile.TileID != 0)
                {

                    temp++;

                }


                if (currentTile.Coordinates.Y - 10 == neighborTile.Coordinates.Y &&
                    currentTile.Coordinates.X == neighborTile.Coordinates.X && neighborTile.BackendTile.TileID != 0)
                {

                    temp++;

                }
                if (currentTile.Coordinates.Y - 10 == neighborTile.Coordinates.Y &&
                    currentTile.Coordinates.X - 10 == neighborTile.Coordinates.X && neighborTile.BackendTile.TileID != 0)
                {

                    temp++;

                }
                if (currentTile.Coordinates.Y + 10 == neighborTile.Coordinates.Y &&
                    currentTile.Coordinates.X - 10 == neighborTile.Coordinates.X && neighborTile.BackendTile.TileID != 0)
                {

                    temp++;

                }
                if (currentTile.Coordinates.Y - 10 == neighborTile.Coordinates.Y &&
                    currentTile.Coordinates.X + 10 == neighborTile.Coordinates.X && neighborTile.BackendTile.TileID != 0)
                {

                    temp++;

                }
                if (currentTile.Coordinates.Y + 10 == neighborTile.Coordinates.Y &&
                    currentTile.Coordinates.X + 10 == neighborTile.Coordinates.X && neighborTile.BackendTile.TileID != 0)
                {

                    temp++;

                }

            }
            if (temp == 8)
            {
                score++;
            }

            return score;
        }

        public int finalScore(BoardTile currentTile)
        {
            int FinalScore = 0;
            foreach (var tile in MainController.PlacedBoardTiles)
            {
                FinalScore += isThereFinalEightNeighbor(tile);
            }

            return FinalScore;
        }



    }
}


