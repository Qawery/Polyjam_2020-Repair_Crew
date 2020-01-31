using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Polyjam2020.Tests
{
	public class UnitMouseSelection : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
	{
		private UnitSelectionComponent selectionComponent;

		private void Awake()
		{
			selectionComponent = GetComponentInChildren<UnitSelectionComponent>();
			Assert.IsNotNull(selectionComponent, $"SelectionComponent is missing on {name}. It is required by {nameof(UnitMouseSelection)}");
		}

		public void OnPointerDown(PointerEventData eventData)
		{
		}

		public void OnPointerUp(PointerEventData eventData)
		{
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (!selectionComponent.IsSelected)
			{
				selectionComponent.Select();
				Debug.Log($"{name} selected");
			}
			else
			{
				selectionComponent.Deselect();
				Debug.Log($"{name} selected");
			}
		}
	}
}