using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

/*You have a maze of Nodes
Node Values:
1:   This value in the maze array represents a passable or walkable area. It indicates that the node can 
be visited or traveled through by an algorithm searching for a path through the maze.
0:   This value indicates an impassable or blocked area. It cannot be visited or crossed by pathfinding 
algorithms. These are essentially the walls or barriers within the maze.


Your search algorithm will return the nodes that are the path through the maze. 

Path Visualization Characters printed out in the Console:
'.' (dot): This character is used to represent passable nodes (those with a value of 1 in the maze array) 
that have not been included in the final path found by the algorithm. It indicates open, traversable space 
that is not part of the solution path.

'#' (hash): Used to represent blocked areas or walls within the maze (those with a value of 0). 
These areas are not passable, and thus the algorithm cannot travel through them.

'X': This character is used to denote the nodes that are part of the path identified by the algorithm 
from the start to the end point. It highlights the successful route taken through the maze, marking 
these nodes distinctively against those that are merely passable or blocked.
*/

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

            var pathBFS = MazeGenerator.TestAlgorithm(BFS, maze, startEnd.start, startEnd.end);
            Console.WriteLine("BFS Path:");
            MazeGenerator.PrintMaze(maze, pathBFS);

            /*
            var pathDFS = MazeGenerator.TestAlgorithm(DFS, maze, startEnd.start, startEnd.end);
            Console.WriteLine("DFS Path:");
            MazeGenerator.PrintMaze(maze, pathDFS);
            Console.WriteLine("----------------");
            */
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
        List<(int, int)> path = new List<(int, int)>();

        Queue<(int, int)> nodes = new Queue<(int, int)>();
        Dictionary<(int, int), (int, int)> parentMap = new Dictionary<(int, int), (int, int)>();
        HashSet<(int, int)> visited = new HashSet<(int, int)>();

        nodes.Enqueue(start);

        while (nodes.Count > 0)
        {
            var currentNode = nodes.Dequeue();
            if (currentNode == end)
            {
                    // Reconstruct the path from end to start
                    while (currentNode != start)
                    {
                        path.Add(currentNode);
                        currentNode = parentMap[currentNode];
                    }
                    path.Add(start);
                    path.Reverse();
                    return path;
            }

            int x = currentNode.Item1;
            int y = currentNode.Item2;

            if (!visited.Contains(currentNode) && maze[x, y] == 1)
            {
                visited.Add(currentNode);
                path.Add(currentNode);
                AddUnvisitedNeighbors(currentNode, nodes, maze, visited);
            }
        }

        return path;
    }

    private static void AddUnvisitedNeighbors((int, int) currentNode, Queue<(int, int)> nodes, int[,] maze, HashSet<(int,int)> visited)
    {
        int x = currentNode.Item1;
        int y = currentNode.Item2;

        //up
        if (y - 1 >= 0 && maze[x, y - 1] == 1 && !visited.Contains(currentNode)) nodes.Enqueue((x, y - 1));
        //down
        if (y + 1 < maze.GetLength(1) && maze[x, y + 1] == 1 && !visited.Contains(currentNode)) nodes.Enqueue((x, y + 1));
        //left
        if (x - 1 >= 0 && maze[x - 1, y] == 1 && !visited.Contains(currentNode)) nodes.Enqueue((x - 1, y));
        //right
        if (x + 1 < maze.GetLength(0) && maze[x + 1, y] == 1 && !visited.Contains(currentNode)) nodes.Enqueue((x + 1, y));

        return;
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