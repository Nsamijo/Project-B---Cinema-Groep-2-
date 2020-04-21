using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FilmSchemaBeheer
{
    class ZiePlanning
    {
        public void Run(Planning planning)
        {
            planning.PrintInhoud();
            Console.WriteLine("Druk op enter om door te gaan");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                Thread.Sleep(1);
            }

        }
    }
}
