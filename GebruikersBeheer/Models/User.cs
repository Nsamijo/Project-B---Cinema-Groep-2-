using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

public class Gebruiker
{
    //naam van de persoon
    public string naam;
    //gebruikersnaam (accountnaam)
    public string gebruikersnaam;
    //gebruikers id
    public string id;
    //wachtwoord (sleutel naar de account toe)
    public string wachtwoord;
    //rechten (wat kan de gebruiker wel of niet doen)
    public bool rechten;

    public Gebruiker(string naam, string gebruikersnaam, string id, string wachtwoord, bool rechten)
    {
        //volledige naam (Voor en achternaam)
        this.naam = naam;
        //gebruikersnaam (naam voor inloggen oftewel accountnaam)
        this.gebruikersnaam = gebruikersnaam;
        //wijs het id toe
        this.id = id;
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
        if (admin.rechten)
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

    public Gebruiker Clone()
    {
        //geeft een shallow copy terug van het object
        //gebruikt om aanpassingen te maken zonder dat dit effect
        //heeft op het aan te passen object
        return (Gebruiker)this.MemberwiseClone();
    }
}