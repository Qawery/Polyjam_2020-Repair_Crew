using UnityEngine;

namespace Polyjam2020
{
	public class UnitSelectionComponent : MonoBehaviour
	{
		public bool IsSelected { get; private set; }
		public event System.Action<UnitSelectionComponent> OnSelected;
		public event System.Action<UnitSelectionComponent> OnDeselected;

		public void Select()
		{
			IsSelected = true;
			OnSelected?.Invoke(this);
		}

		public void Deselect()
		{
			IsSelected = false;
			OnDeselected?.Invoke(this);
		}
	}
}
