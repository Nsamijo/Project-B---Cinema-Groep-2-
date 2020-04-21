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
            new FilmSchemaBeheer().Run();
        }
    }
}
