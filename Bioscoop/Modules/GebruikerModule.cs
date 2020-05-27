using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Bioscoop.Models;
using Bioscoop.Repository;

namespace Bioscoop.Modules
{
    /// <summary>
    /// Hierin zal de gehele functionaliteit plaatsvinden van de gebruikers
    /// Hierbij hoort:
    /// 1. Inloggen
    /// 2. Gebruikers toevoegen
    /// 3. Bestaande gebruikers verwijderen
    /// 4. Bestaande gebruikers rechten veranderen
    /// </summary>
    class GebruikerModule//Nathan
    {
        //lijst voor de gebruikers (wordt aan het begin ingelezen)
        private List<GebruikerModel> Data = new List<GebruikerModel>();
        //huidige gebruiker die is ingelogd
        private GebruikerModel Ingelogd = null;
        //bool voor eerste runtime hierdoor wordt data niet meer dan
        //1 keer ingelezen
        private bool ingelezen = false;
        //object die de menu voorziet
        GebruikersMenu menu;
        LoginModule login;

        void DataInladen(LoginModule login)
        {
            //data inlezen van de gebruikers
            this.Data = new Lezer().gebruikersInlezen();
            this.menu = new GebruikersMenu();
            this.login = login;
        }

        void GebruikersBeheer(LoginModule login)
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
                this.DataInladen(login);

            //inloggen
            if (this.Ingelogd == null)
            {
                //kijken wie is ingelogd
                this.Ingelogd = login.NuIngelogd();
            }

            //in de if springen als er is ingelogd zodat de admin
            //aanpassingen kan maken
            if (this.Ingelogd != null && !this.Ingelogd.GebruikerId.Equals("cancel"))
            {
                //kijken welke rechten de ingelogde heeft
                switch (this.Ingelogd.Rechten)
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

        public void Run(LoginModule login)
        {
            Console.CursorVisible = true;
            this.GebruikersBeheer(login);
        }
    }
}