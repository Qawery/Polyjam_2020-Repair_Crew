using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Polyjam2020
{
	public class SelectedNodePanel : MonoBehaviour
	{

		[SerializeField] private Text nameText;
		[SerializeField] private Text healthText;
		[SerializeField] private Text unitSlotText;
		[SerializeField] private Text edgeText;
		[SerializeField] private GameObject buttonParent;
		[SerializeField] private GameObject buttonPrefab;
		private void Awake()
		{
			var nodeController = FindObjectOfType<NodeController>();
			nodeController.OnSelectedNodeChanged += OnSelectedNodeChanged;
			gameObject.SetActive(false);
		}

		private void OnSelectedNodeChanged(Node node)
		{
			for (int childIndex = buttonParent.transform.childCount - 1; childIndex > -1; --childIndex)
			{
				Destroy(buttonParent.transform.GetChild(childIndex).gameObject);
			}
			
			if (node != null)
			{
				string text = node.name;
				var factory = node.GetComponent<UnitFactory>();
				if (factory != null)
				{
					text += "[factory]";
					int productIndex = 0;
					foreach (var productionData in factory.UnitProductionData)
					{
						var button = Instantiate(buttonPrefab, buttonParent.transform).GetComponent<Button>();
						button.GetComponentInChildren<Text>().text = $"Produce {productionData.Unit.UnitClass.DisplayName}";
						int indexCopy = productIndex;
						button.onClick.AddListener(() => { factory.ProduceUnit(indexCopy);});
						button.interactable = (factory.CheckProductionPossibility(productIndex) == ProductionPossibilityStatus.OK);
						productIndex++;
					}
				}

				var scrapyard = node.GetComponent<Scrapyard>();
				if (scrapyard != null) 
				{
					text += "[scrapyard]";
					foreach (var unitSlot in node.UnitSlots)
					{
						if (unitSlot.IsOccupied)
						{
							var unit = unitSlot.UnitInSlot;
							var button = Instantiate(buttonPrefab, buttonParent.transform).GetComponent<Button>();
							button.GetComponentInChildren<Text>().text = $"Scrap {unit.name}";
							button.onClick.AddListener(() => { scrapyard.ScrapUnit(unit); Destroy(button.gameObject);});
						}
					}
				}

				nameText.text = text;
				var health = node.GetComponent<HealthComponent>();
				healthText.text = $"Health: {health.CurrentValue} / {health.MaxValue}";
				unitSlotText.text = $"Unit Slots: {node.UnitSlots.Count(slot => !slot.IsOccupied)} / {node.UnitSlots.Count}";
				edgeText.text = $"Edges: {node.Edges.Count}";
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
}
