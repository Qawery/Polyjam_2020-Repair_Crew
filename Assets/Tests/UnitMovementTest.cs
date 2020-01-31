using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Polyjam2020.Tests
{
    public class UnitMovementTest
    {
        private UnitMovementController movementController;
        
        [UnityTest]
        public IEnumerator UnitMovementTestWithEnumeratorPasses()
        {
            SceneManager.LoadScene("UnitMovementTest");
            yield return null;
            var movement = Object.FindObjectOfType<UnitMovementController>();
            var points = Object.FindObjectOfType<PointSet>().Points;

            foreach (var currentPoint in points)
            {
                movement.MoveToPoint(currentPoint.transform.position);
                yield return new WaitForSeconds(2.0f);
                while (movement.CurrentSpeed > 0.0001f)
                {
                    yield return new WaitForEndOfFrame();
                }

                float distance = Vector3.Distance(movement.transform.position.Flat(), currentPoint.position.Flat());
                Assert.IsTrue(distance < 0.01f, $"Distance is {distance}");
            }
        }
    }
}
