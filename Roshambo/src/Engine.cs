using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Roshambo.src.Supp;

namespace Roshambo.src
{
    class Engine
    {
        private List<string> Args { get; set; }

        public Engine(List<string> args)
        {
            if (!ValidArgs(args))
            {
                throw new Exception(GetMessage(MessageEnum.UncorrectArgs, MessageEnum.ExampleArgs));
            }

            Args = args;
        }

        private static bool ValidArgs(List<string> args)
        {
            if (args.Count % 2 == 0 || args.Count < 3)
            {
                return false;
            }
            else if (args.Distinct().Count() != args.Count)
            {
                return false;
            }

            return true;
        }

        private bool CalculateWinner(int idxFirst, int idxSecond) => (idxFirst + (Args.Count / 2)) % Args.Count > idxSecond;

        private void CalculateWinnerAndPrint(int idxFirst, int idxSecond)
        {
            if (idxFirst == idxSecond)
            {
                Console.WriteLine("Draw!");
            }
            else if (CalculateWinner(idxFirst, idxSecond))
            {
                Console.WriteLine("You win!");
            }
            else
            {
                Console.WriteLine("You lost!");
            }
        }

        private static string ConvertToString(byte[] bytes)
        {
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }


        private void PrintMoves()
        {
            Console.WriteLine("Available moves:");
            foreach (var item in Args.Select((v, i) => (v, i)))
            {
                Console.WriteLine($"{item.i + 1} - {item.v}.");
            }
            Console.WriteLine("\n0 - exit.");
        }

        private static string GenerateKey()
        {
            byte[] bytes = new byte[16];
            using RNGCryptoServiceProvider rng = new();
            rng.GetBytes(bytes);

            return ConvertToString(bytes);
        }

        private static string ComputeHash(string Key, string value)
        {
            using HMACSHA256 hmac = new(Encoding.UTF8.GetBytes(Key));
            byte[] bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));

            return ConvertToString(bytes);
        }

        public Task Run()
        {
            try
            {
                string HmacKey = GenerateKey();

                int NumberPC = new Random(/*HmacKey*/).Next(Args.Count);

                Console.WriteLine($"HMAC {ComputeHash(HmacKey, Args[NumberPC])}");

                PrintMoves();

                Console.Write("\nEnter number move: ");
                if (!int.TryParse(Console.ReadLine(), out int InputNumer) &&
                    InputNumer < 0 || InputNumer > Args.Count)
                {
                    throw new Exception(GetMessage(MessageEnum.UncorrectMove));
                }

                if (InputNumer == 0)
                {
                    Console.WriteLine("Your quit");
                    return Task.CompletedTask;
                }

                --InputNumer;

                Console.WriteLine($"Your move: {Args[InputNumer]}");
                Console.WriteLine($"Computer move: {Args[NumberPC]}");

                CalculateWinnerAndPrint(InputNumer, NumberPC);

                Console.WriteLine($"HMAC key: {HmacKey}");
            }
            catch
            {
                throw;
            }

            return Task.CompletedTask;
        }
    }
}
