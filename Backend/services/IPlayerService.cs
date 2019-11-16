﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Model;

namespace Backend.services
{
    public interface IPlayerService
    {
        Guid Subscribe(Guid gameId, string playerName);
        bool Unsubscribe(Guid gameId, Guid playerId);
        List<Player> GetPlayers(Guid gameId);
    }
}
