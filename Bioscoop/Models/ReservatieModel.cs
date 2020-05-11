using System;
using System.Collections.Generic;
using System.Text;

namespace Bioscoop.Models
{
    class ReservatieModel
    {
        public int ReservatieId { get; set; }
        public string Code { get; set; }
        public int ProgrammaId { get; set; }
        public List<int> StoelId { get; set; }
        public int Aantal { get; set; }
        public string Totaal { get; set; }

        public ReservatieModel() { }

        public ReservatieModel(int reservatieId, string code, int programmaId, List<int> stoelId, int aantal, string totaal)
        {
            ReservatieId = reservatieId;
            Code = code;
            ProgrammaId = programmaId;
            StoelId = stoelId;
            Aantal = aantal;
            Totaal = totaal;
        }
    }
}
