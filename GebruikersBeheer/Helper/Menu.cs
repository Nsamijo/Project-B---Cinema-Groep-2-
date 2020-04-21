using System;
using System.Collections.Generic;
using System.Text;

class GebruikersMenu
{
    public void PrintGebruikers(List<Gebruiker> users, Gebruiker admin)
    {
        foreach(var gebruiker in users)
        {
            if (!gebruiker.naam.Equals(admin.naam))
            {
                Console.WriteLine("{0, 0}{1, -20}{2, -25}{3, -25}", gebruiker.naam, gebruiker.gebruikersnaam, gebruiker.zieWachtwoordt(admin), gebruiker.rechten);
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
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
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

    public string ReadWithCancel()
    {
        ///<summary>
        ///Deze functie catch special keys
        ///Kan verder worden uitgebreid
        /// </summary>
        string result = "";
        ConsoleKeyInfo key = Console.ReadKey(true);
        while(key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape)
        {
            if (key.Key != ConsoleKey.Backspace && nietArrows(key))
            {
                Console.Write(key.KeyChar);
                result += key.KeyChar;
            }
            else if(result.Length > 0 && key.Key == ConsoleKey.Backspace)
            {
                Console.Write("\b\b");
                result = result.Substring(0, result.Length - 1);
            }
            key = Console.ReadKey(true);
        }

        if(key.Key == ConsoleKey.Escape)
        {
            return "-1";
        }

        return result;
    }

    public Gebruiker Login(List<Gebruiker> gebruikers)
    {
        Console.WriteLine("FilmHaus Inloggen:\n");
        Console.Write("Gebruikersnaam: ");
        string gebruiker = ReadWithCancel();

        if(gebruiker.Equals("-1"))
        {
            return new Gebruiker("cancel", "cancel", "cancel", "cancel");
        }
        Console.Write("\nWachtwoord: ");
        string pass = Wachtwoordt();
        foreach(Gebruiker i in gebruikers)
        {
            {
                if (i.gebruikersnaam.Equals(gebruiker))
                {
                    if (i.checkWachtwoord(pass))
                    {
                        return i;
                    }
                }
            }
        }
        return null;
    }
}
