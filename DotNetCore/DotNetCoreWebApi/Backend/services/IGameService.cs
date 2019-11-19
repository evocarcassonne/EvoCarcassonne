using System;

namespace DotNetCoreWebApi.Backend.services
{
    public interface IGameService
    {
        Guid CreateGameSession();
        void DeleteGameSession(Guid gameId);
    }
}