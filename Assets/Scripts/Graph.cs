using UnityEngine;
using UnityEngine.Assertions;
//using System.Collections.Generic;


namespace Polyjam2020
{
	public class Graph : MonoBehaviour
	{
		[SerializeField] private Node nodePrefab = null;
		[SerializeField] private Edge edgePrefab = null;
		//[SerializeField] private List<Node> nodes = new List<Node>();	//TODO: Przemyslec jak tworzyc i reprezentowac wezly


		private void Awake()
		{
			Assert.IsNotNull(nodePrefab, "Missing nodePrefab on: " + gameObject.name);
			Assert.IsNotNull(edgePrefab, "Missing edgePrefab on: " + gameObject.name);
		}

		public Edge CreateEdgeBetweenNodes((Node first, Node second) nodesToConnect)
		{
			//Assert.IsTrue(nodes.Contains(nodesToConnect.first), "First node not registered on: " + gameObject.name);
			//Assert.IsTrue(nodes.Contains(nodesToConnect.second), "Second node not registered on: " + gameObject.name);
			var newEdge = Instantiate<Edge>(edgePrefab);
			newEdge.Nodes = nodesToConnect;
			return newEdge;
		}
	}
}
