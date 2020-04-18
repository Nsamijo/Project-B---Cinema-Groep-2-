using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;


namespace Schedulerr
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Planning planning = new Planning()
            {
                naam = "planning"
            };
            bool exit = false;
            while (exit != true)
            {
                Console.Clear();
                Console.WriteLine("[A] zie planning\n[B] Maak programma aan\n[C] Verwijder programma\n[E] Verlaat");
                char opdracht = Console.ReadKey().KeyChar;
                switch (opdracht)
                {
                    case 'a':
                        new ZiePlanning().Run(planning);
                        break;
                    case 'b':
                        new MaakProgramma().Run(planning);
                        break;
                    case 'c':
                        new VerwijderProgramma().Run(planning);
                        break;
                    case 'e':
                        planning.UpdateNaarJson();
                        exit = true;
                        break;
                }
            }
        }
    }
}
