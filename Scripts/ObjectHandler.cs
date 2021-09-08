using UnityEngine;
using Random = UnityEngine.Random;


namespace _mrstruijk.Components.SaveSystem.Scripts
{
	public class ObjectHandler : MonoBehaviour
	{
		public ObjectData objectData;


		private void OnEnable()
		{
			GameEvents.current.onLoadEvent += DestroyMe;
		}


		private void Start()
		{
			if (string.IsNullOrEmpty(objectData.id))
			{
				objectData.id = Random.Range(0, 10000).ToString();
				SaveData.current.AddToObjectList(objectData);
			}
		}


		public void SavePositionAndRotation()
		{
			objectData.position = transform.position;
			objectData.rotation = transform.rotation;
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
