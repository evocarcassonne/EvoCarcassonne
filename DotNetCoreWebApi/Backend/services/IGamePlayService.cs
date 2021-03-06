﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetCoreWebApi.Backend.dao;
using DotNetCoreWebApi.Backend.Model;
using DotNetCoreWebApi.Models;

namespace DotNetCoreWebApi.Backend.services
{
    public interface IGamePlayService
    {
        Player GetCurrentPlayer(Guid gameId);
        int GetCurrentRound(Guid gameId);
        bool PlaceTile(Guid gameId, Guid playerId, ITile tileToPlace, Coordinates coordinates);
        bool PlaceFigure(Guid gameId, Guid playerId, int side);
        ITile GetNewTile(Guid gameId, Guid PlayerId);
        GamePlay EndTurn(Guid gameId, Guid playerId);
        Dictionary<int, IFigure> GetFiguresOnTiles(Guid gameId, ITile tile);
        bool StartGame(Guid gameId, Guid playerId);
        GamePlay GetState(Guid gameId);
    }
}
