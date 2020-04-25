using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace FilmSchemaBeheer
{
    public class ZaalCatalogus
    {
        public Zaal[] Inhoud;

        //Constructor
        public ZaalCatalogus()
        {
            LeesZalen();
        }
        //Print alle Inhoud van this.Inhoud
        public void PrintZalen()
        {
            foreach (Zaal f in Inhoud)
            {
                Console.WriteLine(f.Info());
            }
        }
        //Returned een zaal door het ID
        public Zaal VindZaaldDoorId(int id)
        {
            Zaal res;

            foreach (Zaal z in Inhoud)
            {
                if (z.ZaalId == id)
                {
                    res = z;
                    return res;
                }
            }
            res = null;
            return res;
        }
        //Leest de films van zalen.json
        public void LeesZalen()
        {
            using (StreamReader file = File.OpenText(new Finder().SearchFile("Zalen.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                this.Inhoud = (Zaal[])serializer.Deserialize(file, typeof(Zaal[]));
            }
        }
    }
}

