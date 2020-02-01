using UnityEngine;

namespace Polyjam2020
{
	public class RoadSegment : MonoBehaviour
	{
		[SerializeField] private Transform start;
		[SerializeField] private Transform end;

		public Transform Start => start;
		public Transform End => end;
	}
}
