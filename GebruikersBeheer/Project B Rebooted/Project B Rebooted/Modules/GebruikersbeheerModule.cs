﻿using System;
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
class GebruikersbeheerModule
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
    LoginModule login;

    void DataInladen()
    {
        //data inlezen van de gebruikers
        this.Data = new Lezer().gebruikersInlezen();
        this.menu = new GebruikersMenu();
        login = new LoginModule();
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
            this.Ingelogd = this.login.Login(this.Ingelogd, this.Data);
        }

        //in de if springen als er is ingelogd zodat de admin
        //aanpassingen kan maken
        if ( this.Ingelogd != null && !this.Ingelogd.GebruikersID.Equals("cancel"))
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
