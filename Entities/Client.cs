using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TriviadorClient.Entities
{
    public class Client
    {
        private readonly HttpClient _Client;
        private readonly ILogger _Logger;
        private readonly string _Uri;

        private TriviadorMap _Map;
        private int _Turn;

        public Client(ILogger logger)
        {
            _Logger = logger;
            _Client = new HttpClient();
            _Uri = "http://localhost:5000/TriviadorApi";
            GetMapFromServer();
        }

        public TriviadorMap GetMap()
        {
            return _Map;
        }

        public int GetTurn()
        {
            return _Turn;
        }

        public void GetMapFromServer()
        {
            try
            {
                _Logger.LogInformation("Start getting map");
                string responseMap = _Client.GetStringAsync($"{_Uri}/map").Result;

                _Map = JsonConvert.DeserializeObject<TriviadorMap>(responseMap);

                if (_Map.Cells.Count > 0)
                {
                    _Logger.LogInformation("Map takken successfull");
                }
                else
                {
                    throw new JsonSerializationException("Mapping object failed");
                }
            }
            catch (Exception e)
            {
                _Logger.LogError("Error while getting map " + e.Message);
            }
        }

        public void AddPlayer(string name)
        {
            var newPlayer = new Player(name, 0);
            var json = JsonConvert.SerializeObject(newPlayer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = _Client.PutAsync($"{_Uri}/addPlayer", content).Result;
            if (!result.IsSuccessStatusCode)
            {
                _Logger.LogWarning($"Player with name \"{name}\" hasn't been added");
                _Logger.LogInformation($"Reason: {result.RequestMessage.Content.ReadAsStringAsync().Result}");
            }
            else
            {
                GetMapFromServer();
            }
        }

        public void GetWhoseTurn()
        {
            try
            {
                _Logger.LogInformation("Start getting turn");
                string responseTurn = _Client.GetStringAsync($"{_Uri}/getWhoseTurn").Result;

                int turn = int.Parse(responseTurn);

                _Turn = turn;
            }
            catch (FormatException e)
            {
                _Logger.LogWarning("While taking turn, game session is not ready.");
            }
            catch (Exception e)
            {
                _Logger.LogError("Error while getting turn " + e.Message);
            }
        }

        public void NextTurn()
        {
            try
            {
                _Logger.LogInformation("Start getting NextTurn");
                string responseTurn = _Client.GetStringAsync($"{_Uri}/nextTurn").Result;

                int turn = int.Parse(responseTurn);

                _Turn = turn;
            }
            catch (FormatException e)
            {
                _Logger.LogWarning("While taking NextTurn, game session is not ready. Cause: " + e.Message);
            }
            catch (Exception e)
            {
                _Logger.LogError("Error while getting NextTurn " + e.Message);
            }
        }

        public void GetPlayersList()
        {
            try
            {
                _Logger.LogInformation("Start getting PlayersList");
                string responseListPlayers = _Client.GetStringAsync($"{_Uri}/playersList").Result;

                var listPlayers = JsonConvert.DeserializeObject<List<Player>>(responseListPlayers);

                _Map.Players = listPlayers;

                if (listPlayers.Count > 0)
                {
                    _Logger.LogInformation("PlayersList takken successfull");
                }
                else
                {
                    throw new JsonSerializationException("Takken PlayersList failed");
                }
            }
            catch (Exception e)
            {
                _Logger.LogError("Error while getting PlayersList " + e.Message);
            }
        }

        public void UpdateCell(TriviadorMap.Cell cell)
        {
            try
            {
                _Logger.LogInformation("Start updateCell");
                string responseListPlayers = _Client.GetStringAsync($"{_Uri}/updateCell").Result;

                var json = JsonConvert.SerializeObject(cell);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = _Client.PutAsync($"{_Uri}/addPlayer", content).Result;
                if (!result.IsSuccessStatusCode)
                {
                    _Logger.LogWarning($"Cell with id = \"{cell.Id}\" hasn't been updated");
                    _Logger.LogInformation($"Reason: {result.RequestMessage.Content.ReadAsStringAsync().Result}");
                }
                else
                {
                    GetMapFromServer();
                }
            }
            catch (Exception e)
            {
                _Logger.LogError("Error while updateCell " + e.Message);
            }
        }

        public bool GetReadyStatus()
        {
            try
            {
                _Logger.LogInformation("Getting game ready status from server");
                string responseReadyStatus = _Client.GetStringAsync($"{_Uri}/readyStatus").Result;

                return bool.Parse(responseReadyStatus);
            }
            catch (Exception e)
            {
                _Logger.LogError("Unknown status, exception: " + e.Message);
                return false;
            }
        }
    }
}
