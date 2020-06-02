using System;

namespace Bioscoop.Helpers
{
    class Inputs //user input vervanger met keyinputs. Deze functie niet aanraken of aanpassen!
    {
        public enum KeyAction
        {
            Enter,
            Escape,
            Insert,
            Delete,
            F1,
            F2
        }

        public class KeyInput //verwacht een waarde en een key
        {
            public KeyAction action;
            public string val;
        }

        public static KeyInput ReadUserData()
        {
            KeyInput input = new KeyInput();
            ConsoleKeyInfo val;
            do
            {
                val = Console.ReadKey();
                switch (val.Key)
                {
                    case ConsoleKey.Enter:
                        input.action = KeyAction.Enter;
                        return input;
                    case ConsoleKey.Insert:
                        input.action = KeyAction.Insert;
                        return input;
                    case ConsoleKey.F1:
                        input.action = KeyAction.F1;
                        return input;
                    case ConsoleKey.F2:
                        input.action = KeyAction.F2;
                        return input;
                    case ConsoleKey.Delete:
                        input.action = KeyAction.Delete;
                        return input;
                    case ConsoleKey.Backspace:
                        if(!string.IsNullOrEmpty(input.val)) //als er op backspace is gedrukt de val - 1 character omdat de functie looped door elke waarde die word ingedrukt.
                        {
                            Console.Write(" \b");
                            input.val = input.val.Remove(input.val.Length - 1);
                        }
                        break;
                    default:
                        input.val = input.val + val.KeyChar;
                        break;
                }
            } while (val.Key != ConsoleKey.Escape);

            input.action = KeyAction.Escape;
            Console.WriteLine("a");
            return input;
        }
    }
}
