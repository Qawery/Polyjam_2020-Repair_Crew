using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


namespace Polyjam2020
{
	namespace Tests
	{
		public class NodeTest
		{
			private const int CHANGE_VALUE = 20;


			[Test]
			public void ApplyDamageAndHeal()
			{
				var gameObject = new GameObject();
				var node = gameObject.AddComponent<Node>();
				var health = gameObject.AddComponent<HealthComponent>();
				Assert.IsNotNull(health);
				Assert.IsTrue(CHANGE_VALUE < health.MaxValue);
				Assert.IsTrue(health.CurrentValue == health.MaxValue);
				health.ApplyDamage(CHANGE_VALUE);
				Assert.IsTrue(health.CurrentValue == health.MaxValue - CHANGE_VALUE);
				health.ApplyHeal(CHANGE_VALUE);
				Assert.IsTrue(health.CurrentValue == health.MaxValue);
				health.ApplyDamage(2 * health.MaxValue);
				Assert.IsTrue(health.CurrentValue == 0);
				health.ApplyHeal(2 * health.MaxValue);
				Assert.IsTrue(health.CurrentValue == 0);
			}

			[UnityTest]
			public IEnumerator CreateAndDestroyNode()
			{
				//Tworzenie obiektw testowych i przypinanie referencji
				var nodeGameObject1 = new GameObject();
				var node1 = nodeGameObject1.AddComponent<Node>();
				var nodeGameObject2 = new GameObject();
				var node2 = nodeGameObject2.AddComponent<Node>();
				var edgeGameObject = new GameObject();
				var edge = edgeGameObject.AddComponent<Edge>();
				edge.Nodes = (node1, node2);
				yield return null;
				//Sprawdzanie poprawnosci podpiecia referencji
				Assert.IsTrue(node1.Edges.Count == 1);
				Assert.IsTrue(node2.Edges.Count == 1);
				Assert.IsTrue(node1.Edges[0] == node2.Edges[0]);
				Assert.IsTrue(node2.Edges[0] == edge);
				//Zniszczenie jednego z wezlow
				GameObject.Destroy(nodeGameObject1);
				yield return null;
				//Sprawdzenie poprawnosci zniszczenia wezla i krawedzi
				Assert.IsTrue(nodeGameObject1 == null || nodeGameObject1.Equals(null));
				Assert.IsTrue(edgeGameObject == null || edgeGameObject.Equals(null));
				Assert.IsFalse(nodeGameObject2 == null || nodeGameObject2.Equals(null));
				Assert.IsTrue(node2.Edges.Count == 0);
			}
		}
	}
}