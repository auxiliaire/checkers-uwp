using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Logic
{
    public readonly struct Move
    {
        public Position Source { get; }
        public Position Target { get; }

        public Move(Position source, Position target)
        {
            Source = source;
            Target = target;
        }
    }
}
