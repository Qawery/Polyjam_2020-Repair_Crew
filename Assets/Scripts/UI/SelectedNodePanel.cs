using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class SelectedNodePanel : MonoBehaviour
	{
		[SerializeField] private TMPro.TextMeshProUGUI nameText = null;
		private List<Button> existingButtons = new List<Button>();
		[SerializeField] private GameObject buttonParent = null;
		[SerializeField] private Button buttonPrefab = null;


		private void Awake()
		{
			Assert.IsNotNull(nameText);
			Assert.IsNotNull(buttonParent);
			Assert.IsNotNull(buttonPrefab);
			var nodeController = FindObjectOfType<NodeController>();
			Assert.IsNotNull(nodeController);
			nodeController.OnSelectedNodeChanged += OnSelectedNodeChanged;
			existingButtons.AddRange(buttonParent.GetComponentsInChildren<Button>());
			foreach (var button in existingButtons)
			{
				button.gameObject.SetActive(false);
			}
			gameObject.SetActive(false);
		}

		private void OnSelectedNodeChanged(Node node)
		{
			foreach (var button in existingButtons)
			{
				button.gameObject.SetActive(false);
				button.onClick.RemoveAllListeners();
			}
			if (node != null)
			{
				string name = "City";
				var factory = node.GetComponent<UnitFactory>();
				int buttonIndex = 0;
				if (factory != null)
				{
					name += ", Factory";
					int productIndex = 0;
					while (existingButtons.Count < factory.UnitProductionData.Count)
					{
						var newButton = Instantiate(buttonPrefab, buttonParent.transform).GetComponent<Button>();
						existingButtons.Add(newButton);
						newButton.gameObject.SetActive(false);
					}
					foreach (var productionData in factory.UnitProductionData)
					{
						var button = existingButtons[buttonIndex];
						button.gameObject.SetActive(true);
						button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"Produce {productionData.Unit.UnitClass.DisplayName}";
						int indexCopy = productIndex;
						button.onClick.AddListener(() => { factory.ProduceUnit(indexCopy);});
						button.interactable = (factory.CheckProductionPossibility(productIndex) == ProductionPossibilityStatus.OK);
						++productIndex;
						++buttonIndex;
					}
				}
				var scrapyard = node.GetComponent<Scrapyard>();
				if (scrapyard != null) 
				{
					name += ", Scrapyard";
					foreach (var unitSlot in node.UnitSlots)
					{
						if (unitSlot.IsOccupied)
						{
							var unit = unitSlot.UnitInSlot;
							var button = existingButtons[buttonIndex];
							button.gameObject.SetActive(true);
							button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"Scrap {unit.UnitClass.DisplayName}";
							button.onClick.AddListener(() => { scrapyard.ScrapUnit(unit); button.gameObject.SetActive(false);});
							++buttonIndex;
						}
					}
				}
				nameText.text = name;
				var health = node.GetComponent<HealthComponent>();
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
}
