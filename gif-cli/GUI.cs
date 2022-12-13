using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using Tb = Toolbox.Tools;
using Gif_Cli = GIF_CLI.Program;

namespace GUI
{
    class Gui
    {
        // Get OS from the main file
        public static string OS = GIF_CLI.Program.OS;

        public static List<string> gif_list = new List<string>();

        // Pages stufffff
        public static int sel_gif_index = 0;
        public static int current_page = 1;
        public static int num_of_gif_files_in_dir = 0;
        public static int num_of_pages = 0; // Is set once files r checked

        // GUI Main
        public static void Main(bool restart = false) {

            // Clean up after my spastic ctrl C's
            if(!restart){ Tb.ctrl_c_watcher(); }

            // Check for gif files in directory
            check_for_gif_files();

            // Write the menu out
            display_ascii_title();
            display_menu(gif_list, gif_list[sel_gif_index]);

            // Store key info in here
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();
                // Handle each key input (down arrow will write the menu again with a different selected item)
                if(keyinfo.Key == ConsoleKey.Escape){
                    Console.Clear(); Console.CursorVisible = true; Environment.Exit(0);
                }
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (sel_gif_index + 1 < gif_list.Count)
                    {
                        sel_gif_index++;
                        display_menu(gif_list, gif_list[sel_gif_index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (sel_gif_index - 1 >= 0)
                    {
                        sel_gif_index--;
                        display_menu(gif_list, gif_list[sel_gif_index]);
                    }
                }
                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    Gif_Cli.eval_gif_file(gif_list[sel_gif_index]);
                }
            }
            while (keyinfo.Key != ConsoleKey.Escape);
            Console.ReadKey();
        }

        public static void display_gui_page()
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Gray;
            Tb.log(0, $"Page {current_page}/{num_of_pages}", true);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();
        }

        public static void check_for_gif_files(){

            string linux_gifs_path = Directory.GetCurrentDirectory();
            string windows_gifs_path = Path.GetFullPath(Path.Combine(linux_gifs_path, @"..\..\.."));
            var curr_dir = (OS == "LINUX") ? linux_gifs_path : windows_gifs_path;
            var local_gif_files  = Directory.GetFiles(curr_dir, "*.gif");

            // Add files to Array
            foreach (var gif_file in local_gif_files)
            {
                // Log only "file.gif" instead of "/home/.../.../file.gif"
                Console.WriteLine(gif_file);
                string gif_file_short = (OS == "WINDOWS") ? gif_file.Split(@"\").Last() : "LINUX LOL"; // Will fix this later
                num_of_gif_files_in_dir++;
                
                // Debugging
                // Console.WriteLine(gif_file_short);

                // Check if file exists in list and if not, add it
                if(!gif_list.Contains(gif_file_short)){
                   gif_list.Add(gif_file_short);
                }
            }
            num_of_pages = (num_of_gif_files_in_dir / 3);
            Console.WriteLine(num_of_pages);
            Console.ReadKey();
            Console.WriteLine();
        }
        static void display_gif_details(string selected_gif){

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            
            Tb.log(4, "----", true);
            Tb.log(4, $"{selected_gif}", true);
            string selected_gif_no_file_ext = selected_gif.Substring(0, selected_gif.Length - 4);
            Console.WriteLine(selected_gif_no_file_ext);
            Console.WriteLine(File.Exists(selected_gif_no_file_ext));
            Console.ReadKey();
           

            // Get gif information -- size in byes, width, height, frame count, frame delay
            FileInfo file = new FileInfo(selected_gif);
            Bitmap img_bmp = new Bitmap(selected_gif);
            Image gif_image = Image.FromFile(selected_gif);

            long size_in_bytes = file.Length;
            var image_height = img_bmp.Height;
            var image_width = img_bmp.Width;
            PropertyItem? frame_delay = gif_image.GetPropertyItem(0x5100); // Get exact frame delay
            int framerate = (frame_delay.Value[0] + frame_delay.Value[1] * 256) * 10;

            Tb.log(4, $"Image size: {image_width} x {image_height} px", true);
            Tb.log(4, $"Size (KiB): {(size_in_bytes/1024)}", true);
            Tb.log(4, $"Frame delay: {framerate}", true);
            Tb.log(4, "----", true);
        }

        static void display_menu(List<string> gif_list, string selectedOption)
        {
            // Cleanup + disable cursor while in Menu
            Console.Clear();
            Console.CursorVisible = false;

            Console.WriteLine();
            Tb.log(4, "[ GIF-CLI ] Select your gif file", true);
            Console.WriteLine();

            // List available files in directory
            foreach (string option in gif_list)
            {
                int index = gif_list.IndexOf(option);
                Console.WriteLine(option);
            }
            
            // Display details about selected file
            display_gif_details(selectedOption);

            // Display page info
            display_gui_page();
        }
        
        static void display_ascii_title()
        {
            Console.Title = "Gif Cli";
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();

            Tb.log(0, "         oo .8888b                       dP oo ", true);
            Tb.log(0, "            88   \"                       88    ", true);
            Tb.log(0, ".d8888b. dP 88aaa               .d8888b. 88 dP ", true);
            Tb.log(0, "88'  `88 88 88                  88'  `\"\" 88 88 ", true);
            Tb.log(0, "88.  .88 88 88                  88.  ... 88 88 ", true);
            Tb.log(0, "`8888P88 dP dP                  `88888P' dP dP ", true);
            Tb.log(0, "     .88           oooooooooooo                ", true);
            Tb.log(0, " d8888P                                        ", true);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Tb.log(0, "Press any key to continue.", true);
            Console.ReadKey();
        }
    }
}