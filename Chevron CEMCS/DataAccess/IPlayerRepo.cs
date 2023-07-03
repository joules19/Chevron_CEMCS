using System;
using static Chevron_CEMCS.DataAccess.PlayerModel;
using static Chevron_CEMCS.DataAccess.PlayerModel;
using static Chevron_CEMCS.Models.ApiModels;

namespace Chevron_CEMCS.DataAccess
{
    public interface IPlayerRepo
    {
        Task<Player> CreatePlayer(Player model);
        Task<int> ReturnPlayersCount(string pos);
        Task<List<Player>> GetPlayers();
        Task<Player> GetPlayer(int playerId);
        Task<Player> UpdatePlayer(int playerId, Player model);
        void DeletePlayer(int playerId);
        Task<List<Player>> ProcessTeam(List<ProcessTeamVM> searchParams);
    }
}