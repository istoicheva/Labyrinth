using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class Program
    {

        #region Constants
        const int Width = 80;  // Windows Width
        const int Height = 40;  // Windows Height
        const char Wall = '#';  // 1 are walls
        const char Path = ' ';  // 0 are paths
        const char Player = '@';

        static int WinningLocationX;
        static int WinningLocationY;

        static int PlayerLocationX;
        static int PlayerLocationY;

        static int mapWidth;
        static int mapHeight;

        static char[,] map;

        static bool? gameWon;
        #endregion

        // Load Map from File
        static char[,] LoadMap()
        {
            int rowCount = 1;
            int colCount;
            string oneLine;

            using (var reader = File.OpenText(@"..\..\Maps\map.txt"))
            {
                oneLine = reader.ReadLine();
                colCount = oneLine.Length;
                while (reader.ReadLine() != null)
                {
                    rowCount++;
                }
            }


            mapWidth = colCount;
            mapHeight = rowCount + 1;

            char[,] map = new char[mapHeight, mapWidth];

            string line;
            using (var reader = File.OpenText(@"..\..\Maps\map.txt"))
            {
                for (int i = 0; i < mapHeight - 1; i++)
                {
                    line = reader.ReadLine();
                    if (line.Length != mapWidth)
                    {
                        Console.WriteLine("ERROR! Incorrect Map Size");
                    }
                    else
                    {
                        for (int ch = 0; ch < mapWidth; ch++)
                        {
                            if (line[ch] == '0')
                            {
                                map[i, ch] = Path;
                            }
                            else
                            {
                                map[i, ch] = Wall;
                            }
                        }
                    }
                }

                for (int j = 0; j < mapWidth; j++)
                {
                    map[mapHeight - 1, j] = ' ';
                }
            }


            //Set Winning Possition
            for (int col = 0; col < mapWidth; col++)
            {
                if (map[0, col] == ' ')
                {
                    WinningLocationX = 0;
                    WinningLocationY = col;
                }
            }

            return map;
        }

        // Controls
        static void CheckForInput()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.LeftArrow)
                {
                    if ((PlayerLocationY - 1) >= 0 &&
                        map[PlayerLocationX, PlayerLocationY - 1] == ' ')
                    {
                        map[PlayerLocationX, PlayerLocationY] = ' ';
                        PlayerLocationY--;
                    }
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    if ((PlayerLocationY + 1) != mapWidth &&
                        map[PlayerLocationX, PlayerLocationY + 1] == ' ')
                    {
                        map[PlayerLocationX, PlayerLocationY] = ' ';
                        PlayerLocationY++;
                    }
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    if ((PlayerLocationX - 1) >= 0 &&
                        map[PlayerLocationX - 1, PlayerLocationY] == ' ')
                    {
                        map[PlayerLocationX, PlayerLocationY] = ' ';
                        PlayerLocationX--;
                    }
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    if ((PlayerLocationX + 1) != mapHeight &&
                        map[PlayerLocationX + 1, PlayerLocationY] == ' ')
                    {
                        map[PlayerLocationX, PlayerLocationY] = ' ';
                        PlayerLocationX++;
                    }
                }

                map[PlayerLocationX, PlayerLocationY] = '@';
            }
        }

        // Game Logic
        static void ProcessGameLogic()
        {
            if (PlayerLocationX == WinningLocationX && PlayerLocationY == WinningLocationY)
            {
                gameWon = true;
            }
        }

        // Strting Screen
        static void InitilizeScreen()
        {
            Console.SetCursorPosition(0, 0);
            map = LoadMap();

            // Set Player Position
            PlayerLocationX = mapHeight - 1;
            PlayerLocationY = mapWidth / 2;
            map[PlayerLocationX, PlayerLocationY] = Player;

            PrintMap();
        }

        static void PrintMap()
        {
            if (map == null)
            {
                map = LoadMap();
            }
            else
            {
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        if (map[i, j] == '@')
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(map[i, j]);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write(map[i, j]);
                        }
                    }
                    Console.WriteLine();
                }
            }
        }

        // Draw End Screen
        static void DrawEndScreen()
        {
            Console.SetCursorPosition(0, 0);
            string blankLine = new String(' ', mapWidth);
            for (int i = 0; i < mapHeight; i++)
            {
                Console.WriteLine(blankLine);
                System.Threading.Thread.Sleep(400);
            }
            Console.SetCursorPosition(mapWidth / 2, mapHeight / 2);
            if (gameWon == true)
            {
                Console.WriteLine("You win!");
            }
            else if (gameWon == false)
            {
                Console.WriteLine("You lost!");
            }
        }

        // Draw Screen
        static void DrawScreen()
        {
            Console.SetCursorPosition(0, 0);
            PrintMap();
        }

        // Main
        static void Main(string[] args)
        {
            Console.SetWindowSize(Width, Height + 1);    // Window Size
            Console.SetBufferSize(Width, Height + 1);    // Writing Area

            InitilizeScreen();
            while (gameWon == null)
            {
                CheckForInput();
                ProcessGameLogic();
                DrawScreen();
            }

            DrawEndScreen();

            Console.SetCursorPosition(mapWidth / 2, mapHeight - 2);
            Console.WriteLine("Play Again?");
            Console.ReadKey();
        }
    }
}
