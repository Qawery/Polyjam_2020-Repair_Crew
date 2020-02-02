using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class UnitSelectionComponent : MonoBehaviour
	{
		private SelectionCircle selectionCircle = null;
		private Unit unit = null;
		public event System.Action<Unit> OnSelected;
		public event System.Action<Unit> OnDeselected;


		public bool IsSelected { get; private set; }


		private void Awake()
		{
			selectionCircle = GetComponentInChildren<SelectionCircle>();
			Assert.IsNotNull(selectionCircle);
			selectionCircle.gameObject.SetActive(false);
			unit = GetComponent<Unit>();
			Assert.IsNotNull(unit, $"Missing Unit component on {name} that has UnitSelectionComponent");
		}

		public void Select()
		{
			IsSelected = true;
			selectionCircle.gameObject.SetActive(true);
			OnSelected?.Invoke(unit);
		}

		public void Deselect()
		{
			IsSelected = false;
			selectionCircle.gameObject.SetActive(false);
			OnDeselected?.Invoke(unit);
		}
	}
}
