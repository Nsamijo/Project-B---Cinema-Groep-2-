using Bioscoop.Models;
using Bioscoop.Repository;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Bioscoop.Modules
{
    public class LoginModule
    {
        GebruikerModel Ingelogd = null;
        string Wachtwoordt()
        {
            ///<summary>
            ///Deze functie vraagt de wachtwoordt op en zet sterretjes
            ///hierdoor is de wachtwoordt niet zichtbaar voor derden 
            ///als deze wordt ingevoerd.
            /// </summary>
            Console.Write(">");
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

        GebruikerModel Inloggen(List<GebruikerModel> gebruikers)
        {
            ///<summary>
            ///Vraag de account details aan van de gebruiker
            ///en dan kijken of deze echt bestaat
            ///Zo ja dan wordt de account terug gegeven
            ///Zo niet dan wordt een null terug gegeven
            /// </summary>
            Console.Clear();
            Helpers.Display.PrintLine("Bioscoop Inlog Portaal");
            Helpers.Display.PrintMenu("ESC - Terug");
            Helpers.Display.PrintLine("");
            Helpers.Display.PrintLine("Gebruikersnaam: ");
            string gebruiker = new GebruikersMenu().ReadWithSpecialKeys();

            while (gebruiker.Equals("DEL") || gebruiker.Equals("INS"))
            {
                Helpers.Display.PrintLine("\nOnbekende toets! Voer uw gebruikersnaam opnieuw in\n");
                Helpers.Display.PrintLine("Gebruikersnaam: ");
                gebruiker = new GebruikersMenu().ReadWithSpecialKeys();
            }

            if (gebruiker.Equals("ESC"))
                return new GebruikerModel("cancel", "cancel", 0, "cancel", false);

            Helpers.Display.PrintLine("");
            Helpers.Display.PrintLine("Wachtwoord: ");

            string pass = Wachtwoordt();
            //als de sessie wordt onderbroken
            if (pass.Equals("ESC"))
                return new GebruikerModel("cancel", "cancel", 0, "cancel", false);

            //kijken of de account een bestaande account is
            foreach (GebruikerModel i in gebruikers)
            {
                {
                    //komen de gebruikersnamen overeen
                    if (i.Gebruikersnaam.Equals(gebruiker))
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
        public void Login(List<GebruikerModel> data)
        {
            //inloggen in een account
            Ingelogd = Inloggen(data);
            //kijken of er werkelijk is ingelogd
            if (this.Ingelogd == null)
            {
                ///<summary>
                ///als er niet is ingelogd dan wordt dit vermeld
                /// </summary>
                Helpers.Display.PrintLine("\nNiet ingelogd!\nCheck je gebruikersnaam of wachtwoord!");
                Thread.Sleep(500);
            }
            else if (this.Ingelogd.Naam.Equals("cancel"))
            {
                ///<summary>
                ///Als de gebruiker beslist om te stoppen met inloggen
                /// </summary>
                Helpers.Display.PrintLine("\n\nInloggen is geannuleerd");
            }
        }
        public GebruikerModel NuIngelogd()
        {
            //deze functie geeft de ingelogde terug
            return this.Ingelogd;
        }
    }
}