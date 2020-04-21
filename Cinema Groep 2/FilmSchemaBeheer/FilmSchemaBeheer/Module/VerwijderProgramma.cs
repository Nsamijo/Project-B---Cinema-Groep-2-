using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FilmSchemaBeheer
{
    public class VerwijderProgramma
    {
        public void Run(Planning planning)
        {
            bool klaar = false;
            while (klaar != true)
            {
                planning.PrintInhoud();
                Console.WriteLine("Welk programma wilt u verwijderen(typ het nummer)");
                string input = Console.ReadLine();
                try
                {
                    planning.VerwijderProgramma(Int32.Parse(input));
                    klaar = true;
                }
                catch
                {
                    Console.WriteLine("Verkeerde input, probeer het opnieuw");
                }
            }
            Console.WriteLine("Programma verwijderd, druk op enter om door te gaan");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                Thread.Sleep(1);
            }
        }
    }
}
