using UnityEngine;


namespace mrstruijk.SaveSystem
{
	[System.Serializable]
	public class SaveData
	{
		public string id;
		public string name;
		public Vector3 position;
		public Quaternion rotation;
		public Rigidbody rigidBody;
	}
}
