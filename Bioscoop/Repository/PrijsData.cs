using System;
using System.Collections.Generic;
using Bioscoop.Helpers;
using Bioscoop.Models;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace Bioscoop.Repository
{
    class PrijsData
    {
        public static string jsonPath => Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Prijs.json";

        public static List<PrijsModel> LoadData() //ophalen json data als list functie
        {
            string jsonFilePath = jsonPath;
            string _json = File.ReadAllText(jsonFilePath);
            
            return JsonConvert.DeserializeObject<List<PrijsModel>>(_json);
        }
    }
}
