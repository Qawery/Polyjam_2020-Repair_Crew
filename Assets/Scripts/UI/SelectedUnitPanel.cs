using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class SelectedUnitPanel : MonoBehaviour
	{
		[SerializeField] private TMPro.TextMeshProUGUI nameText = null;


		private void Awake()
		{
			Assert.IsNotNull(nameText);
			var unitController = FindObjectOfType<UnitController>();
			Assert.IsNotNull(unitController);
			unitController.OnSelectedUnitChanged += OnSelectedUnitChanged;
			gameObject.SetActive(false);
		}

		private void OnSelectedUnitChanged(Unit unit)
		{
			if (unit != null)
			{
				nameText.text = unit.UnitClass.DisplayName;
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
}
