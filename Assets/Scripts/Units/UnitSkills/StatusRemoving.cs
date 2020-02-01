using UnityEngine;

namespace Polyjam2020
{
	public class StatusRemoving : UnitSkill
	{
		[SerializeField] private float statusRemovalTime = 0;
		protected override void ApplySkillToNode(Node node)
		{
			node.RemoveStatus<FireStatus>();
		}
	}
}
