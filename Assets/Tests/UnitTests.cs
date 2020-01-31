using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;


namespace Polyjam2020
{
	public class UnitTests : MonoBehaviour
	{
		private const string TEST_SCENE_NAME = "UnitTestScene";


		[UnityTest]
		public IEnumerator NodeDetectionOnTriggerEnter()
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
	}
}