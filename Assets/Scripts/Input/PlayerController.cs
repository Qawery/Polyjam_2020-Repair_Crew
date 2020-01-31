using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Polyjam2020
{
	public class PlayerController : MonoBehaviour
	{
		private List<UnitSelectionComponent> selectableUnits = new List<UnitSelectionComponent>();
		private GameObject selectedUnitObject = null;
		private bool isFirstClickProcessed = false;

		[SpawnHandlerMethod]
		private void OnSelectableUnitSpawned(UnitSelectionComponent selectionComponent)
		{
			selectionComponent.OnSelected += OnUnitSelected;
			selectionComponent.OnDeselected += OnUnitDeselected;
			selectableUnits.Add(selectionComponent);
		}
		
		[DestroyHandlerMethod]
		private void OnSelectableUnitDestroyed(UnitSelectionComponent selectionComponent)
		{
			selectableUnits.Remove(selectionComponent);
		}
		
		private void OnUnitSelected(UnitSelectionComponent selectionComponent)
		{
			foreach (var selectable in selectableUnits)
			{
				if (selectable != selectionComponent)
				{
					selectable.Deselect();
				}
			}

			selectedUnitObject = selectionComponent.gameObject;
			isFirstClickProcessed = false;
		}

		private void OnUnitDeselected(UnitSelectionComponent selectionComponent)
		{
			if (selectionComponent.gameObject == selectedUnitObject)
			{
				selectedUnitObject = null;
			}
		}

		private void Update()
		{
			if (selectedUnitObject == null)
			{
				return;
			}

			if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				if (!isFirstClickProcessed)
				{
					isFirstClickProcessed = true;
				}
				else
				{
					var movement = selectedUnitObject.GetComponent<UnitMovementController>();
					Assert.IsNotNull(movement, $"Missing movement controller on {selectedUnitObject.name}");

					var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					if (Physics.Raycast(ray, out var hit))
					{
						movement.MoveToPoint(hit.point);
					}
				}
			}
		}
	}
}
