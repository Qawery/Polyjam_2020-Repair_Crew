using UnityEngine;


namespace Polyjam2020
{
	public class FireStatus : NodeStatus
	{
		[SerializeField] private int damage = 10;


		protected override void ApplyStatusToNode(Node node)
		{
			node.GetComponent<HealthComponent>().ApplyDamage(damage);
		}
	}
}
