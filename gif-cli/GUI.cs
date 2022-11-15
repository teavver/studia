using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using Tb = Toolbox.Tools;

namespace GUI
{
    class Gui
    {
        // Get OS from the main file
        public static string OS = GIF_CLI.Program.OS;
        
        public static List<string> gif_list = new List<string>();
        public static void Main() {
            // Check for gif files in directory
            check_for_gif_files();

            // Set the default index of the selected item to be the first
            int index = 0;

            // Write the menu out
            WriteMenu(gif_list, gif_list[index]);

            // Store key info in here
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

                // Handle each key input (down arrow will write the menu again with a different selected item)
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < gif_list.Count)
                    {
                        index++;
                        WriteMenu(gif_list, gif_list[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(gif_list, gif_list[index]);
                    }
                }
                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine($"ENTER ONCLICK");
                    index = 0;
                }
            }
            while (keyinfo.Key != ConsoleKey.X);

            Console.ReadKey();

        }
        public static void check_for_gif_files(){
            string linux_gifs_path = Directory.GetCurrentDirectory();
            string windows_gifs_path = Path.GetFullPath(Path.Combine(linux_gifs_path, @"..\..\..\*.gif"));
            var curr_dir = (OS == "LINUX") ? linux_gifs_path : windows_gifs_path;
            var local_gif_files  = Directory.GetFiles(curr_dir, "*.gif");
            Console.WriteLine("TESTING");
            // Add files to Array
            foreach (var gif_file in local_gif_files)
            {
                int pos = gif_file.LastIndexOf("/") + 1;
                // Log only "file.gif" instead of "/home/.../.../file.gif"
                string gif_file_short = gif_file.Substring(pos, gif_file.Length - pos);
                // Add the file to <string>List
                gif_list.Add(gif_file_short);
                // Tb.log(3, $"[ .gif ] {gif_file_short}");
            }
            // gif_list.ForEach(Console.WriteLine);
            Console.WriteLine();
        }


        // Default action of all the gif_list. You can create more methods
        static void WriteTemporaryMessage(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            Thread.Sleep(3000);
            WriteMenu(gif_list, gif_list.First());
        }



        static void WriteMenu(List<string> gif_list, string selectedOption)
        {
            // Cleanup + disable cursor while in Menu
            Console.Clear();
            Console.CursorVisible = false;

            Console.WriteLine();
            Console.WriteLine();

            Tb.log(4, "Welcome to GIF-CLI, Select your gif file", true);
            Console.WriteLine();
            
            foreach (string option in gif_list)
            {
                if (option == selectedOption)
                {
                    Tb.log(3, $"> {option} <", true);
                }
                else
                {
                    Tb.log(3, option, true);
                }

                // Tb.log(3, option, true);
            }
        }
    }
}