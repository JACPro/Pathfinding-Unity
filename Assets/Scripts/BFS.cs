using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : SearchRoutine
{
	//Direction vectors - left, up, right, down
	private static int[] _dirRow4 = {-1, 0, 1, 0};
	private static int[] _dirCol4 = {0, 1, 0, -1}; 
	//Direction vectors - left, up-left, up, up-right, right, down-right, down, down-left
	private static int[] _dirRow8 = {-1, -1, 0, 1, 1, 1, 0, -1};
	private static int[] _dirCol8 = {0, 1, 1, 1, 0, -1, -1, -1}; 
	private List<Coordinates> _path;

	private bool TileInRange(Coordinates tile, int xMax, int yMax)
	{
		if (tile._x < 0 || tile._y < 0 || tile._x >= xMax || tile._y >= yMax) return false;
		return true;
	}

	public override Queue<Stack<Tile>> Search(GameObject[,] grid, Coordinates start, Coordinates finish)
	{
		_path = new List<Coordinates>();
		int xMax = grid.GetLength(0), yMax = grid.GetLength(1); 
		if (xMax * yMax == 0) return null;

		Queue<Stack<Tile>> explorePath = new Queue<Stack<Tile>>();

		bool[,] visited = new bool[xMax,yMax];
		Coordinates[,] prev = new Coordinates[xMax,yMax];
		for (int i = 0; i < xMax; i++)
		{
			for (int j = 0; j < yMax; j++)
			{
				visited[i,j] = false;
				PenState tilestate = grid[i,j].GetComponent<Tile>().GetTileState();
				if (tilestate == PenState.Path || tilestate == PenState.Explored) grid[i,j].GetComponent<Tile>().ChangeTile(PenState.Erase, true);
			}
		}

		Queue<Coordinates> toExplore = new Queue<Coordinates>();

		toExplore.Enqueue(start);
		visited[start._x, start._y] = true;

		while (toExplore.Count > 0 && _path.Count == 0)
		{
			Coordinates tile = toExplore.Dequeue();
			Stack<Tile> tileStack = new Stack<Tile>();
			for (int i = 0; i < 4; i++)
			{
				int adjX = tile._x + _dirRow4[i], adjY = tile._y + _dirCol4[i];
				Coordinates adjacent = new Coordinates(adjX, adjY);
				if (TileInRange(adjacent, xMax, yMax) && !visited[adjX, adjY])
				{
					prev[adjX, adjY] = tile;
					if (adjacent.Equals(finish))
					{
						_path.Add(adjacent);
					}//if not wall
					else if (grid[adjX,adjY].GetComponent<Tile>().GetTileState() != PenState.Wall)
					{
						toExplore.Enqueue(adjacent);
						tileStack.Push(grid[adjX,adjY].GetComponent<Tile>());
						visited[adjX, adjY] = true;
						grid[adjX, adjY].GetComponent<Tile>().ChangeTile(PenState.Explored, false);
					}
				}
			}
			if (tileStack.Count > 0) explorePath.Enqueue(tileStack);
		}

		Stack<Tile> foundPath = new Stack<Tile>();
		if (_path.Count != 0)
		{
			Coordinates tile = prev[_path[0]._x, _path[0]._y];
			while (prev[tile._x, tile._y] != null)
			{
				_path.Add(tile);
				foundPath.Push(grid[tile._x, tile._y].GetComponent<Tile>());
				tile = prev[tile._x, tile._y];
			}
			//path.Reverse();
		}
		
		_gameManager.SetFinalPath(foundPath);
		
		return explorePath;
	}
}
