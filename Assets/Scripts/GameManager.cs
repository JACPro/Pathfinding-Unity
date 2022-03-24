using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public enum GameState { Drawing, Executing, Menu }
public class GameManager : MonoBehaviour
{
    [SerializeField] DrawGrid gridManager;
    [SerializeField] PenManager penManager;
    UIManager uiManager;
    GameObject[,] grid;
    Coordinates start = new Coordinates(), finish = new Coordinates();
    AudioSource errorSound;
    Stack<Tile> path;
    Queue<Stack<Tile>> explorePath;
    GameState gameState = GameState.Drawing;
    [SerializeField] int drawSpeed = 1;
    public event Action<GameState> OnStateChanged;

    private void Awake() 
    {
        uiManager = FindObjectOfType<UIManager>();
        errorSound = GetComponent<AudioSource>();
        uiManager.OnInfoUI += SetGameState;
        OnStateChanged += uiManager.SetSearchButtonsActive;
    }

    void Start()
    {
        grid = gridManager.DrawTiles();
    }

    public Coordinates GetStart() => start;
    public Coordinates GetFinish() => finish;
    public GameObject[,] GetGrid() => grid;
    public GameState GetGameState() => gameState;
    public PenState GetPenState() => penManager.GetPenState();

    public void SetStart(Coordinates newStart)
    {
        if (start == newStart) return;
        if (newStart == finish) finish = Coordinates.EMPTY;

        if (!start.IsNull()) 
        {
            Tile prevStart = grid[start.x, start.y].GetComponent<Tile>();
            if (prevStart.GetTileState() == PenState.Start)
            {
                prevStart.ChangeTile(PenState.Erase, true);
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
                prevFinish.ChangeTile(PenState.Erase, true);
            }
        }
        finish = newFinish;
    }

    public void ClearTiles()
    {
        StopDrawingPath();
        gridManager.GetEmptyGrid(grid);
        start = finish = Coordinates.EMPTY;
    }

    public void StopDrawingPath()
    {
        StopAllCoroutines();
        //StopCoroutine(pathDrawRoutine);
        SetGameState(GameState.Drawing);
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        OnStateChanged?.Invoke(gameState);
        Debug.Log(gameState);
    }

    public void StartSearch(SearchRoutine searchRoutine)
    {
        if (start.IsNull())
        {
            errorSound.Play(0);
            uiManager.UpdateMessage("Missing start point", MessageType.Error);
            return;
        } else if (finish.IsNull())
        {
            errorSound.Play(0);
            uiManager.UpdateMessage("Missing end point", MessageType.Error);
            return;
        }
        SetGameState(GameState.Executing);
        uiManager.ClearErrorMessage();
        StopAllCoroutines();
        explorePath = searchRoutine.Search(grid, start, finish);
        DrawExploredTilesAsync();
        
    }

    public void SetFinalPath(Stack<Tile> path)
    {
        if (path.Count == 0)
        {
            errorSound.Play(0);
            uiManager.UpdateMessage("No possible path", MessageType.Error);
        }
        else 
        {
            uiManager.ClearErrorMessage();
            this.path = path; 
        }
    }

    private async void DrawExploredTilesAsync()
    {
        if (explorePath == null) return;

        for (int i = explorePath.Count; i > 0; i--)
        {
            Stack<Tile> tiles = explorePath.Dequeue();
            while (tiles.Count > 0)
            {
                Tile currTile = tiles.Pop();
                currTile.UpdateTileColour();
            }
            await Task.Delay(1);
        }
        uiManager.UpdateMessage("Finished drawing explored tiles", MessageType.Normal);
        DrawPath();
    }

    private void DrawPath()
    {
        if (path == null) return;

        while (path.Count > 0)
        {
            //check if time flag passed
            Tile currTile = path.Pop();
            currTile.ChangeTile(PenState.Path, true);
        }

        uiManager.UpdateMessage("Finished drawing path", MessageType.Normal);
        SetGameState(GameState.Drawing);
    }
}