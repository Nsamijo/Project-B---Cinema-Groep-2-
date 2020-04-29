using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using Helpers;

public class Stoelenbeheer
{
    public StoelData.Data nieuwe_json = new StoelData.Data();

    public object Run()
    {
        string n = Vraagzaal();
        StoelMain(n);
        return "";
    }

    //eerste scherm die de gebruiker ziet
    public static string Vraagzaal()
    {
        Console.Clear();
        Helpers.StoelenDisplay.PrintLine("Stoelbeheer");
        Helpers.StoelenDisplay.PrintLine("ESC - Terug naar overzicht                       ");
        Helpers.StoelenDisplay.PrintLine("\nOm de stoelen van een zaal te bekijken, typ het Id van de zaal in");
        Helpers.StoelenDisplay.PrintLine("\n[1] Zaal 1");
        Helpers.StoelenDisplay.PrintLine("\n[2] Zaal 2");
        Helpers.StoelenDisplay.PrintLine("\n[3] Zaal 3");
        Helpers.StoelenDisplay.PrintLine("\n[4] Zaal 4");
        Helpers.StoelenDisplay.PrintLine("\n[5] Zaal 5");
        Helpers.StoelenDisplay.PrintLine("\n[6] Zaal 6");
        Helpers.StoelenDisplay.PrintLine("\n[7] Zaal 7");
        Helpers.StoelenDisplay.PrintLine("\n[8] Zaal 8");
        Helpers.StoelenDisplay.PrintLine("\n[9] Zaal 9");
        Helpers.StoelenDisplay.PrintLine("\n[10] Zaal 10");
        string ans = Console.ReadLine();

        //hier checkt die of de input een getal tussen de 1 en 10 is
        if (ans.All(char.IsDigit))
        {
            int x = int.Parse(ans);
            if (x >= 1 && x <= 10)
            {
                return ans;
            }
        }
        //anders doet die scherm 1 bij elk ander getal, moet ik nog aan werken
        return "1";
    }

    //OVERZICHT
    private void StoelMain(string zaalnummer = "1")
    {
        Console.Clear();
        List<Stoel> stoelen = StoelLoadJson();

        StoelenDisplay.PrintLine("Stoelbeheer");
        StoelenDisplay.PrintLine("ESC - Naar menu                       INS - stoel toevoegen/verwijderen");
        StoelenDisplay.PrintLine("\n[A] Typ, om een stoel aan te passen, het Id van een stoel in");
        StoelenDisplay.PrintHeader("Id", "Omschrijving", "Rij", "Stoelnummer", "Premium", "ZaalId");

        //data opvragen en weergeven
        int numering = 1;
        foreach (Stoel stoel in stoelen)
        {
            if (stoel.Zaalid == zaalnummer)
            {
                string StoelNummer = "" + stoel.StoelNr;
                string StoelPremium = "" + stoel.Premium;
                string stoelid = "" + stoel.StoelId;
                StoelenDisplay.PrintTable(numering.ToString(), stoel.Omschrijving, stoel.Rij, StoelNummer, StoelPremium, stoel.Zaalid);
                numering++;
            }
        }

        //input INS of ESC
        
        switch (StoelenDisplay.Keypress())
        {
            case ConsoleKey.Insert:
                StoelenAanmaken();
                break;
            case ConsoleKey.Escape:
                Run();
                break;
            default:
                var ans = Console.ReadLine();
                if (ans.All(char.IsDigit))
                {
                    ans = GeldigStoelnummer(ans);
                    int x = int.Parse(ans);
                    StoelenAanpas(x);
                }
                break;
        }
    }

    //STOEL TOEVOEGEN OF VERWIJDEREN
    public void StoelenAanmaken()
    {
        Console.Clear();
        Helpers.StoelenDisplay.PrintLine("Stoelenbeheer");
        Helpers.StoelenDisplay.PrintLine("[A] Voeg een stoel toe");
        Helpers.StoelenDisplay.PrintLine("[B] Verwijder een stoel");
        Helpers.StoelenDisplay.PrintLine("[C] Ga terug naar overzicht");
        var ans = Console.ReadLine();

        switch (ans)
        {
            case "A":
                VoegStoelToe();
                break;
            case "C":
                StoelMain();
                break;
        }
    }

    //STOEL TOEVOEGEN OF VERWIJDEREN /toevoegen
    public void VoegStoelToe()
    {
        bool geldigeInput = true;
        //premium(p) vragen
        Console.Clear();
        StoelenDisplay.PrintHeader("Stoelenbeheer");
        StoelenDisplay.PrintLine("Voeg een stoel toe");
        StoelenDisplay.PrintLine("\nIs de stoel premium?");
        StoelenDisplay.PrintLine("      >[A] Ja");
        StoelenDisplay.PrintLine("      >[B] Nee");
        string premiumAns = Console.ReadLine();

        //zaalid(zaalid) vragen
        StoelenDisplay.PrintLine("\nIn welke zaal staat de stoel?");
        StoelenDisplay.PrintLine("Vul het zaalnummer in:");
        string ans = Console.ReadLine();

        //premium(p) checken
        bool premium = true;
        switch (premiumAns)
        {
            case "A":
            case "a":
                premium = true;
                break;
            case "B":
            case "b":
                premium = false;
                break;
            default:
                geldigeInput = false;
                break;
        }

        //zaalid(zaalid) checken
        int zaalid = 1;
        if (ans.All(char.IsDigit))
        {
            int x = int.Parse(ans);
            if (x >= 1)
            {
                zaalid = x;
            }
            else
            {
                geldigeInput = false;
            }
        }

        //stoelid bepalen
        string jsonFilePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\data.json"; ;
        string _json = File.ReadAllText(jsonFilePath);

        var stoelData = JsonConvert.DeserializeObject<List<Stoel>>(_json);
        int stoelId = stoelData.Max(r => r.StoelId) + 1;

        //stoelnummer(z) bepalen:
        StoelenDisplay.PrintLine("\nWelk stoelnummer heeft de stoel?");
        StoelenDisplay.PrintLine("Vul de stoelnummer in (1-20)");
        var stoelans = Console.ReadLine();
        int z = 1;
        if (stoelans.All(char.IsDigit))
        {
            int x = int.Parse(stoelans);
            if (x >= 1 && x <= 20)
            {
                z = x;
            }
            else
            {
                geldigeInput = false;
            }
        }

        //rij(str) bepalen:
        StoelenDisplay.PrintLine("\nIn welke rij staat de stoel?");
        StoelenDisplay.PrintLine("Vul de rij in (letter A - J)");
        var str = Console.ReadLine();
        switch (str)
        {
            case "A":
            case "a":
                str = "A";
                break;
            case "B":
            case "b":
                str = "B";
                break;
            case "C":
            case "c":
                str = "C";
                break;
            case "D":
            case "d":
                str = "D";
                break;
            case "E":
            case "e":
                str = "E";
                break;
            case "F":
            case "f":
                str = "F";
                break;
            case "G":
            case "g":
                str = "G";
                break;
            case "H":
            case "h":
                str = "H";
                break;
            case "I":
            case "i":
                str = "I";
                break;
            case "J":
            case "j":
                str = "J";
                break;
            default:
                geldigeInput = false;
                break;
        }
        //gegevens checken en stoel aanmaken:
        if (geldigeInput == true)
        {
            string omschr = "Rij " + str + " stoel " + z;
            List<Stoel> stoelen = StoelLoadJson();
            foreach (Stoel stoel in stoelen)
            {
                if (stoel.Zaalid == z.ToString())
                {
                    if (stoel.Omschrijving == omschr)
                    {
                        StoelenDisplay.PrintLine("De ingevulde zijn niet correct omdat de stoel al bestaat, er kan geen nieuwe stoel aangemaakt worden.");
                        StoelenDisplay.PrintLine("ESC - Terug naar overzicht                    INS - Probeer opnieuw");
                        switch (StoelenDisplay.Keypress())
                        {
                            case ConsoleKey.Insert:
                                VoegStoelToe();
                                break;
                            case ConsoleKey.Escape:
                                Run();
                                break;
                            default:
                                Run();
                                break;
                        }
                    }
                }
                StoelToevoegen(stoelId, omschr, str, z, premium, zaalid.ToString());
            }
        }
        //foutmelding als een van de gegevens niet kloppen:
        else
        {
            StoelenDisplay.PrintLine("De ingevulde zijn niet correct, er kan geen nieuwe stoel aangemaakt worden.");
            StoelenDisplay.PrintLine("ESC - Terug naar overzicht                    INS - Probeer opnieuw");
            switch (StoelenDisplay.Keypress())
            {
                case ConsoleKey.Insert:
                    VoegStoelToe();
                    break;
                case ConsoleKey.Escape:
                    Run();
                    break;
                default:
                    Run();
                    break;
            }
        }
    }

    

    public void StoelToevoegen(int stoelid, string omschrijving, string rij, int stoelnr, bool premium, string zaalid)
    {
        //object maken en stoel toevoegen
        dynamic array = this.nieuwe_json.getJson();
        string str = JsonConvert.SerializeObject(array);
        List<Stoel> list = JsonConvert.DeserializeObject<List<Stoel>>(str);

        Stoel nieuwe_stoel = new Stoel();
        nieuwe_stoel.StoelId = stoelid;
        nieuwe_stoel.Omschrijving = omschrijving;
        nieuwe_stoel.Rij = rij;
        nieuwe_stoel.StoelNr = stoelnr;
        nieuwe_stoel.Premium = premium;
        nieuwe_stoel.Zaalid = zaalid;
        list.Add(nieuwe_stoel);
        string str2 = JsonConvert.SerializeObject(list);
        dynamic obj = JsonConvert.DeserializeObject(str2);
        nieuwe_json.UpdateJson(obj);
    }



    //STOELEN AANPAS SCHERM
    public void StoelenAanpas(int stoelNummer)
    {   
        Console.Clear();
        StoelenDisplay.PrintLine("Stoelbeheer");
        StoelenDisplay.PrintLine("Pas stoel " + stoelNummer + " aan");
        StoelenDisplay.PrintHeader("Id", "Omschrijving", "Rij", "Stoelnummer", "Premium", "Status");
        
        //stoel in json zoeken
        List<Stoel> stoelen = StoelLoadJson();
        foreach (Stoel stoel in stoelen)
        {
            if (stoel.StoelId == stoelNummer)
            {
                //als die de stoel gevonden heeft, geeft die de gegevens ervan
                string Stoelnummer = "" + stoel.StoelNr;
                string StoelPremium = "" + stoel.Premium;
                string Stoelid = "" + stoelNummer;
                StoelenDisplay.PrintTable(Stoelid, stoel.Omschrijving, stoel.Rij, Stoelnummer, StoelPremium, stoel.Zaalid);
            }
        }

        Helpers.StoelenDisplay.PrintLine("\nWat wilt u aanpassen");
        Helpers.StoelenDisplay.PrintLine("[A] Verander zaalnummer");
        Helpers.StoelenDisplay.PrintLine("[B] Verander premium");
        Helpers.StoelenDisplay.PrintLine("[C] Ga terug naar overzicht");

        //input
        string ans = Console.ReadLine();
        switch (ans)
        {
            case "A":
//                ChangeStatus(stoelNummer);
                break;
            case "B":
                ChangePremium(stoelNummer);
                break;
            case "C":
                StoelMain();
                break;
        }
    }

    //deze functie laadt de json
    public static List<Stoel> StoelLoadJson()
    {
        string jsonFilePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\data.json";
        string _json = File.ReadAllText(jsonFilePath);

        return JsonConvert.DeserializeObject<List<Stoel>>(_json);
    }

    //Deze functie veranderd true - false en terug van premium
    public void ChangePremium(int stoelNummer)
    {
        Console.Clear();
        List<Stoel> json = StoelLoadJson();
        foreach (Stoel stoel in json)
        {
            if (stoel.StoelId == stoelNummer)
            {
                if (stoel.Premium)
                { stoel.Premium = false; }
                else
                { stoel.Premium = true; }
            }
        }

        using (StreamWriter file = File.CreateText(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\data.json"))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, json);
        }

        StoelenAanpas(stoelNummer);
    }


    //deze functie checkt of de stoelnummer (die toegevoegd moet worden) bestaat en boven de 0 is
    public static string GeldigStoelnummer(string stoelnummer)
    {
        
        var ans = Console.ReadLine();
        if (ans.All(char.IsDigit))
        {
            int x = int.Parse(stoelnummer);
            List<Stoel> json = StoelLoadJson();
            foreach (Stoel stoel in json)
            {
                if (stoel.StoelId == x)
                {
                    return stoelnummer;
                }
            }
            StoelenDisplay.PrintLine("Geen geldige input, probeer het nogmaals");
            stoelnummer = Console.ReadLine();
            GeldigStoelnummer(stoelnummer);
        }
        
        return "";
    }

    public void StoelVerwijderen
        
}


/*    // deze functie veranderd Beschikbaar - Niet beschikbaar van de status
    public void ChangeStatus(int stoelNummer)
    {
        Console.Clear();
        List<Stoel> json = LoadJson();
        foreach (Stoel stoel in json)
        {
            if (stoel.StoelId == stoelNummer)
            {
                //veranderen van waarde (beschikbaar - niet beschikbaar en andersom)
                switch (stoel.Status)
                {
                    case "Beschikbaar":
                        stoel.Status = "Niet beschikbaar";
                        break;
                    case "Niet beschikbaar":
                        stoel.Status = "Beschikbaar";
                        break;
                }
            }
        }

        //streamwriter die terug naar json aanpast
        using (StreamWriter file = File.CreateText(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\data.json"))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, json);
        }

        StoelenAanpas(stoelNummer);
    }*/
