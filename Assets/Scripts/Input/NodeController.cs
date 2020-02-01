using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class NodeController : MonoBehaviour
	{
		[SerializeField] private LayerMask nodeLayers;
		private List<Node> nodes = new List<Node>();
		private Node selectedNode = null;
		private UnitController unitController;
		public event System.Action<Node> OnSelectedNodeChanged;


		public Node SelectedNode
		{
			get => selectedNode;
			private set
			{
				if (selectedNode != null && selectedNode != value)
				{
					selectedNode.GetComponent<NodeSelectionComponent>().Deselect();
				}				
				selectedNode = value;				
				if (selectedNode != value)
				{
					selectedNode.GetComponent<NodeSelectionComponent>().Select();
				}				
				OnSelectedNodeChanged?.Invoke(selectedNode);
			}
		}


		[SpawnHandlerMethod]
		private void OnNodeSpawned(Node spawnedNode)
		{
			var selectionComponent = spawnedNode.GetComponent<NodeSelectionComponent>();
			selectionComponent.OnSelected += OnNodeSelected;
			selectionComponent.OnDeselected += OnNodeDeselected;
			nodes.Add(spawnedNode);
		}

		[DestroyHandlerMethod]
		private void OnSelectableUnitDestroyed(Node destroyedNode)
		{
			nodes.Remove(destroyedNode);
		}

		private void OnNodeSelected(Node node)
		{
			foreach (var registeredNode in nodes)
			{
				if (registeredNode != node)
				{
					registeredNode.GetComponent<UnitSelectionComponent>().Deselect();
				}
			}
			selectedNode = node;
		}

		private void OnNodeDeselected(Node node)
		{
			if (node == SelectedNode)
			{
				selectedNode = null;
			}
		}

		private void Awake()
		{
			unitController = FindObjectOfType<UnitController>();
			unitController.OnSelectedUnitChanged += unit =>
			{
				if (unit != null)
				{
					SelectedNode = null;
				}
			};
		}

		private void Update()
		{
			ProcessNodeControls();
		}

		private void ProcessNodeControls()
		{
			if (unitController.SelectedUnit != null)
			{
				return;
			}
			if (SelectedNode != null)
			{
				if (Input.GetKeyDown(KeyCode.Mouse1))
				{
					SelectedNode = null;
					return;
				}
			}
			if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				var selectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(selectionRay, out var hit, 100, nodeLayers))
				{
					var node = hit.collider.GetComponent<Node>();
					Assert.IsNotNull(node,
						$"There is no Node component on {hit.collider.gameObject.name} and yet it's on Node physics layer");

					if (node == SelectedNode)
					{
						SelectedNode = null;
					}
					else
					{
						SelectedNode = node;
					}
				}
			}
		}
	}
}
