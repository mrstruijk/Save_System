using System.Collections.Generic;
using _mrstruijk.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;


namespace _mrstruijk.SaveSystem
{
	[CreateAssetMenu(menuName = "mrstruijk/SaveSystem/ObjectsToLoadManager", fileName = "ObjectsToLoadManager")]
	public class ObjectsToLoadManagerSO : ScriptableObject
	{
		public List<GameObject> prefabs;


		public void OnLoad(string saveFile)
		{
			EventSystem.Invoke(EventSystem.OnLoadAction);

			SaveData.current = (SaveData) SerializationManager.Load(saveFile);

			for (int i = 0; i < SaveData.current.objectsData.Count; i++)
			{
				var currentObject = SaveData.current.objectsData[i];

				GameObject obj = null;
				foreach (var prefab in prefabs)
				{
					if (prefab.GetComponent<ObjectHandler>().objectData.objectType == currentObject.objectType)
					{
						obj = Instantiate(prefab);
						break;
					}
				}

				if (obj == null)
				{
					Debug.LogError("Could not locate: " + currentObject.objectType + " in list");
					return;
				}

				var handler = obj.GetComponent<ObjectHandler>();
				handler.objectData = currentObject;

				var handlerTransform = handler.transform;
				handlerTransform.position = currentObject.position;
				handlerTransform.rotation = currentObject.rotation;
			}
		}


		[Button]
		private void SpawnRandom()
		{
			Instantiate(prefabs[Random.Range(0, prefabs.Count)]);
		}
	}
}
