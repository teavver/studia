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
                    Console.Clear();
                    // Prototype
                    Bitmap bmp_src = new Bitmap(selected_gif, true);    
                    ConsoleWriteImage(bmp_src);
                    // ConsoleWriteImage(new Bitmap(selected_gif));
                    
                    // Split gif into frames
                    // generate_frames(gif_image, frame_count);
                    // log(0, "[ OK ] Saved frames");

                    // Convert frame to ASCII
                    // frame_to_ASCII(gif_image, 64);
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

        // public static int ToConsoleColor(System.Drawing.Color c)
        // {
        //     int index = (c.R > 128 | c.G > 128 | c.B > 128) ? 8 : 0;
        //     index |= (c.R > 64) ? 4 : 0;
        //     index |= (c.G > 64) ? 2 : 0;
        //     index |= (c.B > 64) ? 1 : 0;
        //     return index;
        // }

        // public static void ConsoleWriteImage(Bitmap src)
        // {
        //     int min = 39;
        //     decimal pct = Math.Min(decimal.Divide(min, src.Width), decimal.Divide(min, src.Height));
        //     Size res = new Size((int)(src.Width * pct), (int)(src.Height * pct));
        //     Bitmap bmpMin = new Bitmap(src, res);
        //     for (int i = 0; i < res.Height; i++)
        //     {
        //         for (int j = 0; j < res.Width; j++)
        //         {
        //             Console.ForegroundColor = (ConsoleColor)ToConsoleColor(bmpMin.GetPixel(j, i));
        //             Console.Write("██");
        //         }
        //         System.Console.WriteLine();
        //     }
        // }

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