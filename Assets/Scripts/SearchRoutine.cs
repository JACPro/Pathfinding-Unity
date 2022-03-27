using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SearchRoutine : MonoBehaviour
{
  protected GameManager _gameManager;

	private void Awake() 
  {
    _gameManager = FindObjectOfType<GameManager>();	
	}

    public void StartSearch()
    {
		  _gameManager.StartSearch(this);
    }
    
    public abstract Queue<Stack<Tile>> Search(GameObject[,] grid, Coordinates start, Coordinates finish);
}
