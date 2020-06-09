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
                Helpers.Display.PrintMenu("ESC - Terug naar het hoofdmenu");// INS - Filmprogramma van vandaag? 
                Helpers.Display.PrintLine("");
                Helpers.Display.PrintLine("Vul een nummer in om de informatie en filmschema in te zien van de desbetreffende film\n");
                Helpers.Display.PrintLine("Het huidige film aabod: \n");
                Helpers.Display.PrintTableFilm("Nr", "Naam", "Genre", "Kijkwijzer");

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
                Helpers.Display.PrintLine("\n Vul uw keuze in en sluit af met een enter");

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
                            FilmData.UpdateKliks(filmAanbod[inputValue]);
                            ViewFilm(filmAanbod[inputValue]); //waarde meegeven van de gekozen film
                        }
                        else
                        {
                            error = "Onjuist waarde ingevuld.";
                        }
                        break;
                    /*case Inputs.KeyAction.Insert: //niet verder implementeren
                        //filmprogramma vandaag
                        break;*/
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
                Helpers.Display.PrintHeader("Bioscoop - Filmoverzicht - ", film.Naam);
                Helpers.Display.PrintMenu("ESC - Terug naar het overzicht");
                Helpers.Display.PrintLine("");
                Helpers.Display.PrintLine("Film informatie: \n");

                Helpers.Display.PrintTableInfo("Benaming", "Waarde");
                Helpers.Display.PrintTableInfo("Naam: ", film.Naam);

                //variablen die kijken voor de filtering
                int maxLength = 55; bool first = true; int index = 0;
                //maken een array van de omschrijving
                var woorden = film.Omschrijving.Split(' ');
                //string voor de omschrijving per lijn
                string lijn = "";

                foreach (string woord in woorden)
                {
                    //tellen de index op om te kijken hoe vaak er geloop is
                    index++;
                    //gefiltered of het volgend woord mag worden opgeteld
                    if ((lijn + woord + " ").Length <= maxLength)
                        //tellen woord samen met een spatie op aan de string
                        lijn += woord + " ";
                    else
                    {
                        //eerste keer voor de print hierdoor wordt omschrijving ook geprint
                        if (first)
                            Helpers.Display.PrintTableInfo("Omschrijving:  ", lijn);
                        else
                            Helpers.Display.PrintTableInfo("  ", lijn);
                        //string wordt opniew gezet met het woord (hierdoor raakt er geen informatie kwijt)
                        lijn = woord + " ";
                        //first wordt op false gezet zodat omschrijving niet nog een keer wordt geprint
                        first = false;
                    }
                    //kijken of de gehele array is door gespitsed en printen dan de overige woorden || niet altijd is de string de maximale lengte
                    if (index >= woorden.Length)
                        Helpers.Display.PrintTableInfo("  ", lijn);
                }

                Helpers.Display.PrintTableInfo("Genre: ", film.Genre);
                Helpers.Display.PrintTableInfo("Duur: ", film.Duur);
                Helpers.Display.PrintTableInfo("Kijkwijzer: ", film.Kijkwijzer);

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
                Helpers.Display.PrintLine("\n Vul uw keuze in en sluit af met een enter");

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
                            Reservering(temp[inputValue], false); //waarde meegeven van de gekozen film
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

        public void Reservering(FilmschemaModel schemaData, bool admin)
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
            bool vipprijs = false;

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
                Helpers.Display.PrintMenu("ESC - Terug", "DEL - Reset\n");
                Helpers.Display.PrintLine("Welkom bij het reserveringsscherm. \n Controleer de gegevens van uw bestelling.\n");
                Helpers.Display.PrintLine("Reservering informatie: \n");

                //weergave data
                List<string> displaystoelen = new List<string>();

                Helpers.Display.PrintTableInfo("Benaming", "Waarde");
                Helpers.Display.PrintTableInfo("Film: ", films.Naam);
                Helpers.Display.PrintTableInfo("Scherm: ", zaal.Scherm);
                Helpers.Display.PrintTableInfo("Prijs: ", (prijs/100).ToString("0.00"));
                Helpers.Display.PrintTableInfo("Vip: ", (vipprijs ? "Ja" : "Nee"));
                Helpers.Display.PrintTableInfo("Tijd: ", schemaData.Tijd);
                Helpers.Display.PrintTableInfo("Datum: ", schemaData.Datum);
                if (newReservering.Aantal >0) Helpers.Display.PrintTableInfo("Aantal: ", newReservering.Aantal.ToString());
                if (newReservering.StoelId.Count > 0)
                {
                    foreach (int stoel in newReservering.StoelId)
                        displaystoelen.Add(stoelen.Where(t => t.StoelId == stoel).Select(s => s.Omschrijving).SingleOrDefault());
                    Helpers.Display.PrintTableInfo("Stoelen: ", String.Join(", ", displaystoelen));
                }
                if(totaal > 0)
                    Helpers.Display.PrintTableInfo("Totaal: ", totaal.ToString("0.00"));
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
                        Helpers.Display.PrintLine("Druk op Insert om de bestelling te betalen \n");
                        Helpers.Display.PrintLine("Druk op Escape om de bestelling te annuleren en terug te gaan naar het hoofdmenu");
                        newReservering.Totaal = totaal.ToString();
                        check = true;
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
                                        vipprijs = true;
                                    }
                                    x++; error = "";
                                }
                                else error = "Vul een gevraagde waarde in";
                                break;
                            case 2:
                                inputValue -= 1;
                                if (inputValue >= 0 && inputValue < plekken.Count())
                                {
                                   newReservering.StoelId.Add(plekken[inputValue].StoelId);
                                   totaal = prijs * ((decimal)persoon / 100);
                                   error = "";
                                }
                                else error = "Deze stoel is niet beschikbaar. Vul een nieuwe waarde in.";
                                plekken = plekken.Where(p => !newReservering.StoelId.Any(p2 => p2 == p.StoelId)).ToList();
                                if(error == "")
                                    persoon++;
                                //als alle stoelen zijn gekozen; naar volgende scherm
                                if (persoon > aantal)
                                    x++;
                                break;
                            case 3:
                                Helpers.Display.PrintLine("Druk op Insert om de bestelling te betalen \n");
                                Helpers.Display.PrintLine("Druk op Escape om de bestelling te annuleren en terug te gaan naar het hoofdmenu");
                                Console.CursorVisible = false;
                                break;
                            default:
                                x++;
                                break;
                        }
                        break;
                    case Inputs.KeyAction.Escape:  //de functie beeindigen
                        Program.Main(null);
                        break;
                    case Inputs.KeyAction.Delete:
                        abort = true;
                        if (admin == false)
                            Reservering(schemaData, false);
                        else
                            Reservering(schemaData, true);
                        break;
                    case Inputs.KeyAction.Insert:
                        if(check == true)
                        {
                            ReserveringBetaling(newReservering);
                        }
                        break;
                }
            }
        }

        public void ReserveringBetaling(ReserveringModel newReservering)
        {
            Random betaalcode = new Random();
            int randomNr = betaalcode.Next();
            string error = "";
            bool abort = false;
            while(!abort)
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

                Helpers.Display.PrintHeader("Bioscoop - Betalen");
                Helpers.Display.PrintMenu("ESC - Terug");
                Helpers.Display.PrintLine("");
                Helpers.Display.PrintLine("Welkom bij het betaalscherm \n Vul de betalingscode in om de bestelling af te ronden\n");
                Helpers.Display.PrintLine("Betaalcode: " + randomNr.ToString());

                Console.Write(">"); Inputs.KeyInput input = Inputs.ReadUserData(); //waarde oplezen met keyinput functie
                int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        if (inputValue == randomNr)
                        {
                            ReserveringData.AddData(newReservering);
                            ConsoleColor originalColor = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Display.PrintLine("De nieuwe reservering is verwerkt. \n U word nu doorverwezen naar het overzicht scherm");
                            Console.ForegroundColor = originalColor;
                            System.Threading.Thread.Sleep(3000);
                            ReserveringData.SortData();
                            ReserveringOverzicht(newReservering, false);
                        }
                        else
                            error = "Verkeerde waarde ingevuld";
                        break;
                    case Inputs.KeyAction.Escape:  //de functie beeindigen
                        return;
                }
            }
        }

        public void ReserveringBeheer()
        {
            Console.Clear();
            bool abort = false;
            string error = "";
            while (!abort)
            {
                if (!String.IsNullOrEmpty(error)) //error handling en text kleur veranderen
                {
                    ConsoleColor originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Helpers.Display.PrintLine("Error: " + error);
                    Helpers.Display.PrintLine("");
                    Console.ForegroundColor = originalColor;
                }
                ReserveringData.SortData();
                Display.PrintLine("Bioscoop - Reserveringbeheer");
                Display.PrintMenu("ESC - Terug");
                Display.PrintLine("");
                Display.PrintLine("Overzicht van alle reserveringen: \n");
                Display.PrintReserveringbeheer("Nr", "Code", "Programma datum");
                for (int i = 0; i < reserveringData.Count(); i++)
                {
                    try
                    {
                        ReserveringModel r = reserveringData[i];
                        FilmschemaModel fs = filmschemaData.Where(fs => fs.ProgrammaId == r.ProgrammaId).ToList()[0];
                        Display.PrintReserveringbeheer(i + 1 + "", r.Code, fs.Datum);
                    }
                    catch
                    {
                        error = "Fout bij data ophalen";
                    }
                }

                Display.PrintLine("");
                Display.PrintLine("Vul een waarde in om naar het overzicht van deze reservering te gaan");
                Console.Write(">"); Inputs.KeyInput input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                        inputValue--; //-1 want count start bij 0
                        if (inputValue >= 0 && inputValue < reserveringData.Count())
                            ReserveringOverzicht(reserveringData[inputValue], true);
                        else if (inputValue == -1)
                            error = "Verkeerde input, vul een nummer in";
                        else
                        {
                            error = "Verkeerd nummer, vul een nummer uit de lijst in";
                        }
                        break;
                    case Inputs.KeyAction.Escape:
                        abort = true;
                        break;
                }
                Console.Clear();
            }
        }
       
        public void ReserveringLogin()
        {
            Console.CursorVisible = true;
            Console.Clear();
            ReserveringData.SortData();
            string error = "";
            bool abort = false;
            while (!abort)
            {
                if (error != "")
                {
                    ConsoleColor originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Helpers.Display.PrintLine("Error: " + error);
                    Helpers.Display.PrintLine("");
                    Console.ForegroundColor = originalColor;
                }
                Display.PrintLine("Bioscoop - Beheren reservering");
                Display.PrintMenu("ESC - Terug naar het hoofdmenu");
                Display.PrintLine("");
                Display.PrintLine("Als u een film heeft gereserveerd krijgt u een unieke code te zien");
                Display.PrintLine("Met deze code kunt de gegevens van uw reservering inzien \n");
                Display.PrintLine("Vul uw unieke code in:");
                Console.Write(">"); Inputs.KeyInput input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        if (input.val == null || input.val.Length != 5)
                        {
                            error = "Vul een bestaande code in";
                            Console.Clear();
                            break;
                        }
                        else
                        {
                            ReserveringModel reservering = reserveringData.Where(r => r.Code == input.val).SingleOrDefault();
                            if (reservering != null)
                                ReserveringOverzicht(reservering, false);
                            else
                                error = "Code is niet gevonden";
                        }
                        break;
                    case Inputs.KeyAction.Escape:
                        abort = true;
                        return;
                }
                Console.Clear();
            }
        }
        public void ReserveringOverzicht(ReserveringModel r, bool admin)
        {
            Console.CursorVisible = false;
            //data
            FilmschemaModel filmschema = filmschemaData.Where(fs => fs.ProgrammaId == r.ProgrammaId).SingleOrDefault();
            FilmModel film = filmData.Where(f => f.FilmId == filmschema.FilmId).SingleOrDefault();
            ZaalModel zaal = zaalData.Where(z => z.ZaalId == filmschema.ZaalId).SingleOrDefault();
            List<StoelModel> stoelen = stoelData.Where(a => a.ZaalId == filmschema.ZaalId).ToList();
            decimal prijs = decimal.Parse(r.Totaal, CultureInfo.InvariantCulture);
            List<StoelModel> stoel = new List<StoelModel>();
            foreach (int id in r.StoelId)
                stoel.Add(stoelData.Where(s => s.StoelId == id).SingleOrDefault());

            //display
            bool abort = false;
            while (!abort)
            {
                Console.Clear();
                Display.PrintLine("Bioscoop - Beheren reservering");
                Display.PrintMenu("ESC - Terug naar het hoofdmenu", "DEL - Opzeggen");
                Display.PrintLine("");

                Display.PrintLine("Hier kunt u de informatie van uw reservering inzien \n");

                if(admin == false)Display.PrintLine("Bewaar deze reserveringscode");
                Display.PrintTableInfo($"Reserveringscode:", r.Code);
                Display.PrintLine("");
                Display.PrintTableInfo("Film:", film.Naam);
                Display.PrintTableInfo("Scherm:", zaal.Scherm);
                Display.PrintTableInfo("Tijd:", filmschema.Tijd);
                Display.PrintTableInfo("Datum:", filmschema.Datum);
                Display.PrintTableInfo("Aantal", r.Aantal.ToString());
                Display.PrintTableInfo("Rij:", stoel[0].Rij);
                Display.PrintTableInfo("Vip:", (stoel[0].Premium ? "Ja" : "Nee"));
                Display.PrintTableInfo("Prijs per persoon: ", (prijs / r.Aantal).ToString());
                List<string> displaystoelen = new List<string>();
                if (r.StoelId.Count > 0)
                {
                    foreach (int s in r.StoelId)
                        displaystoelen.Add(stoelData.Where(t => t.StoelId == s).Select(x => x.Omschrijving).SingleOrDefault());
                    Helpers.Display.PrintTableInfo("Stoelen: ", String.Join(", ", displaystoelen));
                }
                Display.PrintTableInfo("Totaal betaald:", prijs.ToString("0.00"));

                Inputs.KeyInput input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Escape:
                        abort = true;
                        if(admin == false)
                            Program.Main(null);
                        break;
                    case Inputs.KeyAction.Delete:
                        Display.PrintLine("\n Weet u zeker dat u de reservering wilt opzeggen? U krijgt geen terugbetaling");
                        Display.PrintLine("Druk op een nummer om uw keuze te bevestigen");
                        Display.PrintLine("");
                        Display.PrintLine("Nr  Keuze");
                        Display.PrintLine("1.  Ja");
                        Display.PrintLine("2.  Nee");
                        switch (Display.Keypress())
                        {
                            case ConsoleKey.D1:
                                ReserveringData.RemoveData(r);
                                ConsoleColor ogColor = Console.ForegroundColor;
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(" Reservering met resererveringscode: " + r.Code + " is verwijderd");
                                Console.ForegroundColor = ogColor;
                                System.Threading.Thread.Sleep(2000);
                                Program.Main(null);
                                break;
                            case ConsoleKey.D2:
                                break;
                        }
                        break;
                }
            }
        }
    }
}
