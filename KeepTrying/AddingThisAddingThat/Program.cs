using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddingThisAddingThat
{
    class Program
    {
               
        public static byte[] AddRecursive(byte[] a, byte[] b, int carry = 0)
        {
            //Start from bottom of the byte[] array
            a = a.Reverse().ToArray();
            b = b.Reverse().ToArray();

            if (a.Length == 0)
                return new byte[] { };

            int tempresult = a[0] + b[0] + carry;

            byte[] z = new byte[] { (byte)(tempresult) };
            carry = tempresult / (byte.MaxValue + 1);

            return z.Concat(AddRecursive(a.Skip(1).ToArray(), b.Skip(1).ToArray(), carry)).ToArray();
        }

        public static void Main(string[] args)
        {
            var r1 = AddRecursive(new byte[] { 1, 1, 1 }, new byte[] { 1, 1, 1 }).Reverse().ToArray();
            var r2 = AddRecursive(new byte[] { 1, 1, 255 }, new byte[] { 0, 0, 1 }).Reverse().ToArray();
            var r3 = AddRecursive(new byte[] { 0, 100, 200 }, new byte[] { 3, 2, 1 }).Reverse().ToArray();
            var r4 = AddRecursive(new byte[] { 255, 255, 255 }, new byte[] { 255, 255, 255 }).Reverse().ToArray();
            

            // Outputs: 2, 2, 2
            Console.WriteLine(string.Join(", ", r1.Select(n => "" + n).ToArray()));

            // Outputs: 1, 2, 0
            Console.WriteLine(string.Join(", ", r2.Select(n => "" + n).ToArray()));

            // Outputs: 3, 102, 201
            Console.WriteLine(string.Join(", ", r3.Select(n => "" + n).ToArray()));

            // Outputs: 255, 255, 254
            Console.WriteLine(string.Join(", ", r4.Select(n => "" + n).ToArray()));

            Console.Read();
        }
    }
}
