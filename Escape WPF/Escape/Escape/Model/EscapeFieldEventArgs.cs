using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Escape.Model
{
    public class EscapeFieldEventArgs
    {
        private int _changedX;
        private int _changedY;

        public int X { get { return _changedX; } }

        public int Y { get { return _changedY; } }

        public EscapeFieldEventArgs(int x, int y)
        {
            _changedX = x;
            _changedY = y;
        }
    }
}
