// Load file name from user
// Check if file exists in current directory (folder and menu???)
// Check if format / size is correct (below or equal to  1920x1080 ?)
// Take optional arguments [ -l for loop, etc. ]
// Proceed if all good, display gif in CLI

// NOTES
//  You can even get the frame durations with Image.GetPropertyItem(0x5100). 
//

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
            Log(0, "Paste your GIF file in this directory and input it's name.\nFile name: ");
            string? selected_gif = Console.ReadLine();
            if(selected_gif != null){
                if(File.Exists(selected_gif)){
                    Log(0, $"[ OK ] Selected file: {selected_gif}");
                    // Get gif information (size in byes, width, height, frames?)
                    
                    // get file info
                    FileInfo file = new FileInfo(selected_gif);
                    Image gif_image = Image.FromFile(selected_gif);
                    Bitmap img = new Bitmap(selected_gif);
                    
                    var sizeInBytes = file.Length;
                    var imageHeight = img.Height;
                    var imageWidth = img.Width;
                    int frameCount = gif_image.GetFrameCount(FrameDimension.Time);
                    // Log for testing
                    Log(0, $"size in bytes: {sizeInBytes.ToString()}");
                    Log(0, $"img height: {imageHeight.ToString()}");
                    Log(0, $"img width: {imageWidth.ToString()}");
                    Log(0, "[ OK ]");

                    // Save single file (for now)
                    gif_image.Save("frame.png", ImageFormat.Png);
                    Log(0, "[ OK ] Saved frames");
                }
                else {
                    Log(1, "[ WARN ] File not found, try again");
                    select_gif_file();
                }
            }
            else Log(2, "[ ERR ] Filename input is null."); return;
        }
        public static void generate_frames(int frames){
            // save all frames as png
        }
        public static void Log(int log_type, string input) {
            // 0 -- default info log (cyan)
            // 1 -- warn log        (yellow)
            // 2 -- err log           (red)
            if (log_type != 0 && log_type != 1 && log_type != 2){
                Console.WriteLine("LOG ERR - INVALID LOG() CALL");
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