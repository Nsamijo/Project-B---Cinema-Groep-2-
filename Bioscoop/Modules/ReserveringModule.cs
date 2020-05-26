using System;
using System.Collections.Generic;
using System.Text;
using Bioscoop.Helpers;
using Bioscoop.Models;
using Bioscoop.Repository;
namespace Bioscoop.Modules
{
    class ReserveringModule
    {
        public static void Run()
        {
            List<ReserveringModel> reserveringen = Reserveringdata.LoadData();
            bool abort = false;
            while (!abort) {
                Helpers.Display.PrintLine("Type de code van uw reservering");
                Inputs.KeyInput input = Inputs.ReadUserData();
                switch (input.action)
                {
                    case Inputs.KeyAction.Enter:
                        string c = input.val;
                        if(c.Length != 5)
                        {
                            break;
                        }
                        else
                        {
                            ReserveringModel r = Reserveringdata.VindReservering(c);
                            PrintReservering(r);
                            break;
                        }
                    case Inputs.KeyAction.Escape:
                        abort = true;
                        break;

                }
            }
        }
        public static void PrintReservering(ReserveringModel reservering) { 
            
        }
    }
}
