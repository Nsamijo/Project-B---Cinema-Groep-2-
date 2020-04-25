﻿using System;
using System.Collections.Generic;
using System.Text;

public class Gebruiker
{
    //naam van de persoon
    public string naam;
    //gebruikersnaam (accountnaam)
    public string gebruikersnaam;
    //wachtwoord (sleutel naar de account toe)
    public string wachtwoord;
    //rechten (wat kan de gebruiker wel of niet doen)
    public string rechten;

    public Gebruiker(string naam, string gebruikersnaam, string wachtwoord, string rechten)
    {
        //volledige naam (Voor en achternaam)
        this.naam = naam;
        //gebruikersnaam (naam voor inloggen oftewel accountnaam)
        this.gebruikersnaam = gebruikersnaam;
        //wachtwoord (sleutel naar de account)
        this.wachtwoord = wachtwoord;
        //rechten (bevoegheden van de gebruiker)
        this.rechten = rechten;
    }

    public bool checkWachtwoord(string ww)
    {
        ///check of de gegeven wachtwoord de juiste wachtwoord is
        return this.wachtwoord.Equals(ww);
    }

    public string zieWachtwoordt(Gebruiker admin)
    {
        ///<summary>
        ///de wachtwoorden zijn alleen te zien door de Admins
        /// </summary>
        /// 
        //kijken of de huidige gebruiker een admin is
        if (admin.rechten == "Admin")
        {
            //geven de wachtwoordt terug van de aangegeven gebruiker
            //of van zichzelf
            return this.wachtwoord;
        }
        //return niets als de gebruiker geen admin is
        return null;
    }

    public void resetWachtwoord(string ww, string nieuwww)
    {
        ///<summary>
        ///Kijken of de gebruiker de echte gebruiker is door
        ///te checken of de opnieuw ingevoerde wachtwoord zelfde is 
        ///als in het systeem
        /// </summary> 

        //check de doorgegeven wachtwoord
        if(checkWachtwoord(ww))
        {
            //nieuw wachtwoordt wordt gezet
            this.wachtwoord = nieuwww;
        }
    }
}