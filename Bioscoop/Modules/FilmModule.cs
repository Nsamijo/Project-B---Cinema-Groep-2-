using System;
using System.Collections.Generic;
using Bioscoop.Helpers;
using Bioscoop.Repository;
using Bioscoop.Models;

namespace Bioscoop.Modules
{
    class FilmModule
    {
        List<FilmModel> filmen;

        public void Run() //hoofd functie
        {
            //globale data
            Boolean abort = false;
            String error = "";
            while (!abort)
            {
                Console.Clear();
                if (!String.IsNullOrEmpty(error)) //error handling en text kleur veranderen
                {
                    ConsoleColor originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Helpers.Display.PrintLine("Error: " + error);
                    Helpers.Display.PrintLine("");
                    Console.ForegroundColor = originalColor;
                }

                //menu
                Helpers.Display.PrintHeader("Filmbeheer");
                Helpers.Display.PrintLine("ESC - Terug naar menu                       INS - Nieuwe Film aanmaken");
                Helpers.Display.PrintLine(" ");
                Helpers.Display.PrintLine("Vul een nummer in om deze waarde te bewerken");
                Helpers.Display.PrintLine(" ");
                Helpers.Display.PrintFilmHeader("Nr.", "Naam", "Omschrijving", "Genre", "Duur", "Kijkwijzer", "Status");


                //Ophalen json via Repository/FilmData.cs LoadData functie
                filmen = FilmData.LoadData();
                FilmData.SortData(); //sorteer functie
                int nummering = 1; // nummer naast de waarde op het scherm

                foreach (FilmModel film in filmen) //door alle film data loopen en weergeven
                {
                    Helpers.Display.PrintFilmTable(nummering.ToString(), film.Naam, film.Omschrijving, film.Genre, film.Duur, film.Kijkwijzer, film.Status);
                    nummering++;
                }
                Helpers.Display.PrintLine(" ");
                Helpers.Display.PrintLine("Type je keuze in en sluit af met een enter");

                //userinput functie opvragen in Helpers/Inputs
                Inputs.KeyInput input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                        inputValue--; //-1 want count start bij 0
                        if (inputValue >= 0 && inputValue < filmen.Count)
                        {
                            error = "";
                            EditFilm(filmen[inputValue]); //waarde meegeven van de gekozen film
                        }
                        else
                        {
                            error = "Onjuist waarde ingevuld.";
                        }
                        break;


                    case Inputs.KeyAction.Insert: //toevoeg scherm aanroepen
                        AddFilm();
                        break;

                    case Inputs.KeyAction.Escape: //de functie beeindigen
                        abort = true; 
                        break;
                }
            }
        }

        private void EditFilm(FilmModel film) //aanpas scherm
        {
            //globale data
            Boolean abort = false;
            String error = "";
            Inputs.KeyInput input = null;
            Inputs.KeyInput inputData = null;
            FilmModel editFilm = film;

            while (!abort)
            {
                Console.Clear();
                if (!String.IsNullOrEmpty(error)) //error handling en text kleur veranderen
                {
                    ConsoleColor originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Helpers.Display.PrintLine("Error: " + error);
                    Helpers.Display.PrintLine("");
                    Console.ForegroundColor = originalColor;
                }

                //menu
                Helpers.Display.PrintHeader("Aanpassen film : " + film.Naam);
                Helpers.Display.PrintLine("ESC - Terug naar menu            Del - Verwijderen");
                Helpers.Display.PrintLine("                                 INS - Opslaan"); Display.PrintLine("");
                Helpers.Display.PrintLine("Druk op een nummer om deze waarde aan te passen."); Display.PrintLine("");

                int nr = 0; //data weergeven en nummeren voor waarde keuze
                Helpers.Display.PrintHeader("Nr.");
                Helpers.Display.PrintLine((nr += 1) + "| Naam: " + film.Naam);
                Helpers.Display.PrintLine((nr += 1) + "| Omschrijving: " + film.Omschrijving);
                Helpers.Display.PrintLine((nr += 1) + "| Status: " + film.Genre);
                Helpers.Display.PrintLine((nr += 1) + "| Scherm: " + film.Duur);
                Helpers.Display.PrintLine((nr += 1) + "| Scherm: " + film.Kijkwijzer);
                Helpers.Display.PrintLine((nr += 1) + "| Scherm: " + film.Status);
                Console.WriteLine(" ");


                input = Inputs.ReadUserData(); //waarde oplezen met keyinput functie
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0; //controleren of de waarde een int is
                        inputValue--; //waarde -1 want switch start bij 0
                        if (inputValue >= 0 && inputValue < 6)
                        {
                            switch (inputValue)
                            {
                                case 0: Helpers.Display.PrintLine("Vul de nieuwe naam in:"); break;
                                case 1: Helpers.Display.PrintLine("Vul de nieuwe omschrijving in:"); break;
                                case 2: Helpers.Display.PrintLine("Vul de nieuwe genre in: (Avontuur / Actie / Thriller / Horror / Animatie / Drama / Comedy"); break;
                                case 3: Helpers.Display.PrintLine("Vul de nieuwe film duur in: (00:00)"); break;
                                case 4: Helpers.Display.PrintLine("Vul de nieuwe kijkwijzer in: (6+, 12+, 16+, 18+, AL)"); break;
                                case 5: Helpers.Display.PrintLine("Vul de nieuwe film status in: (Beschikbaar / Niet Beschikbaar"); break;
                            }

                            //switch met de instructie en data opvang van de gekozen data soort. met error handling.
                            inputData = Inputs.ReadUserData();
                            switch (inputValue)
                            {
                                case 0:
                                    if (inputData.val.Length > 1) editFilm.Naam = inputData.val;
                                    else error = "De naam moet uit minimaal 2 karakters bestaan ";
                                    break;
                                case 1:
                                    if (inputData.val.Length > 9) editFilm.Omschrijving = inputData.val;
                                    else error = "De omschrijving moet uit minimaal 10 karakters bestaan";
                                    break;
                                case 2:
                                    if ((inputData.val == "Avontuur") || (inputData.val == "Actie") || (inputData.val == "Thriller") || (inputData.val == "Horror") || (inputData.val == "Animatie") || (inputData.val == "Drama") || (inputData.val == "Sci-Fi") || (inputData.val == "Comedy"))
                                    { editFilm.Genre = inputData.val; }
                                    else error = "Onjuiste waarde ingevuld.";
                                    break;
                                case 3:
                                    if (DateTime.TryParseExact(input.val, "HH:mm", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out DateTime dt))
                                    { editFilm.Duur = dt.ToString("HH:mm"); }
                                    else error = "Onjuiste waarde ingevuld.";
                                    break;
                                case 4:
                                    if ((inputData.val == "6+") || (inputData.val == "9+") || (inputData.val == "12+") || (inputData.val == "16+") || (inputData.val == "18+") || (inputData.val == "AL"))
                                    { editFilm.Kijkwijzer = inputData.val; }
                                    else error = "Onjuiste waarde ingevuld.";
                                    break;
                                case 5:
                                    if (inputData.val == "Beschikbaar" || inputData.val == "Niet Beschikbaar") editFilm.Status = inputData.val;
                                    else error = "Onjuiste waarde ingevuld.";
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case Inputs.KeyAction.Escape: //de functie beeindigen
                        abort = true;
                        break;
                    case Inputs.KeyAction.Insert: //de nieuwe waardes opslaan met andere kleur
                        FilmData.EditData(editFilm);
                        ConsoleColor originalColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("De gegevens zijn opgeslagen.");
                        Console.ForegroundColor = originalColor;
                        System.Threading.Thread.Sleep(2000);
                        abort = true;
                        break;
                    case Inputs.KeyAction.Delete: //de specifieke film data verwijderen
                        Console.WriteLine("Weet je zeker dat je " + film.Naam + " wilt verwijderen? (y/n)");
                        string temp = Console.ReadLine();
                        if( temp == "y")
                        {
                            FilmData.RemoveData(film);
                            ConsoleColor ogColor = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(film.Naam + " is verwijderd.");
                            Console.ForegroundColor = ogColor;
                            System.Threading.Thread.Sleep(2000);
                            abort = true;
                        }
                        else EditFilm(film);
                        break;
                }
            }
        }

        private void AddFilm() //toevoeg scherm
        {
            //globale data
            Boolean abort = false;
            bool check = false;
            String error = "";
            Inputs.KeyInput input = null;
            FilmModel newFilm = new FilmModel ();

            int x = 0;
            while (!abort && x < 7)
            {
                Console.Clear();
                if (!String.IsNullOrEmpty(error)) //error handling en text kleur veranderen
                {
                    ConsoleColor originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Helpers.Display.PrintLine("Error: " + error);
                    Helpers.Display.PrintLine("");
                    Console.ForegroundColor = originalColor;
                }

                //menu
                Helpers.Display.PrintHeader("Film toevoegen");
                Helpers.Display.PrintLine("ESC - Terug naar menu                       Del - Reset");
                if (!String.IsNullOrEmpty(newFilm.Status)) Helpers.Display.PrintLine("                                            INS - Opslaan Nieuwe Film");
                Helpers.Display.PrintLine(" ");
                if (!String.IsNullOrEmpty(newFilm.Naam)) Helpers.Display.PrintLine("Naam: " + newFilm.Naam);
                if (!String.IsNullOrEmpty(newFilm.Omschrijving)) Helpers.Display.PrintLine("Omschrijving: " + newFilm.Omschrijving);
                if (!String.IsNullOrEmpty(newFilm.Genre)) Helpers.Display.PrintLine("Genre: " + newFilm.Genre);
                if (!String.IsNullOrEmpty(newFilm.Duur)) Helpers.Display.PrintLine("Duur: " + newFilm.Duur);
                if (!String.IsNullOrEmpty(newFilm.Kijkwijzer)) Helpers.Display.PrintLine("Status: " + newFilm.Kijkwijzer);
                if (!String.IsNullOrEmpty(newFilm.Status)) Helpers.Display.PrintLine("Scherm: " + newFilm.Status);
                Helpers.Display.PrintLine("");

                switch (x) //switch case die vanaf 0 begint. elke waarde moet ingevuld worden.
                {
                    case 0: Helpers.Display.PrintLine("Vul de film naam in:"); break;
                    case 1: Helpers.Display.PrintLine("Vul de film omschrijving in:"); break;
                    case 2: Helpers.Display.PrintLine("Vul de film genre in: (Avontuur / Actie / Thriller / Horror / Animatie / Drama / Comedy"); break;
                    case 3: Helpers.Display.PrintLine("Vul de film duur in: (00:00)"); break;
                    case 4: Helpers.Display.PrintLine("Vul de film kijkwijzer in: (6+, 12+, 16+, 18+, AL)"); break;
                    case 5: Helpers.Display.PrintLine("Vul de film status in: (Beschikbaar / Niet Beschikbaar"); break;
                    case 6: Helpers.Display.PrintLine("druk op Insert om de gegevens op te slaan"); break;
                }

                input = Inputs.ReadUserData(); //waarde oplezen met keyinput functie
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        switch (x)
                        {
                            case 0:
                                if (input.val.Length > 1) { newFilm.Naam = input.val; x++; error = ""; }
                                else error = "De naam moet uit minimaal 2 karakters bestaan ";
                                break;
                            case 1:
                                if (input.val.Length > 9) { newFilm.Omschrijving = input.val; x++; error = ""; }
                                else error = "De omschrijving moet uit minimaal 10 karakters bestaan";
                                break;
                            case 2:
                                if ((input.val == "Avontuur") || (input.val == "Actie") || (input.val == "Thriller") || (input.val == "Horror") || (input.val == "Animatie") || (input.val == "Drama") || (input.val == "Sci-Fi") || (input.val == "Comedy"))
                                { newFilm.Genre = input.val; x++; error = ""; }
                                else error = "Onjuiste waarde ingevuld.";
                                break;
                            case 3:
                                if (DateTime.TryParseExact(input.val, "HH:mm", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out DateTime dt))
                                { newFilm.Duur = dt.ToString("HH:mm"); x++; error = "";}
                                else error = "Onjuiste waarde ingevuld.";
                                break;
                            case 4:
                                if ((input.val == "6+") || (input.val == "9+") || (input.val == "12+") || (input.val == "16+") || (input.val == "18+") || (input.val == "AL"))
                                { newFilm.Kijkwijzer = input.val; x++; error = ""; }
                                else error = "Onjuiste waarde ingevuld.";
                                break;
                            case 5:
                                if (input.val == "Beschikbaar" || input.val == "Niet Beschikbaar")
                                { newFilm.Status = input.val; x++; error = ""; }
                                else error = "Onjuiste waarde ingevuld.";
                                check = true;
                                break;
                            case 6:
                                Console.WriteLine("druk op Insert om de gegevens op te slaan");
                                break;
                            default:
                                x++;
                                break;
                        }
                        break;
                    case Inputs.KeyAction.Escape:  //de functie beeindigen
                        abort = true;
                        break;
                    case Inputs.KeyAction.Insert:  //De nieuwe data opslaan als de laatste waarde is ingevuld. groene kleur text
                        if (check == true)
                        {
                            FilmData.AddData(newFilm);
                            ConsoleColor originalColor = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("De nieuwe film is opgeslagen.");
                            Console.ForegroundColor = originalColor;
                            System.Threading.Thread.Sleep(2000);
                            FilmData.SortData();
                            abort = true;
                        }
                        break;
                    case Inputs.KeyAction.Delete:  //De functie opnieuw aanroepen en alle inputs resetten
                        abort = true;
                        AddFilm();
                        break;
                }
            }
        }
    }
}