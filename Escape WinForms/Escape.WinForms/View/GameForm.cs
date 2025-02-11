using Escape.Model;
using Escape.Persistence;

namespace Escape.View
{
    public partial class GameForm : Form
    {
        #region Fields

        private EscapeGameModel _model = null!;
        private Button[,] _buttonGrid = null!;
        private System.Windows.Forms.Timer _timer = null!;

        #endregion

        #region Constructors
        public GameForm()
        {
            InitializeComponent();

            IEscapeDataAccess _dataAccess = new EscapeFileDataAccess();

            _model = new EscapeGameModel(_dataAccess);
            _model.FieldChanged += new EventHandler<EscapeFieldEventArgs>(Game_FieldChanged);
            _model.GameAdvanced += new EventHandler<EscapeEventArgs>(Game_GameAdvanced);
            _model.GameOver += new EventHandler<EscapeEventArgs>(Game_GameOver);

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += new EventHandler(Timer_Tick);

            GenerateTable();
            SetupMenus();

            _model.NewGame();


            _model.RefreshTable();


            _timer.Start();
        }
        #endregion

        #region Game event handlers
        private void Game_FieldChanged(object? sender, EscapeFieldEventArgs e)
        {

            if (_model.Table.IsEmpty(e.X, e.Y))
            {
                _buttonGrid[e.X, e.Y].Text = string.Empty;
                _buttonGrid[e.X, e.Y].BackColor = Color.Gray;
            }


            else if (_model.Table[e.X, e.Y] == 2)
                _buttonGrid[e.X, e.Y].BackColor = Color.Black;
            else if (_model.Table[e.X, e.Y] == 3)
                _buttonGrid[e.X, e.Y].BackColor = Color.YellowGreen;
            else if (_model.Table[e.X, e.Y] == 4)
                _buttonGrid[e.X, e.Y].BackColor = Color.Red;
            else if (_model.Table[e.X, e.Y] == 5)
                _buttonGrid[e.X, e.Y].BackColor = Color.Cyan;

        }
        private void Game_GameAdvanced(object? sender, EscapeEventArgs e)
        {
            _toolLabelGameTime.Text = TimeSpan.FromSeconds(e.GameTime).ToString("g");
        }

        private void Game_GameOver(object? sender, EscapeEventArgs e)
        {

            _timer.Stop();

            foreach (Button button in _buttonGrid)
                button.Enabled = false;
            _menuSaveGame.Enabled = false;

            if (e.isWon)
            {
                MessageBox.Show("You Win!" + Environment.NewLine + "Game Time: " + TimeSpan.FromSeconds(e.GameTime).ToString("g"),
                                                                   "Escape Game", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("You Lose!" + Environment.NewLine + "Game Time: " + TimeSpan.FromSeconds(e.GameTime).ToString("g"),
                                                                    "Escape Game", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        #endregion

        #region Grid event handlers
        private void ButtonGrid_ArrowClick(object? sender, KeyEventArgs e)
        {
            string dir = "";
            int x = 0;
            int y = 0;

            for (int i = 0; i < _model.Table.Size; i++)
            {
                for (int j = 0; j < _model.Table.Size; j++)
                {
                    if (_model.Table[i, j] == 3)
                    {
                        x = i;
                        y = j;
                        break;
                    }
                }
            }

            switch (e.KeyCode)
            {
                case Keys.W:
                    dir = "up";
                    break;
                case Keys.D:
                    dir = "right";
                    break;
                case Keys.S:
                    dir = "down";
                    break;
                case Keys.A:
                    dir = "left";
                    break;
                default:
                    return;
            }
            Console.WriteLine(dir);
            _model.Step(x, y, 3, dir);
            (int enemy4X, int enemy4Y) = _model.FindEnemy(4);
            if (enemy4X != -1 || enemy4Y != -1)
                _model.EnemyStep(enemy4X, enemy4Y, 4, x, y);
            (int enemy5X, int enemy5Y) = _model.FindEnemy(5);
            if (enemy5X != -1 || enemy5Y != -1)
                _model.EnemyStep(enemy5X, enemy5Y, 5, x, y);
            _buttonGrid[x, y].Update();
        }
        #endregion

        #region Menu event handlers

        private void MenuNewGame_Click(object? sender, EventArgs e)
        {
            _menuSaveGame.Enabled = true;
            _timer.Stop();
            SetupMenus();
            _model.NewGame();
            GenerateTable();

            _model.RefreshTable();
            Console.WriteLine("New game started");
            Console.WriteLine($"Table size: {_model.Table.Size}");
            _timer.Start();
            Console.WriteLine("Timer started");
        }

        private async void MenuLoadGame_Click(object? sender, EventArgs e)
        {
            bool restartTimer = _timer.Enabled;
            _timer.Stop();

            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _model.LoadGameAsync(_openFileDialog.FileName);
                    _menuSaveGame.Enabled = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to load game!" + Environment.NewLine + "Incorrect path or file format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    _model.NewGame();
                    _menuSaveGame.Enabled = true;
                }
                GenerateTable();

                _model.RefreshTable();
                _model.AliveEnemies();

            }

            if (restartTimer)
                _timer.Start();
        }
        private async void MenuSaveGame_Click(object? sender, EventArgs e)
        {
            bool restartTimer = _timer.Enabled;
            _timer.Stop();

            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _model.SaveGameAsync(_saveFileDialog.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to save game!" + Environment.NewLine + "Incorrect path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (restartTimer)
                _timer.Start();
        }

        private void MenuExit_Click(object? sender, EventArgs e)
        {
            bool restartTimer = _timer.Enabled;
            _timer.Stop();
            if (MessageBox.Show("Are you sure you want to exit?", "Escape Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
            else
            {
                if (restartTimer)
                    _timer.Start();
            }
        }
        private void SettingsGameEasy_Click(object? sender, EventArgs e)
        {
            _settingsGameEasy.Checked = true;
            _settingsGameNormal.Checked = false;
            _settingsGameEasy.Checked = false;
            _model.Difficulty = Difficulty.Easy;
        }
        private void SettingsGameMedium_Click(object? sender, EventArgs e)
        {
            _settingsGameEasy.Checked = false;
            _settingsGameNormal.Checked = true;
            _settingsGameEasy.Checked = false;
            _model.Difficulty = Difficulty.Medium;
        }
        private void SettingsGameHard_Click(object? sender, EventArgs e)
        {
            _settingsGameEasy.Checked = false;
            _settingsGameNormal.Checked = false;
            _settingsGameEasy.Checked = true;
            _model.Difficulty = Difficulty.Hard;
        }
        private void Pause_Click(object? sender, EventArgs e)
        {

            if (_model.IsPaused)
            {
                _timer.Start();
                foreach (Button button in _buttonGrid)
                {
                    button.Enabled = true;
                }
                _model.IsPaused = false;
            }
            else
            {
                _timer.Stop();
                foreach (Button button in _buttonGrid)
                {
                    button.Enabled = false;
                }
                _model.IsPaused = true;
            }
        }
        #endregion

        #region Timer event handler

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _model.AdvanceTime();
        }

        #endregion

        #region Private methods

        private void GenerateTable()
        {
            if (_buttonGrid != null)
            {
                foreach (Button button in _buttonGrid)
                {
                    Controls.Remove(button);
                }
            }
            int size = _model.Table.Size;
            _buttonGrid = new Button[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Location = new Point(5 + 45 * i, 35 + 45 * j);
                    _buttonGrid[i, j].Text = string.Empty;
                    _buttonGrid[i, j].Size = new Size(45, 45);
                    _buttonGrid[i, j].Font = new Font(FontFamily.GenericSerif, 20, FontStyle.Bold);
                    _buttonGrid[i, j].Enabled = true;
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat;
                    _buttonGrid[i, j].KeyDown += new KeyEventHandler(ButtonGrid_ArrowClick);

                    Controls.Add(_buttonGrid[i, j]);
                }
            }
            Console.WriteLine("Table generated.");
        }

        private void SetupMenus()
        {
            _settingsGameEasy.Checked = (_model.Difficulty == Difficulty.Easy);
            _settingsGameNormal.Checked = (_model.Difficulty == Difficulty.Medium);
            _settingsGameHard.Checked = (_model.Difficulty == Difficulty.Hard);
        }
        #endregion
    }
}

