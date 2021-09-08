using System.IO;
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
			SaveData.current = new SaveData();
			GetLoadFiles();
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


		private void GetLoadFiles()
		{
			if (!Directory.Exists(Application.persistentDataPath + "/saves/"))
			{
				Directory.CreateDirectory(Application.persistentDataPath + "/saves/");
				Debug.Log("Had to create path: " + Application.persistentDataPath + "/saves/");
			}

			saveFiles = Directory.GetFiles(Application.persistentDataPath + "/saves/");

			foreach (var file in saveFiles)
			{
				Debug.Log("Loaded file: " + file);
			}
			//saveFiles = (string[]) Directory.GetFiles(SerializationManager.saveDir).Where(ext => !ext.EndsWith(".meta"));
		}


		private void ShowLoadScreen()
		{
			GetLoadFiles();

			for (int i = 0; i < saveFiles.Length; i++)
			{
				var buttonObject = Instantiate(loadButtonPrefab);
				buttonObject.transform.SetParent(loadArea.transform, false);

				var index = i;

				buttonObject.GetComponent<Button>().onClick.AddListener(() =>
				{
					toyManager.OnLoad(saveFiles[index]);
				});

				// buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = saveFiles[index].Replace(SerializationManager.saveDir, "").Replace(".save", "");
				buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = saveFiles[index].Replace(Application.persistentDataPath + "/saves/", "");
			}
		}
	}
}
