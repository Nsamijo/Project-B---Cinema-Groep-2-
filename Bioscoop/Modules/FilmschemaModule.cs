using System;
using System.Collections.Generic;
using Bioscoop.Repository;
using Bioscoop.Helpers;
using System.Linq;
using Bioscoop.Models;

namespace Bioscoop.Modules
{
    class FilmschemaModule
    {
        public void Run()
        {
            bool abort = false;
            while (!abort)
            {
                Console.Clear();
                List<FilmschemaModel> filmschema = FilmschemaData.LoadData();
                List<FilmModel> filmData = FilmData.LoadData();
                List<ZaalModel> zaalData = ZaalData.LoadData();

                Helpers.Display.PrintHeader("Filmschemabeheer");
                Helpers.Display.PrintLine("ESC - Terug naar menu                       INS - Nieuwe Filmschema aanmaken");
                Helpers.Display.PrintLine("                                            DEL - Filmschema verwijderen");
                Helpers.Display.PrintLine(" ");
                Helpers.Display.PrintLine("Vul een nummer in om deze waarde te bewerken.");
                Helpers.Display.PrintLine(" ");

                int i = 1;
                Helpers.Display.PrintFilmHeader("No", "Film", "Zaal", "Datum", "Tijd");
                foreach (FilmschemaModel programma in filmschema)
                {
                    try
                    {
                        string filmnaam = filmData.Where(film => film.FilmId == programma.FilmId).ToList()[0].Naam;
                        string zaalnaam = zaalData.Where(zaal => zaal.ZaalId == programma.ZaalId).ToList()[0].Omschrijving;
                        Helpers.Display.PrintFilmTable(i.ToString(), filmnaam, zaalnaam, programma.Datum, programma.Tijd);
                    }
                    catch
                    {

                    }
                    i++;
                }
                Helpers.Display.PrintLine(" ");
                Helpers.Display.PrintLine("Typ het nummer van het programma in dat u wilt veranderen:");

                Inputs.KeyInput input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                        inputValue--; //-1 want count start bij 0
                        if (inputValue >= 0 && inputValue < filmschema.Count)
                        {
                            VeranderProgramma(inputValue);
                        }
                        break;
                    case Inputs.KeyAction.Insert:
                        MaakProgramma();
                        break;
                    case Inputs.KeyAction.Escape:
                        abort = true;
                        break;
                    case Inputs.KeyAction.Delete:
                        VerwijderProgramma();
                        break;
                }
            }
        }

        public void MaakProgramma()
        {
            bool abort = false;
            int zaalid = -1;
            int filmid = -1;
            string datum = "";
            string tijd = "";
            
            Console.Clear();
            datum = AssignDatum();
            if (datum == "abort")
            {
                abort = true;
            }
            if (!abort)
            {
                while (FilmschemaData.DatumSyntax(datum) == false && !abort)
                {
                    if (!abort)
                    {
                        Console.Clear();
                        datum = AssignDatum(datum);
                        if (datum == "abort")
                        {
                            abort = true;
                        }
                    }
                }
            }

            if (!abort)
            {
                Console.Clear();
                ZaalModel z = AssignZaal(datum);

                if (z.ZaalId == -1)
                {
                    abort = true;
                }
                if (z.ZaalId == -2 && !abort)
                {

                    while (z.ZaalId == -2)
                    {
                        Console.Clear();
                        z = AssignZaal(datum,z.Omschrijving);
                        if (z.ZaalId == -1)
                        {
                            abort = true;
                        }
                    }

                }
                else
                {
                    if (!abort)
                    {
                        zaalid = z.ZaalId;
                    }
                }
            }

            Console.Clear();
            if (!abort)
            {
                tijd = AssignTijd(datum, zaalid);
                if (tijd == "abort")
                {
                    abort = true;
                }
                while (FilmschemaData.TijdSyntax(tijd) == false && !abort)
                {
                    Console.Clear();
                    tijd = AssignTijd(datum, zaalid,tijd);
                    if (tijd == "abort")
                    {
                        abort = true;
                    }
                }
            }
            if (!abort)
            {
                Console.Clear();
                FilmModel film = AssignFilm(tijd,datum,zaalid);
                if (film.FilmId == -1)
                {
                    abort = true;
                }
                if (film.FilmId == -2 && !abort)
                {
                    while (film.FilmId == -2 && !abort)
                    {
                        Console.Clear();
                        film = AssignFilm(tijd,datum,zaalid,film.Omschrijving);
                        if (film.FilmId == -1)
                        {
                            abort = true;
                        }
                    }
                }
                else
                {
                    if (!abort)
                    {
                        filmid = film.FilmId;
                    }
                }
            }
            if (!abort && filmid >= 0 && zaalid >= 0)
            {
                FilmschemaData.MaakProgramma(FilmschemaData.getId(), datum, tijd, zaalid, filmid);
            }
        }

        public void VerwijderProgramma()
        {
            Console.Clear();
            List<FilmschemaModel> filmschema = FilmschemaData.LoadData();
            List<FilmModel> filmData = FilmData.LoadData();
            List<ZaalModel> zaalData = ZaalData.LoadData();

            //menu
            Helpers.Display.PrintHeader("Filmschemabeheer - verwijderen");
            Helpers.Display.PrintLine("ESC - Terug naar menu \n");

            int i = 1;
            Helpers.Display.PrintFilmHeader("Nr", "Film", "Zaal", "Datum", "Tijd");
            foreach (FilmschemaModel programma in filmschema)
            {
                string filmnaam = filmData.Where(film => film.FilmId == programma.FilmId).ToList()[0].Naam;
                string zaalnaam = zaalData.Where(zaal => zaal.ZaalId == programma.ZaalId).ToList()[0].Omschrijving;

                Helpers.Display.PrintFilmTable(i.ToString(), filmnaam, zaalnaam, programma.Datum, programma.Tijd);
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

        public ZaalModel AssignZaal(string datum, string error = "")
        {
            Helpers.Display.PrintLine("[ESC] teruggaan");
            
            List<ZaalModel> zalen;
            zalen = ZaalData.LoadData();
            for (int i = 0; i < zalen.Count; i++)
            {
                if (FilmschemaData.HallCollides(datum, zalen[i].ZaalId))
                {
                    zalen.RemoveAt(i);
                }
            }
            Helpers.Display.PrintLine("");
            Helpers.Display.PrintLine($"Datum: {datum}");
            Helpers.Display.PrintLine("");


            int count = 1;
            foreach (ZaalModel zaal in zalen)
            {
                if (FilmschemaData.HallCollides(datum, zaal.ZaalId) == false)
                {
                    Helpers.Display.PrintTable(count.ToString(), zaal.Omschrijving, zaal.Status, zaal.Scherm);
                    count++;
                }
            }
            if (error != "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Helpers.Display.PrintLine("");
                Helpers.Display.PrintLine(error);
                Console.ForegroundColor = ConsoleColor.White;
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
                    else if(inputValue == -1)
                    {
                        return new ZaalModel(-2,"Verkeerde input, type een nummer in","","");
                    }
                    else
                    {
                        return new ZaalModel(-2,"Verkeerd nummer, type een nummer van de lijst in","","");
                    }
                   

                case Inputs.KeyAction.Escape: //de functie beeindigen
                    return new ZaalModel(-1, "", "", "");
            }
            return new ZaalModel(-2, "", "", "");
        }

        public FilmModel AssignFilm(string tijd, string datum, int zaalid,string error = "")
        {
            Helpers.Display.PrintLine("[ESC] teruggaan");
            
            List<FilmModel> films = Repository.FilmData.LoadData();
            int i = 1;
            Helpers.Display.PrintLine("");
            Helpers.Display.PrintLine($"Datum: {datum}");
            Helpers.Display.PrintLine($"Zaal: {ZaalData.LoadData().Where(z => z.ZaalId == zaalid).ToList()[0]}");
            Helpers.Display.PrintLine($"Tijd: {tijd}");
            Helpers.Display.PrintLine("");
            Helpers.Display.PrintHeader("No", "Naam", "Omschrijving", "Genre", "Kijkwijzer");
            foreach (FilmModel film in films)
            {
                Helpers.Display.PrintFilmTable(i.ToString(), film.Naam, film.Omschrijving, film.Genre, film.Kijkwijzer);
                i++;
            }

            if (error != "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Helpers.Display.PrintLine("");
                Helpers.Display.PrintLine(error);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Helpers.Display.PrintLine(" ");
            Helpers.Display.PrintLine("Typ welke film u wilt toevoegen");
            Inputs.KeyInput input = Inputs.ReadUserData();

            switch (input.action)
            {
                case Inputs.KeyAction.Enter:
                    int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;
                    inputValue--; //-1 want count start bij 0
                    if (inputValue >= 0 && inputValue < films.Count)
                    {
                        return films[inputValue]; //waarde meegeven van de gekozen film
                    } else if (inputValue == -1)
                    {
                        return new FilmModel(-2,"","Verkeerde input, vul een nummer in","","","","");
                    } else
                    {
                        return new FilmModel(-2, "", "Verkeerd nummer, vul een nummer uit de lijst in", "", "", "", "");
                    }

                    
                case Inputs.KeyAction.Escape: //de functie beeindigen
                    return new FilmModel(-1, "", "", "", "", "", "");
            }
            return new FilmModel(-2, "", "", "", "", "", "");
        }

        public string AssignTijd(string datum, int zaalid,string error = "")
        {
            Helpers.Display.PrintLine("[ESC] teruggaan");

            string res = "";
            string[] tijden = new string[4] { "10:00", "13:30", "17:00", "20:30" };

            Helpers.Display.PrintLine("");
            Helpers.Display.PrintLine($"Datum: {datum}");
            Helpers.Display.PrintLine($"Zaal: {ZaalData.LoadData().Where(z => z.ZaalId == zaalid).ToList()[0].Omschrijving}");
            Helpers.Display.PrintLine("");
            int count = 0;
            foreach (string tijd in tijden)
            {
                if (FilmschemaData.TimeCollides(datum, zaalid, tijd) == false)
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
            foreach (string tijd in tijden)
            {
                Helpers.Display.PrintTable(count.ToString(), tijd);
                count++;
            }
            



            Helpers.Display.PrintLine("");
            Helpers.Display.PrintLine("Type welke tijd u wilt toevoegen");

            if(error != "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Helpers.Display.PrintLine("");
                Helpers.Display.PrintLine(error);
                Console.ForegroundColor = ConsoleColor.White;
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
                    else if (inputValue == -1)
                    {
                        return "Verkeerde input, vul een nummer in";
                    }
                    else
                    {
                        return "Verkeerd nummer, vul een nummer uit de lijst in";
                    }

                    
                case Inputs.KeyAction.Escape: //de functie beeindigen
                    return "abort";
            }
            return res;
        }

        public string AssignDatum(string error = "")
        {
            string res = "";
            Helpers.Display.PrintLine("[ESC] teruggaan");
            string[] dagen = FilmschemaData.VolgendeDagen(1, 14);
            int count = 0;
            
            foreach (string dag in dagen)
            {
                if (FilmschemaData.DateCollides(dag) == false)
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
            foreach (string dag in dagen)
            {
                Helpers.Display.PrintLine($"{count}.  {dag}");
                count++;
            }
            Helpers.Display.PrintLine("");
            Helpers.Display.PrintLine("Vul in welke dag u wilt toevoegen");
            Helpers.Display.PrintLine("");
            if(error != "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Helpers.Display.PrintLine("");
                Helpers.Display.PrintLine(error);
                Console.ForegroundColor = ConsoleColor.White;
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
                    else if (inputValue == -1)
                    {
                        return "Verkeerde input, vul een nummer in";
                    }
                    else
                    {
                        return "Verkeerd nummer, vul een nummer uit de lijst in";
                    }

                    
                case Inputs.KeyAction.Escape: //de functie beeindigen
                    return "abort";
            }
            return res;
        }

        public void VeranderProgramma(int index)
        {
            bool abort = false;
            List<FilmschemaModel> filmschema = FilmschemaData.LoadData();
            List<ZaalModel> zaalData = ZaalData.LoadData();
            List<FilmModel> filmData = FilmData.LoadData();
            FilmschemaModel programma = filmschema[index];


            Console.Clear();
            while (!abort)
            {
                Helpers.Display.PrintLine($"Datum: {programma.Datum}");
                Helpers.Display.PrintLine($"Zaal: {zaalData.Where(zaal => zaal.ZaalId == programma.ZaalId).ToList()[0].Omschrijving}");
                Helpers.Display.PrintLine($"Film: {filmData.Where(film => film.FilmId == programma.FilmId).ToList()[0].Naam}");
                Helpers.Display.PrintLine($"Tijd: {programma.Tijd}");


                Helpers.Display.PrintLine("[ESC] verlaten               [INS] opslaan");
                Helpers.Display.PrintLine(" ");
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
                        Console.Clear();

                        Helpers.Display.PrintLine("Programma opgeslagen\n\n");
                        Console.Clear();
                        break;
                    case Inputs.KeyAction.Enter:
                        int inputValue = Int32.TryParse(input.val, out inputValue) ? inputValue : 0;

                        if (inputValue >= 1 && inputValue <= 4)
                        {
                            switch (inputValue)
                            {
                                case 1:
                                    Console.Clear();
                                    string datum = AssignDatum();
                                    if (datum != "abort" && FilmschemaData.DatumSyntax(datum) == false)
                                    {
                                        while (FilmschemaData.DatumSyntax(datum) == false && datum != "abort")
                                        {
                                            Console.Clear();
                                            Helpers.Display.PrintLine("Probeer het opnieuw\n\n");
                                            datum = AssignDatum();
                                            Console.Clear();
                                        }
                                        if (datum != "abort")
                                        {
                                            programma.Datum = datum;
                                        }

                                    }
                                    else
                                    {
                                        if (datum != "abort")
                                        {
                                            programma.Datum = datum;
                                        }
                                    }
                                    Console.Clear();
                                    break;
                                case 2:
                                    Console.Clear();
                                    int zaalid = AssignZaal(programma.Datum).ZaalId;
                                    if (zaalid >= 0)
                                    {
                                        programma.ZaalId = zaalid;
                                    }
                                    else
                                    {
                                        while (zaalid == -2)
                                        {
                                            Console.Clear();
                                            Helpers.Display.PrintLine("Probeer het opnieuw\n\n");
                                            zaalid = AssignZaal(programma.Datum).ZaalId;
                                            Console.Clear();
                                        }
                                        if (zaalid != -1)
                                        {
                                            programma.ZaalId = zaalid;
                                        }
                                    }
                                    Console.Clear();
                                    break;
                                case 3:
                                    Console.Clear();
                                    FilmModel film = AssignFilm(programma.Tijd,programma.Datum,programma.ZaalId);
                                    int filmid = film.FilmId;
                                    if (filmid >= 0)
                                    {
                                        programma.FilmId = filmid;
                                    }
                                    else
                                    {
                                        while (filmid == -2)
                                        {
                                            film = AssignFilm(programma.Tijd,programma.Datum,programma.ZaalId,film.Omschrijving);
                                            filmid = film.FilmId;
                                            Console.Clear();
                                        }
                                        if (filmid != -1)
                                        {
                                            programma.FilmId = filmid;
                                        }
                                    }
                                    Console.Clear();
                                    break;
                                case 4:
                                    Console.Clear();
                                    string tijd = AssignTijd(programma.Datum, programma.ZaalId);
                                    if (tijd != "abort" && FilmschemaData.TijdSyntax(tijd) == false)
                                    {
                                        while (!FilmschemaData.TijdSyntax(tijd) && tijd != "abort")
                                        {
                                            Console.Clear();
                                            Helpers.Display.PrintLine("Probeer het opnieuw\n\n");

                                            tijd = AssignTijd(programma.Datum, programma.ZaalId);
                                            Console.Clear();
                                        }
                                        if (tijd != "abort")
                                        {
                                            programma.Tijd = tijd;
                                        }
                                    }
                                    else
                                    {
                                        if (tijd != "abort")
                                        {
                                            programma.Tijd = tijd;
                                        }
                                    }
                                    Console.Clear();
                                    break;

                            }
                            Helpers.Display.PrintLine("Programma aangepast");
                            Helpers.Display.PrintLine("");
                        }
                        else
                        {
                            Console.Clear();
                        }
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            }
        }
    }
}
