using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;

class GebruikersMenu
{
    void SuccesLogin(Gebruiker admin)
    {
        ///<summary>
        ///De gebruiker is succesvol ingelogd
        ///De naam van de gebruiker wordt getoond en de rechten
        ///van deze.
        /// </summary>
        Console.Clear();
        Console.WriteLine("Gebruikersbeheer");
        Console.WriteLine("ESC terug naar het menu\t\tINS nieuwe gebruiker aanmaken");
        Console.WriteLine("\nIngelogd: " + admin.naam);
        Console.WriteLine("ACcount-type: " + ((admin.rechten) ? "Admin" : "Medewerker"));
    }
    public void PrintGebruikers(List<Gebruiker> users, Gebruiker admin)
    {
        ///<summary>
        ///Deze functie print alle gebruikers naar de console toe
        /// </summary>
        /// 
        Console.WriteLine("{0, -5}{1, -20}{2, -18}{3, -5}{4, -15}{5,0}", "Nr", "Naam", "Gebruikersnaam", "ID", "Wachtwoord", "Rechten");
        int index = 1;
        foreach (var gebruiker in users)
        {
            if (!gebruiker.id.Equals(admin.id) && admin.rechten)
            {
                Console.WriteLine("{0, -5}{1, -20}{2, -18}{3, -5}{4, -15}{5,0}", index, gebruiker.naam, gebruiker.gebruikersnaam, gebruiker.id, gebruiker.zieWachtwoordt(admin), gebruiker.rechten);
                index++;
            }
        }
    }
    string Wachtwoordt()
    {
        ///<summary>
        ///Deze functie vraagt de wachtwoordt op en zet sterretjes
        ///hierdoor is de wachtwoordt niet zichtbaar voor derden 
        ///als deze wordt ingevoerd.
        /// </summary>

        //string pass is de holder voor het wacht woordt van de gebruiker
        string pass = "";
        do
        {
            //welke toets wordt hier ingedrukt
            ConsoleKeyInfo key = Console.ReadKey(true);

            //filteren welke key het is en kijken of het geen bijzondere key is
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape)
            {
                //als het een normale key wordt deze opgeteld aan het wachtwoordt
                pass += key.KeyChar;
                //er wordt een "*" in de console meer geprint
                Console.Write("*");
            }
            else
            {
                //een speciale key is ondekt
                //hier wordt gekeken of het er een character moet worden verwijdert uit de password
                if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    //een character verwijderen van de password
                    pass = pass.Substring(0, pass.Length - 1);
                    //een "*" verwijderen van de console
                    Console.Write("\b \b");
                }
                //gekeken of het enter is
                else if (key.Key == ConsoleKey.Enter)
                {
                    //de loop wordt verbroken
                    break;
                }
                //kijken of de key niet de ESC key is
                else if (key.Key == ConsoleKey.Escape)
                {
                    //de cancel command wordt doorgegeven
                    pass = "ESC";
                    //breken uit de loop
                    break;
                }
            }//loopen todat er uit de loop wordt gebroken
        } while (true);
        //de wachtwoordt wordt terug gegeven
        return pass;
    }

    bool nietArrows(ConsoleKeyInfo key)
    {
        ///<summary>
        ///Kijken of de input geen arrowkeys zijn
        ///Dit kan namelijk voor problemen zorgen
        /// </summary>
        return key.Key != ConsoleKey.RightArrow && key.Key != ConsoleKey.LeftArrow && key.Key != ConsoleKey.UpArrow && key.Key != ConsoleKey.DownArrow;
    }

    public string ReadWithSpecialKeys()
    {
        ///<summary>
        ///Deze functie catch special keys
        ///Kan verder worden uitgebreid
        /// </summary>
        string result = "";
        ConsoleKeyInfo key = Console.ReadKey(true);
        while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape && key.Key != ConsoleKey.Insert && key.Key != ConsoleKey.Delete)
        {
            if (key.Key != ConsoleKey.Backspace && nietArrows(key))
            {
                Console.Write(key.KeyChar);
                result += key.KeyChar;
            }
            else if (result.Length > 0 && key.Key == ConsoleKey.Backspace)
            {
                Console.Write("\b \b");
                result = result.Substring(0, result.Length - 1);
            }
            key = Console.ReadKey(true);
        }

        if (key.Key == ConsoleKey.Escape)
            return "ESC";

        if (key.Key == ConsoleKey.Insert)
            return "INS";

        if (key.Key == ConsoleKey.Delete)
            return "DEL";

        return result;
    }

    public Gebruiker Login(List<Gebruiker> gebruikers)
    {
        ///<summary>
        ///Vraag de account details aan van de gebruiker
        ///en dan kijken of deze echt bestaat
        ///Zo ja dan wordt de account terug gegeven
        ///Zo niet dan wordt een null terug gegeven
        /// </summary>
        Console.Clear();
        Console.WriteLine("FilmHaus Inloggen:");
        Console.WriteLine("(Druk op ESC terug te gaan)\nVoer uw gebruikersnaam en wachtwoordt in\n");
        Console.Write("Gebruikersnaam: ");
        string gebruiker = ReadWithSpecialKeys();

        while (gebruiker.Equals("DEL") || gebruiker.Equals("INS"))
        {
            Console.WriteLine("\nOnbekende toets! Voer aub uw gebruikersnaam opnieuw in\n");
            Console.Write("Gebruikersnaam: ");
            gebruiker = ReadWithSpecialKeys();
        }

        if (gebruiker.Equals("ESC"))
            return new Gebruiker("cancel", "cancel", "cancel", "cancel", false);

        Console.Write("\nWachtwoord: ");

        string pass = Wachtwoordt();
        //als de sessie wordt onderbroken
        if (pass.Equals("ESC"))
            return new Gebruiker("cancel", "cancel", "cancel", "cancel", false);

        //kijken of de account een bestaande account is
        foreach (Gebruiker i in gebruikers)
        {
            {
                //komen de gebruikersnamen overeen
                if (i.gebruikersnaam.Equals(gebruiker))
                {
                    //komen de wachtwoorden overeen
                    if (i.checkWachtwoord(pass))
                    {
                        return i;
                    }
                }
            }
        }
        return null;
    }

    void ToonGebruiker(List<Gebruiker> data, string index)
    {
        //Deze functie is bedoeld om een gebruiker te veranderen
        //of te verwijderen

        void ToonGebruiker(Gebruiker user, bool opgeslagen)
        {
            //ingebouwde functie voor het tonen van de gebruiker
            Console.Clear();
            Console.WriteLine("FilmHaus Gebruikerbeheer\n");
            Gebruiker temp = user;
            Console.WriteLine("Aanpassingen opgeslagen: " + opgeslagen + "\n");
            Console.WriteLine("Aanpassen gebruiker: " + temp.naam + "\nESC Terug naar overzicht(CANCEL)\t DEL Gebruikerverwijderen\nINS Opslaan\n");
            Console.WriteLine("1. ID:             " + temp.id);
            Console.WriteLine("2. Naam:           " + temp.naam);
            Console.WriteLine("3. Wachtwoord:     " + temp.wachtwoord);
            Console.WriteLine("4. Account-type:   " + ((temp.rechten) ? "Admin" : "Medewerker"));
            Console.WriteLine("5. Gebruikersnaam: " + temp.gebruikersnaam);
            Console.WriteLine("Welke attribute wilt u veranderen: ");
            Console.Write(">>> ");
        }

        string Verander()
        {
            //ingebouwde functie om te veranderen
            Console.Write("Verander naar >>> ");
            string keuze = ReadWithSpecialKeys();
            while (keuze == "DEL" || keuze == "INS")
            {
                Console.WriteLine("Toets onbekend! Voer aub opnieuw in!");
                Console.Write("Verander naar >>> ");
                keuze = ReadWithSpecialKeys();
            }
            //geeft de verandering terug
            return keuze;
        }

        void PrintOpties()
        {
            //print de optie om terug te gaan
            Console.WriteLine("ESC Terug\n");
        }

        Gebruiker VeranderGebruiker(Gebruiker user)
        {
            //booleans om te kijken of de gebruiker klaar is met
            //aanpassen
            //en opgeslagen heeft
            //indien er is opgeslagen wordt de verandering doorgegevn
            //aan het systeem
            bool opgeslagen = false;
            bool exit = false;
            //het object wordt gecloned zodat er aanpassingen
            //kunnen worden gemaakt
            var temp = user.Clone();
            //veranderingen worden hierin temporeel opgeslagen
            string change;

            //loop die doorloopt totdat de gebruiker stopt
            while (!exit)
            {
                //toon data van een gebruiker
                ToonGebruiker(temp, opgeslagen);
                //welke attribute wordt aangepast
                string keuze = ReadWithSpecialKeys();
                //schone zaak
                Console.Clear();
                switch (keuze)
                {
                    //hierin wordt de id aangepast
                    case "1":
                        PrintOpties();
                        Console.WriteLine("ID: " + temp.id);
                        change = Verander();
                        //terug naar menu zonder aanpassing te maken
                        if (change.Equals("ESC"))
                            break;
                        else
                            temp.id = change;
                        break;
                    //hierin wordt de naam aangepast
                    case "2":
                        PrintOpties();
                        Console.WriteLine("Naam: " + temp.naam);
                        change = Verander();
                        //terug naar menu zonder aanpassing te maken
                        if (change.Equals("ESC"))
                            break;
                        else
                            temp.naam = change;
                        break;
                    //wachtwoordt van veranderen van de gebruiker
                    case "3":
                        PrintOpties();
                        Console.WriteLine("Wachtwoordt: " + temp.wachtwoord);
                        change = Verander();
                        //terug naar menu zonder aanpassing te maken
                        if (change.Equals("ESC"))
                            break;
                        else
                            temp.wachtwoord = change;
                        break;
                    //rechten veranderen van de gebruiker
                    case "4":
                        PrintOpties();
                        Console.WriteLine("Account-type: " + ((temp.rechten)? "Admin":"Medewerker"));
                        change = Verander();
                        //terug naar menu zonder aanpassing te maken
                        if (change.Equals("ESC"))
                            break;
                        //admin rechten toekennen
                        else if (change.Contains("admin"))
                            temp.rechten = true;
                        //medewerker rechten toekennen
                        else if (change.Contains("medewerker"))
                            temp.rechten = false;
                        break;

                    case "5":
                        PrintOpties();
                        Console.WriteLine("Gebruikersnaam: " + temp.gebruikersnaam);
                        change = Verander();
                        //terug naar menu zonder aanpassing te maken
                        if (change.Equals("ESC"))
                            break;
                        else
                            temp.gebruikersnaam = change;
                        break;
                    case "ESC":
                        //terug naar menu met alle gebruikers
                        return user;
                    case "DEL":
                        //veranderen de naam naar DEl om te verwijderen
                        user.naam = "DEL";
                        return user;
                    case "INS":
                        //alle aanpassingen worden opgeslagen
                        user = temp;
                        opgeslagen = true;
                        break;
                }
            }
            //sturen de aangepast object door
            return user;
        }

        //code voor veranderen gebruiker
        int locatie = Int32.Parse(index) - 1;
        Gebruiker temp = null;
        while (temp == null)
        {
            temp = VeranderGebruiker(data[locatie]);
        }
        //kijken of er een gebruiker verandert moet worden of verwijdert
        if (temp.naam == "DEL")
            data.Remove(data[locatie]);
        else
            data[locatie] = temp;
    }

    void VoegGebruikerToe(List<Gebruiker> data)
    {
        //array voor alle input (details van de nieuwe gebruiker)
        string[] user = new string[4];
        //wat er ingevuld moet worden
        string[] todo = { "Naam: ", "Gebruikersnaam: ", "Wachtwoordt: ", "Rechten: " };
        //input string hierin wordt de input gezet
        string input = "";
        Console.Clear();
        //details met wat er moet gebeuren
        Console.WriteLine("ESC om terug te gaan (CANCEL)\nGebruiker Aanmaken, vul aub de benodigde gegevens in:\n");
        //pointer voor hoever de progress is
        int pointer = 0;
        //zolang doorgaan tot alle details zijn ingevuld
        while (pointer < todo.Length)
        {
            Console.Write("\n" + todo[pointer]);
            input = ReadWithSpecialKeys();
            //kijken wat de input
            switch (input)
            {
                //vangen speciale keys die niet worden gebruikt
                case "INS":
                    Console.WriteLine("\nProbeer nogmaals! De toets die is ingedrukt wordt niet ondersteunt momenteel");
                    break;
                case "DEL":
                    Console.WriteLine("\nProbeer nogmaals! De toets die is ingedrukt wordt niet ondersteunt momenteel");
                    break;
                //vangen een lege string
                case "":
                    Console.WriteLine("\nVoer aub iets in!");
                    break;
                //als er gestopt moet worden
                case "ESC":
                    return;
                //goeie input en dan zal er verder gegaan worden
                default:
                    user[pointer] = input;
                    pointer++;
                    input = "";
                    break;
            }
        }
        //genereer een nieuwe id
        string genID = (Int32.Parse(data[data.Count - 1].id) + 1).ToString();
        //kijken welke rechten worden toegekent
        bool rechten = false;
        if (user[3].Contains("admin"))
            rechten = true;
        //nieuwe gebruiker aanmaken
        Gebruiker gebruiker = new Gebruiker(user[0], user[1],  genID, user[2], rechten);
        //gebruiker toevoegen
        data.Add(gebruiker);
    }
    public void AdminsRechten(List<Gebruiker> data, Gebruiker admin)
    {
        //bool om programma af te sluiten
        bool exit = false;
        while (!exit)
        {
            //succes met onloggen
            this.SuccesLogin(admin);
            //print alle gebruikers behalve de admin
            this.PrintGebruikers(data, admin);
            //keuze voor de gebruiker
            Console.WriteLine("\nToets in wat u wilt doen\nToets een nummer in om een gebruiker te veranderen\n");
            Console.Write("Keuze >>> ");
            //keuze inlezen
            string keuze = ReadWithSpecialKeys();
            //als er geen keuze wordt gemaakt
            while (keuze.Equals(""))
            {
                Console.WriteLine("\nMaak aub een geldige keuze!");
                Console.Write("Keuze >>> ");
                keuze = ReadWithSpecialKeys();
            }
            //kijken wat de keuze is
            switch (keuze)
            {
                case "ESC":
                    //stoppen met de programma
                    exit = true;
                    return;
                case "INS":
                    //voeg een gebruiker toe
                    VoegGebruikerToe(data);
                    break;
            }

            //kijken of de gebruiker een nummer heeft ingevoerd
            if (int.TryParse(keuze, out _))
            {
                if (Int32.Parse(keuze) <= data.Count)
                {
                    //gebruiker aanpassen en tonen
                    ToonGebruiker(data, keuze);
                }
            }

            //aanpassingen opslaan in het systeem
            if (!data.Equals(new Lezer().gebruikersInlezen()))
                new Schrijver().updateGebruikers(data);
        }
    }
}