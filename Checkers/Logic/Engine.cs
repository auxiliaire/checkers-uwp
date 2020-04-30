using Checkers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Checkers.Logic
{
    delegate T CellDelegate<T>(T acc, Cell cell, Grid board);

    static class Engine
    {
        const string MSG = "{filled} button{pm} remained.";
        const string GOAL = "\nTry to achieve three or less to earn stars!";
        const string _NOT_BAD = "\nNot bad!";
        const string _WELL_DONE = "\nWell done!";
        const string _PERFECT = "\nPerfect!";

        public const string PLAN = @"..ooo    -> THESE COMMENTS ARE IGNORED (BEWARE OF LOWERCASE O)
                                     ..ooo    <- DOT MEANS SKIP A COLUMN
                                     ooooooo  <- O MEANS ADD A FILLED CELL
                                     ooo_ooo  <- UNDERSCORE MEANS ADD AN EMPTY CELL
                                     ooooooo
                                     ..ooo
                                     ..ooo";

        public static IEnumerable<Plan.Component> Parse(string plan)
        {
            Plan.ComponentAccumulate planComponentAccumulate = new Plan.ComponentAccumulate((Row: 0, Col: -1), new List<Plan.Component>());
            return plan.Select((c, i) => new Plan.Item(c, i)).Aggregate(
                    planComponentAccumulate,
                    Plan.ComponentAccumulate.PlanComponentAccumulator
                ).Components.Select(Identity).Where(IsAppendable);
        }

        public static Plan.Component Identity(Plan.Component c)
        {
            return c;
        }

        public static bool IsAppendable(Plan.Component c)
        {
            return c.Pos.Col != -1 && c.Command != '.';
        }

        public static bool IsInRange(Cell source, Cell target) {
            return (source.Position.Row == target.Position.Row && Math.Abs(source.Position.Col - target.Position.Col) == 2)
                || (source.Position.Col == target.Position.Col && Math.Abs(source.Position.Row - target.Position.Row) == 2);
        }

        public static Position MiddlePosition(Position source, Position target)
        {
            return new Position(MiddleRowOf(source, target), MiddleColOf(source, target));
        }

        public static int MiddleRowOf(Position source, Position target) {
            return MiddleOf(source.Row, target.Row);
        }

        public static int MiddleColOf(Position source, Position target) {
            return MiddleOf(source.Col, target.Col);
        }

        public static int MiddleOf(int source, int target) {
            return (source + target) / 2;
        }

        public static T Fold<T>(T acc, CellDelegate<T> fn, Grid board) {
            T a = acc;
            foreach(UIElement element in board.Children) {
                if(element is Cell)
                {
                    a = fn(a, (Cell)element, board);
                }
            }
            return a;
        }

        public static string GetMessage(int filled)
        {
            string pm = "";
            if (filled > 1)
            {
                pm = "s";
            }
            return MSG.Replace("{filled}", filled.ToString()).Replace("{pm}", pm) + Comments(filled, GOAL);
        }

        public static string Comments(int leftFilled, string defaultMessage = "")
        {
            switch (leftFilled)
            {
                case 3:
                    return _NOT_BAD;
                case 2:
                    return _WELL_DONE;
                case 1:
                    return _PERFECT;
                default:
                    return defaultMessage;
            }
        }

    }
}
