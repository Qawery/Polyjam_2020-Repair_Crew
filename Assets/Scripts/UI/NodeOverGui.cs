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
			node.OnHealthChanged += OnHealthChanged;
			OnHealthChanged();
		}

		private void OnHealthChanged()
		{
			if (node.CurrentHealth > 0.0f)
			{
				healthBar.normalizedValue = node.CurrentHealth / Node.MAX_HEALTH;
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
}
