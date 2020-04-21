using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Hierin zal de gehele functionaliteit plaatsvinden van de gebruikers
/// Hierbij hoort:
/// 1. Inloggen
/// 2. Gebruikers toevoegen
/// 3. Bestaande gebruikers verwijderen
/// 4. Bestaande gebruikers rechten veranderen
/// </summary>
class Manager
{
    //lijst voor de gebruikers (wordt aan het begin ingelezen)
    private List<Gebruiker> Data = new List<Gebruiker>();
    //huidige gebruiker die is ingelogd
    private Gebruiker Ingelogd;
    //bool voor eerste runtime hierdoor wordt data niet meer dan
    //1 keer ingelezen
    private bool ingelezen = false;

    void DataInladen()
    {
        //data inlezen van de gebruikers
        this.Data = new Lezer().gebruikersInlezen();
    }
    public void Login()
    {
        //inloggen in een account
        this.Ingelogd = new GebruikersMenu().Login(this.Data);
        //kijken of er werkelijk is ingelogd
        if (this.Ingelogd == null)
        {
            ///<summary>
            ///als er niet is ingelogd dan wordt dit vermeld
            /// </summary>
            Console.WriteLine("\nNiet ingelogd!\nCheck uw gebruikersnaam of wachtwoord!");
        }else if(this.Ingelogd.naam.Equals("cancel"))
        {
            ///<summary>
            ///Als de gebruiker beslist om te stoppen met inloggen
            /// </summary>
            Console.WriteLine("\n\nInloggen is gecanceld.");
        }
        else
        {
            ///<summary>
            ///De gebruiker is succesvol ingelogd
            ///De naam van de gebruiker wordt getoond en de rechten
            ///van deze.
            /// </summary>
            Console.Clear();
            Console.WriteLine("Ingelogd: " + this.Ingelogd.naam);
            Console.WriteLine("Account-type: " + this.Ingelogd.rechten);
        }
    }

    public void Run()
    {
        //gebruikers inlezen
        if(!this.ingelezen)
            this.DataInladen();
        //inloggen
        Login();
    }
}

