using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] DrawGrid gridManager;
    [SerializeField] PenManager penManager;
    UIManager uiManager;
    GameObject[,] grid;
    Coordinates start = new Coordinates(), finish = new Coordinates();
    AudioSource errorSound;
    BFS bfs;
    private void Awake() 
    {
        uiManager = FindObjectOfType<UIManager>();
        bfs = FindObjectOfType<BFS>();
        errorSound = GetComponent<AudioSource>();
    }

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
        StopAllCoroutines();
    }

    public void BreadthFirstSearch()
    {
        if (start.IsNull())
        {
            errorSound.Play(0);
            uiManager.UpdateErrorMessage("Missing start point");
            return;
        } else if (finish.IsNull())
        {
            errorSound.Play(0);
            uiManager.UpdateErrorMessage("Missing end point");
            return;
        }
        else 
        {
            uiManager.ClearErrorMessage();
        }
        StartCoroutine(bfs.Search(grid, start, finish));
    }

    public IEnumerator DrawPath(List<Coordinates> path)
    {
        if (path.Count == 0)
        {
            errorSound.Play(0);
            uiManager.UpdateErrorMessage("No possible path");
            yield break;
        }
        else 
        {
            uiManager.ClearErrorMessage();
        }

        for (int i = 1; i < path.Count - 1; i++)
        {
            grid[path[i].x, path[i].y].GetComponent<Tile>().ChangeTile(PenState.Path);
            yield return new WaitForSeconds(0.07f);
        }
    }
}
