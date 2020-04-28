using System;
using System.Collections.Generic;
using System.Text;

namespace FilmSchemaBeheer
{
    public class VeranderProgramma
    {
        public void Run(Planning planning)
        {
            bool klaar = false;
            bool isint = false;
            Console.Clear();
            planning.PrintInhoud();
            Console.WriteLine("Welk Programma wilt u veranderen?");
            string input = Console.ReadLine();
            
            

        }
    }
}
