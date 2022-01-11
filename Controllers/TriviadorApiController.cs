using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using TriviadorClient.Entities;
using static TriviadorClient.Entities.TriviadorMap;

namespace TriviadorClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TriviadorApiController : ControllerBase
    {
        private readonly ILogger<TriviadorApiController> _logger;

        public TriviadorApiController(ILogger<TriviadorApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet("readyStatus")]
        public bool GetReadyStatus()
        {
            return GameSession.GetReadyStatus();
        }

        [HttpGet("map")]
        public TriviadorMap GetMap()
        {
            return GameSession.GetMap();
        }

        [HttpGet("playersList")]
        public IList GetPlayersList()
        {
            return GameSession.GetPlayersList();
        }

        [HttpGet("whoseTurn")]
        public string GetWhoseTurn()
        {
            return GameSession.GetWhoseTurn().ToString();
        }

        [HttpGet("nextTurn")]
        public string NextTurn()
        {
            return GameSession.NextTurn().ToString() ?? "Game session is not ready.\nThe list of players <= 1.";
        }

        [HttpGet("getQuestion")]
        public Question GetQuestion()
        {
            return GameSession.GetQuestion();
        }

        [HttpPost("checkQuestion")]
        public bool CheckQuestion([FromBody] object answer)
        {
            try
            {
                return GameSession.CheckQuestion(answer.ToString());
            }
            catch (Exception e)
            {
                _logger.LogWarning("Exception while CheckQuestion: " + e.Message);
                return false;
            }
        }

        [HttpPost("setMap")]
        public StatusCodeResult Post([FromBody] TriviadorMap map)
        {
            try
            {
                GameSession.SetMap(map);
                return new OkResult();
            }
            catch (System.Exception e)
            {
                _logger.LogWarning("Exception while setting map: " + e.Message);
                return new BadRequestResult();
            }
        }

        [HttpPut("addPlayer")]
        public StatusCodeResult AddPlayer([FromBody] Player player)
        {
            try
            {
                GameSession.AddPlayer(player);
                return new OkResult();
            }
            catch (System.Exception e)
            {
                _logger.LogWarning("Exception while adding player: " + e.Message);
                return new BadRequestResult();
            }
        }

        [HttpPut("updateCell")]
        public StatusCodeResult UpdateCell([FromBody] object cellString)
        {
            try
            {
                Cell cell = JsonConvert.DeserializeObject<Cell>(cellString.ToString());
                _logger.LogDebug($"Updating cell with id = {cell.Id}");
                GameSession.ChangeCellInMap(cell);
                _logger.LogDebug($"Updating completed successfull");
                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Exception while updating cell: " + e.Message);
                return new BadRequestResult();
            }
        }
    }
}
