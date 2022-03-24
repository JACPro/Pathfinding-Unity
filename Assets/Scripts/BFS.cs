using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : SearchRoutine
{
	//Direction vectors - left, up, right, down
	static int[] dirRow4 = {-1, 0, 1, 0};
	static int[] dirCol4 = {0, 1, 0, -1}; 
	//Direction vectors - left, up-left, up, up-right, right, down-right, down, down-left
	static int[] dirRow8 = {-1, -1, 0, 1, 1, 1, 0, -1};
	static int[] dirCol8 = {0, 1, 1, 1, 0, -1, -1, -1}; 
	List<Coordinates> path;

	bool TileInRange(Coordinates tile, int xMax, int yMax)
	{
		if (tile.x < 0 || tile.y < 0 || tile.x >= xMax || tile.y >= yMax) return false;
		return true;
	}

	public override Queue<Stack<Tile>> Search(GameObject[,] grid, Coordinates start, Coordinates finish)
	{
		path = new List<Coordinates>();
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
		visited[start.x, start.y] = true;

		while (toExplore.Count > 0 && path.Count == 0)
		{
			Coordinates tile = toExplore.Dequeue();
			Stack<Tile> tileStack = new Stack<Tile>();
			for (int i = 0; i < 4; i++)
			{
				int adjX = tile.x + dirRow4[i], adjY = tile.y + dirCol4[i];
				Coordinates adjacent = new Coordinates(adjX, adjY);
				if (TileInRange(adjacent, xMax, yMax) && !visited[adjX, adjY])
				{
					prev[adjX, adjY] = tile;
					if (adjacent.Equals(finish))
					{
						path.Add(adjacent);
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
		if (path.Count != 0)
		{
			Coordinates tile = prev[path[0].x, path[0].y];
			while (prev[tile.x, tile.y] != null)
			{
				path.Add(tile);
				foundPath.Push(grid[tile.x, tile.y].GetComponent<Tile>());
				tile = prev[tile.x, tile.y];
			}
			//path.Reverse();
		}
		
		gameManager.SetFinalPath(foundPath);
		
		return explorePath;
	}
}
