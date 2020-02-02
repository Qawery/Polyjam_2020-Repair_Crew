using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;


namespace Polyjam2020.Tests
{
    public class ProductionAndScrappingTest
    {
        const string SCENE_NAME = "ProductionAndScrappingTest";


        [UnityTest]
        public IEnumerator ProductionTest()
        {
            SceneManager.LoadScene("ProductionAndScrappingTest");
            yield return null;

            var helper = Object.FindObjectOfType<ProductionTestHelper>();
            var resourceManager = Object.FindObjectOfType<ResourceManager>();
            
            Assert.IsNotNull(helper);
            Assert.IsNotNull(resourceManager);
            
            var factory = helper.FactoryToFillOut;
            int slotCount = factory.GetComponent<Node>().UnitSlots.Count;

            for (int i = 0; i < slotCount; ++i)
            {
                helper.FactoryToFillOut.ProduceUnit(0);
                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
            Assert.IsFalse(factory.GetComponent<Node>().UnitSlots.Exists(slot => !slot.IsOccupied));
            Assert.IsTrue(resourceManager.ResourcesRemaining == (resourceManager.MaxResources - slotCount * helper.FactoryToFillOut.UnitProductionData[0].ProductionCost));
        }
        
        [UnityTest]
        public IEnumerator ScrappingTest()
        {
            SceneManager.LoadScene("ProductionAndScrappingTest");
            yield return null;


            var helper = Object.FindObjectOfType<ProductionTestHelper>();
            var resourceManager = Object.FindObjectOfType<ResourceManager>();
            
            Assert.IsNotNull(helper);
            Assert.IsNotNull(resourceManager);
            
            var units = Object.FindObjectsOfType<Unit>();
            int slotIndex = 0;
            foreach (var unit in units)
            {
                unit.transform.position = helper.Scrapyard.GetComponent<Node>().UnitSlots[slotIndex].transform.position;
                ++slotIndex;
            }
            
            yield return new WaitForSeconds(0.1f);

            foreach (var unit in units)
            {
                helper.Scrapyard.ScrapUnit(unit);
            }
            
            yield return null;

            Assert.IsTrue(resourceManager.ResourcesRemaining == resourceManager.MaxResources);
        }
        
        [UnityTest]
        public IEnumerator IntegrationTest()
        {
            SceneManager.LoadScene(SCENE_NAME);
            yield return null;

            var helper = Object.FindObjectOfType<ProductionTestHelper>();
            var resourceManager = Object.FindObjectOfType<ResourceManager>();
            
            Assert.IsNotNull(helper);
            Assert.IsNotNull(resourceManager);
            
            var unit = helper.NormalFactory.ProduceUnit(0);
            unit.GetComponent<UnitMovementController>().MoveToPoint( helper.Scrapyard.GetComponent<Node>().UnitSlots[0].transform.position);
            yield return new WaitForSeconds(5.0f);
            helper.Scrapyard.ScrapUnit(unit);
            yield return null;
            Assert.IsNull(unit);
        }
    }
}
