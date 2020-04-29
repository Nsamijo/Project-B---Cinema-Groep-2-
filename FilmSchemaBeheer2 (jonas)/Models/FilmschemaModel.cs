using System;
using System.Collections.Generic;
using System.Text;

namespace Bioscoop
{
    class FilmschemaModel 
    {

        // 1 model!!! en houd de model verder leeg van functies! Zie ZaalModel.cs
        //alleen getters setters en de constructor horen in een model. de functies die in de modellen van jou zaten, in Repository/FilmData zetten.

        // de functies die bij jou nu in Helpers staan ook in Filmschemadata.cs stoppen. 

        // de finder.cs in helpers kan weg want: public static string jsonPath => Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Filmbeheer.json";
        // doet precies hetzelfde.

        public int ProgrammaId { get; set; }
        public string Datum { get; set; }
        public string Tijd { get; set; }
        public string FilmId { get; set; } //filmnaam niet, dat is dubbel data opslaan, big nono. want je zoekt gewoon de naam op bij het object met die id:  json.Where(a => a.Naam == data.Naam).ToList();
        public int ZaalId { get; set; }
    }
}
