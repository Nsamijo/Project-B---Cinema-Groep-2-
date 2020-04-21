using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FilmSchemaBeheer
{
    public class MaakProgramma
    {
        public void Run(Planning planning)
        {
            string datum = "";
            string tijd = "";
            string filmid = "-1";

            Console.Clear();
            Console.WriteLine("Kies een dag");
            string[] dagarr = new Dagen().VolgendeDagen(2, 14);
            Console.WriteLine(new Dagen().PrintVolgendeDagen(2,14));
            ConsoleKeyInfo input = Console.ReadKey();
            if (input.Key != ConsoleKey.Escape )
            {
                try
                {
                    datum = dagarr[Int32.Parse(input.KeyChar.ToString())];
                } catch
                {
                    Console.WriteLine("Probeer opnieuw");
                }
            }
            else
            {
                return;
            }

            while (new Checker().DatumSyntax(datum) == false)
            {

                Console.Clear();
                Console.WriteLine("Kies een dag");
                Console.WriteLine(new Dagen().PrintVolgendeDagen(2, 14));
                input = Console.ReadKey();
                if (input.Key != ConsoleKey.Escape)
                {
                    try
                    {
                        datum = dagarr[Int32.Parse(input.KeyChar.ToString())];
                    }
                    catch
                    {
                        Console.WriteLine("Probeer opnieuw");
                    }
                }
                else
                {
                    return;
                }
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
                try
                {
                    filmid = Console.ReadKey().KeyChar.ToString();
                } catch
                {
                    Console.WriteLine("opnieuw");
                }
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
