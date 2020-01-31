using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;


namespace BaseProject
{
	public class UnityEventUpdateManager : MonoBehaviour, IUpdateBroadcaster
	{
		private List<object> pendingObjectsToSubscribe = new List<object>();
		private List<object> pendingObjectsToUnsubscribe = new List<object>();
		private Dictionary<UpdatePhases, Dictionary<object, List<MethodInfo>>> eventsPerUpdatePhase = new Dictionary<UpdatePhases, Dictionary<object, List<MethodInfo>>>();


		private void Awake()
		{
			for (int i = 0; i < Enum.GetNames(typeof (UpdatePhases)).Length; ++i)
			{
				eventsPerUpdatePhase[(UpdatePhases) i] = new Dictionary<object, List<MethodInfo>>();
			}
		}

		private void Update()
		{
			foreach (var pendingObject in pendingObjectsToSubscribe)
			{
				RegisterUpdateFunctionsOfObject(pendingObject);
			}
			pendingObjectsToSubscribe.Clear();
			foreach (var pendingObject in pendingObjectsToUnsubscribe)
			{
				UnregisterUpdateFunctionsOfObject(pendingObject);
			}
			pendingObjectsToUnsubscribe.Clear();
			for (int i = 0; i < (int) UpdatePhases.POST_PHYSICS; ++i)
			{
				foreach (var objectMethods in eventsPerUpdatePhase[(UpdatePhases) i])
				{
					foreach (var methodInfo in objectMethods.Value)
					{
						methodInfo.Invoke(objectMethods.Key, null);
					}
				}
			}
		}

		private void LateUpdate()
		{
			for (int i = (int) UpdatePhases.POST_PHYSICS; i < Enum.GetNames(typeof(UpdatePhases)).Length; ++i)
			{
				foreach (var objectMethods in eventsPerUpdatePhase[(UpdatePhases)i])
				{
					foreach (var methodInfo in objectMethods.Value)
					{
						methodInfo.Invoke(objectMethods.Key, null);
					}
				}
			}
		}

		private void RegisterUpdateFunctionsOfObject(object subscriberObject)
		{
			var methods = subscriberObject.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			foreach (var method in methods)
			{
				var attributes = method.GetCustomAttributes(typeof(UpdateFunction), true);
				foreach (var attribute in attributes)
				{
					if (!eventsPerUpdatePhase[(attribute as UpdateFunction).UpdatePhase].ContainsKey(subscriberObject))
					{
						eventsPerUpdatePhase[(attribute as UpdateFunction).UpdatePhase].Add(subscriberObject, new List<MethodInfo>());
					}
					eventsPerUpdatePhase[(attribute as UpdateFunction).UpdatePhase][subscriberObject].Add(method);
				}
			}
		}

		private void UnregisterUpdateFunctionsOfObject(object subscriberObject)
		{
			bool objectFound = false;
			foreach (var updatePhaseSubscribers in eventsPerUpdatePhase.Values)
			{
				if (!objectFound && updatePhaseSubscribers.ContainsKey(subscriberObject))
				{
					objectFound = true;
				}
				updatePhaseSubscribers.Remove(subscriberObject);
			}
			Assert.IsTrue(objectFound, "Object: " + subscriberObject.ToString() + " is being unregistered while not registered on: " + gameObject.name);
		}

		void IUpdateBroadcaster.AddUpdateSubscriber(object subscriberObject)
		{
			Assert.IsFalse(pendingObjectsToSubscribe.Contains(subscriberObject), "Object: " + subscriberObject.ToString() + "is already pending to subscribe on: " + gameObject.name);
			if (pendingObjectsToUnsubscribe.Contains(subscriberObject))
			{
				pendingObjectsToUnsubscribe.Remove(subscriberObject);
			}
			else
			{
				pendingObjectsToSubscribe.Add(subscriberObject);
			}
		}

		void IUpdateBroadcaster.RemoveUpdateSubscriber(object subscriberObject)
		{
			Assert.IsFalse(pendingObjectsToUnsubscribe.Contains(subscriberObject), "Object: " + subscriberObject.ToString() + "is already pending to unsubscribe on: " + gameObject.name);
			if (pendingObjectsToSubscribe.Contains(subscriberObject))
			{
				pendingObjectsToSubscribe.Remove(subscriberObject);
			}
			else
			{
				pendingObjectsToUnsubscribe.Add(subscriberObject);
			}
		}
	}
}