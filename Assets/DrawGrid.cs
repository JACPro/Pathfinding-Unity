using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGrid : MonoBehaviour
{

    [SerializeField] int gridSizeX = 41;
    [SerializeField] int gridSizeY = 24;

    [SerializeField] GameObject tilePrefab;
    GameObject[,] emptyGrid;

    public GameObject[,] DrawTiles()
    {
        float tileSize = tilePrefab.transform.localScale.x;
        float tileSpacing = 0.07f;

        float xOffset = -8.61f;
        float yOffset = -4.15f;

        GameObject[,] grid = new GameObject[gridSizeX,gridSizeY];

        for (int rows = 0; rows < gridSizeX; rows++)
        {
            for (int cols = 0; cols < gridSizeY; cols++)
            {
                grid[rows, cols] = Instantiate(
                    tilePrefab, 
                    new Vector3 (xOffset + rows * (tileSize + tileSpacing), yOffset + cols * (tileSize + tileSpacing), 0), 
                    Quaternion.identity, 
                    gameObject.transform
                );
                grid[rows, cols].GetComponent<Tile>().SetCoordinates(rows, cols);
            }
        }
        emptyGrid = grid;
        return grid;
    }

    public void GetEmptyGrid(GameObject[,] emptyGrid)
    {
        for (int rows = 0; rows < gridSizeX; rows++)
        {
            for (int cols = 0; cols < gridSizeY; cols++)
            {
                emptyGrid[rows,cols].GetComponent<Tile>().ChangeTile(PenState.Erase);
            }
        }
    }
}
