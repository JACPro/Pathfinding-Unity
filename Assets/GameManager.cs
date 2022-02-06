using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] DrawGrid gridManager;
    [SerializeField] PenManager penManager;

    GameObject[,] grid;
    Coordinates start = new Coordinates(), finish = new Coordinates();

    void Start()
    {
        grid = gridManager.DrawTiles();
    }

    public PenState GetPenState()
    {
        return penManager.GetPenState();
    }

    public void SetStart(Coordinates newStart)
    {
        if (start == newStart) return;
        if (newStart == finish) finish = Coordinates.EMPTY;

        if (!start.IsNull()) 
        {
            Tile prevStart = grid[start.x, start.y].GetComponent<Tile>();
            if (prevStart.GetTileState() == PenState.Start)
            {
                prevStart.ChangeTile(PenState.Erase);
            }
        }
        start = newStart;
    }

    public void SetFinish(Coordinates newFinish)
    {
        if (finish == newFinish) return;
        if (newFinish == start) start = Coordinates.EMPTY;

        if (!finish.IsNull()) 
        {
            Tile prevFinish = grid[finish.x, finish.y].GetComponent<Tile>();
            if (prevFinish.GetTileState() == PenState.Finish)
            {
                prevFinish.ChangeTile(PenState.Erase);
            }
        }
        finish = newFinish;
    }

    public void ClearTiles()
    {
        gridManager.GetEmptyGrid(grid);
        start = finish = Coordinates.EMPTY;
    }
}
