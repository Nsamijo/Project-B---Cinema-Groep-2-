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
            //Print de plannign
            planning.PrintInhoud();
            Console.WriteLine("Druk op enter om door te gaan");
            //Programma blijft slapen totdat de gebruiker op enter drukt
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                Thread.Sleep(1);
            }

        }
    }
}
