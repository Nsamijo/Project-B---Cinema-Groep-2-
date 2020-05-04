using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Bioscoop.Models
{
    public class GebruikerModel
    {
        //gebruikers id
        public int GebruikerId;
        //naam van de persoon
        public string Naam;
        //gebruikersnaam (accountnaam)
        public string Gebruikersnaam;
        //wachtwoord (sleutel naar de account toe)
        public string Wachtwoord;
        //rechten (wat kan de gebruiker wel of niet doen)
        public bool Rechten;

        public GebruikerModel(string naam, string gebruikersnaam, int id, string wachtwoord, bool rechten)
        {
            //volledige naam (Voor en achternaam)
            this.Naam = naam;
            //gebruikersnaam (naam voor inloggen oftewel accountnaam)
            this.Gebruikersnaam = gebruikersnaam;
            //wijs het id toe
            this.GebruikerId = id;
            //wachtwoord (sleutel naar de account)
            this.Wachtwoord = wachtwoord;
            //rechten (bevoegheden van de gebruiker)
            this.Rechten = rechten;
        }

        public bool checkWachtwoord(string ww)
        {
            ///check of de gegeven wachtwoord de juiste wachtwoord is
            return this.Wachtwoord.Equals(ww);
        }

        public string zieWachtwoordt(GebruikerModel admin)
        {
            ///<summary>
            ///de wachtwoorden zijn alleen te zien door de Admins
            /// </summary>
            /// 
            //kijken of de huidige gebruiker een admin is
            if (admin.Rechten)
            {
                //geven de wachtwoordt terug van de aangegeven gebruiker
                //of van zichzelf
                return this.Wachtwoord;
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
            if (checkWachtwoord(ww))
            {
                //nieuw wachtwoordt wordt gezet
                this.Wachtwoord = nieuwww;
            }
        }

        public GebruikerModel Clone()
        {
            //geeft een shallow copy terug van het object
            //gebruikt om aanpassingen te maken zonder dat dit effect
            //heeft op het aan te passen object
            return (GebruikerModel)this.MemberwiseClone();
        }
    }
}