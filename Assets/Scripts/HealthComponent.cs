using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class HealthComponent : MonoBehaviour
	{
		[SerializeField] private int maxValue = 100;
		private int currentValue = 1;
		public event System.Action<(int previous, int current)> OnValueChanged;


		public int MaxValue => maxValue;
		public int CurrentValue
		{
			get => currentValue;
			private set
			{
				int previous = currentValue;
				currentValue = value;
				OnValueChanged?.Invoke((previous, currentValue));
			}
		}


		private void Awake()
		{
			CurrentValue = MaxValue;
		}
		
		public void ApplyDamage(int value)
		{
			Assert.IsTrue(value > 0, "Trying to apply damage not greater than zero on: " + gameObject.name);
			if (CurrentValue > 0)
			{
				CurrentValue -= value;
				CurrentValue = Mathf.Max(CurrentValue, 0);
			}
		}

		public void ApplyHeal(int value)
		{
			Assert.IsTrue(value > 0, "Trying to apply heal not greater than zero on: " + gameObject.name);
			if (CurrentValue > 0)
			{
				CurrentValue += value;
				CurrentValue = Mathf.Min(CurrentValue, maxValue);
			}
		}
	}
}
