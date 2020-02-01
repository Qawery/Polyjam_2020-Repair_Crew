using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Polyjam2020
{
	public class MoveToClosestNode : MonoBehaviour
	{
		private List<Node> nodes = new List<Node>();
		void Start()
		{
			float minDist = float.MaxValue;
			Node closestNode = null;
			foreach (var node in nodes)
			{
				float dist = Vector3.Distance(node.transform.position.Flat(), transform.position.Flat());
				if (dist < minDist)
				{
					minDist = dist;
					closestNode = node;
				}
			}

			if (closestNode != null)
			{
				var movement = GetComponent<UnitMovementController>();
				Assert.IsNotNull(movement, $"UnitMovementController missing on {name}. Required by {nameof(MoveToClosestNode)}");
				movement.MoveToPoint(closestNode.transform.position);
			}
		}
		
		[SpawnHandlerMethod]
		private void OnNodeSpawned(Node node)
		{
			nodes.Add(node);
		}
	}
}
