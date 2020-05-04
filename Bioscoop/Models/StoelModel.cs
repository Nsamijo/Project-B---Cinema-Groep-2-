using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bioscoop.Models
{
    public class StoelModel
    {
        public int StoelId { get; set; }
        public string Omschrijving { get; set; }
        public string Rij { get; set; }
        public int StoelNr { get; set; }
        public bool Premium { get; set; }
        public int ZaalId { get; set; }

        public StoelModel() { }
        public StoelModel(int stoelId, string omschrijving, string rij, int stoelNr, bool premium, int zaalId)
        {
            StoelId = stoelId;
            Omschrijving = omschrijving;
            Rij = rij;
            StoelNr = stoelNr;
            Premium = premium;
            ZaalId = zaalId;
        }
    }
}