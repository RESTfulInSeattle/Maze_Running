using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        string[] mazeTypes = { "simple", "horizontal_bars", "vertical_bars", "open_center" };
        foreach (var mazeType in mazeTypes)
        {
            int[,] maze = MazeGenerator.CreateMaze(mazeType);
            var startEnd = MazeGenerator.FindRandomStartEnd(maze);
            Console.WriteLine($"Maze Type: {mazeType}");
            Console.WriteLine($"Start: {startEnd.start}, End: {startEnd.end}");

            var pathDFS = MazeGenerator.TestAlgorithm(DFS, maze, startEnd.start, startEnd.end);
            Console.WriteLine("DFS Path:");
            MazeGenerator.PrintMaze(maze, pathDFS);
            Console.WriteLine("----------------");

            var pathBFS = MazeGenerator.TestAlgorithm(BFS, maze, startEnd.start, startEnd.end);
            Console.WriteLine("BFS Path:");
            MazeGenerator.PrintMaze(maze, pathBFS);
        }
    }

    // Implement DFS algorithm
    public static List<(int, int)> DFS(int[,] maze, (int, int) start, (int, int) end)
    {
        return new List<(int, int)>();
    }

    // Implement BFS algorithm
    public static List<(int, int)> BFS(int[,] maze, (int, int) start, (int, int) end)
    {
        return new List<(int, int)>();
    }
}

public class MazeGenerator
{
    public static int[,] CreateMaze(string mazeType = "simple")
    {
        switch (mazeType)
        {
            case "simple":
                return new int[,] {
                    {1, 1, 1, 1, 0, 1},
                    {1, 0, 0, 1, 0, 1},
                    {1, 0, 1, 0, 1, 1},
                    {1, 1, 1, 0, 1, 1},
                    {1, 0, 0, 0, 0, 1},
                    {1, 1, 1, 1, 1, 1}
                };
            case "horizontal_bars":
                return new int[,] {
                    {1, 1, 1, 1, 1, 1},
                    {0, 0, 0, 0, 0, 1},
                    {1, 1, 1, 1, 1, 1},
                    {1, 0, 0, 0, 0, 0},
                    {1, 1, 1, 1, 1, 1},
                    {1, 0, 0, 0, 0, 1}
                };
            case "vertical_bars":
                return new int[,] {
                    {1, 0, 1, 0, 1, 0},
                    {1, 0, 1, 0, 1, 0},
                    {1, 0, 1, 0, 1, 0},
                    {1, 0, 1, 0, 1, 0},
                    {1, 0, 1, 0, 1, 0},
                    {1, 1, 1, 1, 1, 1}
                };
            case "open_center":
                return new int[,] {
                    {1, 1, 1, 1, 1, 1},
                    {1, 0, 0, 0, 0, 1},
                    {1, 0, 1, 1, 0, 1},
                    {1, 0, 1, 1, 0, 1},
                    {1, 0, 0, 0, 0, 1},
                    {1, 1, 1, 1, 1, 1}
                };
            default:
                return new int[,] {
                    {1, 1, 1, 1, 0, 1},
                    {1, 0, 0, 1, 0, 1},
                    {1, 0, 1, 0, 1, 1},
                    {1, 1, 1, 0, 1, 1},
                    {1, 0, 0, 0, 0, 1},
                    {1, 1, 1, 1, 1, 1}
                };
        }
    }

    public static void PrintMaze(int[,] maze, List<(int, int)> path)
    {
        int height = maze.GetLength(0);
        int width = maze.GetLength(1);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (path.Contains((i, j)))
                {
                    Console.Write("X ");
                }
                else
                {
                    Console.Write(maze[i, j] == 1 ? ". " : "# ");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public static List<(int, int)> TestAlgorithm(Func<int[,], (int, int), (int, int), List<(int, int)>> pathfindingFunc, int[,] maze, (int, int) start, (int, int) end)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        var path = pathfindingFunc(maze, start, end);
        stopwatch.Stop();
        Console.WriteLine($"{pathfindingFunc.Method.Name} took {stopwatch.ElapsedMilliseconds} ms");
        return path;
    }

    public static ((int, int) start, (int, int) end) FindRandomStartEnd(int[,] maze)
    {
        var random = new Random();
        var validPoints = new List<(int, int)>();
        int height = maze.GetLength(0);
        int width = maze.GetLength(1);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (maze[i, j] == 1)
                {
                    validPoints.Add((i, j));
                }
            }
        }
        var start = validPoints[random.Next(validPoints.Count)];
        var end = validPoints[random.Next(validPoints.Count)];
        while (end == start)
        {
            end = validPoints[random.Next(validPoints.Count)];
        }
        return (start, end);
    }
}