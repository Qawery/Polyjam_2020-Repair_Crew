using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class Unit : MonoBehaviour
	{
		public const float HEAL_AMMOUNT = 1.0f;
		private Node nodeUnderEffect = null;


		public Node NodeUnderEffect => nodeUnderEffect;


		private void Awake()
		{
			var trigger = GetComponent<Collider>();
			Assert.IsNotNull(trigger, "Missing collider component on: " + gameObject.name);
			Assert.IsTrue(trigger.isTrigger, "Collider is not trigger on: " + gameObject.name);
			Assert.IsTrue(HEAL_AMMOUNT > 0.0f);
		}

		private void Update()
		{
			if (nodeUnderEffect != null)
			{
				nodeUnderEffect.ApplyHeal(HEAL_AMMOUNT * Time.deltaTime);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			var otherNode = other.GetComponent<Node>();
			if (otherNode != null)
			{
				Assert.IsNull(nodeUnderEffect, "NodeUnderEffect already present on: " + gameObject.name);
				nodeUnderEffect = otherNode;
			}
		}

		private void OnTriggerExit(Collider other)
		{
			var otherNode = other.GetComponent<Node>();
			if (otherNode != null)
			{
				Assert.IsTrue(nodeUnderEffect == otherNode, "Exiting trigger with node different than nodeUnderEffect on: " + gameObject.name);
				nodeUnderEffect = null; ;
			}
		}
	}
}