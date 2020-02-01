using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;


namespace Polyjam2020
{
	public class GameManagerTests : MonoBehaviour
	{
		private const string TEST_SCENE_NAME = "GameplayManagerTestScene";


		[UnityTest]
		public IEnumerator DefeatAndTransitionToMainMenu()
		{
			SceneManager.LoadScene(TEST_SCENE_NAME);
			yield return null;
			Assert.IsTrue(SceneManager.GetActiveScene().name == TEST_SCENE_NAME, "Test scene not loaded");
			var graph = Object.FindObjectOfType<Graph>();
			Assert.IsNotNull(graph, "No graph on scene");
			var gameplayManager = Object.FindObjectOfType<GameplayManager>();
			Assert.IsNotNull(gameplayManager, "No gameplayManager on scene");
			Assert.IsFalse(gameplayManager.IsDefeat, "GameplayManager registered defeat too early");
			for (int i = 0; i < graph.Nodes.Count / 2; ++i)
			{
				graph.Nodes[i].ApplyDamage(Node.MAX_HEALTH);
			}
			yield return null;
			Assert.IsTrue(gameplayManager.IsDefeat, "GameplayManager didn't register defeat after destroying half of cities");
			yield return new WaitForSeconds(5.0f);
			yield return null;
			Assert.IsTrue(SceneManager.GetActiveScene().name == "MainMenu", "Not returned to main menu after defeat");
		}
	}
}