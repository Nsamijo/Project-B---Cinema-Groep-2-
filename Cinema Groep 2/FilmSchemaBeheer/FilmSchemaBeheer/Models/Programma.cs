using System;
using System.Collections.Generic;
using System.Text;

namespace FilmSchemaBeheer
{
    public class Programma
    {
        public int programmaid { get; set; }
        public string datum { get; set; }
        public string tijd { get; set; }
        public string filmid { get; set; }

        public string filmnaam { get; set; }

        
        //Returned een string met de info van een programma
        public string Info()
        {
            return $"\n{programmaid}.   Datum: {datum}\n     Tijd: {tijd}\n     film: {filmnaam}";
        }
        
    }
}
