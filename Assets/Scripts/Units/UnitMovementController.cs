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

        private bool isDecelerating = false;
        private float deceleration = 0.0f;

        public Vector3 TargetPoint { get; private set; }
        public float CurrentSpeed { get; private set; }
        public float CurrentRotationSpeed { get; private set; }

        public float MaxSpeed => maxSpeed;

        private void Awake()
        {
            StopMovement();
        }

        public void MoveToPoint(Vector3 target)
        {
            TargetPoint = target.Flat() + transform.position.y * Vector3.up;
            isDecelerating = false;
        }

        public void StopMovement()
        {
            MoveToPoint(transform.position);
        }

        private const float rotationTolerance = 0.01f;
        private const float speedTolerance = 0.01f;
        private const float distanceTolerance = 0.01f;
        private void Update()
        {
            Vector3 toTarget = TargetPoint - transform.position;
            float distanceFromTarget = toTarget.magnitude;
            if (distanceFromTarget < distanceTolerance)
            {
                CurrentSpeed = 0.0f;
                transform.position = TargetPoint;
                return;
            }
        
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

            if (distanceFromTarget <= decelerationDistance)
            {
                if (!isDecelerating)
                {
                    isDecelerating = true;
                    deceleration = 0.5f * CurrentSpeed * CurrentSpeed / decelerationDistance;
                }

                CurrentSpeed -= deceleration * Time.deltaTime;
                if (CurrentSpeed < speedTolerance)
                {
                    CurrentSpeed = 0;
                    transform.position = TargetPoint;
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