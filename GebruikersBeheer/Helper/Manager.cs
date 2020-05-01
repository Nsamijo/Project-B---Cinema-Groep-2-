using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
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
    private Gebruiker Ingelogd = null;
    //bool voor eerste runtime hierdoor wordt data niet meer dan
    //1 keer ingelezen
    private bool ingelezen = false;
    //object die de menu voorziet
    GebruikersMenu menu;

    void DataInladen()
    {
        //data inlezen van de gebruikers
        this.Data = new Lezer().gebruikersInlezen();
        this.menu = new GebruikersMenu();
    }
    public void Login()
    {
        //inloggen in een account
        this.Ingelogd = this.menu.Login(this.Data);
        //kijken of er werkelijk is ingelogd
        if (this.Ingelogd == null)
        {
            ///<summary>
            ///als er niet is ingelogd dan wordt dit vermeld
            /// </summary>
            Console.WriteLine("\nNiet ingelogd!\nCheck uw gebruikersnaam of wachtwoord!");
        }
        else if (this.Ingelogd.naam.Equals("cancel"))
        {
            ///<summary>
            ///Als de gebruiker beslist om te stoppen met inloggen
            /// </summary>
            Console.WriteLine("\n\nInloggen is gecanceld.");
        }
    }

    void GebruikersBeheer()
    {
        ///<summary>
        ///Deze functie zal voor alle gebruikersbeheer zijn
        ///-verwijderen van een gebruiker
        ///-toevoegen van een gebruiker
        ///-aanpassen van een gebruiker (Naam, gebruikersnaam, rechten en wachtwoord)
        ///-Inzien van een gebruiker
        /// </summary>
        /// 

        //gebruikers inlezen
        if (!this.ingelezen)
            this.DataInladen();

        //inloggen
        if (this.Ingelogd == null)
        {
            Login();
        }

        //in de if springen als er is ingelogd zodat de admin
        //aanpassingen kan maken
        if (this.Ingelogd != null && !this.Ingelogd.id.Equals("cancel"))
        {
            //kijken welke rechten de ingelogde heeft
            switch(this.Ingelogd.rechten)
            {
                //admin rechten
                case true:
                    menu.AdminsRechten(this.Data, this.Ingelogd);
                    break;

                //medewerker rechten
                case false:
                    break;
            }
        }
    }

    public void Run()
    {
        this.GebruikersBeheer();
    }
}

