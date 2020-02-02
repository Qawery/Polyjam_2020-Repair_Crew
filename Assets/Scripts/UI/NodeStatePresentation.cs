using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class NodeStatePresentation : MonoBehaviour
	{
		[SerializeField] private GameObject livingElements = null;
		[SerializeField] private GameObject fires = null;
		[SerializeField] private GameObject rubble = null;
		private SelectionCircle selectionCircle = null;
		private Node node = null;
		private HealthComponent healthComponent = null;


		private void Awake()
		{
			selectionCircle = GetComponentInChildren<SelectionCircle>();
			Assert.IsNotNull(livingElements);
			Assert.IsNotNull(fires);
			Assert.IsNotNull(rubble);
			Assert.IsNotNull(selectionCircle);
			livingElements.SetActive(true);
			fires.SetActive(false);
			rubble.SetActive(false);
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
				livingElements.SetActive(false);
				rubble.SetActive(true);
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