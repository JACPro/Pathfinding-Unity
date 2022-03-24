using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoButton : MonoBehaviour
{
    SearchRoutine searchRoutine;
    Button button;
    GameManager gameManager;
    
    private void Awake() {
        searchRoutine = GetComponent<SearchRoutine>();
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate{searchRoutine.StartSearch();});
    }
}
