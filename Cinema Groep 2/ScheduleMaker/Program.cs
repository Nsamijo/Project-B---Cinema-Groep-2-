using System;
using System.Threading;
namespace ScheduleMaker
{

    public class Showing
        {
            public string film;
            public int[] startTijd;
            public int[] duur;
            public int Zaal;
            public int vrijeStoelen;
            public int stoelen;
            public int[] eindTijd;

        //constructor showing
            public Showing(string film,int[] sT,int[] d,int zaal,int s)
            {
                this.film = film;
                this.startTijd = sT;
                this.duur = d;
                this.eindTijd = deltaTijd(sT,d);
                this.Zaal = zaal;
                this.vrijeStoelen = s;
                this.stoelen = s;
            }
            //Returned het verschil tussen twee tijden
            public static int[] deltaTijd(int[] sT, int[] d)
            {
                int[] resultaat = new int[2];

                resultaat[0] = sT[0] + d[0];
                int mins = sT[1] + d[1];
                int hrs = mins % 60;
                mins = hrs == 1 ? mins - 60 : mins;
                resultaat[1] = mins;
                resultaat[0] += hrs;
                return resultaat;
            }

    }



    class Program
    {
        //geeft een bool terug wat laat zien of tijd1 later is dan tijd2
        public static bool TijdGroterDan(int[] tijd1, int[] tijd2)
        {
            bool res;

            if(tijd1[0] > tijd2[0])
            {
                res = true;
            } else if(tijd1[0] == tijd2[0])
            {
                if(tijd1[1] > tijd2[1])
                {
                    res = true;
                } 
                else if(tijd1[1] == tijd2[1])
                {
                    res = false;
                }
                else
                {
                    res = false;
                }
            } else
            {
                res = false;
            }

            return res;


        }
        //geeft een bool terug die laat zien
        public static bool Overlap(Showing s1,Showing s2)
        {
            bool res;

            if(TijdGroterDan(s1.eindTijd,s2.startTijd) && TijdGroterDan(s2.eindTijd, s1.startTijd))
            {
                res = false;
            } else if (TijdGroterDan(s2.eindTijd, s1.startTijd) && TijdGroterDan(s1.eindTijd, s2.startTijd))
            {
                res = false;
            } else
            {
                res = true;
            }

                return res;
        }
        public static Showing[] CreateShowing(Showing[] planning,string film, int[] sT, int[] d, int hall, int fs, int s)
        {
            for(int i = 0; i < planning.Length; i++)
            {
                if(planning[i] == null)
                {

                    planning[i] = new Showing(film, sT, d, hall, s);
                    return planning;
                }
            }
            Console.WriteLine("Geen vrije plekken voor deze film");
            return null;
        }

        public static Showing[] SortSchedule(Showing[] schedule)
        {
            Showing lowest = schedule[0];

            for(int i = 0; i < schedule.Length; i++)
            {
                
            }

            return schedule;
        }
        
        static void Main(string[] args)
        {

            Showing[] schedule = new Showing[4];

            
            string input;
            bool exit = false;
            while(exit != true) {
                Console.Clear();
                Console.WriteLine("Hello, what would you like to do?");
                Console.WriteLine("[A] See schedule;[C] Create showing;[D] Delete showing;[E] Exit;");
                input = Console.ReadLine();
                input.ToUpper();
                
                switch (input)
                {
                    case "A":
                    case "a":

                    case "B":
                    case "b":

                    case "C":
                    case "c":
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
                        while(Console.ReadKey().Key != ConsoleKey.Enter)
                        {
                            Thread.Sleep(1);
                        }
                        break;
                     

                }
            }
                
            
        }
    }
}
