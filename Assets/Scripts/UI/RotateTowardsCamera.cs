﻿using UnityEngine;
using UnityEngine.Assertions;


namespace Polyjam2020
{
	public class RotateTowardsCamera : MonoBehaviour
	{
		private Camera camera = null;


		private void Awake()
		{
			camera = Object.FindObjectOfType<Camera>();
			Assert.IsNotNull(camera);
		}

		private void LateUpdate()
		{
			transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
		}
	}
}