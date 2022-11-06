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
            // Ask user to select a file
            select_gif_file();
        }

        private static void select_gif_file() {
            Log(0, "Paste your GIF file in this directory and input it's name.\nFile name: ");
            string? selected_gif = Console.ReadLine();
            if(selected_gif != null){
                if(File.Exists(selected_gif)){
                    Log(0, $"[ OK ] Selected file: {selected_gif}");
                    // Get gif information (size in byes, width, height, frames?)
                    FileInfo file = new FileInfo(selected_gif);
                    Bitmap img = new Bitmap(selected_gif);
                    var sizeInBytes = file.Length;
                    var imageHeight = img.Height;
                    var imageWidth = img.Width;

                    Image gif_image = Image.FromFile(selected_gif);
                    FrameDimension dimension = new FrameDimension(gif_image.FrameDimensionsList[0]);
                    // Number of frames
                    int frameCount = gif_image.GetFrameCount(dimension);
                    Log(0, $"frame_count: {frameCount}");
                    
                    // Return an Image at a certain index
                    // gifImg.SelectActiveFrame(dimension, 0);


                    // Useful log
                    Log(0, $"size in bytes: {sizeInBytes.ToString()}");
                    Log(0, $"img height: {imageHeight.ToString()}");
                    Log(0, $"img width: {imageWidth.ToString()}");
                    // Convert gif frame to byte array
                    byte[] img_byte_array = img_to_byte_arr(img);
                    byte_arr_to_file(img_byte_array, "gif_split");
                    Log(0, "[ OK ]");
                }
                else {
                    // Restart if filename doesn't exist or user misspelled
                    Log(1, "[ WARN ] File not found, try again");
                    select_gif_file();
                }
            }
            else Log(2, "[ ERR ] Filename input is null."); return;
        }

        public static byte[] img_to_byte_arr(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        
        public static void byte_arr_to_file(byte[] data, string filePath)
        {
            using var stream = File.Create(filePath);
            // using var stream = File.Create(Directory.GetCurrentDirectory());
            stream.Write(data, 0, data.Length);
        }

        public static void split_gif_into_frames(){

        }

        // public static IEnumerable<Bitmap> get_gif_frames(Image gif){
        //     var dimension = new FrameDimension(gif.FrameDimensionsList[0]);
        //     var frameCount = gif.GetFrameCount(dimension);
        //     Log(0, $" dimensions: {dimension.ToString()}");
        //     Log(0, $" frame_count: {frameCount.ToString()}");
        //     for (var index = 0; index < frameCount; index++)
        //     {
        //         //find the frame
        //         gif.SelectActiveFrame(dimension, index);
        //         //return a copy of it
        //         yield return (Bitmap) gif.Clone();
        //     }
        // }
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