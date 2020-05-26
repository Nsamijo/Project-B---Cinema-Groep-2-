using System;
using System.Collections.Generic;
using System.IO;
using Bioscoop.Models;
using Newtonsoft.Json;

namespace Bioscoop.Repository
{
    public class StoelData
    {
        //om naar json te updaten vanaf hier
        public dynamic array;

        public StoelData()
        {
            LoadJson();
        }

        public string jsonPath => (Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Stoel.json");
        public void LoadJson(){
            string file = jsonPath;
            string json = File.ReadAllText(file); ;
            this.array = JsonConvert.DeserializeObject(json);
        }



        public dynamic GetJson()
        {
            string file = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Stoel.json";
            string data = File.ReadAllText(file);

            return JsonConvert.DeserializeObject<List<StoelModel>>(data);
        }

        public void UpdateJson(dynamic a)
        {
            if (!JsonConvert.SerializeObject(this.array).Equals(JsonConvert.SerializeObject(a)))
            {
                this.array = a;
                string informatie = JsonConvert.SerializeObject(a);
                try
                {
                    File.WriteAllText(jsonPath, informatie);
                }
                catch (Exception)
                {
                    Helpers.Display.PrintLine("file not found, de json is niet geupdatet");
                }
            }
        }
    }
}
