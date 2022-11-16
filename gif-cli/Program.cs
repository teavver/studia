using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

using Gui = GUI.Gui;
using Tb = Toolbox.Tools;

namespace GIF_CLI
{
    public class Program
    {
        public static string OS = "";
        public static void Main(string[] args)
        {
            gif_CLI();
        }

        private static void gif_CLI(){
            Console.Clear();
            
            // Check operating system before anything to ensure correct file paths
            check_OS();

            // Display menu
            Gui.Main();
        }
        public static void eval_gif_file(string selected_gif, int args = 0)
        {
                Tb.log(0, $"[ OK ] Selected file: {selected_gif}");

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

                Tb.log(0, $"gif frame delay: {framerate.ToString()}");
                Tb.log(0, $"size in bytes: {size_in_bytes.ToString()}");
                Tb.log(0, $"image size: {image_width.ToString()} x {image_height.ToString()} px");
                Tb.log(0, "[ OK ]");
                Thread.Sleep(250);
                Console.Clear();

                // Check for opt args before rendering
                if (args == 1) { render_frames(gif_image, frame_count, args); } // render looped
                else { render_frames(gif_image, frame_count, 0); } //render normal
        }
        // public static void display_help(){

        //     // List all available commands
        //     Console.Clear();
        //     Tb.log(3, "[ HELP ] Options available: ");
        //     Console.WriteLine();
        //     Tb.log(3, "Commands available: ");
        //     Tb.log(3, "[ -ls ] Display all selectable .gif files in current directory");
        //     Console.WriteLine();
        //     Tb.log(3, "Optional arguments available: ");
        //     Tb.log(3, "[ file.gif -l ] Loop output gif");
        //     Tb.log(3, "[ file.gif -s {number} ] Make gif and define its scale/quality");
        //     Console.WriteLine();
        // }
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
            Tb.log(0, "[ OK ] Done", true);
            Gui.Main(true);
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
            int sMax = 34; // (SCALE, PIXEL RATIO IN CLI)
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
    }
}