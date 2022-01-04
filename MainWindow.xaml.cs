using System.Windows;

namespace TriviadorClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            _ = new Startup();

            InitializeComponent();
        }
    }
}
