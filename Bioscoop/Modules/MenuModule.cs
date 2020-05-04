﻿using System;

namespace Bioscoop.Modules
{
    class MenuModule
    {

        public ConsoleKey Run()
        {
            MenuMain();
            return Helpers.Display.Keypress();
        }

        private void MenuMain()
        {
            //main menu
            Console.Clear();
            Helpers.Display.PrintLine("Hoofdmenu");
            Helpers.Display.PrintLine("Druk op een nummer om verder te gaan naar het debetreffende scherm");
            Helpers.Display.PrintLine("");
            Helpers.Display.PrintHeader("Nr.", "Menu");
            Helpers.Display.PrintTable("1", "Zaalbeheer");
            Helpers.Display.PrintTable("2", "Filmbeheer");
            Helpers.Display.PrintTable("3", "Gebruikersbeheer");
            Helpers.Display.PrintTable("4", "Filmschemabeheer");
            Helpers.Display.PrintTable("5", "Stoelenbeheer");
        }
    }
}
