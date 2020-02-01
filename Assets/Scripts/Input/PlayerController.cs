using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Polyjam2020
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] private LayerMask movementTargetLayers;

		private List<Unit> selectableUnits = new List<Unit>();
		private Unit selectedUnit = null;
		private bool isFirstClickProcessed = false;

		[SpawnHandlerMethod]
		private void OnSelectableUnitSpawned(UnitSelectionComponent selectionComponent)
		{
			selectionComponent.OnSelected += OnUnitSelected;
			selectionComponent.OnDeselected += OnUnitDeselected;
			var unit = selectionComponent.GetComponent<Unit>();
			Assert.IsNotNull(unit,
				$"Missing Unit component on {selectionComponent.gameObject.name} that has UnitSelectionComponent");
			selectableUnits.Add(unit);
		}

		[DestroyHandlerMethod]
		private void OnSelectableUnitDestroyed(Unit unit)
		{
			selectableUnits.Remove(unit);
		}

		private void OnUnitSelected(Unit unit)
		{
			foreach (var selectableUnit in selectableUnits)
			{
				if (selectableUnit != unit)
				{
					selectableUnit.GetComponent<UnitSelectionComponent>().Deselect();
				}
			}

			selectedUnit = unit;
			isFirstClickProcessed = false;
		}

		private void OnUnitDeselected(Unit unit)
		{
			if (unit == selectedUnit)
			{
				selectedUnit = null;
			}
		}

		private void Update()
		{
			if (selectedUnit == null)
			{
				return;
			}

			ProcessMovementOrders();
		}

		private void ProcessMovementOrders()
		{
			if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				var movement = selectedUnit.GetComponent<UnitMovementController>();
				Assert.IsNotNull(movement, $"Missing movement controller on {selectedUnit.name}");

				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out var hit, 100, movementTargetLayers))
				{
					var sourceNode = selectedUnit.NodeUnderEffect;
					if (sourceNode != null)
					{
						var targetNode = hit.collider.GetComponent<Node>();
						Assert.IsNotNull(targetNode, $"There is no node component on {hit.collider.gameObject.name} and yet it's on Nodes physics layer");

						if (sourceNode.Edges.Exists(edge =>
							edge.Nodes.first == targetNode || edge.Nodes.second == targetNode))
						{
							movement.MoveToPoint(hit.collider.transform.position);
						}
						else
						{
							Debug.Log($"No direct edge from {sourceNode.name} to {targetNode.name}. Can't perform movement action");
						}
					}
				}
			}
		}
	}
}
