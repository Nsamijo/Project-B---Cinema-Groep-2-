using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace FilmSchemaBeheer
{
    public class Planning
    {
        //zet de naam van de planning
        public string naam { get; set; }
        //Creeërt de Inhoud van programma's
        public Programma[] Inhoud;
        //Get en Set de filmCatalogus
        public FilmCatalogus Films { get; set; }
        //Get en Set de ZaalCatalogus
        public ZaalCatalogus Zalen { get; set; }

        //Constructor, haalt json data op van JSON's
        public Planning()
        {
            this.LeesPlanning();
        }
        
        //Print de Inhoud van de planning
        public void PrintInhoud()
        {
            Console.WriteLine("Programmas:");
            
            for (int i = 0; i < this.Inhoud.Length; i++)
            {
                try
                {
                    Console.WriteLine(Inhoud[i].Info());
                }
                catch
                {
                    Console.WriteLine("");
                }
            }
        }
        //Kiest een nieuw Id op basis van overgebleven Id's in de Array
        public int KiesId()
        {
            int max = 1;
            for (int i = 0; i < this.Inhoud.Length; i++)
            {
                if (this.Inhoud[i] != null)
                {
                    if (this.Inhoud[i].ProgrammaId == max)
                    {
                        max++;
                        i = 0;
                    }
                }
             
            }
            return max;
            
            
        }
        //Voegt een programma toe aan de Array
        public void ProgrammaToevoegen(string Datum,string Tijd,string FilmId,int ZaalId)
        {
            
            Programma[] res = new Programma[this.Inhoud.Length+1];
            int i = 0;
            for(i = 0;i < this.Inhoud.Length; i++)
            {
                res[i] = this.Inhoud[i];
            }
            for (int j = 0; j < res.Length; j++)
            {
                if (res[j] == null)
                {
                    res[j] = new Programma()
                    {
                        ProgrammaId = this.KiesId(),
                        Datum = Datum,
                        Tijd = Tijd,
                        FilmId = FilmId,
                        FilmNaam = Films.VindFilmdDoorId(FilmId).Naam,
                        ZaalId = ZaalId
                    };
                }
            }
            
            this.Inhoud = res;
        }
        //Vindt de index van een programma in de lijst door het id, en returned deze index
        public int VindIndexDoorId(int id)
        {
            for(int i = 0; i < this.Inhoud.Length; i++)
            {
                if(this.Inhoud[i].ProgrammaId == id)
                {
                    return i;
                }
            }
            return -99;
        }

        //Vindt het object van een programma in de lijst door het id, en returned deze index
        public Programma VindObjectDoorId(int id)
        {
            for (int i = 0; i < this.Inhoud.Length; i++)
            {
                if (this.Inhoud[i].ProgrammaId == id)
                {
                    return Inhoud[i];
                }
            }
            return null;
        }
        //verwijdert een programma uit this.Inhoud door het 
        //element op die index naar null te zetten en de null's te 
        //verwijderen uit this.Inhoud
        public void VerwijderProgramma(int n)
        {
            int index = VindIndexDoorId(n);
            this.Inhoud[index] = null;
            this.VerwijderNulls();
        }
        //Verwijdert alle nulls die in this.Inhoud zitten
        public void VerwijderNulls()
        {
            int count = 0;
            for(int i = 0; i < this.Inhoud.Length;i++)
            {
                if (this.Inhoud[i] == null)
                {
                    count++;
                }
            }
            Programma[] res = new Programma[this.Inhoud.Length - count];
            int j = 0;
            for(int i = 0; i < this.Inhoud.Length; i++)
            {
                if(this.Inhoud[i] != null)
                {
                    res[j] = this.Inhoud[i];
                    j++;
                }
            }
            this.Inhoud = res;
        }
        //Dumpt de elementen van this.Inhoud naar Planning.json
        public void UpdateNaarJson()
        {
            using (StreamWriter file = File.CreateText(new Finder().SearchFile("Planning.json")))
            {
                JsonSerializer serializer = new JsonSerializer();

                serializer.Serialize(file, this.Inhoud);

            }
        }
        //Leest de Inhoud van planning.json en deserialized het naar this.Inhoud
        public void LeesPlanning()
        {
            using (StreamReader file = File.OpenText(new Finder().SearchFile("Planning.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                this.Inhoud = (Programma[])serializer.Deserialize(file, typeof(Programma[]));
            }
        }

        

    }
}
