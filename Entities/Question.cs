using System.Collections.Generic;

namespace TriviadorClient.Entities
{
    public class Question
    {
        public string TextQuestion { get; }
        public List<string> ListAnswers { get; }

        public Question(string textQuestion, List<string> listAnswers)
        {
            TextQuestion = textQuestion;
            ListAnswers = listAnswers;
        }
    }
}
