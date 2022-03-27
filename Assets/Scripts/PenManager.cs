using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PenState { Start, Finish, Wall, Erase, Path, Explored }
public class PenManager : MonoBehaviour
{
    private PenState _penState = PenState.Start;
    private UIManager _uiManager;

    private void Awake() 
    {
        _uiManager = FindObjectOfType<UIManager>();    
    }

    private void Update()
    {
        if(Input.GetButtonDown("Previous"))
        {
            ScrollPenState(-1);
        }
        else if(Input.GetButtonDown("Next"))
        {
            ScrollPenState(1);
        }
    }

    public void ScrollPenState(int change)
    {
        PenState prev = _penState;

        _penState += change;
        if ((int) _penState > 3)
        {
            _penState -= 4;
        } else if ((int) _penState < 0)
        {
            _penState += 4;
        }
        _uiManager.UpdatePenUI(prev, _penState);
    }
    public void ChangePenState(int penIndex)
    {
        PenState newPen = (PenState) penIndex;
        if (newPen == _penState) return;
        _uiManager.UpdatePenUI(_penState, newPen);
        _penState = newPen;
    }

    public PenState GetPenState()
    {
        return _penState;
    }
}
