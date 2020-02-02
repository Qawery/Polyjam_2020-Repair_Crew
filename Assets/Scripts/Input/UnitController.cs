using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class UnitController : MonoBehaviour
	{
		[SerializeField] private LayerMask movementTargetLayers;
		[SerializeField] private LayerMask unitClickLayer;
		private List<Unit> selectableUnits = new List<Unit>();
		private Unit selectedUnit = null;
		public event System.Action<Unit> OnSelectedUnitChanged;


		public Unit SelectedUnit
		{
			get => selectedUnit;
			private set
			{
				if (selectedUnit != null && selectedUnit != value)
				{
					selectedUnit.GetComponent<UnitSelectionComponent>().Deselect();
				}				
				selectedUnit = value;				
				if (selectedUnit != null)
				{
					selectedUnit.GetComponent<UnitSelectionComponent>().Select();
				}				
				OnSelectedUnitChanged?.Invoke(selectedUnit);
			}
		}

		
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
		}

		private void OnUnitDeselected(Unit unit)
		{
			if (unit == SelectedUnit)
			{
				selectedUnit = null;
			}
		}

		private void Update()
		{
			ProcesUnitControls();
		}

		private void ProcesUnitControls()
		{
			if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				var unitSelectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(unitSelectionRay, out var unitHit, 100, unitClickLayer))
				{
					var unit = unitHit.collider.GetComponent<Unit>();
					Assert.IsNotNull(unit,
						$"There is no Unit component on {unitHit.collider.gameObject.name} and yet it's on Units physics layer");

					if (unit == SelectedUnit)
					{
						SelectedUnit = null;
					}
					else
					{
						SelectedUnit = unit;
					}
					return;
				}
			}

			if (SelectedUnit != null)
			{
				if (Input.GetKeyDown(KeyCode.Mouse1))
				{
					SelectedUnit = null;
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
							Assert.IsNotNull(targetNode,
								$"There is no node component on {hit.collider.gameObject.name} and yet it's on Nodes physics layer");

							if (targetNode == sourceNode)
							{
								return;
							}

							if (sourceNode.Edges.Exists(edge =>
								edge.Nodes.first == targetNode || edge.Nodes.second == targetNode))
							{
								var slot = targetNode.UnitSlots.Find(candidate => !candidate.IsOccupied);
								if (slot != null)
								{
									movement.MoveToPoint(slot.transform.position);
									slot.IsReserved = true;
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
}
