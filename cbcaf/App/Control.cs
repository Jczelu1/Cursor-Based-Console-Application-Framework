using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using cbcaf.Page;

namespace cbcaf.App
{
    public static class KeyReader
    {
        public static ConsoleKeyInfo GetKey()
        {
            ConsoleKeyInfo keyInfo;
            keyInfo = Console.ReadKey(true);
            return keyInfo;
        }
    }
    public class Control
    {
        public ConsoleKey ConsoleKey;

        public Procedure OnPress;

        public Control(ConsoleKey consolekey, Procedure onPress)
        {
            ConsoleKey = consolekey;
            OnPress = onPress;
        }
    }
}
