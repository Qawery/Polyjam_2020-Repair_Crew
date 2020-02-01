using UnityEngine;

namespace Polyjam2020
{
	public class Healing : UnitSkill
	{
		[SerializeField] private int healingAmount = 10;
		
		protected override void ApplySkillToNode(Node node)
		{
			unit.NodeUnderEffect.GetComponent<HealthComponent>().ApplyHeal(healingAmount);
		}
	}
}

