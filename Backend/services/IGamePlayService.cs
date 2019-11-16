using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.dao;
using Backend.Model;

namespace Backend.services
{
    public interface IGamePlayService
    {
        Player GetCurrentPlayer(Guid gameId);
        int GetCurrentRound(Guid gameId);
        bool PlaceTileAndFigure(Guid gameId, ITile tileToPlace, Coordinates coordinates, int side);
        ITile GetNewTile(Guid gameId);
        GamePlay EndTurn(Guid gameId);
    }
}
