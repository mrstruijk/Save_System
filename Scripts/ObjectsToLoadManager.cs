using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;


namespace _mrstruijk.Components.SaveSystem.Scripts
{
	public class ObjectsToLoadManager : MonoBehaviour
	{
		public List<GameObject> prefabs;


		public void OnLoad(string saveFile)
		{
			Debug.Log("Loading object");

			GameEvents.current.onLoadEvent?.Invoke();

			SaveData.current = (SaveData) SerializationManager.Load(saveFile);

			Debug.Log("ToyCount in save: " + saveFile + " = " + SaveData.current.objectsData.Count);

			for (int i = 0; i < SaveData.current.objectsData.Count; i++)
			{
				var currentToy = SaveData.current.objectsData[i];

				Debug.Log(currentToy.objectType + " is being added");

				GameObject obj = null;
				foreach (var prefab in prefabs)
				{
					if (prefab.GetComponent<ObjectHandler>().objectData.objectType == currentToy.objectType)
					{
						obj = Instantiate(prefab);
						break;
					}
				}

				if (obj == null)
				{
					Debug.LogError("Could not locate: " + currentToy.objectType + " in list");
					return;
				}

				Debug.Log(obj.name);

				var objectHandler = obj.GetComponent<ObjectHandler>();
				objectHandler.objectData = currentToy;

				objectHandler.transform.position = currentToy.position;
				objectHandler.transform.rotation = currentToy.rotation;
			}
		}


		[Button]
		private void SpawnRandom()
		{
			Instantiate(prefabs[Random.Range(0, prefabs.Count)]);
		}
	}
}
