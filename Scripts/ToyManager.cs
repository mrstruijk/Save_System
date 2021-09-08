using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace _mrstruijk.Components.SaveSystem.Scripts
{
	public class ToyManager : MonoBehaviour
	{
		public List<GameObject> toys;


		public void OnSave(string saveFile)
		{
			SerializationManager.Save(saveFile, SaveData.current);
		}


		public void OnLoad(string saveFile)
		{
			Debug.Log("Loading object");

			GameEvents.current.onLoadEvent?.Invoke();

			SaveData.current = (SaveData) SerializationManager.Load(Application.persistentDataPath + "/saves/" + saveFile + ".save");

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


		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				SpawnRandom();
			}
		}


		private void SpawnRandom()
		{
			Instantiate(toys[Random.Range(0, toys.Count)]);
		}
	}
}
