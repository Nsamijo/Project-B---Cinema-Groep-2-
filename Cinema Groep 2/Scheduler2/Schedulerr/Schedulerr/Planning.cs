using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Schedulerr
{
    public class Planning
    {
        public string naam { get; set; }
        public Programma[] inhoud;

        public void ProgrammaToevoegen(string datum,string tijd)
        {
            Programma[] res = new Programma[this.inhoud.Length+1];
            int i = 0;
            for(i = 0;i < this.inhoud.Length; i++)
            {
                res[i] = this.inhoud[i];
            }
            res[i + 1] = new Programma() { 
                programmaid = res[i].programmaid + 1,
                datum = datum,
                tijd = tijd
            };
            this.inhoud = res;
        }

        public void UpdateNaarJson()
        {

        }
    }
}
