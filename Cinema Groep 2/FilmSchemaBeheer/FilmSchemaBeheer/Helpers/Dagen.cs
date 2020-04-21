using System;
using System.Collections.Generic;
using System.Text;

namespace FilmSchemaBeheer
{
    public class Dagen
    {
        
        public string[] VolgendeDagen(int start, int eind)
        {
            DateTime vandaag = DateTime.Today;
            DateTime dag = vandaag.AddDays(start);
            string[] res = new string[eind-start];
            for (int i = start; i < eind; i++)
            {
                string dagnul = dag.Day / 10 == 0 ? "0" : "";
                string maandnul = dag.Month / 10 == 0 ? "0" : "";
                res[i-start] = $"{dagnul}{dag.Day}/{maandnul}{dag.Month}/{dag.Year}";
                dag = dag.AddDays(1);

            }
            return res;

        }
        public string PrintVolgendeDagen(int start, int eind)
        {
          
            string res = "";
            int i = 1;
            foreach(string dag in VolgendeDagen(start,eind))
            {

                res += $"{i}. {dag}\n";
                i++;
            }
            return res;
        }
    }
}
