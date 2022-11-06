// Load file name from user
// Check if file exists in current directory (folder and menu???)
// Check if format / size is correct (below or equal to  1920x1080 ?)
// Take optional arguments [ -l for loop, etc. ]
// Proceed if all good, display gif in CLI

// NOTES
//  You can even get the frame durations with Image.GetPropertyItem(0x5100). 
//

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Gif_CLI();
        }

        private static void Gif_CLI(){
            // Ask user to select a file
            SelectFile();
        }

        private static void SelectFile() {
            Log(0, "Paste your GIF file in this directory and input it's name.\nFile name: ");
            string? selected_gif = Console.ReadLine();
            if(selected_gif != null){
                if(File.Exists(selected_gif)){
                    Log(0, "[ OK ]");
                }
                else {
                    Log(1, "File not found, try again");
                    SelectFile();
                }
            }
            else Console.WriteLine("Filename input is null."); return;
        }




        // public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        // {
        // using (var ms = new MemoryStream())
        // {
        //     imageIn.Save(ms,imageIn.RawFormat);
        //     return  ms.ToArray();
        // }
        // }

        //  public static Image[] GetFramesFromAnimatedGIF(Image IMG)
        // {
        //     List<Image> IMGs = new List<Image>();
        //     int Length = IMG.GetFrameCount(FrameDimension.Time);

        //     for (int i = 0; i < Length; i++)
        //     {
        //         IMG.SelectActiveFrame(FrameDimension.Time, i);
        //         IMGs.Add(IMG);
        //     }

        //     return IMGs.ToArray();
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