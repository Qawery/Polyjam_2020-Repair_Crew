using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Polyjam2020
{
	public class ResourceManager : MonoBehaviour
	{
		[SerializeField] private int resourceRegenerationRate = 0;
		[SerializeField] private float resourceRegenerationInterval = 0;
		[SerializeField] private int maxResources = 10;

		private float resourceRegenerationTimer = 0;
		
		public int ResourceRegenerationRate
		{
			get => resourceRegenerationRate;
			set => resourceRegenerationRate = value;
		}

		public float ResourceRegenerationInterval
		{
			get => resourceRegenerationInterval;
			set => resourceRegenerationInterval = value;
		}

		public int MaxResources
		{
			get => maxResources;
			set => maxResources = value;
		}
		
		public int ResourcesRemaining { get; private set; }

		public void SpendResources(int amount)
		{
			Assert.IsTrue(amount <= ResourcesRemaining, "Trying to spend more resources that is available");
			ResourcesRemaining -= amount;
		}

		public void RestoreResources(int amount)
		{
			ResourcesRemaining += amount;
			ResourcesRemaining = Mathf.Min(ResourcesRemaining, MaxResources);
		}

		private void Start()
		{
			ResourcesRemaining = MaxResources;
		}

		private void Update()
		{
			resourceRegenerationTimer += Time.deltaTime;
			if (resourceRegenerationTimer < resourceRegenerationInterval)
			{
				ResourcesRemaining += resourceRegenerationRate;
				ResourcesRemaining = Mathf.Min(ResourcesRemaining, MaxResources);
			}
		}
	}
}
