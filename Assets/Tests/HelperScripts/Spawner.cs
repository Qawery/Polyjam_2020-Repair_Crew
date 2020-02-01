using System;
using UnityEngine;

namespace Polyjam2020.Tests
{
	public class Spawner : MonoBehaviour
	{
		[SerializeField] private GameObject objectToSpawn;

		private void Start()
		{
			World.Instance.InstantiateObject(objectToSpawn);
		}
	}
}

