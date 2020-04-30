using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Logic.Plan
{
    public struct Item : IEquatable<Item>
    {
        public char Command { get; }
        public int Index { get; }

        public Item(char command, int index)
        {
            Command = command;
            Index = index;
        }

        public override bool Equals(object obj)
        {
            if (obj is Item other)
            {
                return this.Equals(other);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Command;
                hash = hash * 23 + Index;
                return hash;
            }
        }

        public bool Equals(Item other)
        {
            return (other.Command == this.Command) && (other.Index == this.Index);
        }

        public static bool operator ==(Item left, Item right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Item left, Item right)
        {
            return !(left == right);
        }
    }

}
