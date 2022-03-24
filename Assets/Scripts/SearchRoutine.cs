using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SearchRoutine : MonoBehaviour
{
    protected GameManager gameManager;

	private void Awake() {
		gameManager = FindObjectOfType<GameManager>();	
	}

    public void StartSearch()
    {
		gameManager.StartSearch(this);
    }
    public abstract Queue<Stack<Tile>> Search(GameObject[,] grid, Coordinates start, Coordinates finish);
}
