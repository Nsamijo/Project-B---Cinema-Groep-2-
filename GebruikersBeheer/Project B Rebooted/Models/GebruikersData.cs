using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

public class Zoeker
{
    public string SearchFile(string file)
    {
        ///Zoek naar de path van een specifieke document
        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        //vraag de naam op van de solution
        string name = Assembly.GetCallingAssembly().GetName().Name;
        //index opzoeken naar de laatste voorkoming van de naam
        int last = path.LastIndexOf(name);
        //de path bijwerken om zo in project map te komen
        path = path.Substring(0, last);

        ///loop door alle mappen en zoek naar de file
        try
        {
            foreach (string i in Directory.GetDirectories(path))
            {
                foreach (string j in Directory.GetDirectories(i))
                {
                    foreach (string k in Directory.GetFiles(j))
                    {
                        if (k.Contains(file))
                        {
                            return k;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        //de file is niet gevonden
        return null;
    }
}

public class Schrijver
{
    /// <summary>
    /// Deze class wordt gebruikt voor het schrijven naar de json file
    /// </summary>
    public void UpdateGebruikers(List<GebruikerModel> gebruikers)
    {
        //zetten de lijst met objecten om naar JSON notatie (string)
        string users = JsonConvert.SerializeObject(gebruikers.ToArray());
        try
        {
            //schrijf de JSON (string) naar de file toe
            File.WriteAllText(new Zoeker().SearchFile("gebruikers.json"), users);
        }
        catch
        {
            //er is niet naar de file kunnen schrijven
            Console.WriteLine("De Gebruikers data is niet bijgewerkt! Probeer het nogmaals!");
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
            string users = File.ReadAllText(new Zoeker().SearchFile("gebruikers.json"));
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
            Console.WriteLine("Gebruikers data is niet gevonden!");
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
        Console.WriteLine("Gebruikersbeheer");
        Console.WriteLine("ESC terug naar het menu\t\tINS nieuwe gebruiker aanmaken");
        Console.WriteLine("\nIngelogd: " + admin.naam);
        Console.WriteLine("ACcount-type: " + ((admin.rechten) ? "Admin" : "Medewerker"));
    }
    public void PrintGebruikers(List<GebruikerModel> users, GebruikerModel admin)
    {
        ///<summary>
        ///Deze functie print alle gebruikers naar de console toe
        /// </summary>
        /// 
        Console.WriteLine("{0, -5}{1, -20}{2, -18}{3, -5}{4, -15}{5,0}", "Nr", "Naam", "Gebruikersnaam", "ID", "Wachtwoord", "Rechten");
        int index = 1;
        foreach (var gebruiker in users)
        {
            if (!(gebruiker.GebruikersID == admin.GebruikersID) && admin.rechten)
            {
                Console.WriteLine("{0, -5}{1, -20}{2, -18}{3, -5}{4, -15}{5,0}", index, gebruiker.naam, gebruiker.gebruikersnaam, gebruiker.GebruikersID, gebruiker.zieWachtwoordt(admin), ((gebruiker.rechten)? "Admin":"Medewerker"));
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
            Console.WriteLine("FilmHaus Gebruikerbeheer\n");
            GebruikerModel temp = user;
            Console.WriteLine("Aanpassingen opgeslagen: " + opgeslagen + "\n");
            Console.WriteLine("Aanpassen gebruiker: " + temp.naam + "\nESC Terug naar overzicht(CANCEL)\t DEL Gebruikerverwijderen\nINS Opslaan\n");
            Console.WriteLine("1. ID:             " + temp.GebruikersID);
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
                string keuze = ReadWithSpecialKeys();
                //schone zaak
                Console.Clear();
                switch (keuze)
                {
                    //hierin wordt de id aangepast
                    case "1":
                        PrintOpties();
                        Console.WriteLine("ID: " + temp.GebruikersID);
                        change = Verander();
                        //terug naar menu zonder aanpassing te maken
                        if (change.Equals("ESC"))
                            break;
                        else
                            if (Int32.TryParse(change, out _))
                            temp.GebruikersID = Int32.Parse(change);
                        else
                        {
                            Console.WriteLine("\nFout! De ID kan alleen een nummer zijn!");
                            Thread.Sleep(500);
                        }
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
        //zetten de string om naar een geldig index
        int locatie = Int32.Parse(index) - 1;
        //kijken waar de admin zit
        int locAdmin = data.FindIndex(x => x.GebruikersID == admin.GebruikersID) - 1;
        //tellen 1 erbij op zodat de admin niet kan worden aangepast
        //if (locAdmin < locatie)
           // locatie++;

        try
        {


            GebruikerModel temp = null;
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
        catch
        {
            Console.WriteLine("\nSelecteer aub een gebruiker die wel in de lijst staat");
            Thread.Sleep(500);
        }
    }

    void VoegGebruikerToe(List<GebruikerModel> data)
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
        int genID = data.Max(x => x.GebruikersID) + 1;
        //kijken welke rechten worden toegekent
        bool rechten = false;
        if (user[3].Contains("admin"))
            rechten = true;
        //nieuwe gebruiker aanmaken
        GebruikerModel gebruiker = new GebruikerModel(user[0], user[1],  genID, user[2], rechten);
        //gebruiker toevoegen
        data.Add(gebruiker);
    }
    public void AdminsRechten(List<GebruikerModel> data, GebruikerModel admin)
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
                if (Int32.Parse(keuze) < data.Count)
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