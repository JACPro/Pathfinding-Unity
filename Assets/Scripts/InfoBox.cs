using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	[CreateAssetMenu(fileName = "Info Box", menuName = "Data/Info Box")]
	public class InfoBox : ScriptableObject
	{
		public string title;
		[TextArea(5, 20)] public string description;
	}
