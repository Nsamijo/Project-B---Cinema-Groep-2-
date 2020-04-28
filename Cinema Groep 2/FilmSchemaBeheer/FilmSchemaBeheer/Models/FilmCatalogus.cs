using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace FilmSchemaBeheer
{
    public class FilmCatalogus
    {
        public Film[] Inhoud;

        //Constructor
        public FilmCatalogus()
        {
            LeesFilms();
        }
        //Print alle Inhoud van this.Inhoud
        public void PrintFilms()
        {
            foreach (Film f in Inhoud)
            {
                Console.WriteLine(f.Info());
            }
        }
        //Returned een film door het ID
        public Film VindFilmdDoorId(int id)
        {
            Film res;

            foreach (Film f in Inhoud)
            {
                if (f.FilmId == id)
                {
                    res = f;
                    return res;
                }
            }
            res = null;
            return res;
        }
        //Leest de films van Films.json
        public void LeesFilms()
        {
            using (StreamReader file = File.OpenText(new Finder().SearchFile("Films.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                this.Inhoud = (Film[])serializer.Deserialize(file, typeof(Film[]));
            }
        }
    }
}
