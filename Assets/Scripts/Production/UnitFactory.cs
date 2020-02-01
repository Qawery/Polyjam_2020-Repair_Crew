using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Polyjam2020
{
	[System.Serializable]
	public class UnitProductionData
	{
		public Unit Unit;
		public int ProductionCost;
		public int ProductionTime; //ignored for now
	}

	public enum ProductionPossibilityStatus
	{
		OK,
		MISSING_RESOURCES,
		NO_SPAWN_SLOT_AVAILABLE
	}
	
	public class UnitFactory : MonoBehaviour
	{
		[SerializeField] private List<UnitProductionData> unitProductionData;
		private ResourceManager resourceManager;
		private Node node;
		
		public List<UnitProductionData> UnitProductionData => unitProductionData;

		void Awake()
		{
			resourceManager = FindObjectOfType<ResourceManager>();
			Assert.IsNotNull(resourceManager, $"There is no ResourceManager on scene, but UnitFactory {name} relies on it");
			
			node = GetComponent<Node>();
			Assert.IsNotNull(node, $"Node component missing on {name}");
		}

		public ProductionPossibilityStatus CheckProductionPossibility(int optionIndex)
		{
			Assert.IsTrue(optionIndex > -1 && optionIndex < unitProductionData.Count);
			var productionData = unitProductionData[optionIndex];
			if (productionData.ProductionCost > resourceManager.ResourcesRemaining)
			{
				return ProductionPossibilityStatus.MISSING_RESOURCES;
			}

			if (!node.UnitSlots.Exists(slot => !slot.IsOccupied))
			{
				return ProductionPossibilityStatus.NO_SPAWN_SLOT_AVAILABLE;
			}

			return ProductionPossibilityStatus.OK;
		}

		public Unit ProduceUnit(int optionIndex)
		{
			Assert.IsTrue(optionIndex > -1 && optionIndex < unitProductionData.Count);
			var productionData = unitProductionData[optionIndex];
			Assert.IsTrue(CheckProductionPossibility(optionIndex) == ProductionPossibilityStatus.OK, $"Production at {name} is not possible");
			resourceManager.SpendResources(productionData.ProductionCost);
			var spawnSlot = node.UnitSlots.Find(slot => !slot.IsOccupied);
			var producedUnit = World.Instance.InstantiateObject(productionData.Unit);
			producedUnit.transform.position = spawnSlot.transform.position;
			producedUnit.transform.rotation = spawnSlot.transform.rotation;
			return producedUnit;
		}
	}
}
