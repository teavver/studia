using System;

namespace Toolbox
{
    public class Tools
    {
        public static void log(int log_type, string input, bool center = false, bool bgColor = false) {
            // 0 -- default info log (cyan)
            // 1 -- warn log        (yellow)
            // 2 -- err log           (red)
            // 3 -- help log         (green)
            if (log_type != 0 && log_type != 1 && log_type != 2 && log_type != 3 && log_type != 4){
                Console.WriteLine("log ERR - INVALID log() CALL");
                return;
            }
            if(log_type == 0) Console.ForegroundColor = ConsoleColor.Cyan;
            if(log_type == 1) Console.ForegroundColor = ConsoleColor.Yellow;
            if(log_type == 2) Console.ForegroundColor = ConsoleColor.Red;
            if(log_type == 3) Console.ForegroundColor = ConsoleColor.Green;
            if(log_type == 4) Console.ForegroundColor = ConsoleColor.White;
            if(center == true) {
                if(bgColor == true){
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (input.Length / 2)) + "}", input));
                    Console.ResetColor();
                    Console.CursorLeft = 0;
                    Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) - (input.Length / 2)) + "}", ""));
                    Console.WriteLine();
                    return;
                }
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (input.Length / 2)) + "}", input));
                // Console.ResetColor();
                } 
            if(!center){ Console.WriteLine(input); }
            Console.ResetColor();
            return;
        }

        public static void ctrl_c_watcher(){
            Console.CancelKeyPress += delegate {
            Console.Clear();
            Console.CursorVisible = true;
            };
        }
    }
}