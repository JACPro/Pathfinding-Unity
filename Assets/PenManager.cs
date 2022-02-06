using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenManager : MonoBehaviour
{
    [SerializeField] GameObject[] borders;
    PenState penState = PenState.Start;
    [ColorUsageAttribute(false, true)][SerializeField] Color activePen = new Color(0f, 207/255f, 1f, 129/255f);
    [ColorUsageAttribute(false, true)][SerializeField] Color inactivePen = new Color(0f, 0f, 0f, 65/255f);

    void Update()
    {
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            ScrollPenState(-1);
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0)
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
        UpdateUI(prev, penState);
    }
    public void ChangePenState(int penIndex)
    {
        PenState newPen = (PenState) penIndex;
        if (newPen == penState) return;
        UpdateUI(penState, newPen);
        penState = newPen;
    }

    public PenState GetPenState()
    {
        return penState;
    }

    private void UpdateUI(PenState prevPen, PenState newPen)
    {
        borders[(int)prevPen].GetComponent<Image>().color = inactivePen;
        borders[(int)newPen].GetComponent<Image>().color = activePen;
    }

}
