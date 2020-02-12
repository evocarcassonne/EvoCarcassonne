using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetCoreWebApi.Backend.Model;

namespace DotNetCoreWebApi.Backend.services
{
    public interface IPlayerService
    {
        Guid Subscribe(Guid gameId, string playerName);
        bool Unsubscribe(Guid gameId, Guid playerId);
        List<Player> GetPlayers(Guid gameId);
        string SetColor(Guid gameId, Guid playerId, string color);
    }
}
