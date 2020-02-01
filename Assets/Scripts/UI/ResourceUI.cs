using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Polyjam2020
{
	public class ResourceUI : MonoBehaviour
	{
		[SerializeField] private Text resourceText;

		private void Awake()
		{
			FindObjectOfType<ResourceManager>().OnResourceAmountChanged += OnResourceAmountChanged;
		}

		private void OnResourceAmountChanged((int previous, int current) resourceChangeData)
		{
			resourceText.text = $"Resources: {resourceChangeData.current}";
		}
	}
}