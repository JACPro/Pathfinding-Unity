using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGrid : MonoBehaviour
{

    [SerializeField] private int _gridSizeX = 41;
    [SerializeField] private int _gridSizeY = 24;

    [SerializeField] private GameObject _tilePrefab;
    private GameObject[,] _emptyGrid;

    public GameObject[,] DrawTiles()
    {
        float tileSize = _tilePrefab.transform.localScale.x;
        float tileSpacing = 0.07f;

        float xOffset = -8.61f;
        float yOffset = -4.15f;

        GameObject[,] grid = new GameObject[_gridSizeX,_gridSizeY];

        for (int rows = 0; rows < _gridSizeX; rows++)
        {
            for (int cols = 0; cols < _gridSizeY; cols++)
            {
                grid[rows, cols] = Instantiate(
                    _tilePrefab, 
                    new Vector3 (xOffset + rows * (tileSize + tileSpacing), yOffset + cols * (tileSize + tileSpacing), 0), 
                    Quaternion.identity, 
                    gameObject.transform
                );
                grid[rows, cols].GetComponent<Tile>().SetCoordinates(rows, cols);
            }
        }
        _emptyGrid = grid;
        return grid;
    }

    public void GetEmptyGrid(GameObject[,] _emptyGrid)
    {
        for (int rows = 0; rows < _gridSizeX; rows++)
        {
            for (int cols = 0; cols < _gridSizeY; cols++)
            {
                _emptyGrid[rows,cols].GetComponent<Tile>().ChangeTile(PenState.Erase, true);
            }
        }
    }
}
