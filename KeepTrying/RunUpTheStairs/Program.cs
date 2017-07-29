using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunUpTheStairs
{
    class Program
    {
        static void Main(string[] args)
        {

            StridesToTheTop(new int[] { 15 }, 2);
            StridesToTheTop(new int[] { 15, 15 }, 2);
            StridesToTheTop(new int[] { 5, 11, 9, 13, 8, 30, 14 }, 3);

            Console.Read();
        }

        static void StridesToTheTop(int[] flights, int stepsPerStride)
        {
            int stridesToTheTop = 0;

            if (flights.Length == 0 || flights.Length > 50)
                throw new Exception("Staircase must have between 1 and 50 flights of stairs, inclusive.");

            if (stepsPerStride < 2 || stepsPerStride > 5)
                throw new Exception("Steps per Stride must be between 2 and 5 steps, inclusive.");

            for (int i = 0; i < flights.Length; i++)            
            {
                int steps = flights[i];

                if (steps < 5 || steps > 50)
                    throw new Exception("Each flight of stairs must have between 5 and 50 steps, inclusive.");

                int strides = (steps / stepsPerStride);
                stridesToTheTop += strides;

                if (steps % stepsPerStride > 0)
                {
                    stridesToTheTop++;
                }

                if (i > 0)
                {
                    stridesToTheTop += 2;
                }
            }

            Console.WriteLine(string.Format("Test#: {0}", stridesToTheTop));
        }

    }
}
