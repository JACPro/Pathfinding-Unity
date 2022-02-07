using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
	{
		[SerializeField] GameObject[] borders;
		[ColorUsageAttribute(false, true)][SerializeField] Color activePen = new Color(0f, 207/255f, 1f, 129/255f);
		[ColorUsageAttribute(false, true)][SerializeField] Color inactivePen = new Color(0f, 0f, 0f, 65/255f);
		[SerializeField] Text errorMessage;

		public void UpdatePenUI(PenState prevPen, PenState newPen)
   		{
			borders[(int)prevPen].GetComponent<Image>().color = inactivePen;
			borders[(int)newPen].GetComponent<Image>().color = activePen;
		}

		public void UpdateErrorMessage(string newMessage)
		{
			errorMessage.text = "ERROR: " + newMessage;
		}

		public void ClearErrorMessage()
		{
			errorMessage.text = "";
		}
	}
