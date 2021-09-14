using _mrstruijk.Events;
using UnityEngine;
using Random = UnityEngine.Random;


namespace _mrstruijk.SaveSystem
{
	public class ObjectHandler : MonoBehaviour
	{
		public ObjectData objectData;


		private void OnEnable()
		{
			EventSystem.OnLoadAction += DestroyMe;
		}


		private void Start()
		{
			if (string.IsNullOrEmpty(objectData.id))
			{
				objectData.id = Random.Range(0, 10000).ToString();
				objectData.name = this.gameObject.name.Replace("(Clone)", "");
				SaveData.current.AddToObjectList(objectData);
			}
		}


		public void SavePositionAndRotation()
		{
			var trans = transform;
			objectData.position = trans.position;
			objectData.rotation = trans.rotation;
		}


		private void DestroyMe()
		{
			Destroy(gameObject);
		}


		private void OnDisable()
		{
			EventSystem.OnLoadAction -= DestroyMe;
		}
	}
}
