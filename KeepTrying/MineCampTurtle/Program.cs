using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineCampTurtle
{
    class Program
    {

        static void Main(string[] args)
        {
            TurtleChallenge turtleChallenge = new TurtleChallenge(File.ReadAllLines(args[0]), File.ReadAllLines(args[1]));
            string[] output = turtleChallenge.RunTest();
            for (int i = 0; i < output.Length; i++)
            {
                Console.WriteLine(string.Format("Sequence {0}: {1}", i, output[i]));
            }
            File.WriteAllLines(args[1], output);
            Console.Read();
        }
    }


    /// <summary>
    /// defines the directions
    /// </summary>
    public enum Direction
    {
        North,
        South,
        East,
        West
    }

    /// <summary>
    /// Defines the position(x,y) of the turtle and its direction
    /// </summary>
    public class Point
    {
        public int X;
        public int Y;
        public Direction Direction;

        public static Point ParsePoint(string value)
        {
            Point position = new Point();
            string[] values = value.Split(',');
            position.X = int.Parse(values[0]);
            position.Y = int.Parse(values[1]);

            if (values.Length > 2)
                position.Direction = (Direction)Enum.Parse(typeof(Direction), values[2]);

            return position;
        }
    }

    /// <summary>
    /// Turtle Challenge main class
    /// </summary>
    public class TurtleChallenge
    {
        /// <summary>
        /// Marix size
        /// </summary>
        private Point _matrixSize;

        /// <summary>
        /// boolean matrix defining where the mines are
        /// </summary>
        private bool[,] _mines;

        /// <summary>
        /// What is the current position of the Turtle
        /// </summary>
        private Point _currentPosition;

        /// <summary>
        /// What is the exit position
        /// </summary>
        private Point _exitPosition;

        /// <summary>
        /// What are the action to be performed
        /// </summary>
        private string[] _actions;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="actions"></param>
        public TurtleChallenge(string[] settings, string[] actions)
        {
            _matrixSize = Point.ParsePoint(settings[0]);
            _currentPosition = Point.ParsePoint(settings[1]);
            _exitPosition = Point.ParsePoint(settings[2]);

            //creating the matrix
            _mines = new bool[_matrixSize.X, _matrixSize.Y];

            //setting the mines
            for (int i = 3; i < settings.Length; i++)
            {
                Point pointMine = Point.ParsePoint(settings[i]);
                _mines[pointMine.X, pointMine.Y] = true;
            }

            _actions = actions;
        }

        /// <summary>
        /// perform a move of the turtle
        /// </summary>
        /// <param name="newPosition"></param>
        private bool Move(Point newPosition)
        {
            bool allowedToMove = false;
            switch (_currentPosition.Direction)
            {
                case Direction.North:
                    allowedToMove = _currentPosition.X == newPosition.X && _currentPosition.Y - 1 == newPosition.Y && _currentPosition.Y - 1 >= 0;
                    break;
                case Direction.South:
                    allowedToMove = _currentPosition.X == newPosition.X && _currentPosition.Y + 1 == newPosition.Y && _currentPosition.Y + 1 < _matrixSize.Y;
                    break;
                case Direction.East:
                    allowedToMove = _currentPosition.Y == newPosition.Y && _currentPosition.X + 1 == newPosition.X && _currentPosition.X + 1 < _matrixSize.X;
                    break;
                case Direction.West:
                    allowedToMove = _currentPosition.Y == newPosition.Y && _currentPosition.X + 1 == newPosition.X && _currentPosition.X - 1 <= 0;
                    break;
            }

            if (allowedToMove)
            {
                _currentPosition.X = newPosition.X;
                _currentPosition.Y = newPosition.Y;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Rotates the turtle
        /// </summary>
        /// <param name="direction"></param>
        private void Rotate()
        {
            switch (_currentPosition.Direction)
            {
                case Direction.North:
                    _currentPosition.Direction = Direction.East;
                    break;
                case Direction.South:
                    _currentPosition.Direction = Direction.West;
                    break;
                case Direction.East:
                    _currentPosition.Direction = Direction.South;
                    break;
                case Direction.West:
                    _currentPosition.Direction = Direction.North;
                    break;
            }
        }

        /// <summary>
        /// performs all the action and returns the result
        /// </summary>
        public string[] RunTest()
        {
            if (_mines[_exitPosition.X, _exitPosition.Y])
            {
                Console.WriteLine("Exit and Mine cannot be in the same position.");
            }
            else
            {
                for (int i = 0; i < _actions.Length; i++)
                {
                    string action = _actions[i].Split(':')[0];

                    if (action.StartsWith("m=", StringComparison.OrdinalIgnoreCase))
                    {
                        Point pointMove = Point.ParsePoint(_actions[i].Substring(2));

                        if (Move(pointMove))
                        {
                            if (_mines[_currentPosition.X, _currentPosition.Y])
                            {
                                _actions[i] = action + ": Mine hit. End of Game!";
                                break;
                            }
                            else
                                if (_currentPosition.X == _exitPosition.X && _currentPosition.Y == _exitPosition.Y)
                                {
                                    _actions[i] = action + ": Exit Position found!";
                                    break;
                                }
                                else
                                    if (i == _actions.Length - 1)
                                    {
                                        _actions[i] = action + ": Still in Danger!";
                                    }
                                    else
                                    {
                                        _actions[i] = action + ": Successful move!";
                                    }
                        }
                        else
                        {
                            _actions[i] = action + ": Incorrect move!";
                        }
                    }
                    else
                        if (_actions[i].StartsWith("r", StringComparison.OrdinalIgnoreCase))
                        {
                            Rotate();
                            _actions[i] = action + ": Successfull move!";
                        }
                }
            }

            return _actions;
        }

    }
}
