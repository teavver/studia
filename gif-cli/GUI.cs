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
        
        public static int range = 3;
        
        public static List<string> gif_list = new List<string>();
        public static void Main(bool restart = false) {

            // Clean up after my spastic ctrl C's
            if(!restart){ Tb.ctrl_c_watcher(); }

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
                if(keyinfo.Key == ConsoleKey.Escape){
                    Console.Clear(); Console.CursorVisible = true; Environment.Exit(0);
                }
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
                    Console.Clear();
                    Gif_Cli.eval_gif_file(gif_list[index]);
                }
            }
            while (keyinfo.Key != ConsoleKey.Escape);
            Console.ReadKey();
        }
        public static void check_for_gif_files(){
            string linux_gifs_path = Directory.GetCurrentDirectory();
            string windows_gifs_path = Path.GetFullPath(Path.Combine(linux_gifs_path, @"..\..\..\*.gif"));
            var curr_dir = (OS == "LINUX") ? linux_gifs_path : windows_gifs_path;
            var local_gif_files  = Directory.GetFiles(curr_dir, "*.gif");
            // Add files to Array
            foreach (var gif_file in local_gif_files)
            {

                int pos = gif_file.LastIndexOf("/") + 1;
                // Log only "file.gif" instead of "/home/.../.../file.gif"
                string gif_file_short = gif_file.Substring(pos, gif_file.Length - pos);

                // Check if file exists in list and if not, add it
                if(!gif_list.Contains(gif_file_short)){
                    gif_list.Add(gif_file_short);
                }
            }
            // gif_list.ForEach(Console.WriteLine);
            Console.WriteLine();
        }
        static void display_gif_details(string selected_gif){

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Tb.log(4, "----", true);
            Tb.log(4, $"{selected_gif}", true);
            // Get gif information -- size in byes, width, height, frame count, frame delay
            FileInfo file = new FileInfo(selected_gif);
            Bitmap img_bmp = new Bitmap(selected_gif);
            Image gif_image = Image.FromFile(selected_gif);

            long size_in_bytes = file.Length;
            var image_height = img_bmp.Height;
            var image_width = img_bmp.Width;
            int frame_count = gif_image.GetFrameCount(FrameDimension.Time);
            PropertyItem? frame_delay = gif_image.GetPropertyItem(0x5100);
            int framerate = (frame_delay.Value[0] + frame_delay.Value[1] * 256) * 10;

            Tb.log(4, $"Image size: {image_width.ToString()} x {image_height.ToString()} px", true);
            Tb.log(4, $"Size (KiB): {(size_in_bytes/1024)}", true);
            Tb.log(4, $"Frame delay: {framerate.ToString()}", true);
            Tb.log(4, "----", true);
        }

        static void WriteMenu(List<string> gif_list, string selectedOption)
        {
            // Cleanup + disable cursor while in Menu
            Console.Clear();
            Console.CursorVisible = false;
            Console.WriteLine();
            Console.WriteLine();

            Tb.log(4, "[ GIF-CLI ] Select your gif file", true);
            Console.WriteLine();

    
            foreach (string option in gif_list)
            {
            int index = gif_list.IndexOf(option);
                if(option != selectedOption && (option == gif_list[0] || option == gif_list[4])){
                    Tb.log(3, "...", true);
                }
                if (option == selectedOption)
                {
                    Tb.log(3, $"* {option} *", true);
                }
                else
                {
                    Tb.log(3, option, true);
                }
            }
            display_gif_details(selectedOption);
        }
    }
}