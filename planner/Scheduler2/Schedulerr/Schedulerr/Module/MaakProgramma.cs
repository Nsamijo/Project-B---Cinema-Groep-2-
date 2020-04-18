using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Schedulerr
{
    public class MaakProgramma
    {
        public void Run(Planning planning)
        {
            string datum = "";
            string tijd = "";
            
            while (new Checker().DatumSyntax(datum) == false)
            {
                Console.Clear();
                Console.WriteLine("Schrijf datum in syntax: DD/MM/YYYY");
                datum = Console.ReadLine();
            }

            
            while(new Checker().TijdSyntax(tijd) == false)
            {
                Console.Clear();
                Console.WriteLine("Schrijf starttijd in syntax: hh:mm");
                tijd = Console.ReadLine();
            
            
            }

            Console.Clear();
            planning.ProgrammaToevoegen(datum, tijd);
            Console.WriteLine("Programma toegevoegd, druk op enter om door te gaan");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                Thread.Sleep(1);
            }
        }
    }
}
