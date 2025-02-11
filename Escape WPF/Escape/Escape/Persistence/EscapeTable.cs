namespace Escape.Persistence
{
    public class EscapeTable
    {
        #region Fields

        private int _tableSize;
        private int[,] _fieldValues;

        #endregion

        #region Properties

        //fel van e töltve a pálya?
        //(
        //  0 - üres
        //  2 - bomba
        //  3 - játékos
        //  4 - üldöző1
        //  5 - üüldöző2
        //)

        public int Size { get { return _fieldValues.GetLength(0); } }

        public int this[int x, int y] { get { return GetValue(x, y); } }
        #endregion

        #region Constructors
        public EscapeTable(int size)
        {
            _tableSize = size;
            _fieldValues = new int[size, size];

        }
        #endregion

        #region Public methods
        public bool IsEmpty(int x, int y)
        {
            if (x < 0 || x >= _tableSize)
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of the boundaries");
            if (y < 0 || y >= _tableSize)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of the boundaries");

            return _fieldValues[x, y] == 0;
        }


        public int GetValue(int x, int y)
        {
            if (x < 0 || x >= _tableSize)
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of the boundaries");
            if (y < 0 || y >= _tableSize)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of the boundaries");

            return _fieldValues[x, y];
        }

        public int SetValue(int x, int y, int value, string dir)
        {
            if (x < 0 || x >= _tableSize)
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of the boundaries");
            if (y < 0 || y >= _tableSize)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of the boundaries");
            if (!CheckStep(x, y, dir))
                return 1;

            if (value == 3 || value == 4 || value == 5)
            {
                foreach (int val in _fieldValues)
                {
                    if (val == value)
                        _fieldValues[x, y] = 0;
                }
            }

            switch (dir)
            {
                case "start":
                    _fieldValues[x, y] = value;
                    break;
                case "up":
                    if ((_fieldValues[x, y - 1] == 2 || _fieldValues[x, y - 1] == 4 || _fieldValues[x, y - 1] == 5) && value == 3)
                        return 2;
                    else if (_fieldValues[x, y - 1] == 2 && (value == 4 || value == 5))
                        return 3;
                    _fieldValues[x, y - 1] = value;
                    break;
                case "right":
                    if ((_fieldValues[x + 1, y] == 2 || _fieldValues[x + 1, y] == 4 || _fieldValues[x + 1, y] == 5) && value == 3)
                        return 2;
                    else if (_fieldValues[x + 1, y] == 2 && (value == 4 || value == 5))
                        return 3;
                    _fieldValues[x + 1, y] = value;
                    break;
                case "down":
                    if ((_fieldValues[x, y + 1] == 2 || _fieldValues[x, y + 1] == 4 || _fieldValues[x, y + 1] == 5) && value == 3)
                        return 2;
                    else if (_fieldValues[x, y + 1] == 2 && (value == 4 || value == 5))
                        return 3;
                    _fieldValues[x, y + 1] = value;
                    break;
                case "left":
                    if ((_fieldValues[x - 1, y] == 2 || _fieldValues[x - 1, y] == 4 || _fieldValues[x - 1, y] == 5) && value == 3)
                        return 2;
                    else if (_fieldValues[x - 1, y] == 2 && (value == 4 || value == 5))
                        return 3;
                    _fieldValues[x - 1, y] = value;
                    break;
            }

            return 0;
        }

        #endregion

        #region Private methods
        private bool CheckStep(int x, int y, string dir)
        {
            switch (dir)
            {
                case "up":
                    if (y - 1 < 0)
                        return false;
                    else
                        return true;
                case "right":
                    if (x + 1 >= _tableSize)
                        return false;
                    else
                        return true;
                case "down":
                    if (y + 1 >= _tableSize)
                        return false;
                    else
                        return true;
                case "left":
                    if (x - 1 < 0)
                        return false;
                    else
                        return true;
                case "start":
                    return true;
                default:
                    return false;
            }
        }
        #endregion
    }
}
