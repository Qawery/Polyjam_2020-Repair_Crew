using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class Edge : MonoBehaviour
	{
		[SerializeField] private Node firstNode = null;
		[SerializeField] private Node firstSecond = null;
		[SerializeField] private RoadSegment roadSegmentPrefab = null;

		public (Node first, Node second) Nodes
		{
			get => (firstNode, firstSecond);

			set
			{
				Assert.IsNull(firstNode, "Not null first node on: " + gameObject.name);
				Assert.IsNull(firstSecond, "Not null second node on: " + gameObject.name);
				firstNode = value.first;
				firstSecond = value.second;
				CheckNodesValidity();
				firstNode.AddEdge(this);
				firstSecond.AddEdge(this);
				GenerateRoad();
			}
		}

		private void Start()
		{
			CheckNodesValidity();
			GenerateRoad();
		}

		private void OnDestroy()
		{
			if (firstNode != null)
			{
				firstNode.RemoveEdge(this);
				firstNode = null;
			}
			if (firstSecond != null)
			{
				firstSecond.RemoveEdge(this);
				firstSecond = null;
			}
		}

		private void GenerateRoad()
		{
			var firstSegment = World.Instance.InstantiateObject(roadSegmentPrefab);
			Vector3 roadVec = (Nodes.second.transform.position - Nodes.first.transform.position).Flat();
			float roadLength = roadVec.magnitude;
			float segmentLength = Vector3.Distance(firstSegment.Start.position, firstSegment.End.position);
			int segmentsInRoad = (int) (roadLength / segmentLength);
			float unadjustedRoadLength = segmentsInRoad * segmentLength;
			float scale = roadLength / unadjustedRoadLength;

			firstSegment.transform.localScale *= scale;
			firstSegment.transform.rotation = Quaternion.LookRotation(roadVec);
			Vector3 offset = Nodes.first.transform.position - firstSegment.Start.position;
			firstSegment.transform.position += offset;

			var allSegments = new List<RoadSegment>();
			allSegments.Add(firstSegment);

			Vector3 previousSegmentEndPos = firstSegment.End.position;
			for (int i = 1; i < segmentsInRoad; ++i)
			{
				var segment = World.Instance.InstantiateObject(firstSegment);
				offset = previousSegmentEndPos - segment.Start.position;
				segment.transform.position += offset;
				previousSegmentEndPos = segment.End.position;
				allSegments.Add(segment);
			}

			foreach (var segment in allSegments)
			{
				segment.transform.SetParent(transform);
			}
		}

		private void CheckNodesValidity()
		{
			Assert.IsNotNull(firstNode, "Null first node on: " + gameObject.name);
			Assert.IsNotNull(firstSecond, "Null second node on: " + gameObject.name);
			Assert.IsFalse(firstNode == firstSecond, "Edge points to same node on: " + gameObject.name);
		}
	}
}
