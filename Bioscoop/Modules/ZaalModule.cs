using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bioscoop.Helpers;
using Newtonsoft.Json;

namespace Bioscoop.Modules
{
    class ZaalModule
    {

        public void Run()
        {
            //zaalmain functie aanroepen
            ZaalMain();

            switch ( Helpers.Display.Keypress() ) {
                case ConsoleKey.Insert:
                    Helpers.Display.PrintLine("Insert ingedrukt");
                    //volgende scherm functie aanroepen en console clearen
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    //terug naar vorige scherm
                    return;

            }

            Helpers.Display.Keypress();
        }


        public List<ZaalModel> LoadJson()
        {
            string jsonFilePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Zaal.json";
            string _json = File.ReadAllText(jsonFilePath);

            return JsonConvert.DeserializeObject<List<ZaalModel>>(_json);
        }

        private void ZaalMain()
        {
            //json converter ophalen
            List<ZaalModel> zalen = LoadJson();

            //menu
            Helpers.Display.PrintLine("Zaalbeheer");
            Helpers.Display.PrintLine("ESC - Terug naar menu                       INS - Nieuwe Zaal aanmaken");
            Helpers.Display.PrintLine(" ");
            Helpers.Display.PrintHeader("Nr.","Omschrijving", "Status", "Scherm");

            //data opvragen en weergeven
            int numering = 1;
            foreach (ZaalModel zaal in zalen)
            {
                Helpers.Display.PrintTable(numering.ToString(), zaal.Omschrijving, zaal.Status, zaal.Scherm);
                // System.Display.PrintLine("{0} {1} {2}", zaal.Omschrijving, zaal.Status, zaal.Scherm);
                numering++;
            }
        }
    }
}
