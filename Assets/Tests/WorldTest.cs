using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Polyjam2020.Tests
{
    public class BaseMonoBehavior : MonoBehaviour
    {
        
    }

    public class DerivedMonoBehavior : BaseMonoBehavior
    {
        
    }

    public class BaseListener : MonoBehaviour
    {
        public int InstancesCounted { get; private set; } = 0;
        [SpawnHandlerMethod]
        protected virtual void OnSpawned(BaseMonoBehavior baseMonoBehavior)
        {
            InstancesCounted++;
        }

        [DestroyHandlerMethod]
        protected void DestroyHandler(BaseMonoBehavior baseMonoBehavior)
        {
            InstancesCounted--;
        }
    }

    public class DerivedListener : BaseListener
    {
        public int InstancesCountedInDerivedListener { get; private set; } = 0;
        public int DerivedTypesCounted { get; private set; } = 0;
        protected override void OnSpawned(BaseMonoBehavior baseMonoBehavior)
        {
            base.OnSpawned(baseMonoBehavior);
            InstancesCountedInDerivedListener++;
        }

        [SpawnHandlerMethod]
        private void OnDerivedSpawned(DerivedMonoBehavior derivedMonoBehavior)
        {
            DerivedTypesCounted++;
        }
        
        [DestroyHandlerMethod]
        private void OnDestroyed(DerivedMonoBehavior derivedMonoBehavior)
        {
            DerivedTypesCounted--;
        }
    }
    
    public class WorldTest
    {
        [UnityTest]
        public IEnumerator WorldTestWithEnumeratorPasses()
        {
            var baseListener = new GameObject().AddComponent<BaseListener>();
            
            (new GameObject()).AddComponent<World>();
            
            var basePrefab = new GameObject();
            basePrefab.AddComponent<BaseMonoBehavior>();
            var derivedPrefab = new GameObject();
            derivedPrefab.AddComponent<DerivedMonoBehavior>();

            var derivedListenerPrefab = new GameObject();
            derivedListenerPrefab.AddComponent<DerivedListener>();

            var base1 = World.Instance.Instantiate(basePrefab);
            var derived1 = World.Instance.Instantiate(derivedPrefab);
            
            Assert.IsTrue(baseListener.InstancesCounted == 2);
            
            var derivedListener = World.Instance.Instantiate(derivedListenerPrefab).GetComponent<DerivedListener>();
            Assert.IsTrue(derivedListener.InstancesCounted == 0);
            Assert.IsTrue(derivedListener.InstancesCountedInDerivedListener == 0);
            Assert.IsTrue(derivedListener.DerivedTypesCounted == 0);
            
            var base2 = World.Instance.Instantiate(basePrefab);
            var derived2 = World.Instance.Instantiate(derivedPrefab);
            Assert.IsTrue(baseListener.InstancesCounted == 4);
            Assert.IsTrue(derivedListener.InstancesCounted == 2);
            Assert.IsTrue(derivedListener.InstancesCountedInDerivedListener == 2);
            Assert.IsTrue(derivedListener.DerivedTypesCounted == 1);
            
            World.Instance.Destroy(base1);
            World.Instance.Destroy(base2);
            World.Instance.Destroy(derived1);
            World.Instance.Destroy(derived2);
            
            Assert.IsTrue(baseListener.InstancesCounted == 0);
            Assert.IsTrue(derivedListener.InstancesCounted == -2);
            Assert.IsTrue(derivedListener.InstancesCountedInDerivedListener == 2);
            Assert.IsTrue(derivedListener.DerivedTypesCounted == -1);
            
            yield return null;
        }
    }
}
