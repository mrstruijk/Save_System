using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;



namespace _mrstruijk.Components.SaveSystem.Scripts
{
	public class ToyManager : MonoBehaviour
	{
		public List<GameObject> toys;


		public void OnSave(string saveName)
		{
			SerializationManager.Save(saveName, SaveData.current);
		}


		public void OnLoad(string saveName)
		{
			Debug.Log("Loading object");

			GameEvents.current.onLoadEvent?.Invoke();

			SaveData.current = (SaveData) SerializationManager.Load(saveName);

			for (int i = 0; i < SaveData.current.toys.Count; i++)
			{
				var currentToy = SaveData.current.toys[i];
				var obj = Instantiate(toys[(int) currentToy.toyType]);
				var toyHandler = obj.GetComponent<ToyHandler>();
				toyHandler.toyData = currentToy;
				toyHandler.transform.position = currentToy.position;
				toyHandler.transform.rotation = currentToy.rotation;
			}
		}


		[Button]
		private void SpawnRandom()
		{
			Instantiate(toys[Random.Range(0, toys.Count)]);
		}
	}
}
