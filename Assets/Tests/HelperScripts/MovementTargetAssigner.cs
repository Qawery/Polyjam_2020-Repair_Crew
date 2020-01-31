using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Polyjam2020.Tests
{
	public class MovementTargetAssigner : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
	{
		private UnitSelectionComponent selectionComponent;
		private UnitMovementController movementController;

		private void Awake()
		{
			selectionComponent = GetComponentInChildren<UnitSelectionComponent>();
			Assert.IsNotNull(selectionComponent, $"SelectionComponent is missing on {name}. It is required by {nameof(MovementTargetAssigner)}");

			movementController = GetComponent<UnitMovementController>();
			Assert.IsNotNull(movementController, $"MovementController is missing on {name}. It is required by {nameof(MovementTargetAssigner)}");
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			Debug.Log($"{name} pointer down");
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			Debug.Log($"{name} pointer up");
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
			if (Input.GetKeyDown(KeyCode.Mouse0) && selectionComponent.IsSelected)
			{
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out var hit))
				{
					Debug.Log($"Assigning movement target {hit.point}");
					movementController.MoveToPoint(hit.point);
				}
			}
		}
	}
}