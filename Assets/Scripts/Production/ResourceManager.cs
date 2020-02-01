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

		private int resourcesRemaining;
		public int ResourcesRemaining
		{
			get => resourcesRemaining;
			private set
			{
				int previous = resourcesRemaining;
				resourcesRemaining = value;
				resourcesRemaining = Mathf.Min(resourcesRemaining, MaxResources);
				OnResourceAmountChanged?.Invoke((previous, resourcesRemaining));
			}
		}

		public event System.Action<(int previous, int current)> OnResourceAmountChanged;

		public void SpendResources(int amount)
		{
			Assert.IsTrue(amount <= ResourcesRemaining, "Trying to spend more resources that is available");
			ResourcesRemaining -= amount;
		}

		public void RestoreResources(int amount)
		{
			ResourcesRemaining += amount;
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
			}
		}
	}
}
