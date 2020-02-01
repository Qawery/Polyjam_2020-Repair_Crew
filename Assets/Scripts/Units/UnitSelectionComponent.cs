using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Polyjam2020
{
	public class UnitSelectionComponent : MonoBehaviour
	{
		public bool IsSelected { get; private set; }
		public event System.Action<Unit> OnSelected;
		public event System.Action<Unit> OnDeselected;

		private Unit unit;

		private void Awake()
		{
			unit = GetComponent<Unit>();
			Assert.IsNotNull(unit, $"Missing Unit component on {name} that has UnitSelectionComponent");
		}

		public void Select()
		{
			IsSelected = true;
			OnSelected?.Invoke(unit);
		}

		public void Deselect()
		{
			IsSelected = false;
			OnDeselected?.Invoke(unit);
		}
	}
}
