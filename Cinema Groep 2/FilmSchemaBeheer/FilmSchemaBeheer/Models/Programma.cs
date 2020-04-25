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
        public string FilmId { get; set; }

        public string FilmNaam { get; set; }

        public int ZaalId { get; set; }

        
        //Returned een string met de info van een programma
        public string Info()
        {
            return $"\n{ProgrammaId}.   Datum: {Datum}\n     Tijd: {Tijd}\n     film: {FilmNaam}";
        }
        
    }
}
