using System;
using UnityEngine;
using Random = UnityEngine.Random;


namespace _mrstruijk.Components.SaveSystem.Scripts
{
	public class ToyHandler : MonoBehaviour
	{
		public ToyType toyType;
		public ToyData toyData;


		private void OnEnable()
		{
			GameEvents.current.onLoadEvent += DestroyMe;
		}


		private void Start()
		{
			if (string.IsNullOrEmpty(toyData.id))
			{
				toyData.id = Random.Range(0, 10000).ToString();
				toyData.toyType = toyType;
				SaveData.current.AddToToyList(toyData);
			}
		}



		private void Update()
		{
			toyData.position = transform.position;
			toyData.rotation = transform.rotation;
		}


		private void DestroyMe()
		{
			GameEvents.current.onLoadEvent -= DestroyMe;
			Destroy(gameObject);
		}


		private void OnDisable()
		{
			GameEvents.current.onLoadEvent -= DestroyMe;
		}
	}
}
