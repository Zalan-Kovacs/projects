using Escape.Model;
using Escape.Persistence;
using Escape.WPF.ViewModel;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Escape.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private EscapeGameModel _model = null!;
        private EscapeViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private DispatcherTimer _timer = null!;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása
            _model = new EscapeGameModel(new EscapeFileDataAccess());
            _model.GameOver += new EventHandler<EscapeEventArgs>(Model_GameOver);
            _model.NewGame();
                        _model.RefreshTable();

            // nézemodell létrehozása
            _viewModel = new EscapeViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
            _viewModel.PauseGame += new EventHandler(ViewModel_PauseGame);
            _viewModel.KeyPress += new EventHandler<KeyEventArgs>(ViewModel_KeyPress);

            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();

            // időzítő létrehozása
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _model.AdvanceTime();
        }
        private void View_Closing(object? sender, CancelEventArgs e)
        {
            bool restartTimer = _timer.IsEnabled;

            _timer.Stop();

            if (MessageBox.Show("Are you sure you want to exit?", "Escape", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;

                if (restartTimer)
                {
                    _timer.Start();
                }
            }
        }
        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            _model.NewGame();
            _timer.Start();
            _viewModel.GenerateTable();
            _viewModel.RefreshTable();
        }
        private async void ViewModel_LoadGame(object? sender, System.EventArgs e)
        {
            Boolean restartTimer = _timer.IsEnabled;
            foreach (EscapeField field in _viewModel.GameBoard)
            {
                field.IsEnabled = false;
            }
            _timer.Stop();

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog(); // dialógusablak
                openFileDialog.Title = "Escape table load";
                openFileDialog.Filter = "Escape table|*.escp";
                if (openFileDialog.ShowDialog() == true)
                {
                    await _model.LoadGameAsync(openFileDialog.FileName);

                    _timer.Start();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("The file load is failed!", "Escape", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            _viewModel.GenerateTable();
            _viewModel.RefreshTable();

            _model.RefreshTable();
            _model.AliveEnemies();
            if (restartTimer)
                _timer.Start();
            foreach (EscapeField field in _viewModel.GameBoard)
            {
                field.IsEnabled = true;
            }
        }
        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            bool restartTimer = _timer.IsEnabled;

            _timer.Stop();

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógablak
                saveFileDialog.Title = "Escape table save";
                saveFileDialog.Filter = "Escape table|*.escp";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // játéktábla mentése
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("The save is failed!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("The save is failed!", "Escape", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (restartTimer) // ha szükséges, elindítjuk az időzítőt
                _timer.Start();

        }
        private void ViewModel_ExitGame(object? sender, System.EventArgs e)
        {
            _view.Close();
        }
        private void Model_GameOver(object? sender, EscapeEventArgs e)
        {

            _timer.Stop();


            if (e.isWon)
            {
                MessageBox.Show("You Win!" + Environment.NewLine + "Game Time: " + TimeSpan.FromSeconds(e.GameTime).ToString("g"),
                                                                   "Escape Game", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("You Lose, You died!" + Environment.NewLine + "Game Time: " + TimeSpan.FromSeconds(e.GameTime).ToString("g"),
                                                                    "Escape Game", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }
        private void ViewModel_PauseGame(object? sender, EventArgs e)
        {
            if (_viewModel.IsPaused)
            {
                _viewModel.IsPaused = false;
                _timer.Start();
                foreach (EscapeField field in _viewModel.GameBoard)
                {
                    field.IsEnabled = true;
                }
            }
            else if (!_viewModel.IsPaused)
            {
                _viewModel.IsPaused = true;
                _timer.Stop();
                foreach (EscapeField field in _viewModel.GameBoard)
                {
                    field.IsEnabled = false;
                }
            }

        }
        private void ViewModel_KeyPress(object? sender, KeyEventArgs e) {
            if (_viewModel.IsPaused)
                return;
            string dir = "";
            int x = 0;
            int y = 0;

            for (int i = 0; i<_model.Table.Size; i++)
            {
                for (int j = 0; j<_model.Table.Size; j++)
                {
                    if (_model.Table[i, j] == 3)
                    {
                        x = i;
                        y = j;
                        break;
                    }
                }
            }

            switch (e.Key)
            {
                case Key.W:
                    dir = "up";
                    break;
                case Key.D:
                   dir = "right";
                   break;
                case Key.S:
                    dir = "down";
                    break;
                case Key.A:
                   dir = "left";
                    break;
                default:
                    return;
            }

            _model.Step(x, y, 3, dir);

            (int enemy4X, int enemy4Y) = _model.FindEnemy(4);
            if (enemy4X != -1 || enemy4Y != -1)
                _model.EnemyStep(enemy4X, enemy4Y, 4, x, y);

            (int enemy5X, int enemy5Y) = _model.FindEnemy(5);
            if (enemy5X != -1 || enemy5Y != -1)
                _model.EnemyStep(enemy5X, enemy5Y, 5, x, y);
            _viewModel.RefreshTable();
        }

    }

}
