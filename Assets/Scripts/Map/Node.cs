using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace Polyjam2020
{
	public class Node : MonoBehaviour
	{
		private List<Edge> edges = new List<Edge>();
		public List<UnitSlot> UnitSlots { get; } = new List<UnitSlot>();
		public List<Edge> Edges => edges;

		public event System.Action<NodeStatus> OnStatusReceived;
		public event System.Action<NodeStatus> OnStatusRemoved;


		private void Awake()
		{
			UnitSlots.AddRange(GetComponentsInChildren<UnitSlot>());
		}

		private void OnDestroy()
		{
			foreach (var edge in edges)
			{
				Destroy(edge.gameObject);
			}
			edges.Clear();
		}

		public void AddEdge(Edge newEdge)
		{
			Assert.IsFalse(edges.Contains(newEdge), "Duplicate edge on: " + gameObject.name);
			Assert.IsTrue(newEdge.Nodes.first == this || newEdge.Nodes.second == this, "New edge does not contain this node on: " + gameObject.name);
			var otherNode = newEdge.Nodes.first == this ? newEdge.Nodes.second : newEdge.Nodes.first;
			foreach (var edge in edges)
			{
				Assert.IsFalse(edge.Nodes.first == otherNode || edge.Nodes.second == otherNode, "Connection to node <" + otherNode.gameObject.name + "> already exists in form of <" + edge.gameObject.name + "> on: " + gameObject.name);
			}
			edges.Add(newEdge);
		}

		public void RemoveEdge(Edge edge)
		{
			edges.Remove(edge);
		}

		public StatusType AddStatus<StatusType>() where StatusType : NodeStatus
		{
			var oldStatus = gameObject.GetComponent<StatusType>();
			if (oldStatus != null)
			{
				return null;
			}
			
			var status = gameObject.AddComponent<StatusType>();
			OnStatusReceived?.Invoke(status);
			return status;
		}

		public void RemoveStatus<StatusType>() where StatusType : NodeStatus
		{
			var status = GetComponent<StatusType>();
			if (status != null)
			{
				OnStatusRemoved?.Invoke(status);
				Destroy(status);
			}
		}
	}
}
