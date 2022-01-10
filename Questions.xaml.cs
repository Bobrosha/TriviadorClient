using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using TriviadorClient.Entities;

namespace TriviadorClient
{
   /// <summary>
   /// Логика взаимодействия для Questions.xaml
   /// </summary>
   public partial class Questions : Window
   {
      private readonly Client _Client;
      public Questions(Client client)
      {
         _Client = client;
         InitializeComponent();
      }

      private void Button_Click_EndQuestion(object sender, EventArgs e)
      {
         System.Threading.Thread.Sleep(5000);
         WindowQuestions.Close();
      }
   }
}
