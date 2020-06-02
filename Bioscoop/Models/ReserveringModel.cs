using System;
using System.Collections.Generic;
using System.Text;

namespace Bioscoop.Models
{
    class ReserveringModel
    {
        public int ReserveringId { get; set; }
        public string Code { get; set; }
        public int ProgrammaId { get; set; }
        public List<int> StoelId { get; set; }
        public int Aantal { get; set; }
        public string Totaal { get; set; }

        public ReserveringModel() {
            StoelId = new List<int>();
        }

        public ReserveringModel(int reserveringId, string code, int programmaId, List<int> stoelId, int aantal, string totaal)
        {
            ReserveringId = reserveringId;
            Code = code;
            ProgrammaId = programmaId;
            StoelId = stoelId;
            Aantal = aantal;
            Totaal = totaal;
        }
    }
}
