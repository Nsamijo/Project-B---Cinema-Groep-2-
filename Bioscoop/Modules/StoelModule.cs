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
    public class StoelModule//Judith
    {
        public StoelData nieuwe_json = new StoelData();
        public List<StoelModel> data;
        public int zaalnummer;

        public void Run(int zaalnummer)
        {
            this.zaalnummer = zaalnummer;
            List<StoelModel> stoelData = StoelLoadJson();
            this.data = stoelData.Where(a => a.ZaalId == zaalnummer).ToList();
            StoelMain();
        }

        //OVERZICHT
        public void StoelMain(string error = "")
        {
            Console.Clear();
            bool abort = false;
            while (!abort)
            {
                Helpers.Display.PrintLine("");
                Helpers.Display.PrintLine("ESC - Naar menu                           INS - Voeg stoel toe");
                Helpers.Display.PrintLine("Stoelbeheer: Zaal " + zaalnummer);
                Helpers.Display.PrintLine("\nTyp nummer van stoel in om aan te passen of te verwijderen\n");
                Helpers.Display.PrintHeader(error);
                Helpers.Display.PrintHeader("Nr", "Omschrijving", "Rij", "Stoelnummer", "Premium");

                //data opvragen en weergeven
                int numering = 1;
                foreach (StoelModel stoel in data)
                {
                    Helpers.Display.PrintTable(numering.ToString(), stoel.Omschrijving, stoel.Rij, stoel.StoelNr.ToString(), stoel.Premium.ToString());
                    numering++;
                }

                //bovenaan beginenn van scherm
                Console.SetWindowPosition(0, 1);

                //input INS of ESC of DEL
                Inputs.KeyInput input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0; //int check
                        inputValue--; //-1 want count start bij 0
                        if (inputValue >= 0 && inputValue < data.Count)
                        {
                            StoelenAanpas(data[inputValue]); //id waarde meegeven van de stoel
                        }
                        else
                        {
                            StoelMain("Dat is geen geldige input, probeer het nogmaals");
                        }
                        break;
                    case Inputs.KeyAction.Insert:
                        VoegStoelToe();
                        break;
                    case Inputs.KeyAction.Escape:
                        abort = true;
                        break;
                    default:
                        StoelMain("Dat is geen geldige input, probeer het nogmaals");
                        break;
                }
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
            string inpt = Console.ReadLine().ToLower();
            if (inpt == "a" || inpt == "b" || inpt == "c" || inpt == "d" || inpt == "e" || inpt == "f" || inpt == "g" || inpt == "h" || inpt == "j" || inpt == "i" || inpt == "j") str = inpt;
            else { geldigeInput = false; }


            //gegevens checken en stoel aanmaken:
            if (geldigeInput == true)
            {
                string omschr = "Rij " + str + " Stoel " + z;

                //checken of niet een stoel uit diezelfde zaal dezelfde omschrijving heeft
                List<StoelModel> stoelen = StoelLoadJson();
                foreach (StoelModel stoel in stoelen)
                {
                    if (stoel.ZaalId == zaalnummer)
                    {
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
                                    StoelMain();
                                    break;
                                default:
                                    StoelMain();
                                    break;
                            }
                        }
                    }
                }
                StoelToevoegen(stoelId, omschr, str, z, premium, zaalnummer);
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
                        StoelMain();
                        break;
                    default:
                        StoelMain();
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
            NieuweStoelWordtAangemaakt();
        }

        public void NieuweStoelWordtAangemaakt(string error = "")
        {
            Console.Clear();
            Helpers.Display.PrintLine("Stoelbeheer");
            Helpers.Display.PrintLine("De stoel wordt toegevoegd\nESC - Terug naar overzicht         INS - nieuwe stoel");
            Helpers.Display.PrintLine(error);
            switch (Helpers.Display.Keypress())
            {
                case ConsoleKey.Insert:
                    VoegStoelToe();
                    break;
                case ConsoleKey.Escape:
                    StoelMain();
                    break;
                default:
                    NieuweStoelWordtAangemaakt("Dat is geen geldige input, probeer het nogmaals");
                    break;
            }

        }

        //STOELEN AANPAS SCHERM
        public void StoelenAanpas(StoelModel stoel, string error = "")
        {
            Console.Clear();
            Helpers.Display.PrintLine(error);
            Helpers.Display.PrintLine("Stoelbeheer");
            Helpers.Display.PrintLine("ESC - Terug naar overzicht         INS - stoel premium veranderen");
            Helpers.Display.PrintLine("                                   DEL - stoel verwijderen\n");
            Helpers.Display.PrintLine("Pas stoel " + stoel.Omschrijving + " aan\n");

            //premium waarde omzetten naar string omdat een bool is.
            string premium = "";
            if (stoel.Premium == true) premium = "Ja"; else if (stoel.Premium == false) premium = "Nee";

            int nr = 0; //data weergeven en nummeren voor waarde keuze
            Helpers.Display.PrintHeader("Nr.", "Benaming", "Waarde");
            Helpers.Display.PrintTable((nr += 1).ToString(), "Omschrijving: ", stoel.Omschrijving);
            Helpers.Display.PrintTable((nr += 1).ToString(), "Rij: ", stoel.Rij);
            Helpers.Display.PrintTable((nr += 1).ToString(), "Stoel nummer: ", stoel.StoelNr.ToString());
            Helpers.Display.PrintTable((nr += 1).ToString(), "Vip: ", premium);


            Helpers.Display.PrintLine("\nWat wilt u aanpassen? Zie hoofd knoppen opties");

            switch (Helpers.Display.Keypress())
            {
                case ConsoleKey.Insert:
                    ChangePremium(stoel);
                    break;
                case ConsoleKey.Escape:
                    StoelMain();
                    break;
                case ConsoleKey.Delete:
                    StoelVerwijderen(stoel);
                    break;
                default:
                    StoelenAanpas(stoel, "Dat is een ongeldige input, probeer het nogmaals.");
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

        public void StoelVerwijderen(StoelModel data) //verwijder functie
        {
            List<StoelModel> stoelData = StoelLoadJson();
            var toRemove = stoelData.Where(a => a.StoelId == data.StoelId).ToList();
            foreach (var remove in toRemove) stoelData.Remove(remove);

            var jsonData = JsonConvert.SerializeObject(stoelData, Formatting.Indented);
            System.IO.File.WriteAllText((Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Stoel.json"), jsonData);
            Helpers.Display.PrintLine("deze stoel  is verwijderd, Typ enter om naar overzicht te gaan");
            string ans = Console.ReadLine();
            Console.Clear();
        }

        public void ChangePremium(StoelModel data) //aanpas functie
        {
            List<StoelModel> stoelData = StoelLoadJson();
            var toEdit = stoelData.Where(a => a.StoelId == data.StoelId).ToList();

            if (toEdit.Count() == 1)
            {
                foreach (var x in toEdit)
                {
                    Helpers.Display.PrintLine(x.Premium.ToString());
                    if (x.Premium == true) { x.Premium = false; } else { x.Premium = true; }
                }
            }
            var jsonData = JsonConvert.SerializeObject(stoelData, Formatting.Indented);
            System.IO.File.WriteAllText((Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Stoel.json"), jsonData);
            StoelenAanpas(data);
        }
    }
}