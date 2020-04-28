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
            //Loop
            bool klaar = false;
            if (planning.Inhoud.Length == 0)
            {
                klaar = true;
                Console.WriteLine("Druk op enter om door te gaan");
            }
            while (klaar != true)
            {
                planning.PrintInhoud();
                Console.WriteLine("Welk programma wilt u verwijderen(typ het nummer)");
                //Vraagt input van de user
                string input = Console.ReadLine();
                /// probeert de input van de user te veranderen naar een integer, 
                ///als het lukt wordt het programma verwijdert en stopt de loop
                ///zo niet wordt de input opnieuw gevraagd
                try
                {
                    planning.VerwijderProgramma(Int32.Parse(input));
                    klaar = true;
                    Console.WriteLine("Programma verwijderd,druk op insert om op te slaan en druk op enter om door te gaan");
                }
                catch
                {
                    Console.WriteLine("Verkeerde input, probeer het opnieuw");
                }
            }
            //Programma slaapt totdat de gebruiker enter heeft ingedrukt
            
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                Thread.Sleep(1);
                if(Console.ReadKey().Key == ConsoleKey.Insert)
                {

                    planning.UpdateNaarJson();
                    Console.WriteLine("Opgeslagen");
                }
            }
        }
    }
}
