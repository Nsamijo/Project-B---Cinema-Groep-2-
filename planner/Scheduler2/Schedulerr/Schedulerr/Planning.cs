using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Schedulerr
{
    public class Planning
    {
        public string naam { get; set; }
        public Programma[] inhoud;

        public Planning()
        {
            this.LeesJson();
        }
        public void PrintInhoud()
        {
            Console.Clear();
            Console.WriteLine("Programmas:");
            for (int i = 0; i < this.inhoud.Length; i++)
            {
                try
                {
                    Console.WriteLine($"\n{this.inhoud[i].programmaid}.   Datum: {this.inhoud[i].datum}\n     Tijd: {this.inhoud[i].tijd}");
                }
                catch
                {
                    Console.WriteLine("");
                }
            }
        }
        public int KiesId()
        {
            int max = 1;
            for (int i = 0; i < this.inhoud.Length; i++)
            {
                if (this.inhoud[i] != null)
                {
                    if (this.inhoud[i].programmaid == max)
                    {
                        max++;
                        i = 0;
                    }
                }
             
            }
            return max;
            
            
        }
        public void ProgrammaToevoegen(string datum,string tijd)
        {
            
            Programma[] res = new Programma[this.inhoud.Length+1];
            int i = 0;
            for(i = 0;i < this.inhoud.Length; i++)
            {
                res[i] = this.inhoud[i];
            }
            for (int j = 0; j < res.Length; j++)
            {
                if (res[j] == null)
                {
                    res[j] = new Programma()
                    {
                        programmaid = this.KiesId(),
                        datum = datum,
                        tijd = tijd
                    };
                }
            }
            
            this.inhoud = res;
        }
        public int VindIndexDoorId(int id)
        {
            for(int i = 0; i < this.inhoud.Length; i++)
            {
                if(this.inhoud[i].programmaid == id)
                {
                    return i;
                }
            }
            return -99;
        }
        public Programma VindObjectDoorId(int id)
        {
            for (int i = 0; i < this.inhoud.Length; i++)
            {
                if (this.inhoud[i].programmaid == id)
                {
                    return inhoud[i];
                }
            }
            return null;
        }
        public void VerwijderProgramma(int n)
        {
            int index = VindIndexDoorId(n);
            this.inhoud[index] = null;
            this.VerwijderNulls();
        }

        public void VerwijderNulls()
        {
            int count = 0;
            for(int i = 0; i < this.inhoud.Length;)
            {
                if (this.inhoud[i] == null)
                {
                    count++;
                }
            }
            Programma[] res = new Programma[this.inhoud.Length - count];
            int j = 0;
            for(int i = 0; i < this.inhoud.Length; i++)
            {
                if(this.inhoud[i] != null)
                {
                    res[j] = this.inhoud[i];
                    j++;
                }
            }
            this.inhoud = res;
        }

        public void UpdateNaarJson()
        {
            using (StreamWriter file = File.CreateText(@"C:\Users\jonas\Documents\Cinema-ProjB\planner\Scheduler2\Schedulerr\Schedulerr\Planning.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                
                serializer.Serialize(file, this.inhoud);
                
            }
        }
        public void LeesJson()
        {
            using (StreamReader file = File.OpenText(new Finder().SearchFile("Planning.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                this.inhoud = (Programma[])serializer.Deserialize(file, typeof(Programma[]));
            }
        }
        
    }
}
