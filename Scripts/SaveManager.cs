using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


namespace _mrstruijk.SaveSystem
{
	[CreateAssetMenu(menuName = "mrstruijk/SaveSystem/SaveManager", fileName = "SaveManager")]
	public class SaveManager : ScriptableObject
	{
		public ObjectsToLoadManagerSO objectsToLoadManager;

		public readonly List<string> saveFiles = new List<string>();
		private readonly string[] excludedExtentions = {".meta", ".DS_Store"};


		public bool OnSave(string saveName)
		{
			var objectHandlers = FindObjectsOfType<ObjectHandler>();
			foreach (var handler in objectHandlers)
			{
				handler.SavePositionAndRotation();
			}

			var success = SerializationManager.Save(saveName, SaveData.current);

			if (!success)
			{
				Debug.LogError("Saving was not succesfull!");
			}

			return success;
		}


		public void GetLoadFiles()
		{
			if (!Directory.Exists(SerializationManager.saveDir))
			{
				Directory.CreateDirectory(SerializationManager.saveDir);
				Debug.LogFormat("Had to create path: {0}", SerializationManager.saveDir);
			}

			var files = Directory.GetFiles(SerializationManager.saveDir).Where(file => !excludedExtentions.Any(x => file.EndsWith(x, StringComparison.Ordinal)));

			foreach (var file in files)
			{
				if (!saveFiles.Contains(file))
				{
					saveFiles.Add(file);
				}
			}
		}
	}
}
