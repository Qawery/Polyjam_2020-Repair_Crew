using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class Edge : MonoBehaviour
	{
		[SerializeField] private (Node first, Node second) nodes = (null, null);


		public (Node first, Node second) Nodes
		{
			get
			{
				return nodes;
			}

			set
			{
				nodes = value;
				Assert.IsNotNull(nodes.first, "Null first node on: " + gameObject.name);
				Assert.IsNotNull(nodes.second, "Null second node on: " + gameObject.name);
				Assert.IsFalse(nodes.first == nodes.second, "Edge points to same node on: " + gameObject.name);
				nodes.first.AddEdge(this);
				nodes.second.AddEdge(this);
				//TODO: Wyswietlanie i pozycjonowanie
			}
		}


		private void Start()
		{
			Assert.IsNotNull(nodes.first, "On Start null first node on: " + gameObject.name);
			Assert.IsNotNull(nodes.second, "On Start null second node on: " + gameObject.name);
		}

		private void OnDestroy()
		{
			if (nodes.first != null)
			{
				nodes.first.RemoveEdge(this);
				nodes.first = null;
			}
			if (nodes.second != null)
			{
				nodes.second.RemoveEdge(this);
				nodes.second = null;
			}
		}
	}
}