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
			private const string TEST_SCENE_NAME = "GraphTestScene";
			private const int EXPECTED_NUMBER_OF_NODES_ON_SCENE = 3;
			private const int EXPECTED_NUMBER_OF_GRAPHS_ON_SCENE = 1;
			

			[UnityTest]
			public IEnumerator GraphConstructionFromExistingScene()
			{
				SceneManager.LoadScene(TEST_SCENE_NAME);
				yield return null;
				Assert.IsTrue(SceneManager.GetActiveScene().name == TEST_SCENE_NAME, "Test scene not loaded");
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