using UnityEngine;
using System.Collections.Generic;


namespace Polyjam2020
{
	public class Graph : MonoBehaviour
	{
		private List<Node> nodes = new List<Node>();


		public List<Node> Nodes => nodes;


		private void Awake()
		{
			nodes.AddRange(Object.FindObjectsOfType<Node>());
			var existingEdges = Object.FindObjectsOfType<Edge>();
			foreach (var edge in existingEdges)
			{
				edge.Nodes.first.AddEdge(edge);
				edge.Nodes.second.AddEdge(edge);
			}
		}
	}
}
