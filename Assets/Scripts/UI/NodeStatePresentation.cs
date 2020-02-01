using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace Polyjam2020
{
	public class NodeStatePresentation : MonoBehaviour
	{
		[SerializeField] private GameObject livingElements = null;
		[SerializeField] private GameObject fires = null;
		[SerializeField] private GameObject rubble = null;
		private Node node = null;
		private HealthComponent healthComponent = null;


		private void Awake()
		{
			Assert.IsNotNull(livingElements);
			Assert.IsNotNull(fires);
			Assert.IsNotNull(rubble);
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
	}
}