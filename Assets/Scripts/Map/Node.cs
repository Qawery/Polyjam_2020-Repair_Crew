﻿using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace Polyjam2020
{
	public class Node : MonoBehaviour
	{
		[SerializeField] private GameObject livingElements = null;
		[SerializeField] private GameObject fires = null;
		[SerializeField] private GameObject rubble = null;
		public const float MAX_HEALTH = 5.0f;
		private float currentHealth = MAX_HEALTH;
		private List<Edge> edges = new List<Edge>();
		public System.Action OnHealthChanged;


		public List<UnitSlot> UnitSlots { get; } = new List<UnitSlot>();
		public List<Edge> Edges => edges;

		public float CurrentHealth
		{
			get
			{
				return currentHealth;
			}

			private set
			{
				currentHealth = value;
				if (currentHealth <= 0.0f)
				{
					livingElements.SetActive(false);
					rubble.SetActive(true);
				}
				OnHealthChanged?.Invoke();
			}
		}


		private void Awake()
		{
			Assert.IsNotNull(livingElements);
			Assert.IsNotNull(fires);
			Assert.IsNotNull(rubble);
			UnitSlots.AddRange(GetComponentsInChildren<UnitSlot>());
			livingElements.SetActive(true);
			fires.SetActive(false);
			rubble.SetActive(false);
		}

		private void OnDestroy()
		{
			foreach (var edge in edges)
			{
				Destroy(edge.gameObject);
			}
			edges.Clear();
		}

		public void AddEdge(Edge newEdge)
		{
			Assert.IsFalse(edges.Contains(newEdge), "Duplicate edge on: " + gameObject.name);
			Assert.IsTrue(newEdge.Nodes.first == this || newEdge.Nodes.second == this, "New edge does not contain this node on: " + gameObject.name);
			var otherNode = newEdge.Nodes.first == this ? newEdge.Nodes.second : newEdge.Nodes.first;
			foreach (var edge in edges)
			{
				Assert.IsFalse(edge.Nodes.first == otherNode || edge.Nodes.second == otherNode, "Connection to node <" + otherNode.gameObject.name + "> already exists in form of <" + edge.gameObject.name + "> on: " + gameObject.name);
			}
			edges.Add(newEdge);
		}

		public void RemoveEdge(Edge edge)
		{
			edges.Remove(edge);
		}

		public void ApplyDamage(float value)
		{
			Assert.IsTrue(value > 0.0f, "Trying to apply damage not greater than zero on: " + gameObject.name);
			CurrentHealth = Mathf.Max(CurrentHealth - value, 0.0f);
		}

		public void ApplyHeal(float value)
		{
			Assert.IsTrue(value > 0.0f, "Trying to apply heal not greater than zero on: " + gameObject.name);
			if (CurrentHealth > 0.0f)
			{
				CurrentHealth = Mathf.Min(CurrentHealth + value, MAX_HEALTH);
			}
		}

		private void OnStatusChanged()
		{
			//TODO: Podpięcie i reagowanie na zmiany statusu, włączanie i wyłączanie ognia
		}
	}
}
