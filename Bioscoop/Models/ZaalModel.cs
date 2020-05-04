using System;
using System.Collections.Generic;
using System.Text;

namespace Bioscoop.Models
{
    class ZaalModel
    {
        public int ZaalId { get; set; }
        public string Omschrijving { get; set; }
        public string Status { get; set; }
        public string Scherm { get; set; }

        public ZaalModel() { }

        public ZaalModel(int zaalId, string omschrijving, string status, string scherm)
        {
            ZaalId = zaalId;
            Omschrijving = omschrijving;
            Status = status;
            Scherm = scherm;
        }
    }
}
