using UnityEngine;
using UnityEngine.Assertions;

namespace Polyjam2020
{
	public class NodeSelectionComponent : MonoBehaviour
	{
		public bool IsSelected { get; private set; }
		public event System.Action<Node> OnSelected;
		public event System.Action<Node> OnDeselected;

		private Node node;

		private void Awake()
		{
			node = GetComponent<Node>();
			Assert.IsNotNull(node, $"Missing Node component on {name} that has {nameof(NodeSelectionComponent)}");
		}

		public void Select()
		{
			IsSelected = true;
			OnSelected?.Invoke(node);
		}

		public void Deselect()
		{
			IsSelected = false;
			OnDeselected?.Invoke(node);
		}
	}
}
