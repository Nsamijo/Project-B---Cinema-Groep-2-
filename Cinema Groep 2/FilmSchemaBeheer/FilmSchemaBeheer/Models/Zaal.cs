using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace FilmSchemaBeheer
{
    public class Zaal
    {
        public int ZaalId { get; set; }
        public string Omschrijving { get; set; }
        public string Status { get; set; }
        public string Scherm { get; set; }
        //Returned een string met info van de zaal
        public string Info()
        {
            return $"{Omschrijving}\nstatus: {Status}\nScherm: {Scherm}";
        }
    }
}
