using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafestPlaceCubeWithBombs
{
    /// <summary>
    /// defines a tridimentional position x,y,z
    /// </summary>
    struct CubePosition
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int DistToSqrd(CubePosition other)
        {
            int dx = X - other.X;
            int dy = Y - other.Y;
            int dz = Z - other.Z;

            return dx * dx + dy * dy + dz * dz;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    struct Range
    {
        public int Begin { get; private set; }
        public int End { get; private set; }        // Exclusive

        public Range(int begin, int end) : this()
        {
            Begin = begin;
            End = end;
        }
    }

    class Program
    {
        /// <summary>
        /// inclusive cube size
        /// </summary>
        private const int SymetricCubeSize = 1001;

        /// <summary>
        /// Check the exclusions and return the valid out of the intersections
        /// </summary>
        /// <param name="exclusions"></param>
        /// <returns></returns>
        static IEnumerable<int> PossibleValues(List<Range> exclusions)
        {
            int next = 0;
            foreach (var e in exclusions)
            {
                if (e.End > next)
                {
                    for (int result = next; result < e.Begin; result++)
                        yield return result;

                    next = Math.Max(next, e.End);
                }
            }

            for (int result = next; result < SymetricCubeSize; result++)
                yield return result;
        }

        /// <summary>
        /// Simulate the bombs in its positions in order to test the best sqrt
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="zBombs"></param>
        /// <param name="sqrRoot"></param>
        /// <returns></returns>
        static List<Range> SimulateBombPositionsAndTest(int x, int y, List<CubePosition> zBombs, int sqrRoot)
        {
            List<Range> result = new List<Range>();

            foreach (var b in zBombs)
            {
                int dx = b.X - x;
                int dy = b.Y - y;

                int xYDistSqr = dx * dx + dy * dy;

                if (sqrRoot >= xYDistSqr)
                {
                    int maxDz = (int)Math.Sqrt(sqrRoot - xYDistSqr);

                    Range newRange = new Range(b.Z - maxDz, b.Z + maxDz + 1);

                    // The intersection of a bomb is symetric around its Z
                    // Also, each bomb has Z >= all previous ones
                    // Thus, if the Begin of the range is less than a previous Begin, the new range totally superceeds the previous range, 
                    // and the previous range can be removed
                    for (int iExisting = result.Count - 1; iExisting >= 0; iExisting--)
                    {
                        if (newRange.Begin <= result[iExisting].Begin)
                        {
                            result.RemoveAt(iExisting);
                        }
                        else
                        {
                            // result is build up sorted by Begin
                            // Thus, if Begin at iExisting is too small, there is no point continuing back through the list
                            break;
                        }
                    }

                    result.Add(newRange);
                }
            }

            return result;
        }

        /// <summary>
        /// Process the test
        /// </summary>
        /// <param name="bombs"></param>
        /// <returns></returns>
        static int ProcessTest(List<CubePosition> bombs)
        {
            int safestSquareRoot = 0;
            List<CubePosition> zBombs = bombs.OrderBy(b => b.Z).ToList();

            //reading the inclusive cube - each x, y into the inclusive cube
            //Simulate the positions of the bombs and test/find where are the intersections and bring the Zs out of these intersections
            for (int x = 0; x < SymetricCubeSize; x++)
            {
                for (int y = 0; y < SymetricCubeSize; y++)
                {
                    //now we have to go through Z, simulate the bombs positions and calculate the best radius
                    List<Range> exclusions = SimulateBombPositionsAndTest(x, y, zBombs, safestSquareRoot);

                    foreach (int z in PossibleValues(exclusions))
                    {
                        CubePosition p = new CubePosition { X = x, Y = y, Z = z };

                        //getting the closest bomb
                        int newSqrtValue = bombs.Select(b => b.DistToSqrd(p)).Min();

                        //getting the best radius
                        safestSquareRoot = Math.Max(newSqrtValue, safestSquareRoot);
                    }
                }
            }

            return safestSquareRoot;
        }

        /// <summary>
        /// Startup method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            List<List<CubePosition>> tests = new List<List<CubePosition>>();

            StreamReader inputReader = new StreamReader(args[0]);

            //reading input parameters
            int testCasesNumber = int.Parse(inputReader.ReadLine());

            for (int i = 0; i < testCasesNumber; i++)
            {
                List<CubePosition> bombs = new List<CubePosition>();
                int bombsNumber = int.Parse(inputReader.ReadLine());
                for (int b = 0; b < bombsNumber; b++)
                {
                    string[] positions = inputReader.ReadLine().Split(' ');
                    bombs.Add(new CubePosition
                    {
                        X = int.Parse(positions[0]),
                        Y = int.Parse(positions[1]),
                        Z = int.Parse(positions[2])
                    });
                }
                tests.Add(bombs);
            }

            //testing and printing the results
            for (int i = 0; i < tests.Count(); i++)
            {
                int value = i;
                Task.Run(() => 
                {                    
                    int safestPlace = ProcessTest(tests[value]);
                    Console.WriteLine("TestCase #{0}: {1}", value + 1, safestPlace);
                });
            }

            Console.Read();
        }
    }
}
