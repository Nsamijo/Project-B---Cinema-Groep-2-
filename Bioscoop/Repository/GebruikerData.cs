using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Bioscoop.Models;
using Bioscoop.Helpers;
using System.Linq;
using System.Threading;

namespace Bioscoop.Repository
{
    public class Schrijver
    {
        /// <summary>
        /// Deze class wordt gebruikt voor het schrijven naar de json file
        /// </summary>
        public void UpdateGebruikers(List<GebruikerModel> gebruikers)
        {
            //zetten de lijst met objecten om naar JSON notatie (string)
            string users = JsonConvert.SerializeObject(gebruikers.ToArray(), Formatting.Indented);
            try
            {
                //schrijf de JSON (string) naar de file toe
                File.WriteAllText(new Helpers.Zoeker().SearchFile("Gebruiker.json"), users);
            }
            catch
            {
                //er is niet naar de file kunnen schrijven
                Helpers.Display.PrintLine("De Gebruikers data is niet bijgewerkt! Probeer het nogmaals!");
            }
        }
    }

    public class Lezer
    {
        public List<GebruikerModel> gebruikersInlezen()
        {
            try
            {
                //zoek de locatie op van de JSON en lees deze in
                string users = File.ReadAllText(new Helpers.Zoeker().SearchFile("Gebruiker.json"));
                //de ingelezen data omzetten naar een JArray
                JArray temp = JArray.Parse(users);
                //de JArray omzetten naar Gebruiker objecten
                List<GebruikerModel> gebruikers = temp.ToObject<List<GebruikerModel>>();
                //alle gebruikers terug geven
                return gebruikers;
            }
            catch
            {
                //de JSON locatie is niet gevonden
                Helpers.Display.PrintLine("Gebruikers data is niet gevonden!");
            }
            //als de JSON niet kan worden ingelezen wordt er een null terug gegeven
            return null;
        }
    }

    class GebruikersMenu
    {
        void SuccesLogin(GebruikerModel admin)
        {
            ///<summary>
            ///De gebruiker is succesvol ingelogd
            ///De naam van de gebruiker wordt getoond en de rechten
            ///van deze.
            /// </summary>
            Console.Clear();
            Helpers.Display.PrintLine("Gebruikersbeheer");
            Helpers.Display.PrintLine("ESC terug naar het menu\t\tINS nieuwe gebruiker aanmaken");
            Helpers.Display.PrintLine("Ingelogd: " + admin.Naam);
            Helpers.Display.PrintLine("Account-type: " + ((admin.Rechten) ? "Admin" : "Medewerker"));
        }
        public void PrintGebruikers(List<GebruikerModel> users, GebruikerModel admin)
        {
            ///<summary>
            ///Deze functie print alle gebruikers naar de console toe
            /// </summary>
            /// 
            Helpers.Display.PrintHeader("Nr", "Naam", "Gebruikersnaam", "Wachtwoord", "Rechten");
            int index = 1;
            foreach (var gebruiker in users)
            {
                if (!gebruiker.GebruikerId.Equals(admin.GebruikerId) && admin.Rechten)
                {
                    Helpers.Display.PrintTable(index.ToString(), gebruiker.Naam, gebruiker.Gebruikersnaam, gebruiker.zieWachtwoordt(admin), ((gebruiker.Rechten) ? "Admin" : "Medewerker"));
                    index++;
                }
            }
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


        void ToonGebruiker(List<GebruikerModel> data, string index, GebruikerModel admin)
        {
            //Deze functie is bedoeld om een gebruiker te veranderen
            //of te verwijderen

            void ToonGebruiker(GebruikerModel user, bool opgeslagen)
            {
                //ingebouwde functie voor het tonen van de gebruiker
                Console.Clear();
                Helpers.Display.PrintLine("Gebruikerbeheer\n");
                GebruikerModel temp = user;
                Helpers.Display.PrintLine("Aanpassingen opgeslagen: " + opgeslagen + "\n");
                Helpers.Display.PrintLine("Aanpassen gebruiker: " + temp.Naam + "\nESC Terug naar overzicht(CANCEL)\t DEL Gebruikerverwijderen\nINS Opslaan\n");
                Helpers.Display.PrintLine("1. Naam:           " + temp.Naam);
                Helpers.Display.PrintLine("2. Wachtwoord:     " + temp.Wachtwoord);
                Helpers.Display.PrintLine("3. Account-type:   " + ((temp.Rechten) ? "Admin" : "Medewerker"));
                Helpers.Display.PrintLine("4. Gebruikersnaam: " + temp.Gebruikersnaam);
                Helpers.Display.PrintLine("Welke attribute wilt u veranderen: ");
                Console.Write(">>> ");
            }

            string Verander()
            {
                //ingebouwde functie om te veranderen
                Console.Write("Verander naar >>> ");
                string keuze = ReadWithSpecialKeys();
                while (keuze == "DEL" || keuze == "INS")
                {
                    Helpers.Display.PrintLine("Toets onbekend! Voer aub opnieuw in!");
                    Console.Write("Verander naar >>> ");
                    keuze = ReadWithSpecialKeys();
                }
                //geeft de verandering terug
                return keuze;
            }

            void PrintOpties()
            {
                //print de optie om terug te gaan
                Helpers.Display.PrintLine("ESC Terug\n");
            }

            GebruikerModel VeranderGebruiker(GebruikerModel user)
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
                    ConsoleKey pressed = Helpers.Display.Keypress();
                    //schone zaak
                    Console.Clear();
                    switch (pressed)
                    {

                        //hierin wordt de naam aangepast
                        case ConsoleKey.D1:
                            PrintOpties();
                            Helpers.Display.PrintLine("Naam: " + temp.Naam);
                            change = Verander();
                            //terug naar menu zonder aanpassing te maken
                            if (change.Equals("ESC"))
                                break;
                            else
                                temp.Naam = change;
                            break;
                        //wachtwoordt van veranderen van de gebruiker
                        case ConsoleKey.D2:
                            PrintOpties();
                            Helpers.Display.PrintLine("Wachtwoord: " + temp.Wachtwoord);
                            change = Verander();
                            //terug naar menu zonder aanpassing te maken
                            if (change.Equals("ESC"))
                                break;
                            else
                                temp.Wachtwoord = change;
                            break;
                        //rechten veranderen van de gebruiker
                        case ConsoleKey.D3:
                            PrintOpties();
                            Helpers.Display.PrintLine("Account-type: " + ((temp.Rechten) ? "Admin" : "Medewerker"));
                            change = Verander();
                            //terug naar menu zonder aanpassing te maken
                            if (change.Equals("ESC"))
                                break;
                            //admin rechten toekennen
                            else if (change.Contains("admin"))
                                temp.Rechten = true;
                            //medewerker rechten toekennen
                            else if (change.Contains("medewerker"))
                                temp.Rechten = false;
                            break;

                        case ConsoleKey.D4:
                            PrintOpties();
                            Helpers.Display.PrintLine("Gebruikersnaam: " + temp.Gebruikersnaam);
                            change = Verander();
                            //terug naar menu zonder aanpassing te maken
                            if (change.Equals("ESC"))
                                break;
                            else
                                temp.Gebruikersnaam = change;
                            break;
                        case ConsoleKey.Escape:
                            //terug naar menu met alle gebruikers
                            return user;
                        case ConsoleKey.Delete:
                            //veranderen de naam naar DEl om te verwijderen
                            user.Naam = "DEL";
                            return user;
                        case ConsoleKey.Insert:
                            //alle aanpassingen worden opgeslagen
                            user = temp;
                            opgeslagen = true;
                            break;
                    }
                }
                //sturen de aangepast object door
                return user;
            }
            //zetten de string om naar een geldig index
            int locatie = Int32.Parse(index) - 1;

            //kijken waar de admin zit, hebben dit nu ff als comment gezet anders aangezien het een test account is
            int locAdmin = data.FindIndex(x => x.GebruikerId == admin.GebruikerId) - 1;

            //tellen 1 erbij op zodat de admin niet kan worden aangepast
            if (locAdmin < locatie)
                locatie++;

            try
            {
                GebruikerModel temp = null;
                while (temp == null)
                {
                    temp = VeranderGebruiker(data[locatie]);
                }
                //kijken of er een gebruiker verandert moet worden of verwijdert
                if (temp.Naam == "DEL")
                    data.Remove(data[locatie]);
                else
                    data[locatie] = temp;
            }
            catch
            {
                Helpers.Display.PrintLine("\nSelecteer een gebruiker die wel in de lijst staat");
                Thread.Sleep(500);
            }

        }

        void VoegGebruikerToe(List<GebruikerModel> data, GebruikerModel admin)
        {
            //array voor alle input (details van de nieuwe gebruiker)
            string[] user = new string[4];
            //wat er ingevuld moet worden
            string[] todo = { "Naam: ", "Gebruikersnaam: ", "Wachtwoordt: ", "Rechten: " };
            //input string hierin wordt de input gezet
            string input = "";
            Console.Clear();
            //details met wat er moet gebeuren
            Display.PrintLine("Gebruikersbeheer\nWelkom: " + admin.Naam);
            Helpers.Display.PrintLine("ESC - Terug naar overzicht                                     DEL RESET\n\nVoer aub de volgende gegevens in: ");
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
                        Helpers.Display.PrintLine("\nProbeer nogmaals! De toets die is ingedrukt wordt niet ondersteunt momenteel");
                        break;
                    case "DEL":
                        pointer = 0;
                        user = new string[4];
                        Console.Clear();
                        Helpers.Display.PrintLine("Gebruikersbeheer\nWelkom: " + admin.Naam);
                        Helpers.Display.PrintLine("ESC - Terug naar overzicht                                     DEL RESET\n\nVoer aub de volgende gegevens in: ");
                        break;
                    //vangen een lege string
                    case "":
                        Helpers.Display.PrintLine("\nVoer iets in!");
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
            int genID = data.Max(x => x.GebruikerId) + 1;
            //kijken welke rechten worden toegekent
            bool rechten = false;
            if (user[3].Contains("admin"))
                rechten = true;
            //nieuwe gebruiker aanmaken
            GebruikerModel gebruiker = new GebruikerModel(user[0], user[1], genID, user[2], rechten);
            //gebruiker toevoegen
            data.Add(gebruiker);
        }
        public void AdminsRechten(List<GebruikerModel> data, GebruikerModel admin)
        {
            //bool om programma af te sluiten
            bool exit = false;
            string message = "";
            while (!exit)
            {
                string keuze = "";
                while (true)
                {
                    //succes met onloggen
                    this.SuccesLogin(admin);
                    //print alle gebruikers behalve de admin
                    this.PrintGebruikers(data, admin);
                    //keuze voor de gebruiker
                    Helpers.Display.PrintLine("\nToets in wat u wilt doen\nToets een nummer in om een gebruiker te veranderen\n" + message);
                    //keuze inlezen
                    keuze = ReadWithSpecialKeys();

                    //checken of het een nummer is
                    if (Int16.TryParse(keuze, out _))
                    {
                        //checken of het in range is
                        if (int.Parse(keuze) - 1 < data.Count)
                            break;
                        else
                            //error message
                            message = "\nToets een nummer in dat is aangegeven!";
                    }
                    else if (keuze.Equals("ESC") || keuze.Equals("INS"))
                        break;
                    else
                        message = "\nToets alleen nummers in!";
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
                        VoegGebruikerToe(data, admin);
                        break;
                }

                //kijken of de gebruiker een nummer heeft ingevoerd
                if (int.TryParse(keuze, out _))
                {
                    if (Int32.Parse(keuze) - 1 < data.Count)
                    {
                        //gebruiker aanpassen en tonen
                        ToonGebruiker(data, keuze, admin);
                    }
                }

                //aanpassingen opslaan in het systeem
                if (!data.Equals(new Lezer().gebruikersInlezen()))
                    new Schrijver().UpdateGebruikers(data);
            }
        }
    }
}