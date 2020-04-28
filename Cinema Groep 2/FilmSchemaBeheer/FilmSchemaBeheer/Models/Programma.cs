using System;
using System.Collections.Generic;
using System.Text;

namespace FilmSchemaBeheer
{
    public class Programma
    {
        public int ProgrammaId { get; set; }
        public string Datum { get; set; }
        public string Tijd { get; set; }
        public int FilmId { get; set; }

        public string FilmNaam { get; set; }

        public int ZaalId { get; set; }

        
        //Returned een string met de info van een programma
        public string Info()
        {
            return $"\n{ProgrammaId}.   Datum: {Datum}\n     Tijd: {Tijd}\n     film: {FilmNaam}";
        }
        public void VeranderTijd(string tijd)
        {
            this.Tijd = tijd;
        }
        public void VeranderDatum(string datum)
        {
            this.Datum = datum;
        }
        public void VeranderFilm(int filmid,string filmnaam)
        {
            this.FilmId = filmid;
            this.FilmNaam = filmnaam;
        }
        public void VeranderZaal(int zaalid)
        {
            this.ZaalId = zaalid;
        }
        
    }
}
