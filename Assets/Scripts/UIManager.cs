using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum MessageType { Error, Warning, Normal }
public class UIManager : MonoBehaviour
{
	[Header("Pen Selection")]
	[SerializeField] GameObject[] borders;
	[ColorUsageAttribute(false, true)][SerializeField] Color activePen = new Color(0f, 207/255f, 1f, 129/255f);
	[ColorUsageAttribute(false, true)][SerializeField] Color inactivePen = new Color(0f, 0f, 0f, 65/255f);

	[Header("Info Menu")]
	[SerializeField] GameObject infoUI;
	[SerializeField] TextMeshProUGUI title, description;

	[Header("Search buttons")]
	[SerializeField] Button dfsButton;
	[SerializeField] Button bfsButton;
	[SerializeField] Button aStarButton;

	[Header("Error Box")]
	[SerializeField] TextMeshProUGUI errorMessage;
	MessageType messageType;

	public event Action<GameState> OnInfoUI;
	public void UpdatePenUI(PenState prevPen, PenState newPen)
	{
		borders[(int)prevPen].GetComponent<Image>().color = inactivePen;
		borders[(int)newPen].GetComponent<Image>().color = activePen;
	}

	public void UpdateMessage(string newMessage, MessageType messageType)
	{
		string messagePrefix = "";
		switch(messageType) 
		{
			case MessageType.Error:
				messagePrefix = "ERROR: ";
				errorMessage.faceColor = Color.red;
				break;
			case MessageType.Warning:
				messagePrefix = "WARNING: ";
				errorMessage.faceColor = Color.yellow;
				break;
			case MessageType.Normal:
				errorMessage.faceColor = Color.white;
				break;
		}
		errorMessage.text = messagePrefix + newMessage;
	}

	public void ShowInfoBox(InfoBox infoBox)
	{
		OnInfoUI?.Invoke(GameState.Menu);
		infoUI.SetActive(true);
		title.text = infoBox.title;
		description.text = infoBox.description;
	}

	public void HideInfoBox()
	{
		OnInfoUI?.Invoke(GameState.Drawing);
		infoUI.SetActive(false);
	}

	public void ClearErrorMessage()
	{
		errorMessage.text = "";
	}

	public void SetSearchButtonsActive(GameState gameState)
	{
		bool isActive = gameState == GameState.Drawing;
		bfsButton.interactable = isActive;
		dfsButton.interactable = isActive;
		aStarButton.interactable = isActive;
	}
}