using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Polyjam2020
{
    public class UnitMovementController : MonoBehaviour
    {
        [SerializeField] private float maxSpeed = 5.0f;
        [SerializeField] private float acceleration = 5.0f;
        [SerializeField] private float decelerationDistance = 2.0f;
        [SerializeField] private float maxRotationSpeedInDegrees = 360.0f;
        [SerializeField] private float rotationAccelerationInDegrees = 360.0f;

        public Vector3 TargetPoint { get; private set; }
        public float CurrentSpeed { get; private set; }
        public float CurrentRotationSpeed { get; private set; }

        public float MaxSpeed => maxSpeed;
        
        public void MoveToPoint(Vector3 target)
        {
            TargetPoint = target;
        }

        public void StopMovement()
        {
            TargetPoint = transform.position;
            CurrentSpeed = 0;
        }

        private const float rotationTolerance = 0.0001f;
        private const float speedTolerance = 0.0001f;
        private void Update()
        {
            Vector3 toTarget = (TargetPoint - transform.position).Flat();
            Quaternion targetRotation = Quaternion.LookRotation(toTarget.normalized);
            float angleRemaining = Quaternion.Angle(transform.rotation, targetRotation);
            if (angleRemaining > rotationTolerance)
            {
                if (CurrentRotationSpeed < maxRotationSpeedInDegrees)
                {
                    CurrentRotationSpeed += rotationAccelerationInDegrees * Time.deltaTime;
                }
                Mathf.Min(CurrentRotationSpeed, maxRotationSpeedInDegrees);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                    CurrentRotationSpeed * Time.deltaTime);
                return;
            }

            float distanceFromTarget = toTarget.magnitude;
            if (distanceFromTarget <= decelerationDistance)
            {
                float deceleration = 0.5f * maxSpeed * maxSpeed / decelerationDistance;
                CurrentSpeed -= deceleration * Time.deltaTime;
                if (CurrentSpeed < speedTolerance)
                {
                    CurrentSpeed = 0;
                    transform.position = new Vector3(TargetPoint.x, 0, TargetPoint.z);
                }
            }
            else if (CurrentSpeed < maxSpeed)
            {
                CurrentSpeed += acceleration * Time.deltaTime;
            }
            CurrentSpeed = Mathf.Min(CurrentSpeed, maxSpeed);

            transform.position = Vector3.MoveTowards(transform.position, TargetPoint, CurrentSpeed * Time.deltaTime);
        }
    }
}