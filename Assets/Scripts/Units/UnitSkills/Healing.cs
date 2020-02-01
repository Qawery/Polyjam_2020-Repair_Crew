using UnityEngine;

namespace Polyjam2020
{
	public class Healing : UnitSkill
	{
		[SerializeField] private float healingInterval = 1.0f;
		[SerializeField] private int healingAmount = 10;

		private float timer = 0;

		private void Update()
		{
			if (unit.NodeUnderEffect != null)
			{
				timer += Time.deltaTime;
				if (timer > healingInterval)
				{
					timer = 0;
					unit.NodeUnderEffect.GetComponent<HealthComponent>().ApplyHeal(healingAmount);
				}
			}
			else
			{
				timer = 0;
			}
		}
	}
}

