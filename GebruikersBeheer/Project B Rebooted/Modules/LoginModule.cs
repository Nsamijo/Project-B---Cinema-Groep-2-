using System;
using System.Collections.Generic;
using System.Text;

public class LoginModule
{
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

    GebruikerModel Login(List<GebruikerModel> gebruikers)
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
        string gebruiker = new GebruikersMenu().ReadWithSpecialKeys();

        while (gebruiker.Equals("DEL") || gebruiker.Equals("INS"))
        {
            Console.WriteLine("\nOnbekende toets! Voer aub uw gebruikersnaam opnieuw in\n");
            Console.Write("Gebruikersnaam: ");
            gebruiker = new GebruikersMenu().ReadWithSpecialKeys();
        }

        if (gebruiker.Equals("ESC"))
            return new GebruikerModel("cancel", "cancel", "cancel", "cancel", false);

        Console.Write("\nWachtwoord: ");

        string pass = Wachtwoordt();
        //als de sessie wordt onderbroken
        if (pass.Equals("ESC"))
            return new GebruikerModel("cancel", "cancel", "cancel", "cancel", false);

        //kijken of de account een bestaande account is
        foreach (GebruikerModel i in gebruikers)
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
    public GebruikerModel Login(GebruikerModel inloggen, List<GebruikerModel> data)
    {
        //inloggen in een account
        inloggen = Login(data);
        //kijken of er werkelijk is ingelogd
        if (inloggen == null)
        {
            ///<summary>
            ///als er niet is ingelogd dan wordt dit vermeld
            /// </summary>
            Console.WriteLine("\nNiet ingelogd!\nCheck uw gebruikersnaam of wachtwoord!");
        }
        else if (inloggen.naam.Equals("cancel"))
        {
            ///<summary>
            ///Als de gebruiker beslist om te stoppen met inloggen
            /// </summary>
            Console.WriteLine("\n\nInloggen is gecanceld.");
        }

        return inloggen;
    }
}
