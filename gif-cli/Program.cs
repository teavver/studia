// Load file name from user
// Check if file exists in current directory (folder and menu???)
// Check if format / size is correct (below or equal to  1920x1080 ?)
// Take optional arguments [ -l for loop, etc. ]
// Proceed if all good, display gif in CLI

// NOTES
// You can even get the frame durations with Image.GetPropertyItem(0x5100). 
// Optional args
// -- loop (loops CLI output forever)
// -- ascii
// -- blocks (default?)
// -- color

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
            select_gif_file();
        }

        private static void select_gif_file() {
            Console.Clear();
            log(0, "Paste your GIF file in this directory and input it's name.\nFile name: ");
            string? selected_gif = Console.ReadLine();

            // get quality from user as well ( ascii bitmap size later )
            // int quality = rl();
            if(selected_gif != null){
                if(File.Exists(selected_gif)){
                    log(0, $"[ OK ] Selected file: {selected_gif}");
                    // Get gif information (size in byes, width, height, frame count)
                    
                    FileInfo file = new FileInfo(selected_gif);
                    Image gif_image = Image.FromFile(selected_gif);
                    Bitmap img_bmp = new Bitmap(selected_gif);
                    
                    var size_in_bytes = file.Length;
                    var image_height = img_bmp.Height;
                    var image_width = img_bmp.Width;
                    int frame_count = gif_image.GetFrameCount(FrameDimension.Time);
                    // Log for testing
                    log(0, $"size in bytes: {size_in_bytes.ToString()}");
                    log(0, $"img height: {image_height.ToString()}");
                    log(0, $"img width: {image_width.ToString()}");
                    log(0, "[ OK ]");
                    // Split gif into frames
                    // generate_frames(gif_image, frame_count);
                    // log(0, "[ OK ] Saved frames");

                    // Convert frame to ASCII
                    frame_to_ASCII(gif_image, 64);
                }
                else {
                    log(1, "[ WARN ] File not found, try again");
                    select_gif_file();
                    Environment.Exit(0);
                }
            }
            else log(2, "[ ERR ] Filename input is null."); return;
        }
        public static void generate_frames(Image gif_img, int frame_count){
            System.IO.Directory.CreateDirectory("frames");
            log(0, $"[ OK ] Creating directory...");
            for (int i = 0; i < frame_count; i++)
            {
                gif_img.SelectActiveFrame(FrameDimension.Time, i);
                gif_img.Save($"frames/frame{i.ToString()}.png", ImageFormat.Png);
            }
        }

        public static void frame_to_ASCII(Image img, int quality){
            // https://codingvision.net/c-ascii-art-tutorial

            // https://stackoverflow.com/questions/33538527/display-a-image-in-a-console-application

            Bitmap bmp = new Bitmap(img, quality, quality);
        }
        public static void log(int log_type, string input) {
            // 0 -- default info log (cyan)
            // 1 -- warn log        (yellow)
            // 2 -- err log           (red)
            if (log_type != 0 && log_type != 1 && log_type != 2){
                Console.WriteLine("log ERR - INVALID log() CALL");
                return;
            }
            if(log_type == 0) Console.ForegroundColor = ConsoleColor.Cyan;
            if(log_type == 1) Console.ForegroundColor = ConsoleColor.Yellow;
            if(log_type == 2) Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(input);
            Console.ResetColor();
            return;
        }
    }

}