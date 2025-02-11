using Escape.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Escape.WPF.ViewModel
{
    public class EscapeViewModel : ViewModelBase
    {
        private EscapeGameModel _model;
        private ObservableCollection<EscapeField> _gameBoard;
        private int _tableSize;
        private bool _isPaused;

        public ObservableCollection<EscapeField> GameBoard
        {
            get { return _gameBoard; }
            set
            {
                _gameBoard = value;
                OnPropertyChanged();
            }
        }
        public int TableSize
        {
            get { return _tableSize; }
            set
            {
                _tableSize = value;
                OnPropertyChanged();
            }
        }
        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                if (_isPaused != value)
                {
                    _isPaused = value;
                    OnPropertyChanged(nameof(IsPaused));
                }
            }
        }
        public DelegateCommand NewGameCommand { get; private set; }

        public DelegateCommand LoadGameCommand { get; private set; }

        public DelegateCommand SaveGameCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }

        public DelegateCommand PauseCommand { get; private set; }

        public DelegateCommand KeyPressCommand { get; private set; }





        public String GameTime { get { return TimeSpan.FromSeconds(_model.GameTime).ToString("g"); } }

        public bool IsGameEasy
        {
            get { return _model.Difficulty == Difficulty.Easy; }
            set
            {
                if (_model.Difficulty == Difficulty.Easy)
                    return;

                _model.Difficulty = Difficulty.Easy;
                //TableSize = 11;
                OnPropertyChanged(nameof(IsGameEasy));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameHard));
            }
        }

        public bool IsGameMedium
        {
            get { return _model.Difficulty == Difficulty.Medium; }
            set
            {
                if (_model.Difficulty == Difficulty.Medium)
                    return;

                _model.Difficulty = Difficulty.Medium;
                //TableSize = 15;
                OnPropertyChanged(nameof(IsGameEasy));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameHard));
            }
        }

        public bool IsGameHard
        {
            get { return _model.Difficulty == Difficulty.Hard; }
            set
            {
                if (_model.Difficulty == Difficulty.Hard)
                    return;

                _model.Difficulty = Difficulty.Hard;
                //TableSize = 21;
                OnPropertyChanged(nameof(IsGameEasy));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameHard));
            }
        }

        public event EventHandler? NewGame;

        public event EventHandler? LoadGame;

        public event EventHandler? SaveGame;

        public event EventHandler? ExitGame;

        public event EventHandler? PauseGame;

        public event EventHandler<KeyEventArgs>? KeyPress;

        public EscapeViewModel(EscapeGameModel model)
        {
            _gameBoard = new ObservableCollection<EscapeField>();
            _model = model;
            _model.NewGame();

            _model.FieldChanged += new EventHandler<EscapeFieldEventArgs>(Model_FieldChanged);
            _model.GameAdvanced += new EventHandler<EscapeEventArgs>(Model_GameAdvanced);
            _model.GameOver += new EventHandler<EscapeEventArgs>(Model_GameOver);


            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
            PauseCommand = new DelegateCommand(param => OnPauseGame());
            KeyPressCommand = new DelegateCommand(param => {
                if (param is KeyEventArgs e) {
                    OnKeyPress(e);
                }});

            GenerateTable();
            RefreshTable();
        }
        public void GenerateTable()
        {
            if (GameBoard.Count != 0) 
            {
                GameBoard.Clear();
            }

            TableSize = _model.Table.Size;
            for (int i = 0; i < _model.Table.Size; i++)
            {
                for (int j = 0; j < _model.Table.Size; j++)
                {
                    GameBoard.Add(new EscapeField
                    {
                        IsEnabled = true,
                        Color = "",
                        Text = String.Empty,
                        X = j,
                        Y = i,
                        StepCommand = new DelegateCommand(param =>
                        {
                            if (param is Tuple<int, int, int, string> p)
                            {
                                StepGame(p.Item1, p.Item2, p.Item3, p.Item4);
                            }
                        })
                    });
                }
            }
        }
        public void RefreshTable()
        {
            foreach (EscapeField field in GameBoard) 
            {
                field.Text = !_model.Table.IsEmpty(field.X, field.Y) ? _model.Table[field.X, field.Y].ToString() : String.Empty;
                field.IsEnabled = true;
                if (_model.Table.IsEmpty(field.X, field.Y))
                {
                    field.Color = "gray";
                }
                else if (_model.Table[field.X, field.Y] == 2)
                {
                    field.Color = "black";
                }
                else if (_model.Table[field.X, field.Y] == 3)
                {
                    field.Color = "yellow";
                }
                else if (_model.Table[field.X, field.Y] == 4)
                {
                    field.Color = "red";
                }
                else if (_model.Table[field.X, field.Y] == 5)
                {
                    field.Color = "purple";
                }
            }

            OnPropertyChanged(nameof(GameTime));
        }
        private void StepGame(int x, int y, int player, string dir)
        {
            _model.Step(x, y, player, dir);
        }
        private void Model_FieldChanged(object? sender, EscapeFieldEventArgs e)
        {
            EscapeField field = GameBoard.Single(f => f.X == e.X && f.Y == e.Y);

            
            field.Text = !_model.Table.IsEmpty(field.X, field.Y) ? _model.Table[field.X, field.Y].ToString() : String.Empty; 
            OnPropertyChanged();
        }
        private void Model_GameAdvanced(object? sender, EscapeEventArgs e)
        {
            OnPropertyChanged(nameof(GameTime));
        }
        private void Model_GameOver(object? sender, EscapeEventArgs e)
        {
                foreach (EscapeField field in GameBoard)
                {
                    field.IsEnabled = false;
                }
        }
        private void Model_GameCreated(object? sender, EscapeEventArgs e)
        {
            RefreshTable();
        }

        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnLoadGame()
        {
            LoadGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnPauseGame()
        {
            PauseGame?.Invoke(this, EventArgs.Empty);
        }
        public void OnKeyPress(KeyEventArgs e)
        {
            Console.WriteLine(e.Key);
            KeyPress?.Invoke(this,  e);
        }

    }
}
