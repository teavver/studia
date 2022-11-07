using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            gif_CLI();
        }

        private static void gif_CLI(){
            select_gif_file(true);
        }

        private static void select_gif_file(bool clear) {
            Console.CursorVisible = true;
            if(clear) Console.Clear();
            // Get raw user input
            log(0, "Paste your GIF file in this directory and input its name\nExample: \"drake.gif\"\nType \"help\" for available commands\nFile name:");
            string? user_input = Console.ReadLine();
            
            // Ignore 'clear' command 
            if(user_input == "clear" || user_input == "cls"){
                Console.Clear();
                select_gif_file(false);
                return;
            }

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
                eval_gif(user_input); // user_input === selected_gif here
            }
            if(user_input.Any(x => Char.IsWhiteSpace(x))){
                int first_space_user_input = user_input.IndexOf(" "); // i.e "drake.gif -l"
                string?  opt_args = user_input.Substring(first_space_user_input + 1); // i.e "-l"
                int args = check_for_opt_args(opt_args);
                string? selected_gif = user_input.Substring(0, first_space_user_input); // i.e "drake.gif"
                eval_gif(selected_gif, args);
            }
        }

        public static void eval_gif(string selected_gif, int args = 0){
            if(!File.Exists(selected_gif)){
                Console.Clear(); log(2, $"[ ERR ] Can't find file \"{selected_gif}\". try again");
                select_gif_file(false);
                return;
                }
            if(File.Exists(selected_gif)){
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
                int framerate = (frame_delay.Value [0] + frame_delay.Value[1] * 256) * 10;
                
                log(0, $"gif frame delay: {framerate.ToString()}");
                log(0, $"size in bytes: {size_in_bytes.ToString()}");
                log(0, $"image size: {image_width.ToString()} x {image_height.ToString()} px");
                log(0, "[ OK ]");
                Thread.Sleep(2500);
                Console.Clear();

                // Check for opt args before rendering
                if(args == 2){ render_frames(gif_image, frame_count, args); } // render in "high quality"
                if(args == 1){ render_frames(gif_image, frame_count, args); } // render looped
                else { render_frames(gif_image, frame_count, 0); } //render normal
            }
        }

        public static void list_gif_files(){

            Console.Clear();
            var curr_dir = Directory.GetCurrentDirectory();
            var gif_files = Directory.GetFiles(curr_dir, "*.gif");
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
                    if(arg != "-l"){ log(2, "[ ERR ] Invalid opt args"); return 0; }
                    if (arg == "-l"){ log(3, "[ OPT ARG ] LOOP OUTPUT GIF"); return 1; }
                }
            }
            return 0;
        }
        // public static void render_frames(Image gif_img, int frame_count, int args, int scale){
        public static void render_frames(Image gif_img, int frame_count, int args){
            // Loop case
            if(args == 1){
                Console.CursorVisible = false;
                for (int i = 0; i < frame_count; i++)
                {
                    if( i == frame_count - 1){
                        i = 0;
                        Console.CursorLeft = 0;
                        Console.CursorTop = 0;
                    }
                    gif_img.SelectActiveFrame(FrameDimension.Time, i);
                    // render_current_frame(gif_img, scale);
                    render_current_frame(gif_img);
                }
            }
            // Normal
            if(args == 0){
                Console.CursorVisible = false;
                for (int i = 0; i < frame_count; i++)
                    {
                        gif_img.SelectActiveFrame(FrameDimension.Time, i);
                        // render_current_frame(gif_img, scale);
                        render_current_frame(gif_img);
                        // Thread.Sleep(1000 / scale);
                    }
                // Cleanup
                Console.CursorVisible = true;
                Console.Clear();
                log(0, "[ OK ] Done");
                select_gif_file(false);
                return;
            }
        }
        // public static void render_current_frame(Image gif_img, int scale){
        public static void render_current_frame(Image gif_img){
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Bitmap frame_bmp = new Bitmap(gif_img);
            // ConsoleWriteImage(frame_bmp, scale);
            ConsoleWriteImage(frame_bmp);
        }

        // System.Console color palette
        static int[] cColors = { 0x000000, 0x000080, 0x008000, 0x008080, 0x800000, 0x800080, 0x808000, 0xC0C0C0, 0x808080, 0x0000FF, 0x00FF00, 0x00FFFF, 0xFF0000, 0xFF00FF, 0xFFFF00, 0xFFFFFF };

        // Drawing functions
        // Credit: https://stackoverflow.com/questions/33538527/display-a-image-in-a-console-application
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
        public static void ConsoleWriteImage(Bitmap source, int scale = 12) // 32 as default, min 1 max 50 (huge screen tearing above 50)
        {
            int sMax = scale;
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