using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenManager : MonoBehaviour
{
    PenState penState = PenState.Start;
    UIManager uiManager;

    private void Awake() {
        uiManager = FindObjectOfType<UIManager>();    
    }

    void Update()
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
        PenState prev = penState;

        penState += change;
        if ((int) penState > 3)
        {
            penState -= 4;
        } else if ((int) penState < 0)
        {
            penState += 4;
        }
        uiManager.UpdatePenUI(prev, penState);
    }
    public void ChangePenState(int penIndex)
    {
        PenState newPen = (PenState) penIndex;
        if (newPen == penState) return;
        uiManager.UpdatePenUI(penState, newPen);
        penState = newPen;
    }

    public PenState GetPenState()
    {
        return penState;
    }
}
