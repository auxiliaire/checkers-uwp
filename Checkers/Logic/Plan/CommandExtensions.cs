using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Logic.Plan
{
    public static class CommandExtensions
    {
        /**
         * These are the command characters used in the plan below.
         * Any other characters in the plan, are ignored.
         */
        private const char C_FILLED_CELL = 'o';
        private const char C_EMPTY_CELL = '_';
        private const char C_NEXT_COLUMN = '.';
        private const char C_NEXT_ROW = '\r';
        private static readonly IEnumerable<char> BUILD_COMMANDS = $"{C_FILLED_CELL}{C_EMPTY_CELL}{C_NEXT_COLUMN}{C_NEXT_ROW}";

        public static bool IsValid(this char command)
        {
            return BUILD_COMMANDS.Contains(command);
        }

        public static bool IsNextRow(this char command)
        {
            return command == C_NEXT_ROW;
        }

        public static (int Row, int Col) PositionFromAccumulate(this char command, ComponentAccumulate acc)
        {
            if (command.IsNextRow())
            {
                return (Row: acc.Pos.Row + 1, Col: -1);
            }
            else
            {
                return (acc.Pos.Row, Col: acc.Pos.Col + 1);
            }
        }
    }
}
