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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TickTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new TickTacToeViewModel();
            btn_0_0.Click += btnClicked;
            btn_0_1.Click += btnClicked;
            btn_0_2.Click += btnClicked;
            btn_1_0.Click += btnClicked;
            btn_1_1.Click += btnClicked;
            btn_1_2.Click += btnClicked;
            btn_2_0.Click += btnClicked;
            btn_2_1.Click += btnClicked;
            btn_2_2.Click += btnClicked;

            (DataContext as TickTacToeViewModel).ResetBoard();
        }

        public void ResetButtonClick(object sender, EventArgs e)
        {
            (DataContext as TickTacToeViewModel).ResetBoard();

        }

        private void btnClicked(object sender, EventArgs e)
        {
            if (sender == null) return;
            if (sender is not Button) return;
            Button? btnObject = (sender as Button);

            var nameArray = btnObject.Name.Split("_");
            int.TryParse(nameArray[1].ToString(), out int x);
            int.TryParse(nameArray[2].ToString(), out int y);

            (DataContext as TickTacToeViewModel).PerformPlayerTurn(x, y);
        }
    }
}
