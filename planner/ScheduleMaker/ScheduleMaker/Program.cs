using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace ScheduleMaker
{

    public class Programma
    {
        public string film { get; set; }
        public int[] datum { get; set; }
        public int[] startTijd { get; set; }
        public int[] duur { get; set; }
        public int Zaal { get; set; }
        public int stoelen { get; set; }
        public int vrijeStoelen { get; set; }




    }
    public class Planning
    {
        public string name { get; set; }
        public Programma[] content;

        


    }


    class Program
    {
        //geeft een bool terug wat laat zien of tijd1 later is dan tijd2


        public static Planning LeesJson()
        {

        }

        public static Planning MaakProgramma(Planning planning, string film, int sT, int[] d, int hall, int s)
        {
            int[][] tijden = new int[5][];
            int x = 10;
            for (int i = 0; i < tijden.Length; i++)
            {
                tijden[i] = new int[] { x, 0 };
                x += 3;
            }



            for (int i = 0; i < planning.content.Length; i++)
            {
                if (planning.content[i] == null)
                {

                    planning[i] = new Programma
                    {
                        film = film,
                        startTijd = tijden[sT],
                        duur = d,
                        Zaal = hall,
                        stoelen = s

                    };
                    return planning;

                }
            }
            Console.WriteLine("Geen vrije plekken voor deze film");
            return planning;
        }

        

        
        static void Main(string[] args)
        {

            Programma[] planning = new Programma[5];
            


            string input;
            bool exit = false;
            while (exit != true)
            {
                Console.Clear();
                Console.WriteLine("Wat zou je willen doen?");
                Console.WriteLine("[A] Planning bekijken \n[C] Programma maken\n[D] Programma verwijderen\n[E] Verlaten");
                input = Console.ReadLine();
                input.ToUpper();

                switch (input)
                {
                    case "A":
                    case "a":
                        for (int i = 0; i < 5; i++)
                        {
                            if (planning[i] != null)
                            {
                                Console.WriteLine($"Film: {planning[i].film}\nTijd: {planning[i].startTijd[0]}:{planning[i].startTijd[1]}\n");
                            }
                            else
                            {
                                Console.WriteLine("\nLege plek\n");
                            }
                        }
                        Console.WriteLine("\n\n\n druk op enter om door te gaan");
                        while (Console.ReadKey().Key != ConsoleKey.Enter)
                        {
                            Thread.Sleep(1);
                        }
                        break;

                    case "C":
                    case "c":


                        while (Console.ReadKey().Key != ConsoleKey.Enter)
                        {
                            Thread.Sleep(1);
                        }
                        break;
                    case "D":
                    case "d":
                        Console.WriteLine($"Case {input.ToUpper()}");
                        Console.WriteLine("Druk op enter om door te gaan");
                        while (Console.ReadKey().Key != ConsoleKey.Enter)
                        {
                            Thread.Sleep(1);
                        }
                        break;
                    case "e":
                    case "E":
                        Console.WriteLine("Exitting");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Nope");
                        Console.WriteLine("Druk op enter om door te gaan");
                        while (Console.ReadKey().Key != ConsoleKey.Enter)
                        {
                            Thread.Sleep(1);
                        }
                        break;


                }
            }


            }
        }
    }

