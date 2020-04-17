using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace Schedulerr.Module
{
    class Head
    {
        public static void Run()
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
                        planning.PrintInhoud();
                        Console.WriteLine("Druk op enter om door te gaan");
                        while (Console.ReadKey().Key != ConsoleKey.Enter)
                        {
                            Thread.Sleep(1);
                        }
                        break;
                    case 'b':
                        Console.Clear();
                        Console.WriteLine("Schrijf datum in syntax: DD/MM/YYYY");
                        string datum = Console.ReadLine();
                        Console.Clear();
                        Console.WriteLine("Schrijf starttijd in syntax: hh:mm");
                        string tijd = Console.ReadLine();
                        Console.Clear();
                        planning.ProgrammaToevoegen(datum, tijd);
                        Console.WriteLine("Programma toegevoegd, druk op enter om door te gaan");
                        while (Console.ReadKey().Key != ConsoleKey.Enter)
                        {
                            Thread.Sleep(1);
                        }
                        break;
                    case 'c':
                        planning.PrintInhoud();
                        Console.WriteLine("Welk programma wilt u verwijderen(typ het nummer)");
                        string input = Console.ReadLine();

                        Console.WriteLine("Programma verwijderd, druk op enter om door te gaan");
                        while (Console.ReadKey().Key != ConsoleKey.Enter)
                        {
                            Thread.Sleep(1);
                        }
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
