using System;
using System.Collections.Generic;
using System.Text;
using Bioscoop.Repository;
using Bioscoop.Helpers;
using Newtonsoft.Json;
using System.Linq;

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
                                KiesProgramma();
                                break;
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
            List<FilmModel> filmData = FilmData.LoadData();
            List<ZaalModel> zaalData = ZaalData.LoadData();
            int i = 1;
            Helpers.Display.PrintHeader("No", "Zaal","film", "Datum", "Tijd");
            foreach (FilmschemaModel programma in filmschema)
            {
                string filmnaam = filmData.Where(film => film.FilmId == programma.FilmId).ToList()[0].Naam;
                string zaalnaam = zaalData.Where(zaal => zaal.ZaalId == programma.ZaalId).ToList()[0].Omschrijving;

                Helpers.Display.PrintTable(i.ToString(), zaalnaam,filmnaam, programma.Datum, programma.Tijd);
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
            Console.Clear();
            List<FilmschemaModel> filmschema = FilmschemaData.LoadData();
            
            int i = 1;
            Helpers.Display.PrintHeader("No","Zaal","Film","Datum","Tijd");
            List<FilmModel> filmData = FilmData.LoadData();
            List<ZaalModel> zaalData = ZaalData.LoadData();
            foreach (FilmschemaModel programma in filmschema)
            {
                string filmnaam = filmData.Where(film => film.FilmId == programma.FilmId).ToList()[0].Naam;
                string zaalnaam = zaalData.Where(zaal => zaal.ZaalId == programma.ZaalId).ToList()[0].Omschrijving;
                Helpers.Display.PrintTable(i.ToString(),zaalnaam,filmnaam,programma.Datum,programma.Tijd);
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

            Console.Clear();
            datum = AssignDatum();
            while (FilmschemaData.DatumSyntax(datum) == false)
            {
                Console.Clear();
                datum = AssignDatum();
            }

            Console.Clear();
            ZaalModel z = AssignZaal(datum);
            if (z == null)
            {
                while (z == null)
                {
                    Console.Clear();
                    z = AssignZaal(datum);
                }
            }
            else
            {
                zaalid = z.ZaalId;
            }

            Console.Clear();
            tijd = AssignTijd(datum,zaalid);

            while(FilmschemaData.TijdSyntax(tijd) == false)
            {
                Console.Clear();
                tijd = AssignTijd(datum,zaalid);
            }

            Console.Clear();
            FilmModel film = AssignFilm();
            if (film == null)
            {
                while (film == null)
                {
                    Console.Clear();
                    film = AssignFilm();
                }
            }
            else
            {
                filmid = film.FilmId;
            }
            FilmschemaData.MaakProgramma(FilmschemaData.getId(),datum, tijd, zaalid, filmid);
            
        }
        public ZaalModel AssignZaal(string datum)
        {
            string error = "";
            List<ZaalModel> zalen;
            zalen = ZaalData.LoadData();
            for(int i = 0; i < zalen.Count; i++)
            {
                if (FilmschemaData.HallCollides(datum, zalen[i].ZaalId))
                {
                    zalen.RemoveAt(i);
                }
            }
            int count = 1;
            foreach(ZaalModel zaal in zalen)
            {
                if (FilmschemaData.HallCollides(datum,zaal.ZaalId) == false)
                {
                    Helpers.Display.PrintTable(count.ToString(), zaal.Omschrijving, zaal.Status, zaal.Scherm);
                    count++;
                }
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
        public string AssignTijd(string datum,int zaalid)
        {
            string res = "";
            string[] tijden = new string[4] {"10:00","13:30","17:00","20:30" };
            
            int count = 0;
            foreach(string tijd in tijden)
            {
                if (FilmschemaData.TimeCollides(datum,zaalid,tijd) == false)
                {
                    count++;
                }
            }
            string[] temp = new string[count];

            count = 0;
            foreach (string tijd in tijden)
            {
                if (FilmschemaData.TimeCollides(datum, zaalid, tijd) == false)
                {
                    temp[count] = tijd;
                    count++;
                }
            }
            tijden = temp;
            count = 1;
            foreach(string tijd in tijden)
            {
                Helpers.Display.PrintTable(count.ToString(),tijd);
                count++;
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
            
            string[] dagen = FilmschemaData.VolgendeDagen(2, 14);
            int count = 0;
            foreach (string dag in dagen)
            {
                if(FilmschemaData.DateCollides(dag) == false)
                {
                    count++;
                }
            }
            string[] temp = new string[count];
            count = 0;
            foreach (string dag in dagen)
            {
                if (FilmschemaData.DateCollides(dag) == false)
                {
                    temp[count] = dag;
                    count++;
                }
            }
            dagen = temp;
            count = 1;
            foreach(string dag in dagen)
            {
                Helpers.Display.PrintLine($"{count}.  {dag}");
                count++;
            }

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

        public void KiesProgramma()
        {
            bool abort = false;
            Console.Clear();
            List<FilmschemaModel> filmschema = FilmschemaData.LoadData();
            int i = 1;
            Helpers.Display.PrintHeader("No", "Zaal", "Datum", "Tijd");
            List<FilmModel> filmData = FilmData.LoadData();
            List<ZaalModel> zaalData = ZaalData.LoadData();
            foreach (FilmschemaModel programma in filmschema)
            {
                string filmnaam = filmData.Where(film => film.FilmId == programma.FilmId).ToList()[0].Naam;
                string zaalnaam = zaalData.Where(zaal => zaal.ZaalId == programma.ZaalId).ToList()[0].Omschrijving;
                Helpers.Display.PrintTable(i.ToString(), zaalnaam, filmnaam, programma.Datum, programma.Tijd);
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
                    case Inputs.KeyAction.Enter:
                        int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                        inputValue--; //-1 want count start bij 0
                        if (inputValue >= 0 && inputValue < filmschema.Count)
                        {
                            VeranderProgramma(inputValue);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        public void VeranderProgramma(int index)
        {
            bool abort = false; 
            List<FilmschemaModel> filmschema = FilmschemaData.LoadData();
            List<ZaalModel> zaalData = ZaalData.LoadData();
            List<FilmModel> filmData = FilmData.LoadData();
            FilmschemaModel programma = filmschema[index];
            
            



            while (!abort)
            {
                Console.Clear();
                Helpers.Display.PrintLine($"Datum: {programma.Datum}");
                Helpers.Display.PrintLine($"Zaal: {zaalData.Where(zaal => zaal.ZaalId == programma.ZaalId).ToList()[0].Omschrijving}");
                Helpers.Display.PrintLine($"Film: {filmData.Where(film => film.FilmId == programma.FilmId).ToList()[0].Naam}");
                Helpers.Display.PrintLine($"Tijd: {programma.Tijd}");


                Helpers.Display.PrintLine("[ESC] verlaten               [INS] opslaan");


                Helpers.Display.PrintLine("Wat wilt u veranderen?");
                Helpers.Display.PrintLine("1. Datum");
                Helpers.Display.PrintLine("2. Zaal");
                Helpers.Display.PrintLine("3. Film");
                Helpers.Display.PrintLine("4. Tijd");
                Inputs.KeyInput input = Inputs.ReadUserData();

                switch (input.action)
                {
                    case Inputs.KeyAction.Escape:
                        abort = true;
                        break;
                    case Inputs.KeyAction.Insert:
                        FilmschemaData.VerwijderProgramma(index);
                        FilmschemaData.MaakProgramma(programma.ProgrammaId, programma.Datum, programma.Tijd, programma.ZaalId, programma.FilmId);
                        break;
                    case Inputs.KeyAction.Enter:
                        int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;

                        if(inputValue >= 1 && inputValue <= 4)
                        {
                            switch (inputValue)
                            {
                                case 1:
                                    programma.Datum = AssignDatum();
                                    break;
                                case 2:
                                    programma.ZaalId = AssignZaal(programma.Datum).ZaalId;
                                    break;
                                case 3:
                                    programma.FilmId = AssignFilm().FilmId;
                                    break;
                                case 4:
                                    programma.Tijd = AssignTijd(programma.Datum,programma.ZaalId);
                                    break;
                            }
                            Helpers.Display.PrintLine("Programma aangepast");
                            Helpers.Display.PrintLine("");
                        }
                        break;
                    default:
                        break;
                }
            }


        }

    }
}
