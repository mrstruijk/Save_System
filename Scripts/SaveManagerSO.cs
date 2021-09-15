using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _mrstruijk.Events;
using Sirenix.OdinInspector;
using UnityEngine;



namespace _mrstruijk.SaveSystem
{
	[CreateAssetMenu(menuName = "mrstruijk/SaveSystem/SaveManager", fileName = "SaveManager")]
	public class SaveManagerSO : ScriptableObject
	{
		private List<string> groups = new List<string>();
		public List<string> Groups
		{
			get => groups;
		}

		private string currentGroup;
		public string CurrentGroup
		{
			get => currentGroup;
			set => currentGroup = value;
		}

		private List<string> currentGroupSaveFiles = new List<string>();
		public List<string> CurrentGroupSaveFiles
		{
			get => currentGroupSaveFiles;
		}

		private List<string> allSaveFiles = new List<string>();
		public List<string> AllSaveFiles
		{
			get => allSaveFiles;
		}

		private IEnumerable<string> all;

		private readonly string[] excludedExtentions = {".meta", ".DS_Store"};


		public bool OnSave(string saveName)
		{
			EventSystem.OnSaveAction?.Invoke();

			var success = SerializationManager.Save(currentGroup, saveName, Saves.current);

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
			var groupPath = SerializationManager.saveDir + currentGroup + "/";

			if (!Directory.Exists(groupPath))
			{
				Directory.CreateDirectory(groupPath);
				Debug.LogFormat("Had to create path: {0}", groupPath);
			}

			var files = Directory.GetFiles(groupPath).Where(file => !excludedExtentions.Any(x => file.EndsWith(x, StringComparison.Ordinal)));

			ClearCurrentSaveList();

			foreach (var file in files)
			{
				if (!currentGroupSaveFiles.Contains(file))
				{
					currentGroupSaveFiles.Add(file);
				}
			}

			GetAllLoadFiles();
		}

		public void GetAllLoadFiles()
		{

			if (!Directory.Exists(SerializationManager.saveDir))
			{
				Directory.CreateDirectory(SerializationManager.saveDir);
				Debug.LogFormat("Had to create path: {0}", SerializationManager.saveDir);
			}

			ClearAllSaveList();

			foreach (var group in Directory.GetDirectories(SerializationManager.saveDir))
			{
				var files = Directory.GetFiles(group).Where(file => !excludedExtentions.Any(x => file.EndsWith(x, StringComparison.Ordinal)));


				foreach (var file in files)
				{
					if (!allSaveFiles.Contains(file))
					{
						allSaveFiles.Add(file);
					}
				}
			}

		}

		[Button]
		public void DeleteGroup(string groupName)
		{
			SerializationManager.DeleteGroup(groupName);
			GetLoadGroups();
		}

		[Button]
		public void DeleteFile(string groupAndFileName)
		{
			SerializationManager.DeleteSaveFile(groupAndFileName);
		}


		public void ClearGroupList()
		{
			groups = new List<string>();
		}

		public void ClearCurrentSaveList()
		{
			currentGroupSaveFiles = new List<string>();
		}

		public void ClearAllSaveList()
		{
			allSaveFiles = new List<string>();
		}
	}
}
