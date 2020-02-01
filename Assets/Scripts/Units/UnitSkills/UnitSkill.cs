using UnityEngine;
using UnityEngine.Assertions;

namespace Polyjam2020
{
	public class UnitSkill : MonoBehaviour
	{
		protected Unit unit;

		private void Awake()
		{
			unit = GetComponent<Unit>();
			Assert.IsNotNull(unit);
		}
	}
}

