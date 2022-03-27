using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    private GameManager _gameManager;
    private PenState _tileState = PenState.Erase;
    private Material _material;
    private Color[] _colours = { Color.green, Color.red, Color.black, Color.white, Color.blue, Color.yellow };

    private Coordinates coordinates = new Coordinates();

    public void SetCoordinates(int x, int y)
    {
        coordinates._x = x;
        coordinates._y = y;
    }

    public Coordinates GetCoordinates()
    {
        return coordinates;
    }

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnMouseEnter() 
    {
        //draw when mouse held down only if wall or clear
        if (Input.GetMouseButton(0) && (int) _gameManager.GetPenState() > 1)
        {
            ChangeTile();
        }
    }

    private void OnMouseDown() 
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            ChangeTile();
    }

    private void ChangeTile()
    {
        if (_gameManager.GetGameState() != GameState.Drawing) return;
        
        _tileState = _gameManager.GetPenState();
        _material.color = _colours[(int)_tileState];
        if (_tileState == PenState.Finish) 
            _gameManager.SetFinish(coordinates);
        else if (_tileState == PenState.Start) 
            _gameManager.SetStart(coordinates);
    }

    public void ChangeTile(PenState newState, bool updateColour)
    {
        _tileState = newState;
        if (updateColour) UpdateTileColour();
    }

    public void UpdateTileColour()
    {
        _material.color = _colours[(int)_tileState];
    }

    public PenState GetTileState()
    {
        return _tileState;
    }
}
