using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class Unit : MonoBehaviour
	{
		[SerializeField] private UnitClass unitClass;
		private Node nodeUnderEffect = null;
		public Node NodeUnderEffect => nodeUnderEffect;
		public UnitClass UnitClass => unitClass;

		public event System.Action<Node> OnEnteredNode;
		public event System.Action<Node> OnLeftNode;

		private void Awake()
		{
			var trigger = GetComponent<Collider>();
			Assert.IsNotNull(trigger, "Missing collider component on: " + gameObject.name);
			Assert.IsTrue(trigger.isTrigger, "Collider is not trigger on: " + gameObject.name);
		}

		private void OnTriggerEnter(Collider other)
		{
			var otherNode = other.GetComponent<Node>();
			if (otherNode != null)
			{
				Assert.IsNull(nodeUnderEffect, "NodeUnderEffect already present on: " + gameObject.name);
				nodeUnderEffect = otherNode;
				OnEnteredNode?.Invoke(nodeUnderEffect);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			var otherNode = other.GetComponent<Node>();
			if (otherNode != null)
			{
				Assert.IsTrue(nodeUnderEffect == otherNode, "Exiting trigger with node different than nodeUnderEffect on: " + gameObject.name);
				OnLeftNode?.Invoke(nodeUnderEffect);
				nodeUnderEffect = null;
			}
		}
	}
}