using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace _mrstruijk.SaveSystem
{
	public class SaveManagerUI : MonoBehaviour
	{
		public SaveManager saveManager;

		public TMP_InputField saveNameInput;
		public GameObject loadButtonPrefab;
		public Transform buttonArea;


		private void Awake()
		{
			SaveData.current = new SaveData();
			saveManager.GetLoadFiles();
		}


		private void Start()
		{
			CreateLoadButtons();
		}


		private void CreateLoadButtons()
		{
			saveManager.GetLoadFiles();

			for (int i = 0; i < saveManager.saveFiles.Count; i++)
			{
				var button = Instantiate(loadButtonPrefab, buttonArea.transform, false);

				var index = i;

				button.GetComponent<Button>().onClick.AddListener(() => { saveManager.objectsToLoadManager.OnLoad(saveManager.saveFiles[index]); });

				var buttonText = saveManager.saveFiles[index].Replace(SerializationManager.saveDir, "").Replace(SerializationManager.saveExtention, "");
				button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
			}
		}


		/// <summary>
		/// Run from UI
		/// </summary>
		public void Save()
		{
			if (string.IsNullOrEmpty(saveNameInput.text))
			{
				Debug.LogError("Cannot save without a savename");
				return;
			}

			bool success = saveManager.OnSave(saveNameInput.text);

			if (success == true)
			{
				DeleteLoadButtons();

				CreateLoadButtons();
			}
		}


		private void DeleteLoadButtons()
		{
			foreach (Transform button in buttonArea)
			{
				Destroy(button.gameObject);
			}
		}
	}
}
