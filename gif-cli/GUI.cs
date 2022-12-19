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

        // Global list for all the files available in the directory
        public static List<string> gif_list = new List<string>();

        // List for a single page of files
        public static List<string> gif_list_current_page = new List<string>();

        // Set a global directory for the files based on OS
        public static string global_dir = "";

        // Navigation menu stufffff
        public static int num_of_pages = 0; // Is set (to (all files/files_per_page)) once files are checked
        public static int page_number = 1;
        public static int files_per_page = 4;
        public static int num_of_gif_files_in_dir = 0; // Number of gif files in (global) directory
        public static int sel_gif_index = 0; // Index of currently selected gif file

        // GUI Main
        public static void Main(bool restart = false) {

            // Clean up after my spastic ctrl C's and set starting list to page 1
            if(!restart){ Tb.ctrl_c_watcher(); }

            // Check for gif files in directory
            if (!restart) { check_for_gif_files(); update_local_gif_list(page_number); }

            // Write the menu out
            display_ascii_title();
            Console.CursorVisible = true;
            display_menu(gif_list_current_page[sel_gif_index]);

            // Check input
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

                // Escape (close the app)
                if(keyinfo.Key == ConsoleKey.Escape){
                    Console.Clear(); Console.CursorVisible = true; Environment.Exit(0);
                }

                // Up -- Down (move between files)
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (sel_gif_index < (gif_list_current_page.Count() -1))
                    {
                        sel_gif_index++;
                        display_menu(gif_list_current_page[sel_gif_index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (sel_gif_index - 1 >= 0)
                    {
                        sel_gif_index--;
                        display_menu(gif_list_current_page[sel_gif_index]);
                    }
                }

                // Left -- Right (move between pages)
                if (keyinfo.Key == ConsoleKey.LeftArrow)
                {
                    if(page_number == 1){} // DO nothing
                    else {
                        page_number--;
                        update_local_gif_list(page_number);
                        display_menu(gif_list_current_page[sel_gif_index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.RightArrow)
                {
                    if(page_number == num_of_pages){} // Already max page dummy
                    else {
                        page_number++;
                        update_local_gif_list(page_number);
                        display_menu(gif_list_current_page[sel_gif_index]);
                    }
                }
                
                // Enter (Run currently selected gif)
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    Gif_Cli.eval_gif_file(gif_list_current_page[sel_gif_index]);
                }
            }
            while (keyinfo.Key != ConsoleKey.Escape);
            Console.ReadKey();
        }

        public static void display_gui_page_info()
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Gray;
            Tb.log(0, $"Page {page_number}/{num_of_pages}", true, true);
            Console.WriteLine();
        }

        public static void check_for_gif_files(){

            string linux_gifs_path = Directory.GetCurrentDirectory();
            string windows_gifs_path = Path.GetFullPath(Path.Combine(linux_gifs_path, @"..\..\.."));

            var curr_dir = (OS == "LINUX") ? linux_gifs_path : windows_gifs_path;
            // Assign the global directory
            global_dir = curr_dir;
            string[] local_gif_files  = Directory.GetFiles(curr_dir, "*.gif");

            foreach (var item in local_gif_files)
            {
                Console.WriteLine(item);
            }

            // Add files to Array
            foreach (var gif_file in local_gif_files)
            {
                // Log only "file.gif" instead of "/home/.../.../file.gif"
                string gif_file_short = (OS == "WINDOWS")
                ? gif_file.Split(@"\").Last() // Windows path formatting
                : gif_file.Split("/").Last(); // Unix
                num_of_gif_files_in_dir++;
                
                // Check if file exists in list and if not - add it
                if(!gif_list.Contains(gif_file_short)){
                   gif_list.Add(gif_file_short);
                   Console.WriteLine(gif_file_short);
                }
            }

            // Assign number of pages based on available files
            num_of_pages = (num_of_gif_files_in_dir / 3);
            Console.WriteLine();
        }

        static void display_gif_details(string selected_gif){

            Directory.SetCurrentDirectory(global_dir);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Tb.log(4, "----", true);
            Tb.log(4, $"{selected_gif}", true);
            
            // Get gif information -- size in byes, width, height, frame count, frame 
            FileInfo file = new FileInfo(selected_gif);
            Bitmap img_bmp = new Bitmap(selected_gif); // Throws err's
            Image gif_image = Image.FromFile(selected_gif);
            long size_in_bytes = file.Length;
            var image_height = img_bmp.Height;
            var image_width = img_bmp.Width;
            PropertyItem? frame_delay = gif_image.GetPropertyItem(0x5100); // Get exact frame delay
            int framerate = (frame_delay.Value[0] + frame_delay.Value[1] * 256) * 10;

            Tb.log(4, $"Image size: {image_width} x {image_height} px", true);
            Tb.log(4, $"Size (KiB): {(size_in_bytes / 1024)}", true);
            Tb.log(4, $"Frame delay: {framerate}", true);
            Tb.log(4, "----", true);
        }

        static void display_menu(string selectedOption)
        {
            // Cleanup + disable cursor while in Menu
            Console.Clear();
            Console.CursorVisible = false;

            Console.WriteLine();
            Tb.log(4, "[ GIF-CLI ] Select your gif file", true);
            Console.WriteLine();

            // List available files in directory
            foreach (string option in gif_list_current_page)
            {
                if(option == selectedOption)
                {
                    Tb.log(4, $"** {option} **", true);
                }
                else
                {
                    Tb.log(4, $"{option}", true);
                }
            }

            // Display details about selected file
            display_gif_details(selectedOption);

            // Display page info
            display_gui_page_info();
        }

        static void update_local_gif_list(int page){
            sel_gif_index = 0;
            var temp_list = gif_list.Skip((page - 1) * files_per_page).Take(files_per_page);
            gif_list_current_page = temp_list.ToList();
        }
        
        static void display_ascii_title()
        {
            Console.Title = "Gif Cli";
            Console.CursorVisible = false;
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