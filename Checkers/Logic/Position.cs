using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Logic
{
    public readonly struct Position : IEquatable<Position>
    {
        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Row { get; }
        public int Col { get; }

        public override string ToString() => $"({Row}, {Col})";

        public override bool Equals(object obj)
        {
            if (obj is Position other)
            {
                return this.Equals(other);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(Position p1, Position p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return !p1.Equals(p2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Row;
                hash = hash * 23 + Col;
                return hash;
            }
        }

        public bool Equals(Position other)
        {
            return other.Row.Equals(Row) && other.Col.Equals(Col);
        }
    }
}
