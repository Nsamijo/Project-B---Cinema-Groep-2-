using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/// <summary>
/// Deze file heeft 2 classes nl:
/// 1. Lezer => heeft 1 functie die alleen is bedoelt om de data in te lezen
/// 2. Schrijver => heeft 1 functie die alleen is bedoelt om de data naar de JSON te schrijven
/// </summary>
public class Lezer
{
    public List<Gebruiker> gebruikersInlezen()
    {
        try
        {
            //zoek de locatie op van de JSON en lees deze in
            string users = File.ReadAllText(new Zoeker().SearchFile("users.json"));
            //de ingelezen data omzetten naar een JArray
            JArray temp = JArray.Parse(users);
            //de JArray omzetten naar Gebruiker objecten
            List<Gebruiker> gebruikers = temp.ToObject<List<Gebruiker>>();
            //alle gebruikers terug geven
            return gebruikers;
        }
        catch(Exception e)
        {
            //de JSON locatie is niet gevonden
            Console.WriteLine("Unable to get users data");
        }
        //als de JSON niet kan worden ingelezen wordt er een null terug gegeven
        return null;
    }
}

public class Schrijver
{
    public void updateGebruikers(List<Gebruiker> gebruikers)
    {
        //zetten de lijst met objecten om naar JSON notatie (string)
        string users = JsonConvert.SerializeObject(gebruikers.ToArray());
        try
        {
            //schrijf de JSON (string) naar de file toe
            File.WriteAllText(new Zoeker().SearchFile("users.json"), users);
        }catch(Exception e)
        {
            //er is niet naar de file kunnen schrijven
            Console.WriteLine("Unable to update the file");
        }
    }
}
