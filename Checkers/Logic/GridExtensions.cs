using Checkers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Checkers.Logic
{
    public delegate void GameOverHandler(int countFilled);
    public delegate bool CellPredicate(Cell cell, Grid board);

    public static class GridExtensions
    {
        public static void CheckGameOver(this Grid board, GameOverHandler onGameOver)
        {
            if (board.IsOver())
            {
                onGameOver(board.GetCountFilled());
            }
        }

        public static bool IsOver(this Grid board)
        {
            return !board.Any((Cell source, Grid _) => {
                return source.Filled && board.CanMove(source);
            });
        }

        public static bool CanMove(this Grid board, Cell cell)
        {
            return board.Any((Cell dest, Grid _) => {
                return board.IsValidMove(cell, dest);
            });
        }

        public static bool Any(this Grid board, Func<Cell, Grid, bool> predicate)
        {
            foreach (UIElement cell in board.Children)
            {
                if (predicate((Cell)cell, board)) return true;
            }
            return false;
        }

        public static bool IsValidMove(this Grid board, Cell source, Cell target)
        {
            return !target.Filled && Engine.IsInRange(source, target) && board.GetMiddleCell(source, target).Filled;
        }

        public static Cell GetMiddleCell(this Grid board, Cell source, Cell target)
        {
            return board.GetCellAt(Engine.MiddlePosition(source.Position, target.Position));
        }

        public static Cell GetCellAt(this Grid board, Position position)
        {
            return board.Children.OfType<Cell>().Where(cell => cell.Position == position).First();
        }

        public static int GetCountFilled(this Grid board)
        {
            return board.GetCount((Cell cell, Grid brd) => cell.Filled);
        }

        public static int GetCount(this Grid board, CellPredicate predicate)
        {
            return Engine.Fold(0, (acc, cell, brd) => {
                if (predicate(cell, brd))
                {
                    return acc + 1;
                }
                else
                {
                    return acc;
                }
            }, board);
        }


    }
}
