using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle.CUI
{
    class WrongFormatException : ApplicationException
    {
        public WrongFormatException(string message) : base(message) { }
    }
}
