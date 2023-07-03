using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Chevron_CEMCS.DataAccess;
using Chevron_CEMCS.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static Chevron_CEMCS.DataAccess.PlayerModel;
using static Chevron_CEMCS.Models.ApiModels;
using static Chevron_CEMCS.Models.Response;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chevron_CEMCS.Controllers
{
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/")]
    [ApiController]

    public class PlayerController : ControllerBase
    {

        private readonly IPlayerRepo _playerRepo;
        private readonly IConfiguration _configuration;

        public PlayerController(IPlayerRepo playerRepo, IConfiguration configuration)
        {
            _playerRepo = playerRepo;
            _configuration = configuration;
        }

        [ApiExplorerSettings(IgnoreApi = false)]
        [HttpPost, Route("player")]
        public async Task<IActionResult> CreatePlayer([FromBody] Player model)
        {
            string msg = DataValidation.ValidateDataForNewPlayer(model);
            if (!String.IsNullOrEmpty(msg))
            {

                return new JsonResult(new ResponseData
                {
                    Message = msg
                })
                {
                    StatusCode = 400
                };
            }

            var player = await _playerRepo.CreatePlayer(model);

            return new JsonResult(new ResponseData
            {
                Message = "Success",
                Data = player
            })
            {
                StatusCode = 200
            };
        }


        [ApiExplorerSettings(IgnoreApi = false)]
        [HttpGet, Route("player")]
        public async Task<IActionResult> GetPlayers()
        {
            var players = await _playerRepo.GetPlayers();

            return new JsonResult(new ResponseData
            {
                Message = "Success",
                Data = players
            })
            {
                StatusCode = 200
            };
        }



        [ApiExplorerSettings(IgnoreApi = false)]
        [HttpPut, Route("player/{playerId}")]
        public async Task<IActionResult> UpdatePlayer(int playerId, [FromBody] Player model)
        {
            string msg = DataValidation.ValidateDataForNewPlayer(model);

            if (!String.IsNullOrEmpty(msg))
            {

                return new JsonResult(new ResponseData
                {
                    Message = msg
                })
                {
                    StatusCode = 400
                };
            }

            Player player = await _playerRepo.GetPlayer(playerId);

            if (player == null)
            {
                return new JsonResult(new ResponseData
                {
                    Message = "Player with Id " + playerId + " not found"
                })
                {
                    StatusCode = 400
                };
            }

            var updatedPlayer = await _playerRepo.UpdatePlayer(playerId, model);
            return new JsonResult(updatedPlayer)
            {
                StatusCode = 200
            };
        }


        [ApiExplorerSettings(IgnoreApi = false)]
        [HttpDelete, Route("player/{playerId}")]
        public async Task<IActionResult> DeletePlayer(int playerId)
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return BadRequest("Not authorized");

            var authHeader = Request.Headers["Authorization"][0];
            if (!authHeader.StartsWith("Bearer "))
            {
                return BadRequest("Not authorized");
            }

            string storedToken = _configuration.GetValue<string>("Token");
            var token = authHeader.Substring("Bearer ".Length);

            if (storedToken != token)
            {
                return BadRequest("Not authorized");
            }

            Player player = await _playerRepo.GetPlayer(playerId);

            if (player == null)
            {
                return new JsonResult(new ResponseData
                {
                    Message = "Player with Id " + playerId + " not found"
                })
                {
                    StatusCode = 400
                };
            }

            _playerRepo.DeletePlayer(playerId);

            return Ok();
        }


        [ApiExplorerSettings(IgnoreApi = false)]
        [HttpPost, Route("team/process")]
        public async Task<IActionResult> SelectTeam([FromBody] List<ProcessTeamVM> model)
        {
            string msg = DataValidation.ValidateDataForTeamSelection(model);
            if (!String.IsNullOrEmpty(msg))
            {

                return new JsonResult(new ResponseData
                {
                    Message = msg
                })
                {
                    StatusCode = 400
                };
            }

            foreach (var param in model)
            {
                int playersNeeded = param.numberOfPlayers;

                int seenPlayers = await _playerRepo.ReturnPlayersCount(param.position);

                if (seenPlayers < playersNeeded)
                {
                    return new JsonResult(new ResponseData
                    {
                        Message = "Insufficient Players for the " + param.position + " position"
                    })
                    {
                        StatusCode = 400
                    };
                }

            }
            var foundPlayers = await _playerRepo.ProcessTeam(model);

            return Ok(foundPlayers);
        }
    }
}




