using Escape.Persistence;

namespace Escape.Model
{
    public enum Difficulty { Easy, Medium, Hard }
    public class EscapeGameModel
    {
        #region Constants
        private const int _EasyField = 11;
        private const int _MediumField = 15;
        private const int _HardField = 21;
        #endregion

        #region Fields
        private IEscapeDataAccess _dataAccess;
        private EscapeTable _table;
        private Difficulty _difficulty;
        private int _gameTime;
        private bool _isGameOver;
        private bool _isPaused;
        private (int, int) Enemies = (4, 5);
        #endregion

        #region Properties
        public int GameTime { get { return _gameTime; } }
        public EscapeTable Table { get { return _table; } }
        public bool IsGameOver { get { return _isGameOver; } set { _isGameOver = value; } }
        public bool IsPaused { get { return _isPaused; } set { _isPaused = value; } }
        public Difficulty Difficulty { get { return _difficulty; } set { _difficulty = value; } }
        #endregion

        #region Events
        public event EventHandler<EscapeFieldEventArgs>? FieldChanged;
        public event EventHandler<EscapeEventArgs>? GameAdvanced;
        public event EventHandler<EscapeEventArgs>? GameOver;
        #endregion

        #region Constructor
        public EscapeGameModel(IEscapeDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _table = new EscapeTable(_MediumField);
            _difficulty = Difficulty.Medium;
        }
        #endregion

        #region Public game-methods
        public void NewGame()
        {
            Enemies = (4, 5);
            IsGameOver = false;
            switch (Difficulty)
            {
                case Difficulty.Easy:
                    _table = new EscapeTable(_EasyField);
                    GenerateFields(_EasyField);
                    break;
                case Difficulty.Medium:
                    _table = new EscapeTable(_MediumField);
                    GenerateFields(_MediumField);
                    break;
                case Difficulty.Hard:
                    _table = new EscapeTable(_HardField);
                    GenerateFields(_HardField);
                    break;
            }
            _gameTime = 0;
            Console.WriteLine("Time reset");
        }
        public void AdvanceTime()
        {
            if (IsGameOver)
                return;
            _gameTime++;
            OnGameAdvanced();
        }
        public void RefreshTable()
        {
            for (int i = 0; i < _table.Size; i++)
            {
                for (int j = 0; j < _table.Size; j++)
                {
                    OnFieldChanged(i, j);
                }
            }
        }
        public void Step(int x, int y, int player, string dir)
        {
            //3 - játékos, 4, 5 üldöző
            if (IsGameOver)
                return;

            int StepResult = _table.SetValue(x, y, player, dir);
            if (StepResult == 2)
            {
                IsGameOver = true;
                OnGameOver(false);
            }
            if (StepResult == 3)
            {
                if (player == 4)
                {
                    _table.SetValue(x, y, 0, "start");
                    OnFieldChanged(x, y);
                    Enemies.Item1 = 0;
                }
                else if (player == 5)
                {
                    _table.SetValue(x, y, 0, "start");
                    OnFieldChanged(x, y);
                    Enemies.Item2 = 0;
                }
                if (Enemies == (0, 0))
                {
                    IsGameOver = true;
                    OnGameOver(true);
                }
            }
            if (StepResult == 0)
            {
                OnFieldChanged(x, y);
                switch (dir)
                {
                    case "start":
                        OnFieldChanged(x, y);
                        break;
                    case "up":
                        OnFieldChanged(x, y - 1);
                        break;
                    case "right":
                        OnFieldChanged(x + 1, y);
                        break;
                    case "down":
                        OnFieldChanged(x, y + 1);
                        break;
                    case "left":
                        OnFieldChanged(x - 1, y);
                        break;
                }
            }
        }
        public (int, int) FindEnemy(int player)
        {
            int x = -1;
            int y = -1;
            for (int i = 0; i < _table.Size; i++)
            {
                for (int j = 0; j < _table.Size; j++)
                {
                    if (_table.GetValue(i, j) == player)
                    {
                        x = i;
                        y = j;
                        break;
                    }
                }
            }
            return (x, y);
        }
        public void AliveEnemies()
        {
            (int X4, int Y4) = FindEnemy(4);
            (int X5, int Y5) = FindEnemy(4);

            if (X4 != -1 && X5 == -1)
                Enemies = (4, 0);
            else if (X4 == -1 && X5 != -1)
                Enemies = (0, 5);
            else if (X4 != -1 && X5 != -1)
                Enemies = (4, 5);
            Enemies = (0, 0);
        }
        public void EnemyStep(int currentX, int currentY, int player, int playerX, int playerY)
        {
            if (IsGameOver)
                return;
            string dir = "";
            if (Math.Abs(currentX - playerX) > Math.Abs(currentY - playerY))
            {
                if (currentX < playerX)
                {
                    dir = "right";
                }
                else
                {
                    dir = "left";
                }
            }
            else
            {
                if (currentY < playerY)
                {
                    dir = "down";
                }
                else
                {
                    dir = "up";
                }
            }

            switch (dir)
            {
                case "":
                    Console.WriteLine("Baj van.");
                    break;
                case "up":
                    Step(currentX, currentY, player, dir);
                    break;
                case "right":
                    Step(currentX, currentY, player, dir);
                    break;
                case "down":
                    Step(currentX, currentY, player, dir);
                    break;
                case "left":
                    Step(currentX, currentY, player, dir);
                    break;
            }

        }

        public async Task LoadGameAsync(string path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("Invalid data access.");

            _table = await _dataAccess.LoadAsync(path);
            _gameTime = 0;
        }

        public async Task SaveGameAsync(string path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("Invalid data access.");

            await _dataAccess.SaveAsync(path, _table);
        }

        #endregion

        #region Private game methods
        private void GenerateFields(int _tableSize)
        {

            _table.SetValue(_tableSize / 2, 0, 3, "start");
            _table.SetValue(0, _tableSize - 1, 4, "start");
            _table.SetValue(_tableSize - 1, _tableSize - 1, 5, "start");

            //aknák elhelyezése
            Random random = new Random();

            for (int i = 0; i < _tableSize; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(_tableSize);
                    y = random.Next(_tableSize);
                }
                while (!_table.IsEmpty(x, y));

                _table.SetValue(x, y, 2, "start");

            }

        }
        #endregion

        #region Private event methods
        private void OnFieldChanged(int x, int y)
        {
            Console.WriteLine($"Field changed at ({x}, {y})");
            FieldChanged?.Invoke(this, new EscapeFieldEventArgs(x, y));
        }

        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new EscapeEventArgs(_gameTime, false));
        }
        private void OnGameOver(bool isWon)
        {
            GameOver?.Invoke(this, new EscapeEventArgs(_gameTime, isWon));
        }

        #endregion
    }
}
