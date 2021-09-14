using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _mrstruijk.Events;
using UnityEngine;



namespace _mrstruijk.SaveSystem
{
	[CreateAssetMenu(menuName = "mrstruijk/SaveSystem/SaveManager", fileName = "SaveManager")]
	public class SaveManagerSO : ScriptableObject
	{
		private List<string> saveFiles = new List<string>();

		public List<string> SaveFiles
		{
			get => saveFiles;
		}

		private readonly string[] excludedExtentions = {".meta", ".DS_Store"};

		/// <summary>
		/// Called from UI
		/// </summary>
		/// <param name="saveName"></param>
		/// <returns></returns>
		public bool OnSave(string saveName)
		{
			EventSystem.OnSaveAction?.Invoke();

			var success = SerializationManager.Save(saveName, Saves.current);

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
