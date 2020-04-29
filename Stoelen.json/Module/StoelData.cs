using System;
using Newtonsoft.Json;
using System.IO;


namespace StoelData
{
    public class Data
    {
        //om naar json te updaten vanaf hier

        public dynamic array;

        public Data()
        {
            LoadJson();
        }

        public void LoadJson() //informatie van json laden
        {
            using StreamReader file = new StreamReader("C:/Users/Lenovo/source/repos/Stoelen.json/Stoelen.json/Data/data.json");
            string json = file.ReadToEnd();
            this.array = JsonConvert.DeserializeObject(json);
        }

        public dynamic getJson()
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
                    File.WriteAllText("C:/Users/Lenovo/source/repos/Stoelen.json/Stoelen.json/Data/data.json", informatie);
                }
                catch (Exception)
                {
                    Helpers.StoelenDisplay.PrintLine("file not found, de json is niet geupdatet");
                }
            }
        }
    }
}

/*    // deze functie veranderd Beschikbaar - Niet beschikbaar van de status
    public void ChangeStatus(int stoelNummer)
    {
        Console.Clear();
        List<Stoel> json = LoadJson();
        foreach (Stoel stoel in json)
        {
            if (stoel.StoelId == stoelNummer)
            {
                //veranderen van waarde (beschikbaar - niet beschikbaar en andersom)
                switch (stoel.Status)
                {
                    case "Beschikbaar":
                        stoel.Status = "Niet beschikbaar";
                        break;
                    case "Niet beschikbaar":
                        stoel.Status = "Beschikbaar";
                        break;
                }
            }
        }

        //streamwriter die terug naar json aanpast
        using (StreamWriter file = File.CreateText(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\data.json"))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, json);
        }

        StoelenAanpas(stoelNummer);
    }*/
