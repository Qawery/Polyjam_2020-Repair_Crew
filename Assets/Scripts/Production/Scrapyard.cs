using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Polyjam2020
{
	[System.Serializable]
	public class UnitScrapData
	{
		public UnitClass UnitClass;
		public int GainFromScrapping;
		public int ScrapTime; //ignored for now
	}
	
	public class Scrapyard : MonoBehaviour
	{
		[SerializeField] private List<UnitScrapData> unitScrapData;
		
		private ResourceManager resourceManager;
		private Node node;
		
		public List<UnitScrapData> UnitScrapData => unitScrapData;
		void Awake()
		{
			resourceManager = FindObjectOfType<ResourceManager>();
			Assert.IsNotNull(resourceManager, $"There is no ResourceManager on scene, but Scrapyard {name} relies on it");
			
			node = GetComponent<Node>();
			Assert.IsNotNull(node, $"Node component missing on {name}");
		}

		public bool CanBeScrapped(Unit unit)
		{
			return unitScrapData.Exists(data => data.UnitClass == unit.UnitClass);
		}
		
		public void ScrapUnit(Unit unit)
		{
			Assert.IsTrue(node.UnitSlots.Exists(slot => slot.UnitInSlot == unit), $"Trying to scrap unit that's not in scrapyard node {name}");
			var scrapData = unitScrapData.Find(data => data.UnitClass == unit.UnitClass);
			Assert.IsNotNull(scrapData);
			World.Instance.DestroyObject(unit.gameObject);
			resourceManager.RestoreResources(scrapData.GainFromScrapping);
		}
	}
}
