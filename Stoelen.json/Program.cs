using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Stoelen.json
{
    class Program
    {
        static void Main(string[] args)
        {

            Stoelenbeheer beheer = new Stoelenbeheer();

            bool loop = true;
            while (loop)
            {
                switch (beheer.Run())
                {
                    case ConsoleKey.D5:
                        Console.Clear();
                        beheer.Run();
                        break;
                }
            }       
        }
    }
}
