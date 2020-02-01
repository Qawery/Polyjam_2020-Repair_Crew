using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class Edge : MonoBehaviour
	{
		[SerializeField] private Node firstNode = null;
		[SerializeField] private Node firstSecond = null;


		public (Node first, Node second) Nodes
		{
			get => (firstNode, firstSecond);

			set
			{
				Assert.IsNull(firstNode, "Not null first node on: " + gameObject.name);
				Assert.IsNull(firstSecond, "Not null second node on: " + gameObject.name);
				firstNode = value.first;
				firstSecond = value.second;
				CheckNodesValidity();
				firstNode.AddEdge(this);
				firstSecond.AddEdge(this);
				//TODO: Wyswietlanie i pozycjonowanie
			}
		}


		private void Start()
		{
			CheckNodesValidity();
		}

		private void OnDestroy()
		{
			if (firstNode != null)
			{
				firstNode.RemoveEdge(this);
				firstNode = null;
			}
			if (firstSecond != null)
			{
				firstSecond.RemoveEdge(this);
				firstSecond = null;
			}
		}

		private void CheckNodesValidity()
		{
			Assert.IsNotNull(firstNode, "Null first node on: " + gameObject.name);
			Assert.IsNotNull(firstSecond, "Null second node on: " + gameObject.name);
			Assert.IsFalse(firstNode == firstSecond, "Edge points to same node on: " + gameObject.name);
		}
	}
}
