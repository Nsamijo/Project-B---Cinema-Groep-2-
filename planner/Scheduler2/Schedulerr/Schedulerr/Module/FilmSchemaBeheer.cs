using System;
using System.Collections.Generic;
using System.Text;

namespace Schedulerr
{
    public class FilmSchemaBeheer
    {
        public void Run()
        {
            FilmCatalogus catalog = new FilmCatalogus();
            Planning planning = new Planning()
            {
                naam = "planning",
                catalogus = catalog
            };
            bool exit = false;
            while (exit != true)
            {
                Console.Clear();
                Console.WriteLine("[A] zie planning\n[B] Maak programma aan\n[C] Verwijder programma\n[E] Verlaat");
                char opdracht = Console.ReadKey().KeyChar;
                switch (opdracht)
                {
                    case 'a':
                        Console.Clear();
                        new ZiePlanning().Run(planning);
                        break;
                    case 'b':
                        Console.Clear();
                        new MaakProgramma().Run(planning);
                        break;
                    case 'c':
                        Console.Clear();
                        new VerwijderProgramma().Run(planning);
                        break;
                    case 'e':
                        planning.UpdateNaarJson();
                        exit = true;
                        break;
                }
            }
        }
    }
}
