using System;
using System.Collections.Generic;
using System.Text;
class Manager
{
    private List<Gebruiker> Data = new List<Gebruiker>();
    private Gebruiker Ingelogd;
    public void Login()
    {
        this.Data = new Lezer().gebruikersInlezen();
        this.Ingelogd = new GebruikersMenu().Login(this.Data);
        if (this.Ingelogd == null)
        {
            Console.WriteLine("\nNiet ingelogd!\nCheck uw gebruikersnaam of wachtwoord!");
        }else if(this.Ingelogd.naam.Equals("cancel"))
        {
            Console.WriteLine("\nInloggen is gecanceld.");
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Ingelogd: " + this.Ingelogd.naam);
            Console.WriteLine("Account-type: " + this.Ingelogd.rechten);
        }
    }
}

