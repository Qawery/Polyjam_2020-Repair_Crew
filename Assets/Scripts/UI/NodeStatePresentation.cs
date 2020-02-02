using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class NodeStatePresentation : MonoBehaviour
	{
		[SerializeField] private GameObject livingCityElements = null;
		[SerializeField] private GameObject fires = null;
		[SerializeField] private GameObject ruins = null;
		private SelectionCircle selectionCircle = null;
		private Node node = null;
		private HealthComponent healthComponent = null;


		private void Awake()
		{
			selectionCircle = GetComponentInChildren<SelectionCircle>();
			Assert.IsNotNull(livingCityElements);
			Assert.IsNotNull(fires);
			Assert.IsNotNull(ruins);
			Assert.IsNotNull(selectionCircle);
			livingCityElements.SetActive(true);
			fires.SetActive(false);
			ruins.SetActive(false);
			node = GetComponent<Node>();
			Assert.IsNotNull(node);
			node.OnStatusReceived += OnStatusReceived;
			node.OnStatusRemoved += OnStatusRemoved;
			healthComponent = node.gameObject.GetComponent<HealthComponent>();
			Assert.IsNotNull(healthComponent);
			healthComponent.OnValueChanged += OnHealthChanged;
			selectionCircle.gameObject.SetActive(false);
			var nodeController = FindObjectOfType<NodeController>();
			Assert.IsNotNull(nodeController);
			nodeController.OnSelectedNodeChanged += OnSelectedNodeChanged;
		}

		private void OnHealthChanged((int previous, int current) values)
		{
			if (values.current <= 0)
			{
				livingCityElements.SetActive(false);
				ruins.SetActive(true);
			}
		}

		private void OnStatusReceived(NodeStatus status)
		{
			if (status is FireStatus)
			{
				fires.SetActive(true);
			}
			//TODO: pozostałe wizualizacje statusów
		}

		private void OnStatusRemoved(NodeStatus status)
		{
			if (status is FireStatus)
			{
				fires.SetActive(false);
			}
			//TODO: pozostałe wizualizacje statusów
		}

		private void OnSelectedNodeChanged(Node selectedNode)
		{
			selectionCircle.gameObject.SetActive(selectedNode == node);
		}
	}
}