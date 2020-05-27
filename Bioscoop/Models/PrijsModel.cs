using System;
using System.Collections.Generic;
using System.Text;

namespace Bioscoop.Models
{
    class PrijsModel
    {
        public string Soort { get; set; }
        public string Prijs { get; set; }

        public PrijsModel() { }
        public PrijsModel(string prijs, string soort )
        {
            Prijs = prijs;
            Soort = soort;
        }

    }
}
