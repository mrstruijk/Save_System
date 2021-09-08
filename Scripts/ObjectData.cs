using UnityEngine;


namespace _mrstruijk.Components.SaveSystem.Scripts
{
	[System.Serializable]
	public enum ObjectType
	{
		Cube,
		Sphere,
		Capule
	}


	[System.Serializable]
	public class ObjectData
	{
		public string id;
		public ObjectType objectType;
		public Vector3 position;
		public Quaternion rotation;
	}
}
