using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace Polyjam2020
{
	public class Node : MonoBehaviour
	{
		private List<Edge> edges = new List<Edge>();


		public List<Edge> Edges => edges;


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
	}
}