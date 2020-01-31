using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Polyjam2020.Tests
{
	public class MovementTargetAssigner : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
	{
		private UnitSelectionComponent selectionComponent;

		private void Awake()
		{
			selectionComponent = GetComponentInChildren<UnitSelectionComponent>();
			Assert.IsNotNull(selectionComponent, $"SelectionComponent is missing on {name}. It is required by CursorUnitSelection");
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

		private void Update()
		{
			throw new NotImplementedException();
		}
	}
}