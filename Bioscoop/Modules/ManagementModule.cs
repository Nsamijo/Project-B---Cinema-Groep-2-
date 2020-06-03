using System;
using System.Collections.Generic;
using System.Linq;
using Bioscoop.Helpers;
using Bioscoop.Models;
using Bioscoop.Modules;
using Bioscoop.Repository;

namespace Bioscoop.Modules
{
    class ManagementModule
    {
        List<ZaalModel> zaalData = ZaalData.LoadData();
        List<FilmModel> filmData = FilmData.LoadData();
        List<FilmschemaModel> filmschemaData = FilmschemaData.LoadData();
        List<StoelModel> stoelData = StoelData.LoadData();
        List<ReserveringModel> reserveringData = ReserveringData.LoadData();
        ReserveringModule ReserveringModule = new ReserveringModule();

        public void ReservatieManagament()
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
                Helpers.Display.PrintLine("ESC - Terug naar menu                        INS - Reservatie overzicht");
                Helpers.Display.PrintLine(" ");
                Helpers.Display.PrintLine("Vul een nummer in om een reservering aan te maken");
                Helpers.Display.PrintLine(" ");
                Helpers.Display.PrintTableFilm("Nr.", "Film", "Tijd", "Plekken", "Scherm", "Datum");

                //data voor loop
                var land = new System.Globalization.CultureInfo("nl-NL");
                string fromdate = DateTime.UtcNow.ToString("dd/MM/yyyy"); DateTime td = DateTime.Parse(fromdate, land);
                List<FilmschemaModel> filmschema = filmschemaData.Where(s => s.Datum == fromdate).ToList();
                int nummering = 1; // nummer naast de waarde op het scherm
                foreach (FilmschemaModel schema in filmschema)
                {
                    ZaalModel zaaldata = zaalData.Where(a => a.ZaalId == schema.ZaalId).SingleOrDefault();
                    FilmModel filmdata = filmData.Where(a => a.FilmId == schema.FilmId).SingleOrDefault();
                    List<ReserveringModel> reservaties = reserveringData.Where(a => a.ProgrammaId == schema.ProgrammaId).ToList();
                    List<StoelModel> stoelen = stoelData.Where(a => a.ZaalId == schema.ZaalId).ToList();

                    List<int> stoelenList = new List<int>();
                    foreach (var r in reservaties)
                    {
                        foreach (var s in r.StoelId)
                        {
                            if (stoelenList.IndexOf(s) < 0)
                                stoelenList.Add(s);
                        }
                    }
                    var rijen = stoelen.Select(r => r.Rij).Distinct().ToList();
                    List<int> rijPlekken = new List<int>();
                    foreach (string ar in rijen)
                    {
                        List<StoelModel> allePlekken = stoelen.Where(r => r.Rij == ar).ToList(); //alle stoelen per rij
                        List<StoelModel> filterPlekken = allePlekken.Where(d => !stoelenList.Any(d2 => d2 == d.StoelId)).ToList();
                        rijPlekken.Add(filterPlekken.Count());
                    }

                    DateTime d = DateTime.Parse(schema.Datum, land);
                    Helpers.Display.PrintTableFilm(nummering.ToString(), filmdata.Naam, schema.Tijd, rijPlekken.Sum().ToString(), zaaldata.Scherm,  schema.Datum); 
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
                        if (inputValue >= 0 && inputValue < filmschema.Count)
                        {
                            error = "";
                            ReserveringModule.Reservering(filmschema[inputValue], true); //waarde meegeven van de gekozen zaal
                        }
                        else
                        {
                            error = "Onjuist waarde ingevuld.";
                        }
                        break;
                    case Inputs.KeyAction.Escape: //de functie beeindigen
                        abort = true;
                        break;
                    case Inputs.KeyAction.Insert:
                        abort = true;
                        ReserveringModule.ReserveringManagement();
                        break;
                        
                }
            }
        }

        public void RapportageManagement()
        {

        }
    }
}
