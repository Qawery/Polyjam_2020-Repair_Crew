using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class NodeOverGUI : MonoBehaviour
	{
		[SerializeField] private Node node = null;
		[SerializeField] private Slider healthBar = null;


		private void Awake()
		{
			Assert.IsNotNull(node);
			Assert.IsNotNull(healthBar);
			var health = node.GetComponent<HealthComponent>();
			health.OnValueChanged += OnHealthChanged;
			OnHealthChanged((health.CurrentValue, health.CurrentValue));
		}

		private void OnHealthChanged((int previous, int current) data)
		{
			if (data.current > 0.0f)
			{
				healthBar.normalizedValue = ((float)data.current) / node.GetComponent<HealthComponent>().MaxValue;
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
}
