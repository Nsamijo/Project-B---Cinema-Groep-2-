using System;
using System.Collections.Generic;
using System.Text;

public class Gebruiker
{
    public string naam;
    public string gebruikersnaam;
    public string wachtwoord;
    public string rechten;

    public Gebruiker(string naam, string gebruikersnaam, string wachtwoord, string rechten)
    {
        this.naam = naam;
        this.gebruikersnaam = gebruikersnaam;
        zetWachtwoord(wachtwoord);
        this.rechten = rechten;
    }
    void zetWachtwoord(string ww)
    {
        ///Deze functie zet de wachtwoord in de eerste instantie
        this.wachtwoord = ww;
    }

    public bool checkWachtwoord(string ww)
    {
        ///check of de gegeven wachtwoord de juiste wachtwoord is
        return this.wachtwoord.Equals(ww);
    }

    public string zieWachtwoordt(Gebruiker admin)
    {
        if (admin.rechten == "Admin")
        {
            return this.wachtwoord;
        }
        return null;
    }

    public void resetWachtwoord(string ww, string nieuwww)
    {
        ///zet een nieuw wachtwoord
        if(checkWachtwoord(ww))
        {
            this.wachtwoord = nieuwww;
        }
    }
}