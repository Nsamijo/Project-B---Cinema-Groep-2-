using System;
using System.Collections.Generic;
using System.Text;

namespace Bioscoop.Helpers
{
    class Inputs
    {
        public enum KeyAction
        {
            Enter,
            Escape,
            Insert,
            Delete
        }

        public class KeyInput
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
                    case ConsoleKey.Delete:
                        input.action = KeyAction.Delete;
                        return input;
                    case ConsoleKey.Backspace:
                        if(!string.IsNullOrEmpty(input.val))
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
            Console.WriteLine("");
            return input;
        }
    }
}
