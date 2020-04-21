using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class Lezer
{
    public List<Gebruiker> gebruikersInlezen()
    {
        try
        {
            string users = File.ReadAllText(new Finder().SearchFile("users.json"));
            JArray temp = JArray.Parse(users);
            List<Gebruiker> gebruikers = temp.ToObject<List<Gebruiker>>();
            return gebruikers;
        }
        catch(Exception e)
        {
            Console.WriteLine("Unable to get users data");
        }
        return null;
    }
}

public class Schrijver
{
    public void updateGebruikers(List<Gebruiker> gebruikers)
    {
        string users = JsonConvert.SerializeObject(gebruikers.ToArray());
        try
        {
            File.WriteAllText(new Finder().SearchFile("users.json"), users);
        }catch(Exception e)
        {
            Console.WriteLine("Unable to update the file");
        }
    }
}
