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
		private List<string> groups = new List<string>();
		private string currentGroup;
		public string CurrentGroup
		{
			get => currentGroup;
			set
			{
				EventSystem.OnGroupChangeAction?.Invoke();
				currentGroup = value;
			}
		}
		public List<string> Groups
		{
			get => groups;
			set => groups = value;
		}

		private List<string> saveFiles = new List<string>();
		public List<string> SaveFiles
		{
			get => saveFiles;
			set => saveFiles = value;
		}

		private readonly string[] excludedExtentions = {".meta", ".DS_Store"};


		/// <summary>
		/// Called from UI
		/// </summary>
		/// <param name="saveName"></param>
		/// <returns></returns>
		public bool OnSave(string currentGroup, string saveName)
		{
			EventSystem.OnSaveAction?.Invoke();

			this.currentGroup = currentGroup;
			var success = SerializationManager.Save(this.currentGroup, saveName, Saves.current);

			if (!success)
			{
				Debug.LogError("Saving was not succesfull!");
			}

			return success;
		}


		public void GetLoadGroups()
		{
			if (!Directory.Exists(SerializationManager.saveDir))
			{
				Directory.CreateDirectory(SerializationManager.saveDir);
				Debug.LogFormat("Had to create path: {0}", SerializationManager.saveDir);
			}

			var folders = Directory.GetDirectories(SerializationManager.saveDir);

			foreach (var folder in folders)
			{
				if (!groups.Contains(folder))
				{
					groups.Add(folder);
				}
			}
		}


		public void GetLoadFiles(string currentGroup)
		{
			if (!Directory.Exists(SerializationManager.saveDir + currentGroup + "/"))
			{
				Directory.CreateDirectory(SerializationManager.saveDir + currentGroup + "/");
				Debug.LogFormat("Had to create path: {0}", SerializationManager.saveDir + currentGroup + "/");
			}

			var files = Directory.GetFiles(SerializationManager.saveDir + currentGroup + "/").Where(file => !excludedExtentions.Any(x => file.EndsWith(x, StringComparison.Ordinal)));

			ClearSaveList();

			foreach (var file in files)
			{
				if (!saveFiles.Contains(file))
				{
					saveFiles.Add(file);
				}
			}
		}


		public void ClearSaveList()
		{
			saveFiles = new List<string>();
		}
	}
}
