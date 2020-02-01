using System;
using System.Collections;
using NUnit.Framework;
using Polyjam2020.Tests;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;


namespace Polyjam2020
{
	public class UnitTests : MonoBehaviour
	{
		private const string TEST_SCENE_NAME = "UnitTestScene";
		private const int CHANGE_AMOUNT = 1;
		private const float TEST_DURATION_IN_SECONDS = 2.0f;

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
			var helper = FindObjectOfType<UnitTestHelper>();
			Assert.IsNotNull(helper);
			Assert.IsNotNull(helper.HealingUnit);
			var health = node.GetComponent<HealthComponent>();
			Assert.IsTrue(health.CurrentValue == health.MaxValue);
			health.ApplyDamage(CHANGE_AMOUNT);
			Assert.IsTrue(health.CurrentValue == health.MaxValue - CHANGE_AMOUNT);
			helper.HealingUnit.transform.position = node.transform.position;
			yield return new WaitForSeconds(TEST_DURATION_IN_SECONDS);
			Assert.IsTrue(health.CurrentValue == health.MaxValue);
		}
		
		[UnityTest]
		public IEnumerator UnitRemoveStatus()
		{
			SceneManager.LoadScene(TEST_SCENE_NAME);
			yield return null;
			yield return null;
			Assert.IsTrue(SceneManager.GetActiveScene().name == TEST_SCENE_NAME, "Test scene not loaded");
			var node = Object.FindObjectOfType<Node>();
			Assert.IsNotNull(node, "Missing node");
			var helper = FindObjectOfType<UnitTestHelper>();
			Assert.IsNotNull(helper);
			Assert.IsNotNull(helper.StatusRemovingUnit);
			node.AddStatus<FireStatus>();
			yield return null;
			yield return null;

			helper.StatusRemovingUnit.transform.position = node.transform.position;
			yield return new WaitForSeconds(TEST_DURATION_IN_SECONDS);
			Assert.IsNull(node.GetComponent<FireStatus>());
		}
	}
}