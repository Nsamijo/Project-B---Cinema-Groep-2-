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
            Console.CursorVisible = true;
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
                Helpers.Display.PrintHeader("No", "Zaal", "Film", "Datum", "Tijd");
                foreach (FilmschemaModel programma in filmschema)
                {
                    try
                    {
                        string filmnaam = filmData.Where(film => film.FilmId == programma.FilmId).ToList()[0].Naam;
                        string zaalnaam = zaalData.Where(zaal => zaal.ZaalId == programma.ZaalId).ToList()[0].Omschrijving;
                        Helpers.Display.PrintTable(i.ToString(), zaalnaam, filmnaam, programma.Datum, programma.Tijd);
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
                        datum = AssignDatum();
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
                        z = AssignZaal(datum);
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
                    tijd = AssignTijd(datum, zaalid);
                    if (tijd == "abort")
                    {
                        abort = true;
                    }
                }
            }
            if (!abort)
            {
                Console.Clear();
                FilmModel film = AssignFilm();
                if (film.FilmId == -1)
                {
                    abort = true;
                }
                if (film.FilmId == -2 && !abort)
                {
                    while (film.FilmId == -2 && !abort)
                    {
                        Console.Clear();
                        film = AssignFilm();
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
            Helpers.Display.PrintHeader("Nr", "Zaal", "film", "Datum", "Tijd");
            foreach (FilmschemaModel programma in filmschema)
            {
                string filmnaam = filmData.Where(film => film.FilmId == programma.FilmId).ToList()[0].Naam;
                string zaalnaam = zaalData.Where(zaal => zaal.ZaalId == programma.ZaalId).ToList()[0].Omschrijving;

                Helpers.Display.PrintTable(i.ToString(), zaalnaam, filmnaam, programma.Datum, programma.Tijd);
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

        public ZaalModel AssignZaal(string datum)
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
            int count = 1;
            foreach (ZaalModel zaal in zalen)
            {
                if (FilmschemaData.HallCollides(datum, zaal.ZaalId) == false)
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
                        Console.WriteLine( "Onjuist waarde ingevuld.");
                    }
                    break;

                case Inputs.KeyAction.Escape: //de functie beeindigen
                    return new ZaalModel(-1, "", "", "");
            }
            return new ZaalModel(-2, "", "", "");
        }

        public FilmModel AssignFilm()
        {
            Helpers.Display.PrintLine("[ESC] teruggaan");

            List<FilmModel> films = Repository.FilmData.LoadData();
            int i = 1;
            Helpers.Display.PrintHeader("No", "Naam", "Omschrijving", "Genre", "Kijkwijzer");
            foreach (FilmModel film in films)
            {
                Helpers.Display.PrintTable(i.ToString(), film.Naam, film.Omschrijving, film.Genre, film.Kijkwijzer);
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
                    return new FilmModel(-1, "", "", "", "", "", "");
            }
            return new FilmModel(-2, "", "", "", "", "", "");
        }

        public string AssignTijd(string datum, int zaalid)
        {
            Helpers.Display.PrintLine("[ESC] teruggaan");

            string res = "";
            string[] tijden = new string[4] { "10:00", "13:30", "17:00", "20:30" };

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
                    return "abort";
            }
            return res;
        }

        public string AssignDatum()
        {
            string res = "";
            Helpers.Display.PrintLine("[ESC] teruggaan");
            string[] dagen = FilmschemaData.VolgendeDagen(2, 14);
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
                                    int filmid = AssignFilm().FilmId;
                                    if (filmid >= 0)
                                    {
                                        programma.FilmId = filmid;
                                    }
                                    else
                                    {
                                        while (filmid == -2)
                                        {
                                            Console.Clear();
                                            Helpers.Display.PrintLine("Probeer het opnieuw\n\n");
                                            filmid = AssignFilm().FilmId;
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
