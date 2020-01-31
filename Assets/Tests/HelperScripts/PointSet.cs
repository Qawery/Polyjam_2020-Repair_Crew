using System.Collections.Generic;
using UnityEngine;

namespace Polyjam2020.Tests
{
	public class PointSet : MonoBehaviour
	{
		[SerializeField] private List<Transform> points;
		public List<Transform> Points => points;

		private void Awake()
		{
			points.AddRange(GetComponentsInChildren<Transform>());
			points.Remove(transform);
		}
	}
}
