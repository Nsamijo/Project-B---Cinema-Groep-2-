using System;
using System.Collections.Generic;
using System.Text;

namespace Bioscoop.Models
{
    class FilmschemaModel 
    {
        public int ProgrammaId { get; set; }
        public string Datum { get; set; }
        public string Tijd { get; set; }
        public int FilmId { get; set; } 
        public int ZaalId { get; set; }

        public FilmschemaModel(int programmaId, string datum, string tijd, int filmId, int zaalId)
        {
            this.ProgrammaId = programmaId;
            this.Datum = datum;
            this.Tijd = tijd;
            this.FilmId = filmId;
            this.ZaalId = zaalId;
        }
    }
}
