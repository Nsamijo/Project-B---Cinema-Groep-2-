using System;
using System.IO;
using Newtonsoft.Json;


namespace Schedulerr
{
    

    
    
    class Program
    {
        public static void SerializeObject(Planning p)
        {

        }
        static void Main(string[] args)
        {
            
            bool exit = false;
            while(exit != true)
            {
                Console.WriteLine("[A] zie planning\n[B] Maak programma aan\n[C] Verwijder programma [E] Verlaat");
                string opdracht = Console.ReadLine();
                switch (opdracht)
                {
                    case "a":
                    case "A":
                        break;

                }
            }
        }
    }
}
