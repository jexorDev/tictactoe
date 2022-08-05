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

            var playerWon = HasWinningRow(_board, _playerSymbol);

            if (playerWon)
            {
                MainLabelText = "You won!";
                NotifyPropertyChanged(nameof(MainLabelText));

                return;
            }
            
            if (!HasSpotAvailable(_board))
            {
                MainLabelText = "Cat's game";
                NotifyPropertyChanged(nameof(MainLabelText));

                return;
            }

            PerformComputersTurn();

            var computerWon = HasWinningRow(_board, _computerSymbol);

            if (computerWon)
            {
                MainLabelText = "Computer won :[";
                NotifyPropertyChanged(nameof(MainLabelText));

                return;
            }
        }

        private void PerformComputersTurn()
        {
            if (Difficulty == "Easy")
            {
                //PerformComputersTurnEasy();
            }
            else if (Difficulty == "Hard")
            {
            }
            PerformComputersTurnHard();
            //TODO: Impossible
        }

        private void PerformComputersTurnEasy()
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

        private void PerformComputersTurnHard()
        {
            //var boardCopy = GetCopyOfBoard(_board);
            //var nextSpot = NextAvailableSpot(boardCopy);
            (int?, int?) chosenSpot = (null, null);

            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    if (_board[row, column] == _playerSymbol)
                    {
                        if (IsSpotOpen(row, column, _board, Direction.Up))
                        {
                            if (NextPlayWouldResultInWin(row - 1, column, GetCopyOfBoard(_board), _playerSymbol))
                            {
                                chosenSpot = (row - 1, column);
                            }
                        }
                        if (IsSpotOpen(row, column, _board, Direction.Down))
                        {
                            if (NextPlayWouldResultInWin(row + 1, column, GetCopyOfBoard(_board), _playerSymbol))
                            {
                                chosenSpot = (row + 1, column);
                            }
                        }
                        if (IsSpotOpen(row, column, _board, Direction.Left))
                        {
                            if (NextPlayWouldResultInWin(row, column - 1, GetCopyOfBoard(_board), _playerSymbol))
                            {
                                chosenSpot = (row, column - 1);
                            }
                        }
                        if (IsSpotOpen(row, column, _board, Direction.Right))
                        {
                            if (NextPlayWouldResultInWin(row, column + 1, GetCopyOfBoard(_board), _playerSymbol))
                            {
                                chosenSpot = (row, column + 1);
                            }
                        }
                    }
                }
            }

            //while (nextSpot != (null, null) & chosenSpot == (null, null))
            //{
            //    boardCopy[nextSpot.Item1.Value, nextSpot.Item2.Value] = _playerSymbol;

            //    if (HasWinningRow(boardCopy, _playerSymbol))
            //    {
            //        chosenSpot = nextSpot;
            //    }
            //    else
            //    {
            //        nextSpot = NextAvailableSpot(boardCopy);
            //    }
            //}
            
            if (chosenSpot == (null, null))
            {
                PerformComputersTurnEasy();
            }
            else
            {
                _board[chosenSpot.Item1.Value, chosenSpot.Item2.Value] = _computerSymbol;
                NotifyPropertyChanged($"Button{chosenSpot.Item1.Value}{chosenSpot.Item2.Value}Text");
            }
        }

        private bool NextPlayWouldResultInWin(int x, int y, char[,] board, char player)
        {
            board[x, y] = player;
            return HasWinningRow(board, player);
        }

        private static bool IsSpotOpen(int x, int y, char[,] board, Direction direction)
        {
            if (direction == Direction.Down)
            {
                if (y == 2) return false;
                y++;
            }
            if (direction == Direction.Up)
            {
                if (y == 0) return false;
                y--;
            }
            if (direction == Direction.Right)
            {
                if (x == 2) return false;
                x++;
            }
            if (direction == Direction.Left)
            {
                if (x == 0) return false;
                x--;
            }

            return board[x, y] == ' ';
        }

        private static bool HasSpotAvailable(char[,] board)
        {
            return NextAvailableSpot(board) != (null, null);
        }

        private static (int?, int?) NextAvailableSpot(char[,] board)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    if (board[row, column] == ' ')
                    {
                        return (row, column);
                    }
                }
            }

            return (null, null);
        }

        private static char[,] GetCopyOfBoard(char[,] board)
        {
            var boardCopy = new char[3,3];

            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    boardCopy[row, column] = board[row, column];
                }
            }

            return boardCopy;
        }

        private static bool HasWinningRow(char[,] board, char symbol)
        {
            for (int row = 0; row < 3; row++)
            {
                if (board[row, 0] == symbol && board[row, 1] == symbol && board[row, 2] == symbol)
                {
                    return true;
                }
            }
            for (int column = 0; column < 3; column++)
            {
                if (board[0, column] == symbol && board[1, column] == symbol && board[2, column] == symbol)
                {
                    return true;
                }
            }
            if (board[0, 0] == symbol && board[1, 1] == symbol && board[2, 2] == symbol)
            {
                return true;
            }
            if (board[0, 2] == symbol && board[1, 1] == symbol && board[2, 0] == symbol)
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

            NotifyPropertyChanged(nameof(Button00Text));
            NotifyPropertyChanged(nameof(Button01Text));
            NotifyPropertyChanged(nameof(Button02Text));
            NotifyPropertyChanged(nameof(Button10Text));
            NotifyPropertyChanged(nameof(Button20Text));
            NotifyPropertyChanged(nameof(Button11Text));
            NotifyPropertyChanged(nameof(Button12Text));
            NotifyPropertyChanged(nameof(Button21Text));
            NotifyPropertyChanged(nameof(Button22Text));

        }

        private string _mainLabelText;
        public string MainLabelText
        {
            get => _mainLabelText;
            set
            {
                _mainLabelText = value;
                NotifyPropertyChanged(nameof(MainLabelText));
            }
        }

        private string _difficulty = "Easy";
        public string Difficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                NotifyPropertyChanged(nameof(Difficulty));
            }
        }

        private enum Direction
        {
            Left,
            Right,
            Up,
            Down
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
