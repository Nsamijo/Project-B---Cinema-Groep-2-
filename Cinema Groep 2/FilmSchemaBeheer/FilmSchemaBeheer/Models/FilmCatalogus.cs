using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace FilmSchemaBeheer
{
    public class FilmCatalogus
    {
        public Film[] inhoud;


        public FilmCatalogus()
        {
            LeesFilms();
        }
        public void PrintFilms()
        {
            foreach (Film f in inhoud)
            {
                Console.WriteLine(f.Info());
            }
        }
        public Film VindFilmdDoorId(string id)
        {
            Film res;

            foreach (Film f in inhoud)
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
        public void LeesFilms()
        {
            using (StreamReader file = File.OpenText(new Finder().SearchFile("Films.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                this.inhoud = (Film[])serializer.Deserialize(file, typeof(Film[]));
            }
        }
    }
}
