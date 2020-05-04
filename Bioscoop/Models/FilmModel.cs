using System;
using System.Collections.Generic;
using System.Text;

namespace Bioscoop.Models
{
    class FilmModel
    {
        public int FilmId { get; set; }
        public string Naam { get; set; }
        public string Omschrijving { get; set; }
        public string Genre { get; set; }
        public string Duur { get; set; }
        public string Kijkwijzer { get; set; }
        public string Status { get; set; }

        public FilmModel() { }

        public FilmModel(int filmId, string naam, string omschrijving, string genre, string duur, string kijkwijzer, string status)
        {
            FilmId = filmId;
            Naam = naam;
            Omschrijving = omschrijving;
            Genre = genre;
            Duur = duur;
            Kijkwijzer = kijkwijzer;
            Status = status;
        }
    }

}
