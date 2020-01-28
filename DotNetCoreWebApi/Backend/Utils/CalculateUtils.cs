using System.Collections.Generic;
using DotNetCoreWebApi.Backend.Model;

namespace DotNetCoreWebApi.Backend
{
    public class CalculateUtils
    {
        public static void GiveBackFigureToOwner(IFigure figure, ref List<Player> players)
        {
            if (figure != null)
            {
                foreach (var player in players)
                {
                    if (player.Owner.Equals(figure.Owner))
                    {
                        player.Figures.Add(new Figure(figure.Owner));
                    }
                }
            }
        }

        public static void DistributePoints(int result, List<IFigure> figuresOnTiles)
        {
            var points = new List<int>();
            var players = new List<IOwner>();
            var playersToGetPoints = new List<IOwner>();
            foreach (var i in figuresOnTiles)
            {
                if (!players.Contains(i.Owner))
                {
                    players.Add(i.Owner);
                }
            }
            int maxIndex = 0;
            for (int i = 0; i < players.Count; i++)
            {
                int currentCount = 0;
                for (int j = 0; j < figuresOnTiles.Count; j++)
                {
                    if (players[i].Equals(figuresOnTiles[j].Owner))
                    {
                        currentCount++;
                    }
                }
                points.Add(currentCount);
                if (points[i] > points[maxIndex])
                {
                    maxIndex = i;
                }
            }

            if (players.Count != 0)
            {
                playersToGetPoints.Add(players[maxIndex]);
            }
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i] == points[maxIndex] && i != maxIndex)
                {
                    playersToGetPoints.Add(players[i]);
                }
            }
            foreach (var i in playersToGetPoints)
            {
                i.Points += result;
            }
        }
    }
}
