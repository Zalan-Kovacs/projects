namespace Escape.Model
{
    public class EscapeEventArgs : EventArgs
    {
        private int _gameTime;
        private bool _isWon;

        public int GameTime { get { return _gameTime; } }
        public bool isWon { get { return _isWon; } }

        public EscapeEventArgs(int gameTime, bool isWon)
        {
            _gameTime = gameTime;
            _isWon = isWon;
        }
    }
}
  