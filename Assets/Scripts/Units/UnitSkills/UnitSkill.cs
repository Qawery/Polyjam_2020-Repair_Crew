using UnityEngine;
using UnityEngine.Assertions;

namespace Polyjam2020
{
	public abstract class UnitSkill : MonoBehaviour
	{
		[SerializeField] private float skillApplicationInterval = 1.0f;

		protected Unit unit;
		protected float timer = 0;
		public float TimeRemainingToApplySkill => skillApplicationInterval - timer;
		
		protected virtual void Awake()
		{
			unit = GetComponent<Unit>();
			Assert.IsNotNull(unit);
		}
		
		private void Update()
		{
			if (unit.NodeUnderEffect != null)
			{
				timer += Time.deltaTime;
				if (timer > skillApplicationInterval)
				{
					timer = 0;
					ApplySkillToNode(unit.NodeUnderEffect);
				}
			}
			else
			{
				timer = 0;
			}
		}

		protected abstract void ApplySkillToNode(Node node);
	}
}

