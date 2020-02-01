using UnityEngine;

namespace Polyjam2020.Tests
{
	public class UnitTestHelper : MonoBehaviour
	{
		[SerializeField] private Unit healingUnit;
		[SerializeField] private Unit statusRemovingUnit;

		public Unit HealingUnit => healingUnit;
		public Unit StatusRemovingUnit => statusRemovingUnit;
	}
}
