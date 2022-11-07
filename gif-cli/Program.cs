// Load file name from user
// Check if file exists in current directory (folder and menu???)
// Check if format / size is correct (below or equal to  1920x1080 ?)
// Take optional arguments [ -l for loop, etc. ]
// Proceed if all good, display gif in CLI

// NOTES
// todo:
// opt args listed in [help]
// escape to stop gif and program

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
            if(clear) Console.Clear();
            // display_help();
            log(0, "Paste your GIF file in this directory and input its name.\nType \"help\" for available commands\nFile name:");
            string? user_input = Console.ReadLine();
            var first_space_user_input = user_input.IndexOf(" "); // i.e "drake.gif -l"
            string selected_gif = user_input.Substring(0, first_space_user_input); // i.e "drake.gif"
            string?  opt_args = user_input.Substring(first_space_user_input + 1); // i.e "-l"
            // Console.WriteLine($"entire string (user input) : {user_input}");
            // Console.WriteLine($"before space (selected gif) : {selected_gif}");
            // Console.WriteLine($"opt args : {opt_args}");
            // Check if file exists in current dir
            if(selected_gif == "help"){
                display_help();
                select_gif_file(false);
            }
            if(selected_gif != null){
                log(3, selected_gif);
                if(File.Exists(selected_gif)){
                    log(0, $"[ OK ] Selected file: {selected_gif}");

                    // Check for optional arguments after the file
                    check_for_opt_args(opt_args);

                    // Get gif information (size in byes, width, height, frame count)
                    FileInfo file = new FileInfo(selected_gif);
                    Bitmap img_bmp = new Bitmap(selected_gif);
                    Image gif_image = Image.FromFile(selected_gif);
                    
                    var size_in_bytes = file.Length;
                    var image_height = img_bmp.Height;
                    var image_width = img_bmp.Width;
                    int frame_count = gif_image.GetFrameCount(FrameDimension.Time);
                    // Getting framerate
                    PropertyItem? frame_delay = gif_image.GetPropertyItem(0x5100);
                    int framerate = (frame_delay.Value [0] + frame_delay.Value[1] * 256) * 10;
                    // Log for testing
                    log(0, $"gif frame delay: {framerate.ToString()}");
                    log(0, $"size in bytes: {size_in_bytes.ToString()}");
                    log(0, $"image size: {image_width.ToString()} x {image_height.ToString()} px");
                    log(0, "[ OK ]");
                    // Thread.Sleep(750);
                    // Console.Clear();
                    // Testing
                    // render_frames(gif_image, frame_count, framerate);
                    // log(0, "[ OK ] Saved frames");
                }
                else {
                    log(1, "[ WARN ] File not found, try again");
                    select_gif_file(false);
                }
            }
            else log(2, "[ ERR ] Filename input is null."); return;
        }
        // public static void render_frames(Image gif_img, int frame_count, int frame_rate){
        //     System.IO.Directory.CreateDirectory("frames");
        //     log(0, $"[ OK ] Creating directory...");
        //     Console.CursorVisible = false;
        //         for (int i = 0; i < frame_count; i++)
        //         {
        //             gif_img.SelectActiveFrame(FrameDimension.Time, i);
        //             Console.CursorLeft = 0;
        //             Console.CursorTop = 0;
        //             Bitmap frame_bmp = new Bitmap(gif_img);
        //             // Draw current frame in cli 
        //             ConsoleWriteImage(frame_bmp);
        //             // .Save("test", ImageFormat.Png);
        //         }
        //     Console.Clear();
        //     log(0, "[ OK ] Done");
        // }
        public static void display_help(){
            log(3, "[ HELP ] Optional arguments available: ");
            log(3, "[ -ls ] Display all selectable .gif files in current directory");
            log(3, "[ file.gif -l ] Loop output gif");
            log(3, "[ file.gif -s ] Save output gif when finished");
        }

        public static void check_for_opt_args(string? opts){
            if(opts == null) log(0, "No opt args, moving on...");
            if(opts != null){
                string[] opts_arr = opts.Split(" ");
                foreach (var arg in opts_arr)
                {
                    if(arg != "-l" && arg != "-s"){ log(2, "[ ERR ] Invalid opt args"); }
                    if (arg == "-l"){ log(3, "[ OPT ARG ] LOOP OUTPUT GIF"); }
                    if (arg == "-s"){ log(3, "[ OPT ARG ] SAVE OUTPUT WHEN FINISHED"); }
                }
            }
        }

        public static void save_new_frame(){

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
            int sMax = 39;
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

        // Better readability
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