using UnityEngine;


namespace _mrstruijk.Components.SaveSystem.Scripts
{
	[System.Serializable]
	public enum ToyType
	{
		Cube,
		Sphere,
		Capule
	}


	[System.Serializable]
	public class ToyData
	{
		public string id;
		public ToyType toyType;
		public Vector3 position;
		public Quaternion rotation;
	}
}
