//
//
//
//
// projecteuler.net
// ----------------

using System;


namespace ProjectEuler
{
    public class Program
    {
        public static void Main(string[] args)
        {
           pe_16(2,5);
        }
        public static int last_res = 1; // result of last iteration
        public static int placeholder = 0; // db-dig num placeholder

        public static void pe_16(int num, int power){

            // 2^15 = 32768, sum of digits = 3 + 2 + 7 + 6 + 8
            // Whats the sum of digits in the number 2^1000?

            // testing 2^5

            for (int i = 1; i < power; i++)
            {
                int small_res = num * last_res;
            }
            Console.WriteLine("-------");
        }
    }
}
