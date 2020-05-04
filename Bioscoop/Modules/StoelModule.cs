using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Bioscoop.Helpers;
using Bioscoop.Repository;
using Bioscoop.Models;

namespace Bioscoop.Modules
{
    public class StoelModule
    {
        public StoelData nieuwe_json = new StoelData();

        public void Run()
        {
            bool abort = false;
            String error = "";

            while (!abort)
            {
                Console.Clear();
                if (!String.IsNullOrEmpty(error)) Helpers.Display.PrintLine("Error: " + error + "\n");
                Helpers.Display.PrintLine("Stoelbeheer");
                Helpers.Display.PrintLine("ESC - Terug naar het menu \n");
                Helpers.Display.PrintLine("Om de stoelen van een zaal te bekijken, vul de nummer voor de zaal in \n");
                Helpers.Display.PrintHeader("Nr.", "Omschrijving", "Status", "Scherm");

                var zalen = ZaalData.LoadData();
                ZaalData.SortData();
                int nummering = 1;
                foreach (ZaalModel zaal in zalen)
                {
                    Helpers.Display.PrintTable(nummering.ToString(), zaal.Omschrijving, zaal.Status, zaal.Scherm);
                    nummering++;
                }
                Helpers.Display.PrintLine(" ");
                Helpers.Display.PrintLine("Type je keuze in en sluit af met een enter");

                //userinput functie opvragen in Helpers/Inputs
                Inputs.KeyInput input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 1;
                        if (inputValue > 0 && inputValue <= zalen.Count)
                        {
                            error = "";
                            StoelMain(inputValue);
                        }
                        else error = "Onjuist waarde ingevuld.";
                        break;
                    case Inputs.KeyAction.Escape: //de functie beeindigen
                        abort = true;
                        break;
                }
            }
        }

        //OVERZICHT
        private void StoelMain(int zaalnummer)
        {
            Console.Clear();
            List<StoelModel> stoelen = StoelLoadJson();
            bool abort = false;

            while (!abort)
            {
                Helpers.Display.PrintLine("Stoelbeheer: Zaal " + zaalnummer );
                Helpers.Display.PrintLine("ESC - Naar menu                       INS - Maak een nieuwe stoel");
                Helpers.Display.PrintLine("                                      DEL - Pas stoel aan of verwijder stoel");
                Helpers.Display.PrintHeader("Nummer", "Omschrijving", "Rij", "Stoelnummer", "Premium");

                //data opvragen en weergeven
                int numering = 1;
                foreach (StoelModel stoel in stoelen)
                {
                    if (stoel.ZaalId == zaalnummer + 1)
                    {
                        Helpers.Display.PrintTable(numering.ToString(), stoel.Omschrijving, stoel.Rij, stoel.StoelNr.ToString(), stoel.Premium.ToString());
                        numering++;
                    }
                }

                //input INS of ESC of DEL
                Inputs.KeyInput input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Insert:
                        VoegStoelToe();
                        break;
                    case Inputs.KeyAction.Escape:
                        abort = true;
                        break;
                    case Inputs.KeyAction.Delete:
                        StoelNummerAanpassen();
                        break;
                }
            }
        }

        public void StoelNummerAanpassen()
        {
            Console.Clear();
            Helpers.Display.PrintLine("Stoelbeheer");
            Helpers.Display.PrintLine("ESC - Terug naar overzicht                       \n");
            Helpers.Display.PrintLine("INS - Typ het stoelnummer dat u wilt aanpassen/verwijderen");

            switch (Helpers.Display.Keypress())
            {
                case ConsoleKey.Insert:
                    Helpers.Display.PrintLine("\nStoelnummer:");
                    var ans = Console.ReadLine();
                    if (ans.All(char.IsDigit))
                    {
                        List<StoelModel> json = StoelLoadJson();
                        int x = int.Parse(ans);
                        foreach (StoelModel stoel in json)
                        {
                            if (stoel.StoelId == x)
                            {
                                StoelenAanpas(x);
                            }
                        }
                    }
                    break;
                case ConsoleKey.Escape:
                    Run();
                    break;
                default:
                    Helpers.Display.PrintLine("Dat is geen geldige input of de stoel bestaat niet");
                    Helpers.Display.PrintLine("ESC - Terug naar overzicht                    INS - Probeer opnieuw");
                    switch (Helpers.Display.Keypress())
                    {
                        case ConsoleKey.Insert:
                            StoelNummerAanpassen();
                            break;
                        case ConsoleKey.Escape:
                            Run();
                            break;
                        default:
                            StoelNummerAanpassen();
                            break;
                    }
                    break;
            }
        }

        //STOEL TOEVOEGEN OF VERWIJDEREN /toevoegen
        public void VoegStoelToe()
        {
            bool geldigeInput = true;
            //premium(premiumAns) vragen
            Console.Clear();
            string premiumAns = null;
            Helpers.Display.PrintHeader("Stoelenbeheer");
            Helpers.Display.PrintLine("Voeg een stoel toe");
            Helpers.Display.PrintLine("\nIs de stoel premium?");
            Helpers.Display.PrintLine("      >[A] Ja");
            Helpers.Display.PrintLine("      >[B] Nee");
            premiumAns = Console.ReadLine();

            //zaalid(zaalid) vragen
            string ans = null;
            Helpers.Display.PrintLine("\nIn welke zaal staat de stoel?");
            Helpers.Display.PrintLine("Vul het zaalnummer in:");
            ans = Console.ReadLine();

            //premium(premiumAns -> premium) checken
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
            else
            {
                geldigeInput = false;
            }

            //stoelid bepalen
            string jsonFilePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Stoel.json"; ;
            string _json = File.ReadAllText(jsonFilePath);

            var stoelData = JsonConvert.DeserializeObject<List<StoelModel>>(_json);
            int stoelId = stoelData.Max(r => r.StoelId) + 1;

            //stoelnummer(z) bepalen:
            string stoelans = null;
            Helpers.Display.PrintLine("\nWelk stoelnummer heeft de stoel?");
            Helpers.Display.PrintLine("Vul de stoelnummer in (1-20)");
            stoelans = Console.ReadLine();
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
            string str = null;
            Helpers.Display.PrintLine("\nIn welke rij staat de stoel?");
            Helpers.Display.PrintLine("Vul de rij in (letter A - J)");
            str = Console.ReadLine();
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
                string omschr = "Rij " + str + " Stoel " + z;
                Helpers.Display.PrintLine(omschr);

                //checken of niet een stoel uit diezelfde zaal dezelfde omschrijving heeft
                List<StoelModel> stoelen = StoelLoadJson();
                foreach (StoelModel stoel in stoelen)
                {
                    Helpers.Display.PrintLine(stoel.Omschrijving);
                    if (stoel.ZaalId == z)
                    {
                        Helpers.Display.PrintLine(stoel.Omschrijving);
                        if (stoel.Omschrijving == omschr)
                        {
                            Helpers.Display.PrintLine("De ingevulde zijn niet correct omdat de stoel al bestaat, \ner kan geen nieuwe stoel aangemaakt worden.");
                            Helpers.Display.PrintLine("ESC - Terug naar overzicht                    INS - Probeer opnieuw");
                            switch (Helpers.Display.Keypress())
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
                }
                StoelToevoegen(stoelId, omschr, str, z, premium, zaalid);
            }
            //foutmelding als een van de gegevens niet kloppen:
            else
            {
                Helpers.Display.PrintLine("De ingevulde zijn niet correct, er kan geen nieuwe stoel aangemaakt worden.");
                Helpers.Display.PrintLine("ESC - Terug naar overzicht                    INS - Probeer opnieuw");
                switch (Helpers.Display.Keypress())
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



        public void StoelToevoegen(int stoelid, string omschrijving, string rij, int stoelnr, bool premium, int zaalid)
        {
            //object maken en stoel toevoegen
            dynamic array = this.nieuwe_json.GetJson();
            string str = JsonConvert.SerializeObject(array);
            List<StoelModel> list = JsonConvert.DeserializeObject<List<StoelModel>>(str);

            StoelModel nieuwe_stoel = new StoelModel();
            nieuwe_stoel.StoelId = stoelid;
            nieuwe_stoel.Omschrijving = omschrijving;
            nieuwe_stoel.Rij = rij;
            nieuwe_stoel.StoelNr = stoelnr;
            nieuwe_stoel.Premium = premium;
            nieuwe_stoel.ZaalId = zaalid;
            list.Add(nieuwe_stoel);
            string str2 = JsonConvert.SerializeObject(list);
            dynamic obj = JsonConvert.DeserializeObject(str2);
            nieuwe_json.UpdateJson(obj);
            Console.Clear();
            Helpers.Display.PrintLine("Stoelbeheer");
            Helpers.Display.PrintLine("De stoel wordt toegevoegd\nESC - Terug naar overzicht         INS - nieuwe stoel");
            switch (Helpers.Display.Keypress())
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

        //STOELEN AANPAS SCHERM
        public void StoelenAanpas(int stoelNummer)
        {
            Console.Clear();
            Helpers.Display.PrintLine("Stoelbeheer");
            Helpers.Display.PrintLine("Pas stoel " + stoelNummer + " aan\n");
            Helpers.Display.PrintHeader("Nr", "Omschrijving", "Rij", "Stoelnummer", "Premium", "Status");

            //stoel in json zoeken
            List<StoelModel> stoelen = StoelLoadJson();
            int nummering = 1;
            foreach (StoelModel stoel in stoelen)
            {
                if (stoel.StoelId == stoelNummer)
                {
                    //als die de stoel gevonden heeft, geeft die de gegevens ervan
                    Helpers.Display.PrintTable(nummering.ToString(), stoel.Omschrijving, stoel.Rij, stoel.StoelNr.ToString(), stoel.Premium.ToString(), stoel.ZaalId.ToString());
                    nummering++;
                }
            }

            Helpers.Display.PrintLine("\nWat wilt u aanpassen");
            Helpers.Display.PrintLine("ESC - Terug naar overzicht                    INS - Verander premium");
            Helpers.Display.PrintLine("                                              DEL - Verwijder stoel");

            switch (Helpers.Display.Keypress())
            {
                case ConsoleKey.Insert:
                    ChangePremium(stoelNummer);
                    break;
                case ConsoleKey.Escape:
                    Run();
                    break;
                case ConsoleKey.Delete:
                    StoelVerwijderen(stoelNummer);
                    break;
                default:
                    Helpers.Display.PrintLine("Dat is geen geldige input.");
                    Helpers.Display.PrintLine("ESC - Terug naar overzicht                    INS - Probeer opnieuw");
                    switch (Helpers.Display.Keypress())
                    {
                        case ConsoleKey.Insert:
                            StoelenAanpas(stoelNummer);
                            break;
                        case ConsoleKey.Escape:
                            Run();
                            break;
                        default:
                            StoelenAanpas(stoelNummer);
                            break;
                    }
                    break;
            }
        }

        //deze functie laadt de json
        public static List<StoelModel> StoelLoadJson()
        {
            string jsonFilePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Stoel.json";
            string _json = File.ReadAllText(jsonFilePath);

            return JsonConvert.DeserializeObject<List<StoelModel>>(_json);
        }

        //Deze functie veranderd true - false en terug van premium
        public void ChangePremium(int stoelNummer)
        {
            Console.Clear();
            List<StoelModel> json = StoelLoadJson();
            foreach (StoelModel stoel in json)
            {
                if (stoel.StoelId == stoelNummer)
                {
                    if (stoel.Premium)
                    { stoel.Premium = false; }
                    else
                    { stoel.Premium = true; }
                }
            }

            using (StreamWriter file = File.CreateText(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Stoel.json"))
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
                List<StoelModel> json = StoelLoadJson();
                foreach (StoelModel stoel in json)
                {
                    if (stoel.StoelId == x)
                    {
                        return stoelnummer;
                    }
                }
            }
            else
            {
                Helpers.Display.PrintLine("Geen geldige input, probeer het nogmaals");
                stoelnummer = Console.ReadLine();
                GeldigStoelnummer(stoelnummer);
            }
            return "";
        }

        public void StoelVerwijderen(int a)
        {
            dynamic array = this.nieuwe_json.GetJson();
            string str = JsonConvert.SerializeObject(array);
            List<StoelModel> list = JsonConvert.DeserializeObject<List<StoelModel>>(str);
            list.RemoveAll(r => r.StoelId == a);
            string str2 = JsonConvert.SerializeObject(list);
            dynamic obj = JsonConvert.DeserializeObject(str2);
            nieuwe_json.UpdateJson(obj);
            Console.Clear();
            Helpers.Display.PrintLine("stoel nummer " + a + " is verwijderd, Typ enter om naar overzicht te gaan");
            string ans = Console.ReadLine();
        }
    }
}