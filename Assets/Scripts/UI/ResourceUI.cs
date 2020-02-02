using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class ResourceUI : MonoBehaviour
	{
		[SerializeField] private TMPro.TextMeshProUGUI resourceText = null;


		private void Awake()
		{
			Assert.IsNotNull(resourceText);
			FindObjectOfType<ResourceManager>().OnResourceAmountChanged += OnResourceAmountChanged;
		}

		private void OnResourceAmountChanged((int previous, int current) resourceChangeData)
		{
			resourceText.text = $"Resources: {resourceChangeData.current}";
		}
	}
}