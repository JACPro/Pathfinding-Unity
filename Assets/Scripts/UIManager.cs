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
	[SerializeField] private GameObject[] _borders;
	[ColorUsageAttribute(false, true)][SerializeField] private Color _activePen = new Color(0f, 207/255f, 1f, 129/255f);
	[ColorUsageAttribute(false, true)][SerializeField] private Color _inactivePen = new Color(0f, 0f, 0f, 65/255f);

	[Header("Info Menu")]
	[SerializeField] private GameObject _infoUI;
	[SerializeField] private TextMeshProUGUI _title, _description;

	[Header("Search buttons")]
	[SerializeField] private Button _dfsButton;
	[SerializeField] private Button _bfsButton;
	[SerializeField] private Button _aStarButton;

	[Header("Error Box")]
	[SerializeField] private TextMeshProUGUI _errorMessage;
	private MessageType _messageType;

	public event Action<GameState> OnInfoUI;

	public void UpdatePenUI(PenState prevPen, PenState newPen)
	{
		_borders[(int)prevPen].GetComponent<Image>().color = _inactivePen;
		_borders[(int)newPen].GetComponent<Image>().color = _activePen;
	}

	public void UpdateMessage(string newMessage, MessageType _messageType)
	{
		string messagePrefix = "";
		switch(_messageType) 
		{
			case MessageType.Error:
				messagePrefix = "ERROR: ";
				_errorMessage.faceColor = Color.red;
				break;
			case MessageType.Warning:
				messagePrefix = "WARNING: ";
				_errorMessage.faceColor = Color.yellow;
				break;
			case MessageType.Normal:
				_errorMessage.faceColor = Color.white;
				break;
		}
		_errorMessage.text = messagePrefix + newMessage;
	}

	public void ShowInfoBox(InfoBox infoBox)
	{
		OnInfoUI?.Invoke(GameState.Menu);
		_infoUI.SetActive(true);
		_title.text = infoBox._title;
		_description.text = infoBox._description;
	}

	public void HideInfoBox()
	{
		OnInfoUI?.Invoke(GameState.Drawing);
		_infoUI.SetActive(false);
	}

	public void ClearErrorMessage()
	{
		_errorMessage.text = "";
	}

	public void SetSearchButtonsActive(GameState gameState)
	{
		bool isActive = gameState == GameState.Drawing;
		_bfsButton.interactable = isActive;
		_dfsButton.interactable = isActive;
		_aStarButton.interactable = isActive;
	}
}