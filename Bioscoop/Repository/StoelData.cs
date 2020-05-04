using System;
using System.IO;
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

        public string jsonPath => Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Zaal.json";
        public void LoadJson()
        {
            string file = jsonPath;
            string json = File.ReadAllText(file); ;
            this.array = JsonConvert.DeserializeObject(json);
        }

        public dynamic GetJson()
        {
            return this.array;
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
