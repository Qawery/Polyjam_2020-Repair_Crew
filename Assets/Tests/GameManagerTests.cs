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
		private bool wasDefeat = false;


		private void OnDefeat()
		{
			wasDefeat = true;
		}

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
			gameplayManager.OnDefeat += OnDefeat;
			Assert.IsFalse(gameplayManager.IsDefeat, "GameplayManager registered defeat too early");
			for (int i = 0; i < graph.Nodes.Count / 2; ++i)
			{
				var health = graph.Nodes[i].GetComponent<HealthComponent>();
				health.ApplyDamage(health.MaxValue);
			}
			yield return null;
			Assert.IsTrue(gameplayManager.IsDefeat, "GameplayManager didn't register defeat after destroying half of cities");
			Assert.IsTrue(wasDefeat, "GameplayManager didn't called event on defeat");
			yield return new WaitForSeconds(5.0f);
			yield return null;
			Assert.IsTrue(SceneManager.GetActiveScene().name == "MainMenu", "Not returned to main menu after defeat");
		}
	}
}