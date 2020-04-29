using System;
using System.Collections.Generic;
using System.Text;
using Bioscoop.Repository;
using Bioscoop.Helpers;
using Newtonsoft.Json;

namespace Bioscoop.Modules
{
    class FilmschemaModule
    { 
        //hier al je functies van je schermen.


        //FilmSchemaBeheer.cs -public void main()
        public void Run()
        {
            bool abort = false;
            
            while (!abort)
            {
                Console.Clear();
                Helpers.Display.PrintHeader("Zaalbeheer");
                Helpers.Display.PrintLine("ESC - Terug naar menu");
                Helpers.Display.PrintLine(" ");
                Helpers.Display.PrintLine("1. Zie planning");
                Helpers.Display.PrintLine("2. Maak programma");
                Helpers.Display.PrintLine("3. Verwijder programma");
                Helpers.Display.PrintLine("4. Verander programma");
                Inputs.KeyInput input = Inputs.ReadUserData();

                switch (input.action)
                {
                    case Inputs.KeyAction.Escape:
                        abort = true;
                        break;
                    case Inputs.KeyAction.Enter:
                        int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                        inputValue--; //-1 want count start bij 0
                        switch (inputValue)
                        {
                            case 0:
                                ZiePlanning();
                                break;
                            case 1:
                                MaakProgramma();
                                break;
                            case 2:
                                VerwijderProgramma();
                                break;
                            case 3:
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }



        //VerwijderProgramma.cs -public void VerwijderProgramma()
        public void VerwijderProgramma()
        {
            List<FilmschemaModel> filmschema = FilmschemaData.LoadData();
            int i = 1;
            Helpers.Display.PrintHeader("No", "Zaal", "Datum", "Tijd");
            foreach (FilmschemaModel f in filmschema)
            {
                Helpers.Display.PrintTable(i.ToString(), f.ZaalId.ToString(), f.Datum, f.Tijd);
                i++;
            }
            Helpers.Display.PrintLine(" ");
            Helpers.Display.PrintLine("Typ het nummer van het programma dat u wilt verwijderen");

            Inputs.KeyInput input = Inputs.ReadUserData();

            switch (input.action)
            {
                case Inputs.KeyAction.Enter:
                    int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                    inputValue--; //-1 want count start bij 0
                    if (inputValue >= 0 && inputValue < filmschema.Count)
                    {
                        FilmschemaData.VerwijderProgramma(inputValue);
                    }
                    break;
                case Inputs.KeyAction.Escape:
                    return;
            }
        }

        //ZiePlanning? vul zelf maar in
        public void ZiePlanning()
        {
            bool abort = false;
            List<ZaalModel> zaalData = ZaalData.LoadData();
            Console.Clear();
            List<FilmschemaModel> filmschema = FilmschemaData.LoadData();
            int i = 1;
            Helpers.Display.PrintHeader("No","Zaal","Datum","Tijd");
            foreach(FilmschemaModel f in filmschema)
            {
                Helpers.Display.PrintTable(i.ToString(),f.ZaalId.ToString(),f.Datum,f.Tijd);
                i++;
            }
            Helpers.Display.PrintLine(" ");
            Helpers.Display.PrintLine("[ESC] teruggaan");
            while (!abort)
            {
                Inputs.KeyInput input = Inputs.ReadUserData();

                switch (input.action)
                {
                    case Inputs.KeyAction.Escape:
                        return;
                    default:
                        break;
                }
            }
        }


        //MaakProgramma.cs -public void MaakProgramma()
        public void MaakProgramma()
        {
            int zaalid = -1;
            int filmid = -1;
            string datum = "";
            string tijd = "";

            datum = AssignDatum();


            while (FilmschemaData.DatumSyntax(datum) == false)
            {
                Console.Clear();
                datum = AssignDatum();
            }

            Console.Clear();
            ZaalModel z = AssignZaal();
            if (z == null)
            {
                while (z == null)
                {
                    Console.Clear();
                    z = AssignZaal();
                }
            }
            else
            {
                zaalid = z.ZaalId;
            }

            Console.Clear();
            tijd = AssignTijd();

            while(FilmschemaData.TijdSyntax(tijd) == false)
            {
                Console.Clear();
                tijd = AssignTijd();
            }

            FilmModel film = AssignFilm();
            if (film == null)
            {
                while (film == null)
                {
                    film = AssignFilm();
                }
            }
            else
            {
                filmid = film.FilmId;
            }
            FilmschemaData.MaakProgramma(datum, tijd, zaalid, filmid);
            
        }
        public ZaalModel AssignZaal()
        {
            string error = "";
            List<ZaalModel> zalen;
            zalen = ZaalData.LoadData();
            int i = 1;
            foreach(ZaalModel zaal in zalen)
            {
                Helpers.Display.PrintTable(i.ToString(), zaal.Omschrijving, zaal.Status, zaal.Scherm);
                i++;
            }
            Helpers.Display.PrintLine(" ");
            Helpers.Display.PrintLine("Typ welke zaal u wilt toevoegen");
            Inputs.KeyInput input = Inputs.ReadUserData();
            
            switch (input.action)
            {
                case Inputs.KeyAction.Enter:
                    int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                    inputValue--; //-1 want count start bij 0
                    if (inputValue >= 0 && inputValue < zalen.Count)
                    {
                        return zalen[inputValue]; //waarde meegeven van de gekozen zaal
                    }
                    else
                    {
                        error = "Onjuist waarde ingevuld.";
                    }
                    break;


                
                case Inputs.KeyAction.Escape: //de functie beeindigen
                    return null;
                    
            }
            return null;
        }

        public FilmModel AssignFilm()
        {
            List<FilmModel> films = Repository.FilmData.LoadData();
            int i = 1;
            Helpers.Display.PrintHeader("No", "Naam", "Omschrijving", "Genre", "Kijkwijzer");
            foreach(FilmModel film in films)
            {
                Helpers.Display.PrintTable(i.ToString(),film.Naam,film.Omschrijving,film.Genre,film.Kijkwijzer);
                i++;
            }


            Helpers.Display.PrintLine(" ");
            Helpers.Display.PrintLine("Typ welke zaal u wilt toevoegen");
            Inputs.KeyInput input = Inputs.ReadUserData();

            switch (input.action)
            {
                case Inputs.KeyAction.Enter:
                    int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                    inputValue--; //-1 want count start bij 0
                    if (inputValue >= 0 && inputValue < films.Count)
                    {
                        return films[inputValue]; //waarde meegeven van de gekozen film
                    }
                    
                    break;



                case Inputs.KeyAction.Escape: //de functie beeindigen
                    return null;

            }
            return null;
        }
        public string AssignTijd()
        {
            string res = "";
            string[] tijden = new string[4] {"10:00","13:30","17:00","20:30" };
            int i = 1;
            foreach(string tijd in tijden)
            {
                Helpers.Display.PrintTable(i.ToString(),tijd);
                i++;
            }

            Inputs.KeyInput input = Inputs.ReadUserData();
            switch (input.action)
            {
                case Inputs.KeyAction.Enter:
                    int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                    inputValue--; //-1 want count start bij 0
                    if (inputValue >= 0 && inputValue < tijden.Length)
                    {
                        return tijden[inputValue]; //waarde meegeven van de gekozen zaal
                    }

                    break;
                case Inputs.KeyAction.Escape: //de functie beeindigen
                    return "";
            }
            return res;

        }
        public string AssignDatum()
        {
            string res = "";

            Helpers.Display.PrintLine(FilmschemaData.PrintVolgendeDagen(2, 14));
            string[] dagen = FilmschemaData.VolgendeDagen(2, 14);
            Helpers.Display.PrintLine("Typ welke dag: ");
            Inputs.KeyInput input = Inputs.ReadUserData();
            switch (input.action)
            {
                case Inputs.KeyAction.Enter:
                    int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                    inputValue--; //-1 want count start bij 0
                    if (inputValue >= 0 && inputValue < dagen.Length)
                    {
                        return dagen[inputValue]; //waarde meegeven van de gekozen datum
                    }
                    
                    break;
                case Inputs.KeyAction.Escape: //de functie beeindigen
                    return "";
            }
            return res;
        }

    }
}
