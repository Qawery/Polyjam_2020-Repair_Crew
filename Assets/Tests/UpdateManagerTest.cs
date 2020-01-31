using BaseProject;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;



namespace Tests
{
    public class UpdateManagerTest
	{
		private const int NUMBER_OF_TEST_UPDATES = 10;


		private class TestClassToUpdate
		{
			public int FirstValueToIncrement { get; private set; } = 0;
			public int LastValueToIncrement { get; private set; } = 0;


			[UpdateFunction(UpdatePhases.FIRST)]
			private void FirstUpdatefunction()
			{
				++FirstValueToIncrement;
				Assert.IsTrue(FirstValueToIncrement == LastValueToIncrement + 1, "FirstValueToIncrement not by 1 then LastValueToIncrement adter update; " +
								"FirstValueToIncrement: " + FirstValueToIncrement.ToString() +
								"; LastValueToIncrement: " + LastValueToIncrement.ToString());
			}

			[UpdateFunction(UpdatePhases.LAST)]
			private void LastUpdatefunction()
			{
				++LastValueToIncrement;
				Assert.IsTrue(FirstValueToIncrement == LastValueToIncrement, "LastValueToIncrement not equal to FirstValueToIncrement after update; " +
								"FirstValueToIncrement: " + FirstValueToIncrement.ToString() + 
								"; LastValueToIncrement: " + LastValueToIncrement.ToString());
			}
		}


		private class DerivingTestClassToUpdate : TestClassToUpdate
		{
		}


		[UnityTest]
		public IEnumerator UpdateTargetProvidedAsClassType()
		{
			var testGameObject = new GameObject();
			var updateManager = testGameObject.AddComponent<UnityEventUpdateManager>() as IUpdateBroadcaster;
			var testClassToUpdate = new TestClassToUpdate();
			updateManager.AddUpdateSubscriber(testClassToUpdate);
			for (int i = 0; i < NUMBER_OF_TEST_UPDATES; ++i)
			{
				Assert.IsTrue(testClassToUpdate.FirstValueToIncrement == i, "FirstValueToIncrement != i; " +
								"FirstValueToIncrement: " + testClassToUpdate.FirstValueToIncrement.ToString() +
								"; i: " + i.ToString());
				yield return null;
				Assert.IsTrue(testClassToUpdate.LastValueToIncrement == i, "LastValueToIncrement != i; " +
								"LastValueToIncrement: " + testClassToUpdate.LastValueToIncrement.ToString() +
								"; i: " + i.ToString());
			}
			updateManager.RemoveUpdateSubscriber(testClassToUpdate);
			GameObject.Destroy(testGameObject);
		}

		[UnityTest]
		public IEnumerator UpdateTargetProvidedAsDerivingClassType()
		{
			var testGameObject = new GameObject();
			var updateManager = testGameObject.AddComponent<UnityEventUpdateManager>() as IUpdateBroadcaster;
			var testDerivingClassToUpdate = new DerivingTestClassToUpdate();
			updateManager.AddUpdateSubscriber(testDerivingClassToUpdate);
			for (int i = 0; i < NUMBER_OF_TEST_UPDATES; ++i)
			{
				Assert.IsTrue(testDerivingClassToUpdate.FirstValueToIncrement == i, "FirstValueToIncrement != i; " +
								"FirstValueToIncrement: " + testDerivingClassToUpdate.FirstValueToIncrement.ToString() +
								"; i: " + i.ToString());
				yield return null;
				Assert.IsTrue(testDerivingClassToUpdate.LastValueToIncrement == i, "LastValueToIncrement != i; " +
								"LastValueToIncrement: " + testDerivingClassToUpdate.LastValueToIncrement.ToString() +
								"; i: " + i.ToString());
			}
			updateManager.RemoveUpdateSubscriber(testDerivingClassToUpdate);
			GameObject.Destroy(testGameObject);
		}

		[UnityTest]
		public IEnumerator UpdateTargetProvidedAsGenericObjectType()
		{
			var testGameObject = new GameObject();
			var updateManager = testGameObject.AddComponent<UnityEventUpdateManager>() as IUpdateBroadcaster;
			var testObjectToUpdate = new TestClassToUpdate();
			updateManager.AddUpdateSubscriber(testObjectToUpdate as object);
			for (int i = 0; i < NUMBER_OF_TEST_UPDATES; ++i)
			{
				Assert.IsTrue(testObjectToUpdate.FirstValueToIncrement == i, "FirstValueToIncrement != i; " +
								"FirstValueToIncrement: " + testObjectToUpdate.FirstValueToIncrement.ToString() +
								"; i: " + i.ToString());
				yield return null;
				Assert.IsTrue(testObjectToUpdate.LastValueToIncrement == i, "LastValueToIncrement != i; " +
								"LastValueToIncrement: " + testObjectToUpdate.LastValueToIncrement.ToString() +
								"; i: " + i.ToString());
			}
			updateManager.RemoveUpdateSubscriber(testObjectToUpdate as object);
			GameObject.Destroy(testGameObject);
		}

		[UnityTest]
		public IEnumerator MultipleUpdateTargets()
		{
			var testGameObject = new GameObject();
			var updateManager = testGameObject.AddComponent<UnityEventUpdateManager>() as IUpdateBroadcaster;
			var testClassToUpdate = new TestClassToUpdate();
			var testDerivingClassToUpdate = new DerivingTestClassToUpdate();
			var testObjectToUpdate = new TestClassToUpdate();
			updateManager.AddUpdateSubscriber(testClassToUpdate);
			updateManager.AddUpdateSubscriber(testDerivingClassToUpdate);
			updateManager.AddUpdateSubscriber(testObjectToUpdate as object);
			for (int i = 0; i < NUMBER_OF_TEST_UPDATES; ++i)
			{
				Assert.IsTrue(testClassToUpdate.FirstValueToIncrement == i, "testClassToUpdate.FirstValueToIncrement != i; " +
								"FirstValueToIncrement: " + testClassToUpdate.FirstValueToIncrement.ToString() +
								"; i: " + i.ToString());
				Assert.IsTrue(testDerivingClassToUpdate.FirstValueToIncrement == i, "testDerivingClassToUpdate.FirstValueToIncrement != i; " +
								"FirstValueToIncrement: " + testDerivingClassToUpdate.FirstValueToIncrement.ToString() +
								"; i: " + i.ToString());
				Assert.IsTrue(testObjectToUpdate.FirstValueToIncrement == i, "testObjectToUpdate.FirstValueToIncrement != i; " +
								"FirstValueToIncrement: " + testObjectToUpdate.FirstValueToIncrement.ToString() +
								"; i: " + i.ToString());
				yield return null;
				Assert.IsTrue(testClassToUpdate.LastValueToIncrement == i, "testClassToUpdate.LastValueToIncrement != i; " +
								"LastValueToIncrement: " + testClassToUpdate.LastValueToIncrement.ToString() +
								"; i: " + i.ToString());
				Assert.IsTrue(testDerivingClassToUpdate.LastValueToIncrement == i, "testDerivingClassToUpdate.LastValueToIncrement != i; " +
								"LastValueToIncrement: " + testDerivingClassToUpdate.LastValueToIncrement.ToString() +
								"; i: " + i.ToString());
				Assert.IsTrue(testObjectToUpdate.LastValueToIncrement == i, "testObjectToUpdate.LastValueToIncrement != i; " +
								"LastValueToIncrement: " + testObjectToUpdate.LastValueToIncrement.ToString() +
								"; i: " + i.ToString());
			}
			updateManager.RemoveUpdateSubscriber(testClassToUpdate);
			updateManager.RemoveUpdateSubscriber(testDerivingClassToUpdate);
			updateManager.RemoveUpdateSubscriber(testObjectToUpdate as object);
			GameObject.Destroy(testGameObject);
		}
	}
}
