﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Schedulerr
{
    public class Programma
    {
        public int programmaid { get; set; }
        public string datum { get; set; }
        public string tijd { get; set; }
        public string filmid { get; set; }

        public string filmnaam { get; set; }

        

        public string Info()
        {
            return $"\n{programmaid}.   Datum: {datum}\n     Tijd: {tijd}\n     film: {filmnaam}";
        }
        
    }
}
