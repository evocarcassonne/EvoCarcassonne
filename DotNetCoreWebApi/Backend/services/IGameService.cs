using System;

namespace DotNetCoreWebApi.Backend.services
{
    public interface IGameService
    {
        Guid CreateGameSession(string playerName);
        bool DeleteGameSession(Guid gameId, Guid playerId);
    }
}