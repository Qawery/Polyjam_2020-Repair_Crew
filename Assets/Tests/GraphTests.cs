using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;


namespace Polyjam2020
{
	namespace Tests
	{
		public class GraphTests
		{
			private const int EXPECTED_NUMBER_OF_NODES_ON_SCENE = 3;
			private const int EXPECTED_NUMBER_OF_GRAPHS_ON_SCENE = 1;
			

			[UnityTest]
			public IEnumerator GraphTestsWithEnumeratorPasses()
			{
				SceneManager.LoadScene("GraphTestScene");
				yield return null;
				Assert.IsTrue(Object.FindObjectsOfType<Node>().Length == EXPECTED_NUMBER_OF_NODES_ON_SCENE, "Invalid number of nodes on scene");
				var graphs = Object.FindObjectsOfType<Graph>();
				Assert.IsTrue(graphs.Length == EXPECTED_NUMBER_OF_GRAPHS_ON_SCENE, "Invalid number of graphs on scene");
				var graph = graphs[0];
				Assert.IsTrue(graph.Nodes.Count == EXPECTED_NUMBER_OF_NODES_ON_SCENE, "Graph has invalid number of registered nodes");
				yield return null;
			}
		}
	}
}