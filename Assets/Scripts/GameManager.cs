using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public enum GameState { Drawing, Executing, Menu }
public class GameManager : MonoBehaviour
{
    [SerializeField] private DrawGrid _gridManager;
    [SerializeField] private PenManager _penManager;
    private UIManager _uiManager;
    private GameObject[,] _grid;
    private Coordinates _start = new Coordinates(), _finish = new Coordinates();
    private AudioSource _errorSound;
    private Stack<Tile> _path;
    private Queue<Stack<Tile>> _explorePath;
    private GameState _gameState = GameState.Drawing;
    [SerializeField] private int _drawSpeed = 1;
    public event Action<GameState> OnStateChanged;

    private void Awake() 
    {
        _uiManager = FindObjectOfType<UIManager>();
        _errorSound = GetComponent<AudioSource>();
        _uiManager.OnInfoUI += SetGameState;
        OnStateChanged += _uiManager.SetSearchButtonsActive;
    }

    private void _Start()
    {
        _grid = _gridManager.DrawTiles();
    }

    public Coordinates GetStart() => _start;
    public Coordinates GetFinish() => _finish;
    public GameObject[,] GetGrid() => _grid;
    public GameState GetGameState() => _gameState;
    public PenState GetPenState() => _penManager.GetPenState();

    public void SetStart(Coordinates new_Start)
    {
        if (_start == new_Start) return;
        if (new_Start == _finish) _finish = Coordinates._empty;

        if (!_start.IsNull()) 
        {
            Tile prev_Start = _grid[_start._x, _start._y].GetComponent<Tile>();
            if (prev_Start.GetTileState() == PenState.Start)
            {
                prev_Start.ChangeTile(PenState.Erase, true);
            }
        }
        _start = new_Start;
    }

    public void SetFinish(Coordinates new_Finish)
    {
        if (_finish == new_Finish) return;
        if (new_Finish == _start) _start = Coordinates._empty;

        if (!_finish.IsNull()) 
        {
            Tile prev_Finish = _grid[_finish._x, _finish._y].GetComponent<Tile>();
            if (prev_Finish.GetTileState() == PenState.Finish)
            {
                prev_Finish.ChangeTile(PenState.Erase, true);
            }
        }
        _finish = new_Finish;
    }

    public void ClearTiles()
    {
        StopDrawingPath();
        _gridManager.GetEmptyGrid(_grid);
        _start = _finish = Coordinates._empty;
    }

    public void StopDrawingPath()
    {
        StopAllCoroutines();
        //StopCoroutine(_pathDrawRoutine);
        SetGameState(GameState.Drawing);
    }

    public void SetGameState(GameState _gameState)
    {
        this._gameState = _gameState;
        OnStateChanged?.Invoke(_gameState);
        Debug.Log(_gameState);
    }

    public void StartSearch(SearchRoutine searchRoutine)
    {
        if (_start.IsNull())
        {
            _errorSound.Play(0);
            _uiManager.UpdateMessage("Missing _start point", MessageType.Error);
            return;
        } else if (_finish.IsNull())
        {
            _errorSound.Play(0);
            _uiManager.UpdateMessage("Missing end point", MessageType.Error);
            return;
        }
        SetGameState(GameState.Executing);
        _uiManager.ClearErrorMessage();
        StopAllCoroutines();
        _explorePath = searchRoutine.Search(_grid, _start, _finish);
        DrawExploredTilesAsync();
        
    }

    public void SetFinalPath(Stack<Tile> _path)
    {
        if (_path.Count == 0)
        {
            _errorSound.Play(0);
            _uiManager.UpdateMessage("No possible _path", MessageType.Error);
        }
        else 
        {
            _uiManager.ClearErrorMessage();
            this._path = _path; 
        }
    }

    private async void DrawExploredTilesAsync()
    {
        if (_explorePath == null) return;

        for (int i = _explorePath.Count; i > 0; i--)
        {
            Stack<Tile> tiles = _explorePath.Dequeue();
            while (tiles.Count > 0)
            {
                Tile currTile = tiles.Pop();
                currTile.UpdateTileColour();
            }
            await Task.Delay(1);
        }
        _uiManager.UpdateMessage("_Finished drawing explored tiles", MessageType.Normal);
        Draw_Path();
    }

    private void Draw_Path()
    {
        if (_path == null) return;

        while (_path.Count > 0)
        {
            //check if time flag passed
            Tile currTile = _path.Pop();
            currTile.ChangeTile(PenState.Path, true);
        }

        _uiManager.UpdateMessage("_Finished drawing _path", MessageType.Normal);
        SetGameState(GameState.Drawing);
    }
}