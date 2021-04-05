using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roshambo.src
{
    class Supp
    {
        internal enum MessageEnum
        {
            ExampleArgs = 0,
            UncorrectArgs = 1,
            UncorrectMove = 2,
        }

        internal static readonly Dictionary<int, string> Messages = new()
        {
            [(int)MessageEnum.ExampleArgs] = "Correct args: rock paper scissors.\n",
            [(int)MessageEnum.UncorrectArgs] = "The number of arguments must be odd, without repetitions and more than 3.\n",
            [(int)MessageEnum.UncorrectMove] = "Your move went beyond the prescribed limits.\n"
        };

        internal static string GetMessage(params MessageEnum[] MessageEnums)
        {
            StringBuilder sb = new();
            foreach (var messageEnum in MessageEnums)
            {
                sb.Append($"{Messages.First(o => o.Key == (int)messageEnum).Value}\n");
            }

            return sb.ToString();
        }
    }

}
