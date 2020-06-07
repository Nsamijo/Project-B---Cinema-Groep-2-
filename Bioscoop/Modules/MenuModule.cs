using System;
using System.Threading;
using Bioscoop.Helpers;
using Bioscoop.Models;
using Bioscoop.Modules;
using Bioscoop.Repository;

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
            Console.Clear(); Console.CursorVisible = false;
            Helpers.Display.PrintLine("Bioscoop - Hoofdmenu                                                INS - Login \n");
            Helpers.Display.PrintLine("Welkom op het klanten portaal van de Bioscoop \n");
            Helpers.Display.PrintLine("Navigeer naar het filmoverzicht om alle film informatie in te zien en een film te reserveren");
            Helpers.Display.PrintLine("Als je een reservatie hebt aangemaakt kan je deze inzien bij beheren reservatie \n");
            Helpers.Display.PrintHeader("Nr.", "Menu");
            Helpers.Display.PrintTable("1", "Filmoverzicht");
            Helpers.Display.PrintTable("2", "Beheren reservatie");
        }
        public void MenuAdmin()
        {
            Console.Clear(); Console.CursorVisible = true;
            LoginModule admin = new LoginModule();
            while (admin.NuIngelogd() == null)
            {
                admin.Login(new Lezer().gebruikersInlezen());
            }

            if (admin.NuIngelogd().Naam.Equals("cancel"))
                return;

            if (admin.NuIngelogd().Rechten == false)
            {
                this.MenuMedewerker(admin.NuIngelogd());
                return;
            }

            bool loop = true;
            while (loop)
            {
                Console.Clear(); Console.CursorVisible = false;
                Helpers.Display.PrintLine("Bioscoop - Admin Portaal                            Welkom: " + admin.NuIngelogd().Naam);
                Helpers.Display.PrintLine("ESC - Uitloggen                            INS - Medewerker Portaal");
                Helpers.Display.PrintLine("");
                Helpers.Display.PrintHeader("Nr.", "Menu");
                Helpers.Display.PrintTable("1", "Filmbeheer");
                Helpers.Display.PrintTable("2", "Filmschemabeheer");
                Helpers.Display.PrintTable("3", "Zaalbeheer");
                Helpers.Display.PrintTable("4", "Gebruikerbeheer");

                ZaalModule zaalbeheer = new ZaalModule();
                FilmModule filmbeheer = new FilmModule();
                GebruikerModule gebruikerbeheer = new GebruikerModule();
                FilmschemaModule filmschemabeheer = new FilmschemaModule();
                StoelModule stoelbeheer = new StoelModule();

                switch (Helpers.Display.Keypress())
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        filmbeheer.Run();
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        filmschemabeheer.Run();
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        zaalbeheer.Run();
                        break;
                    case ConsoleKey.D4:
                        Console.Clear();
                        gebruikerbeheer.Run(admin);
                        break;
                    case ConsoleKey.Escape:
                        Display.PrintLine("\nBent u zeker dat u wilt uitloggen? (y/n)");
                        if (Helpers.Display.Keypress() == ConsoleKey.Y)
                            loop = false;
                        break;
                    case ConsoleKey.Insert:
                        MenuMedewerker(admin.NuIngelogd());
                        break;
                }
            }
        }
        private void MenuMedewerker(GebruikerModel medewerker)
        {
            bool loop = true;
            while (loop)
            {
                Console.Clear();
                Helpers.Display.PrintLine("Bioscoop - Medewerkers Portaal                       Welkom: " + medewerker.Naam);
                if (medewerker.Rechten)
                    Helpers.Display.PrintLine("ESC - Terug naar Admin Portaal");
                else
                    Helpers.Display.PrintLine("ESC - Uitloggen");
                Helpers.Display.PrintLine("");
                Helpers.Display.PrintHeader("Nr.", "Menu");
                Helpers.Display.PrintTable("1", "Reservatie beheer");
                Helpers.Display.PrintTable("2", "Rapportage");

                ReserveringModule reservering = new ReserveringModule();
                ManagementModule management = new ManagementModule();

                switch (Helpers.Display.Keypress())
                {
                    case ConsoleKey.D1:
                        management.ReservatieManagament();
                        break;
                    case ConsoleKey.D2:
                        management.RapportageManagement();
                        break;
                    case ConsoleKey.Escape:
                        if (!medewerker.Rechten)
                            Display.PrintLine("\n Weet u zeker dat u wilt uitloggen? (y/n)");
                        if (Helpers.Display.Keypress() == ConsoleKey.Y)
                            loop = false;
                        else
                            loop = false;
                        break;
                }
            }
        }
    }
}
