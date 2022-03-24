using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    GameManager gameManager;
    PenState tileState = PenState.Erase;
    Material material;
    Color[] colours = {Color.green, Color.red, Color.black, Color.white, Color.blue, Color.yellow};

    Coordinates coordinates = new Coordinates();

    public void SetCoordinates(int x, int y)
    {
        coordinates.x = x;
        coordinates.y = y;
    }

    public Coordinates GetCoordinates()
    {
        return coordinates;
    }

    void Start()
    {
        material = GetComponent<Renderer>().material;
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnMouseEnter() {
        //draw when mouse held down only if wall or clear
        if (Input.GetMouseButton(0) && (int) gameManager.GetPenState() > 1)
        {
            ChangeTile();
        }
    }

    private void OnMouseDown() {
        if (!EventSystem.current.IsPointerOverGameObject())
            ChangeTile();
    }

    void ChangeTile()
    {
        if (gameManager.GetGameState() != GameState.Drawing) return;
        
        tileState = gameManager.GetPenState();
        material.color = colours[(int)tileState];
        if (tileState == PenState.Finish) 
            gameManager.SetFinish(coordinates);
        else if (tileState == PenState.Start) 
            gameManager.SetStart(coordinates);
    }

    public void ChangeTile(PenState newState, bool updateColour)
    {
        tileState = newState;
        if (updateColour) UpdateTileColour();
    }

    public void UpdateTileColour()
    {
        material.color = colours[(int)tileState];
    }

    public PenState GetTileState()
    {
        return tileState;
    }
}
