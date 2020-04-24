using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Bioscoop.Helpers;
using Bioscoop.Repository;

namespace Bioscoop.Modules
{
    class ZaalModule
    {
        Inputs Inputs = new Inputs();
        List<ZaalModel> zalen;

        public void Run()
        {
            Boolean abort = false;
            String error = "";
            while (!abort)
            {
                zalen = ZaalData.LoadData();
                Console.Clear();
                if (!String.IsNullOrEmpty(error)) //error handling
                {
                    Helpers.Display.PrintLine("Error: " + error);
                    Helpers.Display.PrintLine("");
                }

                //menu
                Helpers.Display.PrintHeader("Zaalbeheer");
                Helpers.Display.PrintLine("ESC - Terug naar menu                       INS - Nieuwe Zaal aanmaken");
                Helpers.Display.PrintLine(" ");
                Helpers.Display.PrintLine("Vul een nummer in om deze waarde te bewerken");
                Helpers.Display.PrintLine(" ");
                Helpers.Display.PrintHeader("Nr.", "Omschrijving", "Status", "Scherm");

                int nummering = 1;
                foreach (ZaalModel zaal in zalen)
                {
                    Helpers.Display.PrintTable(nummering.ToString(), zaal.Omschrijving, zaal.Status, zaal.Scherm);
                    nummering++;
                }
                Helpers.Display.PrintLine(" ");
                Helpers.Display.PrintLine("Type je keuze in en sluit af met een enter");

                Inputs.KeyInput input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        int inputValue;
                        inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                        inputValue--; //start bij 0
                        if (inputValue >= 0 && inputValue < zalen.Count)
                        {
                            error = "";
                            EditZaal(zalen[inputValue]);
                        }
                        else
                        {
                            error = "Onjuist waarde ingevuld.";
                        }
                        break;


                    case Inputs.KeyAction.Insert:
                        AddZaal();
                        break;

                    case Inputs.KeyAction.Escape:
                        abort = true;
                        break;
                }
            }
        }


        private void EditZaal(ZaalModel zaal)
        {
            Boolean abort = false;
            String error = "";
            Inputs.KeyInput input = null;
            Inputs.KeyInput inputData = null;
            ZaalModel editZaal = zaal;

            while (!abort)
            {
                Console.Clear();
                if (!String.IsNullOrEmpty(error))
                {
                    Helpers.Display.PrintLine("Error: " + error);
                    Helpers.Display.PrintLine("");
                }

                //menu
                Helpers.Display.PrintHeader("Aanpassen zaal : " + zaal.Omschrijving);
                Helpers.Display.PrintLine("ESC - Terug naar menu            INS - Opslaan"); Display.PrintLine("");
                Helpers.Display.PrintLine("Druk op een nummer om deze waarde aan te passen."); Display.PrintLine("");

                int nr = 0;
                Helpers.Display.PrintHeader("Nr.");
                Helpers.Display.PrintLine((nr += 1) + "| Omschrijving: " + zaal.Omschrijving);
                Helpers.Display.PrintLine((nr += 1) + "| Status: " + zaal.Status);
                Helpers.Display.PrintLine((nr += 1) + "| Scherm: " + zaal.Scherm);
                Console.WriteLine(" ");


                input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter: // process input als enter
                        int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                        inputValue--; //start bij 0
                        switch (inputValue)
                        {
                            case 0: Helpers.Display.PrintLine("Vul de nieuwe Zaal Omschrijving in: "); break;
                            case 1: Helpers.Display.PrintLine("Vul de nieuwe Zaal Status waarde in: (Beschikbaar / Niet beschikbaar)"); break;
                            case 2: Helpers.Display.PrintLine("Vul de nieuwe Zaal Scherm waarde in: (2D, 3D, IMAX)"); break;
                        }

                        inputData = Inputs.ReadUserData();
                        switch (inputValue)
                        {
                                case 0:
                                if (inputData.val.Length > 5)
                                {
                                    editZaal.Omschrijving = inputData.val;
                                }
                                else
                                {
                                    error = "De omschrijving moet minimaal 6 karakters zijn";
                                }

                                break;
                            case 1:
                                if (inputData.val == "Beschikbaar" || inputData.val == "Niet Beschikbaar")
                                {
                                    editZaal.Status = inputData.val;
                                }
                                else
                                {
                                    error = "Onjuiste waarde ingevuld.";
                                }
                                break;
                            case 2:
                                if (inputData.val == "2D" || inputData.val == "3D" || inputData.val == "IMAX")
                                {
                                    editZaal.Scherm = inputData.val;
                                }
                                else
                                {
                                    error = "Onjuiste waarde ingevuld.";
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case Inputs.KeyAction.Escape:
                        abort = true;
                        break;
                    case Inputs.KeyAction.Insert:
                        ZaalData.EditData(editZaal);
                        Console.WriteLine("De gegevens zijn opgeslagen.");
                        System.Threading.Thread.Sleep(2000);
                        abort = true;
                        break;
                    case Inputs.KeyAction.Delete:
                        Console.WriteLine("Weet je het zeker?");
                        string aaaa = Console.ReadLine();
                        if( aaaa == "y")
                        {
                            ZaalData.RemoveData(zaal);
                            Console.WriteLine(zaal.Omschrijving + " is verwijderd.");
                            System.Threading.Thread.Sleep(2000);
                            abort = true;
                        }
                        break;
                }
            }
        }




        private void AddZaal()
        {
            Boolean abort = false;
            String error = "";
            Inputs.KeyInput input = null;
            ZaalModel newZaal = new ZaalModel();
            int x = 0;
            while (!abort && x < 4)
            {
                Console.Clear();
                if (!String.IsNullOrEmpty(error))
                {
                    Helpers.Display.PrintLine("Error: " + error);
                    Helpers.Display.PrintLine("");
                }

                //menu
                Helpers.Display.PrintHeader("Zaal toevoegen");
                Helpers.Display.PrintLine("ESC - Terug naar menu                       Del - Reset");
                if (!String.IsNullOrEmpty(newZaal.Scherm)) Helpers.Display.PrintLine("                                            INS - Opslaan Nieuwe Zaal");
                Helpers.Display.PrintLine(" ");
                if (!String.IsNullOrEmpty(newZaal.Omschrijving)) Helpers.Display.PrintLine("Omschrijving: " + newZaal.Omschrijving);
                if (!String.IsNullOrEmpty(newZaal.Status)) Helpers.Display.PrintLine("Status: " + newZaal.Status);
                if (!String.IsNullOrEmpty(newZaal.Scherm)) Helpers.Display.PrintLine("Scherm: " + newZaal.Scherm);
                Helpers.Display.PrintLine("");



                switch (x)
                {
                    case 0: Helpers.Display.PrintLine("Vul de nieuwe Zaal Omschrijving in: "); break;
                    case 1: Helpers.Display.PrintLine("Vul de Zaal Status in: (Beschikbaar / Niet beschikbaar)"); break;
                    case 2: Helpers.Display.PrintLine("Vul het zaal scherm in: (2D, 3D, IMAX)"); break;
                    case 3: Helpers.Display.PrintLine("druk op Insert om de gegevens op te slaan"); break;
                }

                input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        switch (x)
                        {
                            case 0:
                                if (input.val.Length > 5)
                                {
                                    newZaal.Omschrijving = input.val;
                                    x++;
                                }
                                else
                                {
                                    error = "De omschrijving moet minimaal 6 karakters zijn";
                                }

                                break;
                            case 1:
                                if (input.val == "Beschikbaar" || input.val == "Niet Beschikbaar")
                                {
                                    newZaal.Status = input.val;
                                    x++;
                                }
                                else
                                {
                                    error = "Onjuist waarde ingevuld.";
                                }
                                break;
                            case 2:
                                if (input.val == "2D" || input.val == "3D" || input.val == "IMAX")
                                {
                                    newZaal.Scherm = input.val;
                                    x++;
                                }
                                else
                                {
                                    error = "Onjuist waarde ingevuld.";
                                }
                                break;
                            case 3:
                                Console.WriteLine("druk op Insert om de gegevens op te slaan");
                                break;
                            default:
                                x++;
                                break;
                        }
                        break;
                    case Inputs.KeyAction.Escape:
                        abort = true;
                        break;
                    case Inputs.KeyAction.Insert:
                        ZaalData.AddData(newZaal);
                        Console.WriteLine("De zaal is opgeslagen.");
                        System.Threading.Thread.Sleep(2000);
                        abort = true;
                        break;
                    case Inputs.KeyAction.Delete:
                        abort = true;
                        AddZaal();
                        break;

                }
            }
        }
    }
}