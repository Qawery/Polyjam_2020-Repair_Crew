using System;
using UnityEngine;
using UnityEngine.UI;

namespace Polyjam2020
{
	public class SelectedUnitPanel : MonoBehaviour
	{
		[SerializeField] private Text nameText;
		private void Awake()
		{
			var unitController = FindObjectOfType<UnitController>();
			unitController.OnSelectedUnitChanged += OnSelectedUnitChanged;
			gameObject.SetActive(false);
		}

		private void OnSelectedUnitChanged(Unit unit)
		{
			if (unit != null)
			{
				nameText.text = unit.name + "; Class: " + unit.UnitClass.DisplayName;
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
}
