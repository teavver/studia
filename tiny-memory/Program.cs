
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace TinyMemory
{
    public class Program
    {
        public static string OS = "";
        public struct Score {
            public int score;
            public int highscore;
        }
        public static void Main(string[] args)
        {
            menu();
        }
        

        public static void menu(){
            Console.Clear();
            check_os();

            Score score;
            score.score = 0;
            score.highscore = 0; // default highscore
            log(0, "[ OK ] starting");
            log(3, $"score: {score.score}");
            log(3, $"highscore: {score.highscore}");
            log(3, $"operating system: {OS}");
            log(0, "[ OK ]");

            int local_highscore = read_highscore(OS);
            // Console.WriteLine(read_highscore(OS));
            score.highscore = local_highscore + 100;
            log(0, "[ OK ] Saving new highscore");
            save_highscore(OS, score.highscore);
            Console.WriteLine(read_highscore(OS));
            log(0, "[ OK ] Done");
        }
        
        public static void check_os(){
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                OS = "WIN";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                OS = "LINUX";
            }
            return;
        }

        public static void save_highscore(string OS, int highscore){
            string highscore_str = highscore.ToString();
            // string root_dir = Directory.GetCurrentDirectory();
            if(OS == "LINUX"){
                File.WriteAllText("highscore.txt", highscore_str);
            }
            if(OS == "WIN"){}
        }

        public static int read_highscore(string OS){
            if(OS == "LINUX"){
                if(!File.Exists("highscore.txt")){
                    log(2, "[ ERR ] No highscore");
                    return 0;
                }
                if(File.Exists("highscore.txt")){
                Console.WriteLine("Reading highscore: ");
                string current_dir = Directory.GetCurrentDirectory();
                string highscore_file_str = File.ReadAllText("highscore.txt");
                int highscore_value = Int32.Parse(highscore_file_str);
                return highscore_value;
                }
            }
            if(OS == "WIN"){
                // Find the file on windows,
                // retunr int val
            }
            return 1337;
        }
        public static void log(int log_type, string input) {
            // 0 -- default info log (cyan)
            // 1 -- warn log        (yellow)
            // 2 -- err log           (red)
            // 3 -- help log         (green)
            if (log_type != 0 && log_type != 1 && log_type != 2 && log_type != 3){
                Console.WriteLine("log ERR - INVALID log() CALL");
                return;
            }
            if(log_type == 0) Console.ForegroundColor = ConsoleColor.Cyan;
            if(log_type == 1) Console.ForegroundColor = ConsoleColor.Yellow;
            if(log_type == 2) Console.ForegroundColor = ConsoleColor.Red;
            if(log_type == 3) Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(input);
            Console.ResetColor();
            return;
        }
    }
}