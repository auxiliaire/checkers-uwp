using Checkers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Checkers.Logic.Plan
{
    public struct Component : IEquatable<Component>
    {
        public char Command { get; }
        public (int Row, int Col) Pos { get; }

        public Component(char command, (int Row, int Col) pos)
        {
            Command = command;
            Pos = pos;
        }

        public Cell ToCell()
        {
            Cell cell = new Cell
            {
                Filled = Command == 'o'
            };
            Grid.SetRow(cell, Pos.Row);
            Grid.SetColumn(cell, Pos.Col);

            return cell;
        }

        public override bool Equals(object obj)
        {
            if (obj is Component other)
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
                hash = hash * 23 + Pos.Row;
                hash = hash * 23 + Pos.Col;
                return hash;
            }
        }

        public static bool operator ==(Component left, Component right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Component left, Component right)
        {
            return !(left == right);
        }

        public bool Equals(Component other)
        {
            return (other.Command == this.Command) && (other.Pos == this.Pos);
        }

    }

}
