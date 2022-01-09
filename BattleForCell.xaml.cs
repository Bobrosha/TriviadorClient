using System.Windows;
using TriviadorClient.Entities;
using static TriviadorClient.Entities.TriviadorMap;

namespace TriviadorClient
{
    /// <summary>
    /// Логика взаимодействия для BattleForCell.xaml
    /// </summary>
    public partial class BattleForCell : Window
    {
        private Cell _Cell;
        private Question _Question;
        private Client _Client;

        public BattleForCell(Cell cell, Question question, Client client)
        {
            _Cell = cell;
            _Question = question;
            _Client = client;

            InitializeComponent();
        }
    }
}
