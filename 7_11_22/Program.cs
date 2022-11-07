using System;

namespace ConsoleApp
{
    public class Program
    {
        public struct Osoba{
            public string imie;
            public string nazwisko;
            public int wiek;
        }

        public static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            Osoba osoba = new Osoba();
            osoba.imie = "Foo";
            osoba.nazwisko = "Bar";
            osoba.wiek = 20;

            Console.WriteLine(osoba.imie);
            Console.WriteLine(osoba.nazwisko);
            Console.WriteLine(osoba.wiek);

            // SOurce code na moodle
        }
    }
}