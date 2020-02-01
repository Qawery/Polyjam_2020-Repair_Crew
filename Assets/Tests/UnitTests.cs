using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;


namespace Polyjam2020
{
	public class UnitTests : MonoBehaviour
	{
		private const string TEST_SCENE_NAME = "UnitTestScene";
		private const float CHANGE_AMOUNT = 0.01f;
		private const int TEST_DURATION_IN_FRAMES = 5;
		private const float TOLERANCE = 0.01f;


		[UnityTest]
		public IEnumerator UnitDetectNode()
		{
			SceneManager.LoadScene(TEST_SCENE_NAME);
			yield return null;
			yield return null;
			Assert.IsTrue(SceneManager.GetActiveScene().name == TEST_SCENE_NAME, "Test scene not loaded");
			var node = Object.FindObjectOfType<Node>();
			Assert.IsNotNull(node, "Missing node");
			var unit = Object.FindObjectOfType<Unit>();
			Assert.IsNotNull(unit, "Missing unit");
			Assert.IsNull(unit.NodeUnderEffect, "Unit already has NodeUnderEffect");
			unit.transform.position = node.transform.position;
			yield return null;
			yield return null;
			Assert.IsNotNull(unit.NodeUnderEffect, "Unit didn't register NodeUnderEffect");
		}

		[UnityTest]
		public IEnumerator UnitHealNode()
		{
			SceneManager.LoadScene(TEST_SCENE_NAME);
			yield return null;
			yield return null;
			Assert.IsTrue(SceneManager.GetActiveScene().name == TEST_SCENE_NAME, "Test scene not loaded");
			var node = Object.FindObjectOfType<Node>();
			Assert.IsNotNull(node, "Missing node");
			var unit = Object.FindObjectOfType<Unit>();
			Assert.IsNotNull(unit, "Missing unit");
			Assert.IsTrue(Math.Abs(node.CurrentHealth - Node.MAX_HEALTH) < TOLERANCE);
			node.ApplyDamage(CHANGE_AMOUNT);
			for (int i = 0; i < TEST_DURATION_IN_FRAMES; ++i)
			{
				Assert.IsTrue(Math.Abs(node.CurrentHealth - (Node.MAX_HEALTH - CHANGE_AMOUNT)) < TOLERANCE);
				yield return null;
			}
			unit.transform.position = node.transform.position;
			float previousHealth = node.CurrentHealth;
			for (int i = 0; i < TEST_DURATION_IN_FRAMES; ++i)
			{
				yield return null;
				Assert.IsTrue(node.CurrentHealth >= previousHealth);
				previousHealth = node.CurrentHealth;
			}
			Assert.IsTrue(Math.Abs(node.CurrentHealth - Node.MAX_HEALTH) < TOLERANCE);
		}
	}
}