using System;
using System.Collections.Generic;
using Bioscoop.Helpers;
using Bioscoop.Repository;
using Bioscoop.Models;

namespace Bioscoop.Modules
{
    class ZaalModule//Jan
    {
        List<ZaalModel> zalen;

        public void Run() //hoofd functie
        {
            Console.CursorVisible = true;
            //globale data
            Boolean abort = false;
            String error = "";
            while (!abort)
            {
                Console.Clear();
                if (!String.IsNullOrEmpty(error)) //error handling en text kleur veranderen
                {
                    ConsoleColor originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Helpers.Display.PrintLine("Error: " + error);
                    Helpers.Display.PrintLine("");
                    Console.ForegroundColor = originalColor;
                }

                //menu
                Helpers.Display.PrintHeader("Zaalbeheer");
                Helpers.Display.PrintMenu("ESC - Terug naar het menu", "INS - Nieuwe Zaal aanmaken");
                Helpers.Display.PrintLine(" ");
                Helpers.Display.PrintLine("Vul een nummer in om deze waarde te bewerken");
                Helpers.Display.PrintLine(" ");
                Helpers.Display.PrintHeader("Nr.", "Omschrijving", "Status", "Scherm");

                //Ophalen json via Repository/ZaalData.cs LoadData functie
                zalen = ZaalData.LoadData();
                ZaalData.SortData(); //sorteer functie
                int nummering = 1; // nummer naast de waarde op het scherm

                foreach (ZaalModel zaal in zalen) //door alle zaal data loopen en weergeven
                {
                    Helpers.Display.PrintTable(nummering.ToString(), zaal.Omschrijving, zaal.Status, zaal.Scherm);
                    nummering++;
                }
                Helpers.Display.PrintLine(" ");
                Helpers.Display.PrintLine("Type je keuze in en sluit af met een enter");

                //userinput functie opvragen in Helpers/Inputs
                Console.Write(">"); Inputs.KeyInput input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                        inputValue--; //-1 want count start bij 0
                        if (inputValue >= 0 && inputValue < zalen.Count)
                        {
                            error = "";
                            EditZaal(zalen[inputValue]); //waarde meegeven van de gekozen zaal
                        }
                        else
                        {
                            error = "Onjuist waarde ingevuld.";
                        }
                        break;
                    case Inputs.KeyAction.Insert: //toevoeg scherm aanroepen
                        AddZaal();
                        break;
                    case Inputs.KeyAction.Escape: //de functie beeindigen
                        abort = true; 
                        break;
                }
            }
        }

        private void EditZaal(ZaalModel zaal) //aanpas scherm
        {
            //globale data
            Boolean abort = false;
            String error = "";
            Inputs.KeyInput input = null;
            Inputs.KeyInput inputData = null;
            ZaalModel editZaal = zaal;

            while (!abort)
            {
                Console.Clear();
                if (!String.IsNullOrEmpty(error)) //error handling en text kleur veranderen
                {
                    ConsoleColor originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Helpers.Display.PrintLine("Error: " + error);
                    Helpers.Display.PrintLine("");
                    Console.ForegroundColor = originalColor;
                }

                //menu
                Helpers.Display.PrintHeader("Aanpassen zaal : " + zaal.Omschrijving);
                Helpers.Display.PrintMenu("ESC - Terug", "Del - Verwijderen");
                Helpers.Display.PrintMenu("F1 - Stoelenbeheer", "INS - Opslaan"); 
                Display.PrintLine("");

                int nr = 0; //data weergeven en nummeren voor waarde keuze
                Helpers.Display.PrintHeader("Nr.", "Benaming", "Waarde");
                Helpers.Display.PrintTable((nr += 1).ToString(), "Omschrijving: ", zaal.Omschrijving);
                Helpers.Display.PrintTable((nr += 1).ToString(),"Status: ", zaal.Status);
                Helpers.Display.PrintTable((nr += 1).ToString(), "Scherm: ", zaal.Scherm);
                Console.WriteLine(" ");

                Helpers.Display.PrintLine("Druk op een nummer om deze waarde aan te passen.");
                Console.Write(">");
                input = Inputs.ReadUserData(); //waarde oplezen met keyinput functie
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0; //controleren of de waarde een int is
                        inputValue--; //waarde -1 want switch start bij 0
                        if (inputValue >= 0 && inputValue < 3)
                        {
                            switch (inputValue)
                            {
                                case 0: Helpers.Display.PrintLine("Vul de nieuwe Zaal Omschrijving in: (Zaal 'nummer')"); break;
                                case 1: Helpers.Display.PrintLine("Vul de nieuwe Zaal Status waarde in: (Beschikbaar / Niet beschikbaar)"); break;
                                case 2: Helpers.Display.PrintLine("Vul de nieuwe Zaal Scherm waarde in: (2D, 3D, IMAX)"); break;
                            }

                            //switch met de instructie en data opvang van de gekozen data soort. met error handling.
                            Console.Write(">");
                            inputData = Inputs.ReadUserData();
                            switch (inputValue)
                            {
                                case 0:
                                    if (inputData.val.Length > 5) editZaal.Omschrijving = inputData.val;
                                    else error = "De omschrijving moet uit minimaal 5 karakters bestaan";
                                    break;
                                case 1:
                                    if (inputData.val == "Beschikbaar" || inputData.val == "Niet Beschikbaar") editZaal.Status = inputData.val;
                                    else error = "Onjuiste waarde ingevuld.";
                                    break;
                                case 2:
                                    if (inputData.val == "2D" || inputData.val == "3D" || inputData.val == "IMAX") editZaal.Scherm = inputData.val;
                                    else error = "Onjuiste waarde ingevuld.";
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case Inputs.KeyAction.F1:
                        StoelModule stoelenbeheer = new StoelModule();
                        int value = editZaal.ZaalId;
                        stoelenbeheer.Run(value);//de zaalid meegeven van de geselecteerde zaal
                        break;
                    case Inputs.KeyAction.Escape: //de functie beeindigen
                        abort = true;
                        break;
                    case Inputs.KeyAction.Insert: //de nieuwe waardes opslaan met andere kleur
                        ZaalData.EditData(editZaal);
                        ConsoleColor originalColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("De gegevens zijn opgeslagen.");
                        Console.ForegroundColor = originalColor;
                        System.Threading.Thread.Sleep(2000);
                        abort = true;
                        break;
                    case Inputs.KeyAction.Delete: //de specifieke zaal data verwijderen
                        Display.PrintLine("\n Weet je zeker dat je " + zaal.Omschrijving + " wilt verwijderen? (y/n)");
                        string temp = Console.ReadLine();
                        if( temp == "y")
                        {
                            ZaalData.RemoveData(zaal);
                            StoelModule.DeleteStoel144(zaal.ZaalId);
                            ConsoleColor ogColor = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(" " + zaal.Omschrijving + " is verwijderd.");
                            Console.ForegroundColor = ogColor;
                            System.Threading.Thread.Sleep(2000);
                            abort = true;
                        }
                        else EditZaal(zaal);
                        break;
                }
            }
        }

        private void AddZaal() //toevoeg scherm
        {
            //globale data
            Boolean abort = false;
            bool check = false;
            String error = "";
            Inputs.KeyInput input = null;
            ZaalModel newZaal = new ZaalModel();

            int x = 0;
            while (!abort && x < 4)
            {
                Console.Clear();
                if (!String.IsNullOrEmpty(error)) //error handling en text kleur veranderen
                {
                    ConsoleColor originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Helpers.Display.PrintLine("Error: " + error);
                    Helpers.Display.PrintLine("");
                    Console.ForegroundColor = originalColor;
                }

                //menu
                Helpers.Display.PrintHeader("Zaal toevoegen");
                Helpers.Display.PrintMenu("ESC - Terug","Del - Reset");
                if (!String.IsNullOrEmpty(newZaal.Scherm)) Helpers.Display.PrintMenu(" ", "INS - Opslaan nieuwe Zaal");
                Helpers.Display.PrintLine(" ");
                if (!String.IsNullOrEmpty(newZaal.Omschrijving)) Helpers.Display.PrintTableInfo("Omschrijving: " ,newZaal.Omschrijving);
                if (!String.IsNullOrEmpty(newZaal.Status)) Helpers.Display.PrintTableInfo("Status: ", newZaal.Status);
                if (!String.IsNullOrEmpty(newZaal.Scherm)) Helpers.Display.PrintTableInfo("Scherm: ", newZaal.Scherm);
                Helpers.Display.PrintLine("");

                switch (x) //switch case die vanaf 0 begint. elke waarde moet ingevuld worden.
                {
                    case 0: Helpers.Display.PrintLine("Vul de Zaal Omschrijving in: (Zaal (nummer))"); break;
                    case 1: Helpers.Display.PrintLine("Vul de Zaal Status in: (Beschikbaar / Niet Beschikbaar)"); break;
                    case 2: Helpers.Display.PrintLine("Vul het zaal scherm in: (2D, 3D, IMAX)"); break;
                    case 3: Helpers.Display.PrintLine("druk op Insert om de gegevens op te slaan"); break;
                }

                Console.Write(">");
                input = Inputs.ReadUserData(); //waarde oplezen met keyinput functie
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        switch (x)
                        {
                            case 0:
                                if (input.val.Length > 5)
                                {
                                    newZaal.Omschrijving = input.val;
                                    error = "";
                                    x++;
                                }
                                else error = "De omschrijving moet uit minimaal 6 karakters bestaan";
                                break;
                            case 1:
                                if (input.val == "Beschikbaar" || input.val == "Niet Beschikbaar")
                                {
                                    newZaal.Status = input.val;
                                    error = "";
                                    x++;
                                }
                                else error = "Onjuist waarde ingevuld.";
                                break;
                            case 2:
                                if (input.val == "2D" || input.val == "3D" || input.val == "IMAX 2D" || input.val == "IMAX 3D")
                                {
                                    newZaal.Scherm = input.val;
                                    error = "";
                                    x++;
                                    check = true;
                                }
                                else error = "Onjuist waarde ingevuld.";
                                break;
                            case 3:
                                Console.WriteLine("druk op Insert om de gegevens op te slaan");
                                break;
                            default:
                                x++;
                                break;
                        }
                        break;
                    case Inputs.KeyAction.Escape:  //de functie beeindigen
                        abort = true;
                        break;
                    case Inputs.KeyAction.Insert:  //De nieuwe data opslaan als de laatste waarde is ingevuld. groene kleur text
                        if (check == true)
                        {
                            ZaalData.AddData(newZaal);
                            ConsoleColor originalColor = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("De nieuwe zaal is opgeslagen.");
                            Console.ForegroundColor = originalColor;
                            System.Threading.Thread.Sleep(2000);
                            ZaalData.SortData();
                            StoelModule.Stoel144(newZaal.ZaalId);
                            abort = true;
                        }
                        break;
                    case Inputs.KeyAction.Delete:  //De functie opnieuw aanroepen en alle inputs resetten
                        abort = true;
                        AddZaal();
                        break;
                }
            }
        }
    }
}