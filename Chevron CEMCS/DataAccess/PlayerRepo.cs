using System;
using System.Collections.Generic;
using System.Reflection;
using static Chevron_CEMCS.DataAccess.PlayerModel;
using static Chevron_CEMCS.Helpers.Methods;
using static Chevron_CEMCS.Models.ApiModels;

namespace Chevron_CEMCS.DataAccess
{
    public class PlayerRepo : IPlayerRepo
    {
        List<Player> playersData = new List<Player>() { };

        public async Task<List<Player>> GetPlayers()
        {
            return playersData;
        }

        public async Task<Player> GetPlayer(int id)
        {
            return playersData.Where(x => x.id == id).FirstOrDefault();
        }

        public async Task<int> ReturnPlayersCount(string pos)
        {
            return playersData.Count(x => x.position == pos);
        }

        public async Task<Player> CreatePlayer(Player model)
        {
            Player newPlayer = new Player()
            {
                id = nextPlayerId(playersData),
                name = model.name,
                position = model.position,
            };

            int ind = 1;
            foreach (var skill in model.playerSkills)
            {
                skill.id = ind;
                skill.playerId = newPlayer.id;
                ind++;
            }
            newPlayer.playerSkills = model.playerSkills;

            playersData.Add(newPlayer);

            return newPlayer;

        }


        public async Task<Player> UpdatePlayer(int playerId, Player model)
        {
            List<Player> players = await GetPlayers();
            Player player = players.Where(x => x.id == playerId).FirstOrDefault();
            player.name = model.name;
            player.position = model.position;
            return player;
        }

        public async void DeletePlayer(int playerId)
        {
            List<Player> players = await GetPlayers();
            Player player = players.Where(x => x.id == playerId).FirstOrDefault();

            playersData.Remove(player);
        }

        public async Task<List<Player>> ProcessTeam(List<ProcessTeamVM> searchParams)
        {
            List<Player> foundPlayers = new();
            HashSet<int> HS1 = new HashSet<int>();

            foreach (var param in searchParams)
            {
                int playersNeeded = param.numberOfPlayers;

                int seenPlayers = await ReturnPlayersCount(param.position);

                for (int i = 0; i < playersNeeded; i++)
                {
                    HashSet<int> HS2 = new HashSet<int>();

                    foreach (var player in playersData)
                    {
                        if (player.position == param.position && player.playerSkills.Any(u => u.skill == param.mainSkill))
                        {
                            if (HS2.Count() >= playersNeeded)
                            {
                                break;
                            }

                            HS2.Add(player.id);
                        }
                    }

                    foreach (var id in HS2)
                    {
                        HS1.Add(id);
                    }
                }
            }



            foreach (var id in HS1)
            {
                Player player = playersData.Where(x => x.id == id).FirstOrDefault();
                foundPlayers.Add(player);
            }


            return foundPlayers;
        }


    }
}

