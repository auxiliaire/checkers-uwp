using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Logic.Plan
{
    public struct ComponentAccumulate : IEquatable<ComponentAccumulate>
    {
        public (int Row, int Col) Pos { get; }
        public List<Component> Components { get; }

        public ComponentAccumulate((int Row, int Col) pos, List<Component> planComponents)
        {
            Pos = pos;
            Components = planComponents;
        }

        public static ComponentAccumulate PlanComponentAccumulator(ComponentAccumulate acc, Item o)
        {
            {
                if (!o.Command.IsValid()) { return acc; }

                (int Row, int Col) pos = o.Command.PositionFromAccumulate(acc);
                return new ComponentAccumulate(
                    pos,
                    acc.Components.Concat(new List<Component> { new Component(o.Command, pos) }).ToList()
                );
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is ComponentAccumulate other)
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
                hash = hash * 23 + Pos.Row;
                hash = hash * 23 + Pos.Col;
                return hash;
            }
        }

        public bool Equals(ComponentAccumulate other)
        {
            return (this.Pos == other.Pos) && (this.Components == other.Components);
        }

        public static bool operator ==(ComponentAccumulate left, ComponentAccumulate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ComponentAccumulate left, ComponentAccumulate right)
        {
            return !(left == right);
        }
    }

}
