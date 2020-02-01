using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Polyjam2020
{
	public class StatusApplier : MonoBehaviour
	{
		[SerializeField] private float fireStartInterval = 5.0f;
		private float fireStartCooldown;
		private Graph graph;
		
		private void Awake()
		{
			graph = FindObjectOfType<Graph>();
			fireStartCooldown = fireStartInterval;
		}

		void Update()
		{
			if (fireStartCooldown > 0.0f)
			{
				fireStartCooldown -= Time.deltaTime;
			}
			else
			{
				fireStartCooldown = fireStartInterval;
				var aliveNodes = graph.Nodes.Where(t => t.GetComponent<HealthComponent>().CurrentValue > 0 &&  t.GetComponent<FireStatus>() == null).ToList();
				if (aliveNodes.Count == 0)
				{
					return;
				}
				int nodeToDamageIndex = Random.Range(0, aliveNodes.Count);
				aliveNodes[nodeToDamageIndex].AddStatus<FireStatus>();
			}
		}
	}
}
