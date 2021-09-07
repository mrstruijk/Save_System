using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
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
		private ToyManager toyManager;
		public string[] saveFiles;




		private void Awake()
		{
			toyManager = FindObjectOfType<ToyManager>();
			GetSaveFiles();
		}


		private void Start()
		{
			ShowLoadScreen();
		}


		public void OnSave()
		{
			SerializationManager.Save(saveNameInput.text, SaveData.current);
			Debug.Log("Saved");
		}


		private void GetSaveFiles()
		{
			saveFiles = Directory.GetFiles(SerializationManager.saveDir);
		}


		private void ShowLoadScreen()
		{
			for (int i = 0; i < saveFiles.Length; i++)
			{
				var buttonObject = Instantiate(loadButtonPrefab);
				buttonObject.transform.SetParent(loadArea.transform, false);

				var index = i;

				buttonObject.GetComponent<Button>().onClick.AddListener(() =>
				{
					toyManager.OnLoad(saveFiles[index]);
				});

				buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = saveFiles[index].Replace(SerializationManager.saveDir, "").Replace(".save", "");
			}
		}
	}
}
