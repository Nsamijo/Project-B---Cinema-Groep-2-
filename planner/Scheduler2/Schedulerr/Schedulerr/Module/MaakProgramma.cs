using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Schedulerr
{
    public class MaakProgramma
    {
        public void Run(Planning planning)
        {
            string datum = "";
            string tijd = "";
            string filmid = "-1";

            Console.Clear();
            Console.WriteLine("Schrijf datum in syntax: DD/MM/YYYY");
            datum = Console.ReadLine();

            while (new Checker().DatumSyntax(datum) == false)
            {
                Console.Clear();
                Console.WriteLine("Probeer het opnieuw");
                Console.WriteLine("Schrijf datum in syntax: DD/MM/YYYY");
                datum = Console.ReadLine();
            }

            Console.Clear();
            planning.catalogus.PrintFilms();
            Console.WriteLine("Schrijf het Id van de film");
            filmid = Console.ReadLine();

            while (planning.catalogus.VindFilmdDoorId(filmid) ==  null)
            {
                Console.Clear();
                planning.catalogus.PrintFilms();
                Console.WriteLine("Probeer het opnieuw\n");
                Console.WriteLine("Schrijf het Id van de film");
                filmid = Console.ReadLine();
            }

            Console.Clear();
            Console.WriteLine("Schrijf starttijd in syntax: hh:mm");
            tijd = Console.ReadLine();

            while (new Checker().TijdSyntax(tijd) == false)
            {
                Console.Clear();
                    Console.WriteLine("Probeer het opnieuw");
                Console.WriteLine("Schrijf starttijd in syntax: hh:mm");
                tijd = Console.ReadLine();
            }



            Console.Clear();
            planning.ProgrammaToevoegen(datum, tijd,filmid);
            Console.WriteLine("Programma toegevoegd, druk op enter om door te gaan");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                Thread.Sleep(1);
            }
        }
    }
}
