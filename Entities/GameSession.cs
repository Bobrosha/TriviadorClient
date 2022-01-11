using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System;
using System.Drawing;
using System.IO;
using static TriviadorClient.Entities.TriviadorMap;

namespace TriviadorClient.Entities
{
    public class GameSession
    {
        private static TriviadorMap _Map;
        private static int _Turn;
        private static List<Question> _Questions;
        private static Question _CurrentQuestion;
        private static Random random;

        public static void Initialize()
        {
            _Map = new TriviadorMap(true);
            _Questions = new List<Question>();
            _Turn = 0;
            random = new Random();
            InitQuestions();
        }

        private static void InitQuestions()
        {
            StreamReader sr = new StreamReader("FileQuestions");
            string inputJson = sr.ReadToEnd();

            _Questions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Question>>(inputJson);
        }

        public static Question GetQuestion()
        {
            if (_CurrentQuestion == null)
            {
                GetRandomQuestion();
            }

            var answers = _CurrentQuestion.ListAnswers.ToList();

            int n = answers.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                string value = answers[k];
                answers[k] = answers[n];
                answers[n] = value;
            }

            return new Question(_CurrentQuestion.TextQuestion, answers);
        }

        public static bool CheckQuestion(string answer)
        {
            return _CurrentQuestion.ListAnswers[0].Equals(answer);
        }

        private static void GetRandomQuestion()
        {
            int randomIndex = random.Next(0, _Questions.Count);
            _CurrentQuestion = _Questions[randomIndex];
        }

        public static string GetSerializedMap()
        {
            return JsonSerializer.Serialize(_Map);
        }

        public static TriviadorMap GetMap()
        {
            return _Map;
        }

        public static void ChangeCellInMap(Cell newCell)
        {
            Cell oldCell = _Map.Cells[newCell.Id - 1];

            _Map.Players[_Turn].Score += newCell.Value;

            if (oldCell.OwnerId != null)
            {
                _Map.Players[_Turn == 0 ? 1 : 0].Score -= newCell.Value;
            }

            _Map.Cells[newCell.Id - 1] = newCell;

        }

        public static void AddPlayer(Player player)
        {
            var id = _Map.Players.Count;
            player.Id = id;
            player.ColorName = Enum.GetValues(typeof(KnownColor)).GetValue(id).ToString();
            player.Score = 1000;
            _Map.Players.Add(player);
        }

        public static List<Player> GetPlayersList()
        {
            return _Map.Players;
        }

        public static bool GetReadyStatus()
        {
            return _Map.Players.Count > 1;
        }

        public static int GetWhoseTurn()
        {
            return _Turn;
        }

        public static int? NextTurn()
        {
            if (GetReadyStatus())
            {
                _Turn = _Turn == 0 ? 1 : 0;
                _CurrentQuestion = null;
                return _Turn;
            }
            else
            {
                return null;
            }
        }
    }
}
