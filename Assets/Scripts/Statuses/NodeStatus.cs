using UnityEngine;
using UnityEngine.Assertions;

namespace Polyjam2020
{
	public abstract class NodeStatus : MonoBehaviour
	{
		[SerializeField] private float statusApplicationInterval = 1.0f;

		protected Node node;
		protected float timer = 0;
		public float TimeRemainingToAffectNode => statusApplicationInterval - timer;
		
		protected virtual void Awake()
		{
			node = GetComponent<Node>();
			Assert.IsNotNull(node);
		}
		
		private void Update()
		{
			timer += Time.deltaTime;
			if (timer > statusApplicationInterval)
			{
				timer = 0;
				ApplyStatusToNode(node);
			}
		}

		protected abstract void ApplyStatusToNode(Node node);
	}
}

