using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TickTacToe
{
    public class TickTacToeViewModel : INotifyPropertyChanged
    {
        private char[,] _board = new char[3, 3];
        private char _playerSymbol = 'X';
        private char _computerSymbol = 'O';

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public void PerformPlayerTurn(int x, int y)
        {
            

            _board[x, y] = _playerSymbol;
            NotifyPropertyChanged($"Button{x}{y}Text");


            var playerWon = HasWinningRow(_playerSymbol);

            if (playerWon)
            {
                MainLabelText = "You won!";
                NotifyPropertyChanged(nameof(MainLabelText));

                return;
            }

           
            
            if (!HasSpotAvailable())
            {
                MainLabelText = "Cat's game";
                NotifyPropertyChanged(nameof(MainLabelText));

                return;
            }

            PerformComputersTurn();

            var computerWon = HasWinningRow(_playerSymbol);

            if (computerWon)
            {
                MainLabelText = "Computer won :[";
                NotifyPropertyChanged(nameof(MainLabelText));

                return;
            }

        }

        private void PerformComputersTurn()
        {
            var random = new Random();

            while (true)
            {
                var nextX = random.Next(3);
                var nextY = random.Next(3);
                if (_board[nextX, nextY] == ' ')
                {
                    _board[nextX, nextY] = _computerSymbol;
                    NotifyPropertyChanged($"Button{nextX}{nextY}Text");
                    return;
                }
            }
        }

        private bool HasSpotAvailable()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    if (_board[row, column] == ' ')
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool HasWinningRow(char symbol)
        {
            for (int row = 0; row < 3; row++)
            {
                if (_board[row, 0] == symbol && _board[row, 1] == symbol && _board[row, 2] == symbol)
                {
                    return true;
                }
            }
            for (int column = 0; column < 3; column++)
            {
                if (_board[0, column] == symbol && _board[1, column] == symbol && _board[2, column] == symbol)
                {
                    return true;
                }
            }
            if (_board[0, 0] == symbol && _board[1, 1] == symbol && _board[2, 2] == symbol)
            {
                return true;
            }
            if (_board[0, 2] == symbol && _board[1, 1] == symbol && _board[2, 0] == symbol)
            {
                return true;
            }

            return false;
        }

        public void ResetBoard()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    _board[row, column] = ' ';
                }
            }
            MainLabelText = $"You are {_playerSymbol}";
        }

        public string MainLabelText
        {
            get; private set;
        }

        #region buttons
        private string GetButtonText(int x, int y)
        {
            return _board[x, y].ToString();
        }

        public string Button00Text
        {
            get { return GetButtonText(0, 0); }
        }


        public string Button01Text
        {
            get { return GetButtonText(0, 1); }
        }

        public string Button02Text
        {
            get { return GetButtonText(0, 2); }
        }

        public string Button10Text
        {
            get { return GetButtonText(1, 0); }
        }

        public string Button11Text
        {
            get { return GetButtonText(1, 1); }
        }

        public string Button12Text
        {
            get { return GetButtonText(1, 2); }
        }

        public string Button20Text
        {
            get { return GetButtonText(2, 0); }
        }

        public string Button21Text
        {
            get { return GetButtonText(2, 1); }
        }

        public string Button22Text
        {
            get { return GetButtonText(2, 2); }
        }

        #endregion

    }
}
