using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

public class Stoel
{
    public int StoelId { get; set; }
    public string Omschrijving { get; set; }
    public string Rij { get; set; }
    public int StoelNr { get; set; }
    public bool Premium { get; set; }
    public string Zaalid { get; set; }

    public Stoel() { }

    public Stoel(int stoelId, string omschrijving, string rij, int stoelNr, bool premium, string zaalId)
    {
        StoelId = stoelId;
        Omschrijving = omschrijving;
        Rij = rij;
        StoelNr = stoelNr;
        Premium = premium;
        Zaalid = zaalId;
    }
}

