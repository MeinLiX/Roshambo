using System;
using System.Collections.Generic;
using Roshambo.src;

namespace Roshambo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Engine(new List<string>(args)).Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
