using System;
using System.Collections.Generic;
using System.Text;

namespace FilmSchemaBeheer
{
    public class FilmSchemaBeheer
    {
        public void Run()
        {
            //maakt de catalogus aan
            FilmCatalogus catalog = new FilmCatalogus();
            //maakt de Planning aan
            Planning planning = new Planning()
            {
                naam = "planning",
                catalogus = catalog
            };
            bool exit = false;
            //Start de loop
            while (exit != true)
            {
                Console.Clear();
                Console.WriteLine("[A] zie planning\n[B] Maak programma aan\n[C] Verwijder programma\n[E] Verlaat");
                char opdracht = Console.ReadKey().KeyChar;
                //Switch case die de input van de gebruiker leest
                switch (opdracht)
                {
                    case 'a':
                        //laat de planning zien
                        Console.Clear();
                        new ZiePlanning().Run(planning);
                        break;
                    case 'b':
                        //laat de gebruiker een programma aanmaken
                        Console.Clear();
                        new MaakProgramma().Run(planning);
                        break;
                    case 'c':
                        //laat de gebruiker een programma verwijderen
                        Console.Clear();
                        new VerwijderProgramma().Run(planning);
                        break;
                    case 'e':
                        //Verlaat de app, update de data naar de json
                        planning.UpdateNaarJson();
                        exit = true;
                        break;
                }
            }
        }
    }
}
