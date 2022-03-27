using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoButton : MonoBehaviour
{
    private SearchRoutine _searchRoutine;
    private Button _button;
    private GameManager _gameManager;
    
    private void Awake() 
    {
        _searchRoutine = GetComponent<SearchRoutine>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(delegate{_searchRoutine.StartSearch();});
    }
}
