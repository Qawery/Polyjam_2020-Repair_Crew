using UnityEngine;

namespace Polyjam2020
{
	public class UnitSlot : MonoBehaviour
	{
		[SerializeField] private float unitRotationSpeed = 360.0f;
		public Unit UnitInSlot { get; private set; }
		public bool IsOccupied => UnitInSlot != null;
		private void OnTriggerEnter(Collider other)
		{
			if (UnitInSlot == null)
			{
				UnitInSlot = other.GetComponent<Unit>();
			}
		}

		private void OnTriggerExit(Collider other)
		{
			var unit = other.GetComponent<Unit>();
			if (unit == UnitInSlot)
			{
				UnitInSlot = null;
			}
		}

		private void Update()
		{
			if (UnitInSlot != null && Vector3.Distance(UnitInSlot.transform.position, UnitInSlot.GetComponent<UnitMovementController>().TargetPoint) < 0.001f)
			{
				UnitInSlot.transform.rotation = Quaternion.RotateTowards(UnitInSlot.transform.rotation,
					transform.rotation, unitRotationSpeed * Time.deltaTime);
			}
		}
	}
}
