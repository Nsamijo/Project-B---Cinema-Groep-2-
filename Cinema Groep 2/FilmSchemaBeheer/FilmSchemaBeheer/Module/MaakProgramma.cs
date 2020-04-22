﻿using System;
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
                datum = dagarr[Int32.Parse(input)];
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
                    datum = dagarr[Int32.Parse(input)];
                }
                catch
                {
                    Console.WriteLine("Probeer opnieuw");
                }
                
            }
            //Het kiezen van een film
            Console.Clear();
            planning.catalogus.PrintFilms();
            Console.WriteLine("Schrijf het Id van de film");
            //leest input van user
            filmid = Console.ReadLine();
            //Als de input incorrect is, start de loop
            while (planning.catalogus.VindFilmdDoorId(filmid) ==  null)
            {
                Console.Clear();
                planning.catalogus.PrintFilms();
                Console.WriteLine("Probeer het opnieuw\n");
                Console.WriteLine("Schrijf het Id van de film");
                try
                {
                    filmid = Console.ReadLine();
                } catch
                {
                    Console.WriteLine("opnieuw");
                }
            }

            //invullen van tijd
            Console.Clear();
            Console.WriteLine("Schrijf starttijd in syntax: hh:mm");
            tijd = Console.ReadLine();
            //Als tijd niet correct is, start de loop
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
            //Het programma blijft wachten totdat de user op enter drukt
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                Thread.Sleep(1);
            }
        }
    }
}
