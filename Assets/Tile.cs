using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        ChangeTile();
    }

    void ChangeTile()
    {
        tileState = gameManager.GetPenState();
        material.color = colours[(int)tileState];
        if (tileState == PenState.Finish) 
            gameManager.SetFinish(coordinates);
        else if (tileState == PenState.Start) 
            gameManager.SetStart(coordinates);

        // switch (tileState) {
        //     case PenState.Erase :
        //         material.color = Color.white;
        //         break;
        //     case PenState.Wall :
        //         material.color = Color.black;
        //         break;
        //     case PenState.Start :
        //         gameManager.SetStart(coordinates);
        //         material.color = Color.green;
        //         break;
        //     case PenState.Finish :
        //         gameManager.SetFinish(coordinates);
        //         material.color = Color.red;
        //         break;
        // }
    }

    public void ChangeTile(PenState newState)
    {
        tileState = newState;
        material.color = colours[(int)newState];
    }

    public PenState GetTileState()
    {
        return tileState;
    }
}
