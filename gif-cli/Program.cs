﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using GUI;

namespace GIF_CLI
{
    public class Program
    {
        public static string OS = "";
        public static void Main(string[] args)
        {
            GUI.Gui.Main();
            Thread.Sleep(2500);
            gif_CLI();
        }

        private static void gif_CLI(){
            // Console.Clear();
            // Check operating system before anything to ensure correct file paths
            check_OS();
            // select_gif_file(true);
        }

        private static void select_gif_file(bool clear) {

            // Cleanup after a spastic Ctrl-C
            Console.CursorVisible = true;
            if(clear) Console.Clear();

            // Get raw user input
            log(0, "Paste your GIF file in this directory and input its name\nExample: \"drake.gif\"\nType \"help\" for available commands\nFile name:");
            string? user_input = Console.ReadLine();
            
            // Check if user asked for help
            if(user_input == "help"){
                display_help();
                select_gif_file(false);
                return;
            }
            
            // Check if user wants to list .gif files
            if(user_input == "-ls"){
                list_gif_files();
                select_gif_file(false);
                return;
            }

            // Check if user input is file + args or just file.gif and continue
            if(!user_input.Any(x => Char.IsWhiteSpace(x))){
                eval_gif_OS(user_input,OS); // user_input === selected_gif here
            }
            if(user_input.Any(x => Char.IsWhiteSpace(x))){
                int first_space_user_input = user_input.IndexOf(" "); // i.e "drake.gif -l"
                string?  opt_args = user_input.Substring(first_space_user_input + 1); // i.e "-l"
                int args = check_for_opt_args(opt_args);
                string? selected_gif = user_input.Substring(0, first_space_user_input); // i.e "drake.gif"
                log(0, $"[ OK ] opt args: {opt_args}");
                log(0, $"[ OK ] opt args: {args}");
                eval_gif_OS(selected_gif,OS,args);
            }
        }

        public static void eval_gif_OS(string selected_gif, string OS, int args = 0){
            if(OS == "WINDOWS"){

                // Check if file exists in directory (Windows)
                string default_path = Directory.GetCurrentDirectory();
                string main_folder_path = Path.GetFullPath(Path.Combine(default_path, @"..\..\..\"));
                string selected_gif_file_win = Path.GetFullPath(Path.Combine(main_folder_path, $@"{selected_gif}"));
                // testing
                bool file_exists = File.Exists(selected_gif_file_win);
                eval_gif_file(file_exists, selected_gif_file_win, args);
            }
            if(OS == "LINUX"){

                // Check if file exists in directory (Linux)
                bool file_exists = File.Exists(selected_gif);
                eval_gif_file(file_exists, selected_gif, args);
            }
        }

        public static void eval_gif_file(bool exists, string selected_gif, int args = 0)
        {
            if (!exists)
            {
                Console.Clear(); log(2, $"[ ERR ] Can't find file \"{selected_gif}\". try again");
                select_gif_file(false);
                return;
            }
            if (exists)
            {
                log(0, $"[ OK ] Selected file: {selected_gif}");

                // Get gif information -- size in byes, width, height, frame count, frame delay
                FileInfo file = new FileInfo(selected_gif);
                Bitmap img_bmp = new Bitmap(selected_gif);
                Image gif_image = Image.FromFile(selected_gif);

                var size_in_bytes = file.Length;
                var image_height = img_bmp.Height;
                var image_width = img_bmp.Width;
                int frame_count = gif_image.GetFrameCount(FrameDimension.Time);
                PropertyItem? frame_delay = gif_image.GetPropertyItem(0x5100);
                int framerate = (frame_delay.Value[0] + frame_delay.Value[1] * 256) * 10;

                log(0, $"gif frame delay: {framerate.ToString()}");
                log(0, $"size in bytes: {size_in_bytes.ToString()}");
                log(0, $"image size: {image_width.ToString()} x {image_height.ToString()} px");
                log(0, "[ OK ]");
                Thread.Sleep(250);
                Console.Clear();

                // Check for opt args before rendering
                if (args == 1) { render_frames(gif_image, frame_count, args); } // render looped
                else { render_frames(gif_image, frame_count, 0); } //render normal
            }
        }
        public static void list_gif_files(){

            Console.Clear();
            string linux_gifs_path = Directory.GetCurrentDirectory();
            string windows_gifs_path = Path.GetFullPath(Path.Combine(linux_gifs_path, @"..\..\..\*.gif"));
            var curr_dir = (OS == "LINUX") ? linux_gifs_path : windows_gifs_path;
            var gif_files = Directory.GetFiles(curr_dir, "*.gif");

            Console.WriteLine("TESTING (126");
            Console.WriteLine(curr_dir);
            Console.WriteLine(windows_gifs_path);
            Console.WriteLine(linux_gifs_path);
            
            foreach (var gif_file in gif_files)
            {
                int pos = gif_file.LastIndexOf("/") + 1;
                // Log only "file.gif" instead of "/home/.../.../file.gif"
                log(3, $"[ .gif ] {gif_file.Substring(pos, gif_file.Length - pos)}");
            }
            Console.WriteLine();
        }
        public static void display_help(){

            // List all available commands
            Console.Clear();
            log(3, "[ HELP ] Options available: ");
            Console.WriteLine();
            log(3, "Commands available: ");
            log(3, "[ -ls ] Display all selectable .gif files in current directory");
            Console.WriteLine();
            log(3, "Optional arguments available: ");
            log(3, "[ file.gif -l ] Loop output gif");
            log(3, "[ file.gif -s {number} ] Make gif and define its scale/quality");
            Console.WriteLine();
        }

        public static int check_for_opt_args(string? opts){

            if(opts == null){
                log(0, "No opt args, moving on...");
                return 0;
            }
            if(opts != null){
                string[] opts_arr = opts.Split(" ");
                foreach (var arg in opts_arr)
                {
                    if(arg != "-l" && arg != "-s"){ log(2, "[ ERR ] Invalid opt args"); return 0; }
                    if (arg == "-l"){ log(3, "[ OPT ARG ] LOOP OUTPUT GIF"); return 1; }
                    if (arg == "-s"){ log(3, "[ OPT ARG ] GIF WITH SCALE/QUALITY"); return 2; }
                }
            }
            return 0;
        }
        public static void render_frames(Image gif_img, int frame_count, int args){

            Console.CursorVisible = false;

            // Loop case (High CPU usage linux)
            if(args == 1){
                for (int i = 0; i < frame_count; i++)
                {
                    if( i == frame_count - 1){ i = 0; Console.Clear(); } // Restart on last frame
                    gif_img.SelectActiveFrame(FrameDimension.Time, i);
                    Console.CursorLeft = 0;
                    Console.CursorTop = 0;
                    Bitmap frame_bmp = new Bitmap(gif_img);
                    ConsoleWriteImage(frame_bmp);
                }
            }

            // Normal case
            for (int i = 0; i < frame_count; i++)
            {
                gif_img.SelectActiveFrame(FrameDimension.Time, i);
                Console.CursorLeft = 0;
                Console.CursorTop = 0;
                Bitmap frame_bmp = new Bitmap(gif_img);
                ConsoleWriteImage(frame_bmp);
            }

            // Cleanup
            Console.Clear();
            Console.CursorVisible = true;
            log(0, "[ OK ] Done");
        }

        // Drawing functions
        // Credit: https://stackoverflow.com/questions/33538527/display-a-image-in-a-console-application
        static int[] cColors = { 0x000000, 0x000080, 0x008000, 0x008080, 0x800000, 0x800080, 0x808000, 0xC0C0C0, 0x808080, 0x0000FF, 0x00FF00, 0x00FFFF, 0xFF0000, 0xFF00FF, 0xFFFF00, 0xFFFFFF };

        public static void ConsoleWritePixel(Color cValue)
        {
            Color[] cTable = cColors.Select(x => Color.FromArgb(x)).ToArray();
            char[] rList = new char[] { (char)9617, (char)9618, (char)9619, (char)9608 }; // 1/4, 2/4, 3/4, 4/4
            int[] bestHit = new int[] { 0, 0, 4, int.MaxValue }; //ForeColor, BackColor, Symbol, Score

            for (int rChar = rList.Length; rChar > 0; rChar--)
            {
                for (int cFore = 0; cFore < cTable.Length; cFore++)
                {
                    for (int cBack = 0; cBack < cTable.Length; cBack++)
                    {
                        int R = (cTable[cFore].R * rChar + cTable[cBack].R * (rList.Length - rChar)) / rList.Length;
                        int G = (cTable[cFore].G * rChar + cTable[cBack].G * (rList.Length - rChar)) / rList.Length;
                        int B = (cTable[cFore].B * rChar + cTable[cBack].B * (rList.Length - rChar)) / rList.Length;
                        int iScore = (cValue.R - R) * (cValue.R - R) + (cValue.G - G) * (cValue.G - G) + (cValue.B - B) * (cValue.B - B);
                        if (!(rChar > 1 && rChar < 4 && iScore > 50000)) // rule out too weird combinations
                        {
                            if (iScore < bestHit[3])
                            {
                                bestHit[3] = iScore; //Score
                                bestHit[0] = cFore;  //ForeColor
                                bestHit[1] = cBack;  //BackColor
                                bestHit[2] = rChar;  //Symbol
                            }
                        }
                    }
                }
            }
            Console.ForegroundColor = (ConsoleColor)bestHit[0];
            Console.BackgroundColor = (ConsoleColor)bestHit[1];
            Console.Write(rList[bestHit[2] - 1]);
        }
        public static void ConsoleWriteImage(Bitmap source)
        {
            int sMax = 30; // (SCALE, PIXEL RATIO IN CLI)
            decimal percent = Math.Min(decimal.Divide(sMax, source.Width), decimal.Divide(sMax, source.Height));
            Size dSize = new Size((int)(source.Width * percent), (int)(source.Height * percent));   
            Bitmap bmpMax = new Bitmap(source, dSize.Width * 2, dSize.Height);
            for (int i = 0; i < dSize.Height; i++)
            {
                for (int j = 0; j < dSize.Width; j++)
                {
                    ConsoleWritePixel(bmpMax.GetPixel(j * 2, i));
                    ConsoleWritePixel(bmpMax.GetPixel(j * 2 + 1, i));
                }
                System.Console.WriteLine();
            }
            Console.ResetColor();
        }

        public static void check_OS()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                OS = "WINDOWS";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                OS = "LINUX";
            }
        }

        public static void log(int log_type, string input, bool center = false) {
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
            if(center == true) { Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (input.Length / 2)) + "}", input)); } 
            if(!center){ Console.WriteLine(input); }
            Console.ResetColor();
            return;
        }
    }
}