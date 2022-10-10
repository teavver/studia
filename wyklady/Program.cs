using System;
using System.Runtime.InteropServices;

/// fun do tesotwania
ConsoleHelper.SetCurrentFont("Consolas", 32);
Console.SetWindowSize(50, 30);
Console.Clear();

Main.PP_Laborki();


public static class Main
{
    public static void Say(string input)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(input);
        Console.ResetColor();
    }
    public static void Zad_1()
    {



        // Obliczyc wartosc (a^2 + b)
        //                  ---------
        //                  (a + b)^2


        float a;
        float b;
        Console.WriteLine("Podaj liczbe a: ");
        string? a_input = Console.ReadLine();
        bool is_a_num = float.TryParse(a_input, out a);
        Console.WriteLine("Podaj liczbe b");
        string? b_input = Console.ReadLine();
        bool is_b_num = float.TryParse(b_input, out b);

        if(!(is_a_num && is_b_num))
        {
            Console.Write("ktorys input nie jest liczba.");
        }  
        {
            Eval(a,b);
        }
        static void Eval(float a, float b)
        {
            float a_squared = a * a;
            float b_squared = b * b;
            float top_result = a_squared + b;
            float bottom_result = a_squared + b_squared + (2 * a * b);
            if(bottom_result == 0)
            {
                Console.WriteLine("mianownik jest rowny 0, wprowadz inne dane");
            }
            float result = top_result / bottom_result;
            Console.WriteLine($"wynik: {result}");
        }
    }
    // ---------------------------------------------------------------------------- //

    public static void Zad_2()
    {
        double a;
        double b;
        double c;

        Main.Say("podaj liczbe a:");
        string? str_a = Console.ReadLine();
        Main.Say("podaj liczbe b:");
        string? str_b = Console.ReadLine();
        Main.Say("podaj liczbe c:");
        string? str_c = Console.ReadLine();
        Double.TryParse(str_a, out a);
        Double.TryParse(str_b, out b);
        Double.TryParse(str_c, out c);
        Eval(a,b,c);

        static void Eval(double a, double b, double c)
        {
            if (c == 0)
            {
                if(a - b == 0)
                {
                    Console.WriteLine("proba dzielenia przez 0");
                    return;
                }
                double result = 1 / (a - b);
                Console.WriteLine($"result: {result}");
            }
            if (c > 0)
            {
                double result = (a * a) + b;
                Console.WriteLine($"result: {result}");
            }
            if (c < 0)
            {
                double result = a - (b * b);
                Console.WriteLine($"result: {result}");
            }
        }
    }

    // ---------------------------------------------------------------------------- //

    public static void Zad_3()
    {
        // Alg Euklidesa bez funkcji " / " dzielenia.

        Main.Say("podaj pierwsza liczbe");
        string? str_a = Console.ReadLine();
        Main.Say("podaj druga liczbe");
        string? str_b = Console.ReadLine();
        int a = int.Parse(str_a);
        int b = int.Parse(str_b);
        Console.WriteLine($"NWD to: {NWD(a, b)}");

        static int NWD(int a, int b)
        {
            if (a % b == 0) return b;
            if (a % b != 0)
            {
                int r = a % b;
                a = b;
                b = r;
                if (a % b == 0) return b;
                NWD(a, b);
            }
            return NWD(a,b);
        }

    }

    public static void Zad_4()
    {
        // Licz sume cyfr rozwiniecia dziesietnego liczby naturalnej

        Main.Say("podaj liczbe naturalna");
        string str_input = Console.ReadLine();
        int result = str_input.Sum(c => c - '0');
        Main.Say($"suma cyfr: {result}");
    }

    // ---------------------------------------------------------------------------- //

    public static void Zad_5()
    {

    }

    // ---------------------------------------------------------------------------- //


    public static void PP_Laborki()
    {
        // 10.10.2022
        
        // switch / case


/*
        switch (input)
        {
            case 1:
                Console.WriteLine("case1");
                break;
        }*/

        // ReadKey
        // Console.ReadKey().KeyChar;

        // for loop
        // nieskonczony loop,
        // Scope block dla i

        // scoped case


        /*for(int j = 0; j < 10; j++)
        {
            // WriteLine
        }

        // global case
        int i = 0;
        for (; i < 3; i++)
        {
            // WriteLine
        }
        Console.WriteLine(i);*/

        // While loop / Do while loop
        // While -> sprawdza warunek przed wykonaniem
        // Do while -> sprawdza po wykonaniu

        // TryParse -> if(int tryparse(c.RL), out foo == true && bar)
        // -> true -> while loop / switch case, bledy

        static void Kalkulator()
        {
            Main.Say("Wybierz dzialanie (wpisz cyfre)");
            Console.WriteLine("0) Dodawanie");
            Console.WriteLine("1) Odejmowanie");
            Console.WriteLine("2) Mnozenie");
            Console.WriteLine("3) Dzielenie");

            switch(Console.ReadLine())
            {
                case "0":
                {
                        Dodawanie();
                        break;
                }
                case "1":
                    {
                        Odejmowanie();
                        break;
                    }
                case "2":
                    {
                        Mnozenie();
                        break;
                    }
                case "3":
                    {
                        Dzielenie();
                        break;
                    }
                default:
                    {
                        Main.Say("wybrales niepoprawna cyfre, sprobuj od nowa");
                        Kalkulator();
                        break;
                    }
            }
        }

        static void Dodawanie()
        {
            Main.Say("podaj pierwsza liczbe");
             string? str_a = Console.ReadLine();
            Main.Say("podaj druga liczbe");
            string? str_b = Console.ReadLine();
            int a = int.Parse(str_a);
            int b = int.Parse(str_b);

            Main.Say($"wynik: {a + b}");
            Console.ReadKey();
        }

        static void Odejmowanie()
        {
            Main.Say("podaj odjemną");
             string? str_a = Console.ReadLine();
            Main.Say("podaj odjemnik");
            string? str_b = Console.ReadLine();
            int a = int.Parse(str_a);
            int b = int.Parse(str_b);

            Main.Say($"wynik: {a - b}");
            Console.ReadKey();
        }

        static void Mnozenie()
        {
            Main.Say("podaj czynnik mnozenia");
             string? str_a = Console.ReadLine();
            Main.Say("podaj mnoznik");
            string? str_b = Console.ReadLine();
            int a = int.Parse(str_a);
            int b = int.Parse(str_b);

            Main.Say($"wynik: {a * b}");
            //
            Console.ReadKey();
        }

        static void Dzielenie()
        {
            Main.Say("podaj pierwsza liczbe");
             string? str_a = Console.ReadLine();
            Main.Say("podaj druga liczbe");
            string? str_b = Console.ReadLine();
            int a = int.Parse(str_a);
            int b = int.Parse(str_b);
            if(b == 0)
            {
                Main.Say("nie mozna dzielic przez 0, zacznij od nowa.");
                Dzielenie();
            }
            Main.Say($"wynik: {a / b}");
            Console.ReadKey();
        }
        Kalkulator();
    }
}

public static class ConsoleHelper
{
    private const int FixedWidthTrueType = 54;
    private const int StandardOutputHandle = -11;

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern IntPtr GetStdHandle(int nStdHandle);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);


    private static readonly IntPtr ConsoleOutputHandle = GetStdHandle(StandardOutputHandle);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct FontInfo
    {
        internal int cbSize;
        internal int FontIndex;
        internal short FontWidth;
        public short FontSize;
        public int FontFamily;
        public int FontWeight;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.wc, SizeConst = 32)]
        public string FontName;
    }

    public static FontInfo[] SetCurrentFont(string font, short fontSize = 0)
    {
        Console.WriteLine("Set Current Font: " + font);

        FontInfo before = new FontInfo
        {
            cbSize = Marshal.SizeOf<FontInfo>()
        };

        if (GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
        {

            FontInfo set = new FontInfo
            {
                cbSize = Marshal.SizeOf<FontInfo>(),
                FontIndex = 0,
                FontFamily = FixedWidthTrueType,
                FontName = font,
                FontWeight = 400,
                FontSize = fontSize > 0 ? fontSize : before.FontSize
            };

            // Get some settings from current font.
            if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set))
            {
                var ex = Marshal.GetLastWin32Error();
                Console.WriteLine("Set error " + ex);
                throw new System.ComponentModel.Win32Exception(ex);
            }

            FontInfo after = new FontInfo
            {
                cbSize = Marshal.SizeOf<FontInfo>()
            };
            GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

            return new[] { before, set, after };
        }
        else
        {
            var er = Marshal.GetLastWin32Error();
            Console.WriteLine("Get error " + er);
            throw new System.ComponentModel.Win32Exception(er);
        }
    }
}