using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace _mrstruijk.Components.SaveSystem.Scripts
{
	public class SaveManager : MonoBehaviour
	{
		public TMP_InputField saveNameInput;
		public GameObject loadButtonPrefab;
		public Transform loadArea;
		public List<string> saveFiles;

		private ObjectsToLoadManager objectsToLoadManager;
		private readonly string[] excludedExtentions = {".meta", ".DS_Store"};


		private void Awake()
		{
			objectsToLoadManager = FindObjectOfType<ObjectsToLoadManager>();
			SaveData.current = new SaveData();
			GetLoadFiles();
		}


		private void Start()
		{
			ShowLoadScreen();
		}


		public void OnSave()
		{
			var objectHandlers = FindObjectsOfType<ObjectHandler>();
			foreach (var handler in objectHandlers)
			{
				handler.SavePositionAndRotation();
			}

			var success = SerializationManager.Save(saveNameInput.text, SaveData.current);

			if (!success)
			{
				Debug.LogError("Saving was not succesfull!");
			}
		}


		private void GetLoadFiles()
		{
			if (!Directory.Exists(SerializationManager.saveDir))
			{
				Directory.CreateDirectory(SerializationManager.saveDir);
				Debug.LogFormat("Had to create path: {0}", SerializationManager.saveDir);
			}

			var files = Directory.GetFiles(SerializationManager.saveDir).Where(file => !excludedExtentions.Any(x => file.EndsWith(x, StringComparison.Ordinal)));

			foreach (var file in files)
			{
				saveFiles.Add(file);
			}
		}


		private void ShowLoadScreen()
		{
			GetLoadFiles();

			for (int i = 0; i < saveFiles.Count; i++)
			{
				var button = Instantiate(loadButtonPrefab, loadArea.transform, false);

				var index = i;

				button.GetComponent<Button>().onClick.AddListener(() =>
				{
					objectsToLoadManager.OnLoad(saveFiles[index]);
				});

				var buttonText = saveFiles[index].Replace(SerializationManager.saveDir, "").Replace(SerializationManager.saveExtention, "");
				button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
			}
		}
	}
}
