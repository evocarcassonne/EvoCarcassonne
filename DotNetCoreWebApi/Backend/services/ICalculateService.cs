using DotNetCoreWebApi.Backend.Model;
using System.Collections.Generic;

namespace DotNetCoreWebApi.Backend.services
{
    public interface ICalculateService
    {
        void Calculate(ITile currentTile, bool gameover, ref List<Player> players);
    }
}