using Core.Games;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Players
{
    public class ConsolePlayer : IPlayer
    {
        public IMove GetMove(IState state)
        {
            var moves = state.GetValidMoves().ToArray();
            
            Console.WriteLine("Board coordinates: ");

            for (int i = 0; i < 8 * 8; i++)
            { 
                Console.Write("{0}{1} ", new string(' ', i <= 9 ? 2 : 1), i);

                if ((i + 1) % 8 == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine();

            for (int i = 0; i < moves.Length; i++)
            {
                Console.Write("{0}. {1} \t\t", i + 1, moves[i]);

                if((i + 1) % 4 == 0)
                {
                    Console.WriteLine();
                }
            }
            if ((moves.Length) % 4 != 0)
            {
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.Write("ConsolePlayer | Enter move number: ");
            var input = Console.ReadLine();

            var match = Regex.Match(input, @"^\d+$"); 
            if (!match.Success || !int.TryParse(match.Value, out int moveNr) || moveNr <= 0 || moveNr > moves.Length)
            {
                Console.WriteLine("Invalid input!");
                Console.WriteLine();
                return GetMove(state);
            }
            
            Console.WriteLine();

            return moves[moveNr - 1];
        }

        public void OpponentMoved(IMove move)
        {
        }

        public override string ToString()
        {
            return $"ConsolePlayer()";
        }
    }
}
