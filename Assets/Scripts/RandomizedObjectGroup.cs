using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polyjam2020
{
    public class RandomizedObjectGroup : MonoBehaviour
    {
        [SerializeField] private float minScaleFactor = 0.6f;
        [SerializeField] private float maxScaleFactor = 1.1f;
        [SerializeField] private float minRotation = 0.0f;
        [SerializeField] private float maxRotation = 360.0f;
        private void Awake()
        {
            for (int childIndex = 0; childIndex < transform.childCount; ++childIndex)
            {
                Transform child = transform.GetChild(childIndex);
                float scale = Random.Range(minScaleFactor, maxScaleFactor);
                float rotationAngle = Random.Range(minRotation, maxRotation);
                child.localScale *= scale;
                child.rotation *= Quaternion.AngleAxis(rotationAngle, Vector3.forward);
            }
        }
    }
}
