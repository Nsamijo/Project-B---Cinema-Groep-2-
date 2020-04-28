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
            int filmid = -1;
            int zaalid = -1;

            
            Console.Clear();
            Console.WriteLine("Kies een dag");
            //Maakt een array aan met strings in de syntax: "DD/MM/YYYY"
            //van over 2 dagen tot over 14 dagen
            string[] dagarr = new Dagen().VolgendeDagen(2, 14);
            //Print deze dagen
            Console.WriteLine(new Dagen().PrintVolgendeDagen(2,14));
            //Leest de input van de user
            string input = Console.ReadLine();
            //Probeert de input naar een integer om te zetten, 
            //als dit niet lukt gebeurt er niets en gaat hij over naar de loop
            try
            {
                datum = dagarr[Int32.Parse(input)-1];
            } catch
            {
                Console.WriteLine("Probeer opnieuw");
            }
            
            //Als er nog geen correcte datum is gaat hij over naar deze loop
            while (new Checker().DatumSyntax(datum) == false)
            {

                Console.Clear();
                Console.WriteLine("Kies een dag");
                Console.WriteLine(new Dagen().PrintVolgendeDagen(2, 14));
                input = Console.ReadLine();
                
                try
                {
                    datum = dagarr[Int32.Parse(input)-1];
                }
                catch
                {
                    Console.WriteLine("Probeer opnieuw");
                }
                
            }

            //Het kiezen van een zaal
            Console.Clear();
            planning.Zalen.PrintZalen();
            Console.WriteLine("Schrijf het Id van de zaal");
            //leest input van user
            try
            {
                zaalid = Int32.Parse(Console.ReadLine());
            }
            catch
            {
                zaalid = -1;
            }
            //Als de input incorrect is, start de loop
            while (planning.Zalen.VindZaaldDoorId(zaalid) == null)
            {
                Console.Clear();
                planning.Zalen.PrintZalen();
                Console.WriteLine("Probeer het opnieuw\n");
                Console.WriteLine("Schrijf het Id van de zaal");
                try
                {
                    zaalid = Int32.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("opnieuw");
                }
            }

            //invullen van tijd
            Console.Clear();
            string[] tijdenarr = new string[5] {"09:00","12:00","15:00","18:00","21:00" };
            Console.WriteLine("Maak een keuze uit de tijden");
            int i = 1;
            foreach(string t in tijdenarr)
            {
                Console.WriteLine($"{i}.   {t}");
                i++;
            }
            input = Console.ReadLine();
            try
            {
                tijd = tijdenarr[Int32.Parse(input) - 1];
            }
            catch
            {
                Console.WriteLine("Probeer opnieuw");
                tijd = "";
            }
            //Als tijd niet correct is, start de loop
            while (new Checker().TijdSyntax(tijd) == false)
            {
                Console.WriteLine("Maak een keuze uit de tijden");
                i = 1;
                foreach (string t in tijdenarr)
                {
                    Console.WriteLine($"{i}.   {t}");
                    i++;
                }
                input = Console.ReadLine();
                try
                {
                    tijd = tijdenarr[Int32.Parse(input) - 1];
                }
                catch
                {
                    Console.WriteLine("Probeer opnieuw");
                    tijd = "";
                }
            }

            

            //Het kiezen van een film
            Console.Clear();
            planning.Films.PrintFilms();
            Console.WriteLine("Schrijf het Id van de film");
            //leest input van user
            filmid = Int32.Parse(Console.ReadLine());
            //Als de input incorrect is, start de loop
            while (planning.Films.VindFilmdDoorId(filmid) == null)
            {
                Console.Clear();
                planning.Films.PrintFilms();
                Console.WriteLine("Probeer het opnieuw\n");
                Console.WriteLine("Schrijf het Id van de film");
                try
                {
                    filmid = Int32.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("opnieuw");
                }
            }


            Console.Clear();
            planning.ProgrammaToevoegen(datum, tijd,filmid,zaalid);
            Console.WriteLine("Programma toegevoegd, druk op Insert om het op te slaan en op ENTER om door te gaan");
            
            //Het programma blijft wachten totdat de user op enter drukt
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                if(Console.ReadKey().Key == ConsoleKey.Insert)
                {
                    planning.UpdateNaarJson();
                    Console.WriteLine("Planning opgeslagen");
                }
                Thread.Sleep(1);
            }


        }
    }
}
