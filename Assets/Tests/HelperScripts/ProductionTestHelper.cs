using UnityEngine;

namespace Polyjam2020.Tests
{
	public class ProductionTestHelper : MonoBehaviour
	{
		[SerializeField] private UnitFactory factoryToFillOut;
		[SerializeField] private UnitFactory normalFactory;
		[SerializeField] private Scrapyard scrapyard;

		public UnitFactory FactoryToFillOut => factoryToFillOut;
		public UnitFactory NormalFactory => normalFactory;
		public Scrapyard Scrapyard => scrapyard;
	}
}
