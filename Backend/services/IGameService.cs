using System;

namespace Backend.services
{
    public interface IGameService
    {
        Guid CreateGameSession();
        void DeleteGameSession(Guid gameId);
    }
}