using System;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	public static World Instance { get; private set; }
	//Dictionary<System.Type, List<(object, System)> >

	private void Awake()
	{
		if (Instance != this)
		{
			Destroy(Instance.gameObject);
		}
		Instance = this;
	}

	public GameObject Instantiate(GameObject originalObject)
	{
		var spawnedObject = GameObject.Instantiate(originalObject);
		return spawnedObject;
	}

	public void Destroy(GameObject objectToDestroy)
	{
		GameObject.Destroy(objectToDestroy);
	}
}
