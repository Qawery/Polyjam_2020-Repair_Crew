using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Polyjam2020
{
	public class UnitController : MonoBehaviour
	{
		[SerializeField] private LayerMask movementTargetLayers;

		private List<Unit> selectableUnits = new List<Unit>();
		private Unit selectedUnit = null;

		private Unit SelectedUnit
		{
			get => selectedUnit;
			set
			{
				selectedUnit = value;
				OnSelectedUnitChanged?.Invoke(selectedUnit);
			}
		}

		public event System.Action<Unit> OnSelectedUnitChanged;
		
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

			SelectedUnit = unit;
		}

		private void OnUnitDeselected(Unit unit)
		{
			if (unit == SelectedUnit)
			{
				SelectedUnit = null;
			}
		}

		private void Update()
		{
			if (SelectedUnit != null)
			{
				ProcessUnitOrders();
			}
		}

		private void ProcessUnitOrders()
		{
			if (Input.GetKeyDown(KeyCode.Mouse1))
			{
				SelectedUnit.GetComponent<UnitSelectionComponent>().Deselect();
				return;
			}
			
			if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				var movement = SelectedUnit.GetComponent<UnitMovementController>();
				Assert.IsNotNull(movement, $"Missing movement controller on {SelectedUnit.name}");

				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out var hit, 100, movementTargetLayers))
				{
					var sourceNode = SelectedUnit.NodeUnderEffect;
					if (sourceNode != null)
					{
						var targetNode = hit.collider.GetComponent<Node>();
						Assert.IsNotNull(targetNode, $"There is no node component on {hit.collider.gameObject.name} and yet it's on Nodes physics layer");

						if (targetNode == sourceNode)
						{
							return;
						}
						
						if (sourceNode.Edges.Exists(edge => edge.Nodes.first == targetNode || edge.Nodes.second == targetNode))
						{
							var slot = targetNode.UnitSlots.Find(candidate => !candidate.IsOccupied);
							if (slot != null)
							{
								movement.MoveToPoint(slot.transform.position);
							}
							else
							{
								Debug.Log($"No free slots on {targetNode}. Can't perform movement action");
							}
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
