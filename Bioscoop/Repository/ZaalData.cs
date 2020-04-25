using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;

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

            var toRemove = zaalData.Where(a => a.ZaalId == data.ZaalId).ToList();

            //var setToRemove = new HashSet<ZaalModel>(zaalData);
            //zaalData.RemoveAll(x => setToRemove.Contains(x));
            foreach (var remove in toRemove)
            {
                zaalData.Remove(remove);
            }

            // Update json data string
            var jsondata = JsonConvert.SerializeObject(zaalData, Formatting.Indented);
            System.IO.File.WriteAllText(jsonPath, jsondata);
        }
        public static void EditData(ZaalModel data)
        {
            dynamic zaalData = LoadData();

            zaalData[data.ZaalId - 1] = data;

            //string json = File.ReadAllText("settings.json");
            //dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            //jsonObj["Bots"][0]["Password"] = "new password";
            //string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            //File.WriteAllText("settings.json", output);

            // Update json data string
            var jsondata = JsonConvert.SerializeObject(zaalData, Formatting.Indented);
            System.IO.File.WriteAllText(jsonPath, jsondata);
        }
    }
}
