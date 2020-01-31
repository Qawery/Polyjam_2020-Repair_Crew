using UnityEngine;

namespace Polyjam2020
{
	public class UnitSelectionComponent : MonoBehaviour
	{
		public bool IsSelected { get; private set; }
		public event System.Action OnSelected;
		public event System.Action OnDeselected;

		public void Select()
		{
			IsSelected = true;
			OnSelected?.Invoke();
		}

		public void Deselect()
		{
			IsSelected = false;
			OnDeselected?.Invoke();
		}
	}
}
