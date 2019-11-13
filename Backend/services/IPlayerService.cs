using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Model;

namespace Backend.services
{
    interface IPlayerService
    {
        void Subscribe(Guid gameID, Player player);
        void Unsubscribe(Guid gameId, Player player);
        List<Player> GetPlayers(Guid gameId);
        Player GetCurrentPlayer(Guid gameId);
    }
}
