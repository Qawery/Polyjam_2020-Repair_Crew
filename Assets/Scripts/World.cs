using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
 public class SpawnHandlerMethod : Attribute
 {
 }

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class DestroyHandlerMethod : Attribute
{
}

public class World : MonoBehaviour
{
	private static World instance;
	public static World Instance
	{
		get
		{
			Init();
			return instance;
		}
	}

	private static void Init()
	{
		if (instance == null)
		{
			instance = FindObjectOfType<World>();
			instance.ProcesPrespawnedObjects(SceneManager.GetActiveScene().GetRootGameObjects());
		}
	}

	private void Awake()
	{
		Init();
	}

	private readonly Dictionary<System.Type, List<(object listener, MethodInfo method)>> componentSpawnEventMapping 
		= new Dictionary<Type, List<(object, MethodInfo)>>();
	private readonly Dictionary<System.Type, List<(object listener, MethodInfo method)>> componentDestroyEventMapping 
		= new Dictionary<Type, List<(object, MethodInfo)>>();

	public void RegisterComponentListener(object listener)
	{
		var allMethods = listener.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		foreach (var method in allMethods)
		{
			//TODO: copy-paste programming
			if (method.GetCustomAttribute<SpawnHandlerMethod>() != null)
			{
				var methodParameters = method.GetParameters();
				Assert.AreEqual(methodParameters.Length, 1);

				var parameterType = methodParameters[0].ParameterType;
				Assert.IsTrue(parameterType.IsSubclassOf(typeof(MonoBehaviour)));
				if (!componentSpawnEventMapping.TryGetValue(parameterType, out var objectList))
				{
					objectList = new List<(object, MethodInfo)>();
					componentSpawnEventMapping[parameterType] = objectList;
				}

				objectList.Add((listener, method));
			}
			
			if (method.GetCustomAttribute<DestroyHandlerMethod>() != null)
			{
				var methodParameters = method.GetParameters();
				Assert.AreEqual(methodParameters.Length, 1);

				var parameterType = methodParameters[0].ParameterType;
				Assert.IsTrue(parameterType.IsSubclassOf(typeof(MonoBehaviour)));
				if (!componentDestroyEventMapping.TryGetValue(parameterType, out var objectList))
				{
					objectList = new List<(object, MethodInfo)>();
					componentDestroyEventMapping[parameterType] = objectList;
				}
				
				objectList.Add((listener, method));
			}
		}
	}

	//TODO: copy paste programming
	private void InvokeSpawnListenerMethods(MonoBehaviour spawnedComponent)
	{
		var currentType = spawnedComponent.GetType();
		while (currentType != null && currentType != typeof(MonoBehaviour))
		{
			if (componentSpawnEventMapping.TryGetValue(currentType, out var methodList))
			{
				foreach (var (listener, method) in methodList)
				{
					method.Invoke(listener, new object[] {spawnedComponent});
				}
			}

			currentType = currentType.BaseType;
		}
	}

	private void InvokeDestroyListenerMethods(MonoBehaviour destroyedComponent)
	{
		var currentType = destroyedComponent.GetType();
		while (currentType != null && currentType != typeof(MonoBehaviour))
		{
			if (componentDestroyEventMapping.TryGetValue(currentType, out var methodList))
			{
				foreach (var (listener, method) in methodList)
				{
					method.Invoke(listener, new object[] {destroyedComponent});
				}
			}

			currentType = currentType.BaseType;
		}
	}

	
	public GameObject Instantiate(GameObject originalObject)
	{
		var spawnedObject = GameObject.Instantiate(originalObject);
		ProcessSpawnedObject(spawnedObject);
		return spawnedObject;
	}

	public void Destroy(GameObject objectToDestroy)
	{
		ProcessDestroyedObject(objectToDestroy);
		GameObject.Destroy(objectToDestroy);
	}

	private void ProcesPrespawnedObjects(IEnumerable<GameObject> prespawnedObjects)
	{
		var allMonoBehaviors = new List<MonoBehaviour>();
		foreach (var prespawnedObject in prespawnedObjects)
		{
			allMonoBehaviors.AddRange(prespawnedObject.GetComponentsInChildren<MonoBehaviour>());
		}

		foreach (var component in allMonoBehaviors)
		{
			RegisterComponentListener(component);
		}

		foreach (var component in allMonoBehaviors)
		{
			InvokeSpawnListenerMethods(component);
		}
	}
	
	private void ProcessSpawnedObject(GameObject spawnedObject)
	{
		var monoBehaviours = spawnedObject.GetComponentsInChildren<MonoBehaviour>();
		foreach (var component in monoBehaviours)
		{
			InvokeSpawnListenerMethods(component);
			RegisterComponentListener(component);
		}
	}

	private void ProcessDestroyedObject(GameObject destroyedObject)
	{
		var monoBehaviours = destroyedObject.GetComponentsInChildren<MonoBehaviour>();
		foreach (var component in monoBehaviours)
		{
			InvokeDestroyListenerMethods(component);
		}
	}
}
