using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace Bioscoop.Repository
{
    class ZaalData
    {
        public static string jsonPath => Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Zaal.json";

        public static List<ZaalModel> LoadData()
        {
            string jsonFilePath = jsonPath;
            string _json = File.ReadAllText(jsonFilePath);

            return JsonConvert.DeserializeObject<List<ZaalModel>>(_json);
        }
        public static int getId()
        {
            //laatste id+1 voor toevoegen van nieuwe zaal. moest dubbel op definitieren anders error.
            var jsonData = LoadData();
            int lastId = jsonData.Count() - 1;
            int zaalId = lastId + 2;
            return zaalId;
            //var lastElement = jsonData[lastId].ZaalId;
        }

        public static void AddData(ZaalModel data)
        {
            List<ZaalModel> zaalData = LoadData();
            data.ZaalId = getId();
            zaalData.Add(data);

            // Update json data string
            var jsondata = JsonConvert.SerializeObject(zaalData, Formatting.Indented);
            System.IO.File.WriteAllText(jsonPath, jsondata);
        }
        public static void RemoveData(ZaalModel data)
        {
            List<ZaalModel> zaalData = LoadData();
            zaalData.Remove(data);

            // Update json data string
            var jsondata = JsonConvert.SerializeObject(zaalData, Formatting.Indented);
            System.IO.File.WriteAllText(jsonPath, jsondata);
        }
        public static void EditData(ZaalModel data)
        {
            List<ZaalModel> zaalData = LoadData();
            zaalData.Remove(data);
            zaalData.Add(data);

            // Update json data string
            var jsondata = JsonConvert.SerializeObject(zaalData, Formatting.Indented);
            System.IO.File.WriteAllText(jsonPath, jsondata);
        }
    }
}
