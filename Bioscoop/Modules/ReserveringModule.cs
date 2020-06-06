using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Bioscoop.Helpers;
using Bioscoop.Models;
using Bioscoop.Repository;
using Newtonsoft.Json;

namespace Bioscoop.Modules
{
    class ReserveringModule
    {
        //globals
        List<ZaalModel> zaalData = ZaalData.LoadData();
        List<FilmModel> filmData = FilmData.LoadData();
        List<FilmschemaModel> filmschemaData = FilmschemaData.LoadData();
        List<PrijsModel> prijsData = PrijsData.LoadData();
        List<StoelModel> stoelData = StoelData.LoadData();
        List<ReserveringModel> reserveringData = ReserveringData.LoadData();

        public void SelectFilm()
        {
            Console.CursorVisible = true;
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
                Helpers.Display.PrintHeader("Bioscoop - Filmoverzicht");
                Helpers.Display.PrintLine("ESC - Terug naar menu                       INS - Filmprogramma van vandaag\n");
                Helpers.Display.PrintLine("Vul een nummer in om de informatie en fimschema in te zien van de desbetreffende film\n");
                Helpers.Display.PrintLine("Het huidige film aabod: \n");
                Helpers.Display.PrintTableFilm("Nr", "Naam","Genre", "Kijkwijzer");

                List<FilmModel> filmAanbod = filmData.Where(f => f.Status == "Beschikbaar").ToList();
                int nummering = 1; // nummer naast de waarde op het scherm
                if (filmAanbod.Count() > 0)
                {
                    foreach (FilmModel film in filmAanbod) //door alle film data loopen en weergeven
                    {
                        Helpers.Display.PrintTableFilm(nummering.ToString(), film.Naam, film.Genre, film.Kijkwijzer);
                        nummering++;
                    }
                }
                else
                    Helpers.Display.PrintLine("\n Er zijn geen beschikbare films gevonden");
                Helpers.Display.PrintLine("\n Type je keuze in en sluit af met een enter");

                //userinput functie opvragen in Helpers/Inputs
                Console.Write(">");
                Inputs.KeyInput input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                        inputValue--; //-1 want count start bij 0
                        if (inputValue >= 0 && inputValue < filmAanbod.Count)
                        {
                            error = "";
                            ViewFilm(filmAanbod[inputValue]); //waarde meegeven van de gekozen film
                        }
                        else
                        {
                            error = "Onjuist waarde ingevuld.";
                        }
                        break;

                    case Inputs.KeyAction.Insert: //toevoeg scherm aanroepen
                        //filmprogramma vandaag
                        break;

                    case Inputs.KeyAction.Escape: //de functie beeindigen
                        abort = true;
                        break;
                }
            }
        }

        private void ViewFilm(FilmModel film)
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
                Helpers.Display.PrintHeader("Bioscoop - Filmoverzicht - " + film.Naam);
                Helpers.Display.PrintLine("ESC - Terug naar menu\n");
                Helpers.Display.PrintLine("Film informatie: \n");

                Helpers.Display.PrintHeader("|", "Benaming", "Waarde");
                Helpers.Display.PrintTable("|", "Naam: ", film.Naam);
                int maxLength = 50; int index = 0;
                while (index + maxLength < film.Omschrijving.Length) // bij omschrijvingen lager dan 40 oplossingen verzinnen. pakt deze anders niet
                {
                    if (index == 0) Helpers.Display.PrintTable("|", "Omschrijving: ", film.Omschrijving.Substring(index, maxLength));
                    else Helpers.Display.PrintTable(" ", " ", film.Omschrijving.Substring(index, maxLength));
                    index += maxLength;
                }
                Helpers.Display.PrintTable("|", "Genre: ", film.Genre);
                Helpers.Display.PrintTable("|", "Duur: ", film.Duur);
                Helpers.Display.PrintTable("|", "Kijkwijzer: ", film.Kijkwijzer);

                Console.WriteLine("\n");
                Helpers.Display.PrintLine("Druk op een waarde om naar het reservatie scherm te gaan voor deze dag:");
                Helpers.Display.PrintLine("Filmprogramma voor de komende week: \n");
                Helpers.Display.PrintHeader("Nr.","dag","Datum", "Tijd", "Scherm");

                //data voor loop
                var land = new System.Globalization.CultureInfo("nl-NL");
                string fromdate = DateTime.UtcNow.ToString("dd/MM/yyyy"); DateTime td = DateTime.Parse(fromdate, land);
                List<FilmschemaModel> filmschema = filmschemaData.Where(a => a.FilmId == film.FilmId).ToList();
                List<FilmschemaModel> temp = new List<FilmschemaModel>();
                foreach (FilmschemaModel schema in filmschema)
                {
                    DateTime d = DateTime.Parse(schema.Datum, land);
                    if (d > td)
                        temp.Add(schema);
                }
                int nummering = 1;
                foreach (FilmschemaModel schema in temp)
                {
                    ZaalModel data = zaalData.Where(a => a.ZaalId == schema.ZaalId).SingleOrDefault();
                    DateTime d = DateTime.Parse(schema.Datum, land);
                    Helpers.Display.PrintTable(nummering.ToString(), land.DateTimeFormat.GetDayName(d.DayOfWeek), schema.Datum, schema.Tijd, data.Scherm);
                    nummering++;
                }
                Helpers.Display.PrintLine("\n Vul je keuze in en sluit af met een enter");

                //userinput functie opvragen in Helpers/Inputs
                Console.Write(">");
                Inputs.KeyInput input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                        inputValue--; //-1 want count start bij 0
                        if (inputValue >= 0 && inputValue < temp.Count)
                        {
                            error = "";
                            Reservering(temp[inputValue]); //waarde meegeven van de gekozen film
                        }
                        else
                        {
                            error = "Onjuist waarde ingevuld.";
                        }
                        break;

                    case Inputs.KeyAction.Escape: //de functie beeindigen
                        abort = true;
                        break;
                }
            }
        }

        public void Reservering(FilmschemaModel schemaData)
        {
            //globale data
            Boolean abort = false;
            String error = "";
            FilmModel films = filmData.Where(a => a.FilmId == schemaData.FilmId).SingleOrDefault();
            ZaalModel zaal = zaalData.Where(a => a.ZaalId == schemaData.ZaalId).SingleOrDefault();
            PrijsModel prijzen = prijsData.Where(a => a.Soort == zaal.Scherm).SingleOrDefault(); decimal prijs = decimal.Parse(prijzen.Prijs);
            List<StoelModel> stoelen = stoelData.Where(a => a.ZaalId == schemaData.ZaalId).ToList();
            List<ReserveringModel> reservaties = reserveringData.Where(a => a.ProgrammaId == schemaData.ProgrammaId).ToList();

            ReserveringModel newReservering = new ReserveringModel();
            newReservering.ProgrammaId = schemaData.ProgrammaId;

            //data for input
            bool check = false;
            int aantal = 0;
            decimal totaal = 0;
            string rij = "";
            int persoon = 1;

            int x = 0;
            while (!abort && x < 4)
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

                //Data
                //alle rijen ( A, B, C, D, E, F, G, H, I)
                var rijen = stoelen.Select(r => r.Rij).Distinct().ToList();

                //alle gereserveerde stoelen van de geselecteerde zaal.
                List<int> stoelenList = new List<int>();
                foreach (var r in reservaties)
                {
                    foreach (var s in r.StoelId)
                    {
                        if (stoelenList.IndexOf(s) < 0)
                            stoelenList.Add(s);
                    }
                }

                //aantal plekken vrij per rij
                List<int> rijPlekken = new List<int>();
                foreach (string ar in rijen)
                {
                    List<StoelModel> allePlekken = stoelen.Where(r => r.Rij == ar).ToList(); //alle stoelen per rij
                    List<StoelModel> filterPlekken = allePlekken.Where(d => !stoelenList.Any(d2 => d2 == d.StoelId)).ToList();
                    rijPlekken.Add(filterPlekken.Count());
                }

                //alle plekken over in de zaal?
                int alleZaalPlekken = rijPlekken.Sum();

                //Alle stoelen van de gekozen rij?
                List<StoelModel> rijStoelen = stoelen.Where(r => r.Rij == rij).ToList();

                //alle overige stoelen in de geselecteerde rij
                List<StoelModel> plekken = rijStoelen
                   .Where(p => !stoelenList.Any(p2 => p2 == p.StoelId))
                   .Where( p=> !newReservering.StoelId.Any(p2 => p2 == p.StoelId) )
                   .ToList();

                //Display
                //menu
                Helpers.Display.PrintHeader("Bioscoop - Reserveren");
                Helpers.Display.PrintLine("ESC - Terug naar het filmoverzicht                                  DEL - Reset\n");
                Helpers.Display.PrintLine("Welkom bij het reservatie scherm. \n Controleer de gegevens van uw bestelling.\n");
                Helpers.Display.PrintLine("Reservatie informatie: \n");

                //weergave data
                int nr = 0; 
                List<string> displaystoelen = new List<string>();

                Helpers.Display.PrintHeader("Nr", "Benaming", "Waarde");
                Helpers.Display.PrintTable((nr += 1).ToString(), "Film: ", films.Naam);
                Helpers.Display.PrintTable((nr += 1).ToString(), "Scherm: ", zaal.Scherm);
                Helpers.Display.PrintTable((nr += 1).ToString(), "Prijs: ", (prijs/100).ToString("0.00"));
                Helpers.Display.PrintTable((nr += 1).ToString(), "Tijd: ", schemaData.Tijd);
                Helpers.Display.PrintTable((nr += 1).ToString(), "Datum: ", schemaData.Datum);
                if (newReservering.Aantal >0) Helpers.Display.PrintTable((nr += 1).ToString(), "Aantal: ", newReservering.Aantal.ToString());
                if (newReservering.StoelId.Count > 0)
                {
                    foreach (int stoel in newReservering.StoelId)
                        displaystoelen.Add(stoelen.Where(t => t.StoelId == stoel).Select(s => s.Omschrijving).SingleOrDefault());
                    Helpers.Display.PrintTable((nr += 1).ToString(), "Stoelen: ", String.Join(", ", displaystoelen));
                }
                if(totaal > 0)
                    Helpers.Display.PrintTable((nr += 1).ToString(), "Totaal: ", totaal.ToString());
                Helpers.Display.PrintLine("");



                switch (x) //switch case met de vragen voor de input
                {
                    case 0: Helpers.Display.PrintLine("Voor hoeveel personen wilt u bestellen? Er zijn nog " + alleZaalPlekken.ToString() + " plekken vrij in de zaal"); break; // foreach stoelen over in rijen. max = aantal stoelen over per rij
                    case 1:
                        Helpers.Display.PrintLine("In welke rij wilt u stoelen reserveren? De rij begint bij A en eindigd bij H (van beneden naar boven)");
                        Helpers.Display.PrintHeader("Nr", "Rij","Vip", "plekken");
                        string vip = "";
                        int nummering = 1;
                        foreach (string row in rijen)
                        {
                            List<StoelModel> stoelData = stoelen.Where(r => r.Rij == row).Distinct().ToList();
                            foreach(StoelModel stoel in stoelData)
                            {
                                if (stoel.Premium == true) vip = "Ja"; else vip = "Nee";
                            }
                            Helpers.Display.PrintTable(nummering.ToString(), row, vip, rijPlekken[nummering - 1].ToString()); //sws fout
                            nummering++;
                        }
                        break;
                    case 2:                       
                        int i = 0;
                        Helpers.Display.PrintLine("De stoelen beginnen bij 1 en eindigen bij 16 (van links naar rechts).");
                        Helpers.Display.PrintLine("Welke stoel wilt u reserveren voor persoon " + persoon.ToString()  +"? De stoel begint bij 1 en eindig bij 16 (van links naar rechts).");
                        foreach (StoelModel seet in plekken)
                        {
                            Helpers.Display.PrintTable((i += 1).ToString(), seet.Omschrijving);
                        }                          
                        break;
                    case 3:
                        Console.WriteLine("druk op Insert om de bestelling te betalen");
                        Console.WriteLine("druk op ESC om de bestelling te cancelen en terug te gaan naar het hoofdmenu"); 
                        break;
                }

                //Input    
                Console.Write(">"); Inputs.KeyInput input = Inputs.ReadUserData(); //waarde oplezen met keyinput functie
                int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        switch (x)
                        {
                            case 0:
                                if (inputValue > 0 && inputValue <= alleZaalPlekken) { aantal = inputValue; newReservering.Aantal = aantal; x++; error = ""; }//fout in bij count waarschijnlijk
                                else error = "Er zijn maar " + plekken + " stoelen vrij ";
                                break;
                            case 1:
                                inputValue -= 1;
                                if (inputValue >= 0 && inputValue < rijen.Count())
                                {
                                    rij = rijen[inputValue];
                                    if (rij == "H" || rij == "I")
                                    {
                                        PrijsModel vipPrijs = prijsData.Where(a => a.Soort == "VIP").SingleOrDefault(); decimal plus = decimal.Parse(vipPrijs.Prijs);
                                        prijs += plus;
                                    }
                                    x++; error = "";
                                }
                                else error = "Vul een gevraagde waarde in";
                                break;
                            case 2:
                                inputValue -= 1;
                                if (inputValue >= 0 && inputValue <= plekken.Count())
                                {
                                   newReservering.StoelId.Add(plekken[inputValue].StoelId);
                                   totaal = prijs * ((decimal)persoon / 100);
                                   error = "";
                                }
                                else error = "Deze stoel is niet beschikbaar. Voel een nieuwe waarde in.";
                                plekken = plekken.Where(p => !newReservering.StoelId.Any(p2 => p2 == p.StoelId)).ToList();
                                if(error == "")
                                    persoon++;
                                //als alle stoelen zijn gekozen; naar volgende scherm
                                if (persoon > aantal)
                                    x++;
                                break;
                            case 3:
                                Console.WriteLine("druk op Insert om de bestelling te betalen");
                                Console.WriteLine("druk op ESC om de bestelling te cancelen en terug te gaan naar het hoofdmenu");
                                check = true;
                                break;
                            default:
                                x++;
                                break;
                        }
                        break;
                    case Inputs.KeyAction.Escape:  //de functie beeindigen
                        abort = true;
                        break;
                    case Inputs.KeyAction.Delete:
                        abort = true;
                        Reservering(schemaData);
                        break;
                    case Inputs.KeyAction.Insert:
                        //betalen(ReserveringModel newReservering
                        //daar meot deze functie eigenlijk in
                        if (check == true)
                        {
                            newReservering.Totaal = totaal.ToString();
                            ReserveringData.AddData(newReservering);
                            ConsoleColor originalColor = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("De nieuwe reservering is opgeslagen.");
                            Console.ForegroundColor = originalColor;
                            System.Threading.Thread.Sleep(2000);
                            ReserveringData.SortData();
                            abort = true;
                        }
                        break;
                }
            }
        }

        public void AddReservering()
        {
            
        }
       
        public void Run() //hoofd functie
        {
            bool abort = false;
            string message = "";
            while (!abort)
            {
                Display.PrintLine("Bioscoop - Beheren reservering");
                Display.PrintLine("ESC - Terug");
                Display.PrintLine("");
                if(message != "")
                {
                    Display.PrintLine("");
                    Display.PrintLine(message);
                }
                Display.PrintLine("");

                Display.PrintTableFilm("No", "Titel", "Zaal", "Datum", "Tijd", "Code");
                for(int i = 0; i < reserveringData.Count(); i++)
                {
                    ReserveringModel r = reserveringData[i];
                    FilmschemaModel fs = filmschemaData.Where(fs => fs.ProgrammaId == r.ProgrammaId).ToList()[0];
                    FilmModel f = filmData.Where(f => f.FilmId == fs.FilmId).ToList()[0];
                    ZaalModel z = zaalData.Where(z => z.ZaalId == fs.ZaalId).ToList()[0];
                    Display.PrintTableFilm("" + i + 1, f.Naam, z.Omschrijving, fs.Datum, fs.Tijd, r.Code);
                }

                Display.PrintLine("");
                Display.PrintLine("Vul de code van de reservering die u wilt aanpassen");
                Inputs.KeyInput input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        string code = input.val;
                        if (code.Length != 5)
                        {
                            Console.Clear();
                            break;
                        }
                        else
                        {

                            ReserveringModel reservering = ReserveringData.GetReservering(code);
                            PrintReservering(reservering);
                        }
                        break;
                    case Inputs.KeyAction.Escape:
                        abort = true;
                        break;
                }
                Console.Clear();

            }
        }
        public void ManageReservationData()
        {
            string message = "";
            bool abort = false;
            while (!abort)
            {
                Display.PrintLine("Bioscoop - Beheren reservering");
                Display.PrintLine("ESC - Terug");
                Display.PrintLine("");
                Display.PrintLine("");
                if (message != "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Display.PrintLine("");
                    Display.PrintLine(message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Display.PrintLine("");
                Display.PrintLine("Vul uw unieke code in");
                Inputs.KeyInput input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        string code = input.val;
                        if (code.Length != 5)
                        {
                            Console.Clear();
                            break;
                        }
                        else
                        {

                            ReserveringModel reservering = ReserveringData.GetReservering(code);
                            PrintReservering(reservering);
                        }
                        break;
                    case Inputs.KeyAction.Escape:
                        abort = true;
                        break;
                }
                Console.Clear();
            }
        }
        public void PrintReservering(ReserveringModel r)
        {
            FilmschemaModel filmschema = filmschemaData.Where(fs => fs.ProgrammaId == r.ProgrammaId).ToList()[0];
            FilmModel film = filmData.Where(f => f.FilmId == filmschema.FilmId).ToList()[0];
            ZaalModel zaal = zaalData.Where(z => z.ZaalId == filmschema.ZaalId).ToList()[0];
            List<StoelModel> stoelen = new List<StoelModel>();
            foreach (int id in r.StoelId) {
                stoelen.Add(stoelData.Where(s => s.StoelId == id).ToList()[0]);
            }
            string ststring = "" + stoelen[0].StoelNr;
            for (int i = 1; i < stoelen.Count; i++)
            {
                ststring += "," + stoelen[i].StoelNr;
            }
            string stpp = string.Format("{0:0.00}", double.Parse(r.Totaal) / stoelen.Count);
            Console.Clear();
            Display.PrintLine("Bioscoop - Beheren reservering");
            Display.PrintLine("ESC - Terug naar menu                       DEL - Opzeggen");
            Display.PrintLine("");

            Display.PrintLine("Hier kunt u de informatie van uw reservering inzien");
            Display.PrintLine($"Reserveringscode: {r.Code}");
            Display.PrintLine("");

            Display.PrintLine($"Film:              {film.Naam}");
            Display.PrintLine($"Scherm:            {zaal.Scherm}");
            Display.PrintLine($"Tijd               {filmschema.Tijd}");
            Display.PrintLine($"Datum:             {filmschema.Datum}");
            Display.PrintLine($"Aantal:            {stoelen.Count}");
            Display.PrintLine($"Rij:               {stoelen[0].Rij}");
            Display.PrintLine("Vip:               " + (stoelen[0].Premium ? "Ja" : "Nee"));
            Display.PrintLine($"Prijs per persoon: {stpp}");
            Display.PrintLine($"Stoel:             {ststring}");
            Display.PrintLine($"Totaal betaald:    {r.Totaal}");
            Inputs.KeyInput input = Inputs.ReadUserData();
            switch (input.action) {
                case Inputs.KeyAction.Escape:
                    return;
                case Inputs.KeyAction.Delete:
                    Display.PrintLine("Weet u zeker dat je de reservering wilt opzeggen? U krijgt geen terugbetaling");
                    Display.PrintLine("Druk op een nummer om uw keuze te bevestigen");
                    Display.PrintLine("");
                    Display.PrintLine("Nr  Keuze");
                    Display.PrintLine("1.  Ja");
                    Display.PrintLine("2.  Nee");
                    switch (Display.Keypress())
                    {
                        case ConsoleKey.D1:
                            ReserveringData.RemoveData(r);
                            break;
                        case ConsoleKey.D2:
                            break;
                    }
                    break;
            }
        }
       
    }
}
