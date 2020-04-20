using System;
using System.Collections.Generic;
using System.Text;

namespace Schedulerr
{
    public class Film
    {
        public string FilmId { get; set; }
        public string Naam { get; set; }
        public string Omschrijving { get; set; }
        public string Genre { get; set; }
        public string Duur { get; set; }
        public string Kijkwijzer { get; set; }
        public string Status { get; set; }

        public string Info()
        {
            return $"\n{FilmId}.   Naam: {Naam}\n    Genre: {Genre}\n    Duur: {Duur}\n    Kijkwijzer: {Kijkwijzer}\n    Status: {Status}\n";
        }
    }
}
