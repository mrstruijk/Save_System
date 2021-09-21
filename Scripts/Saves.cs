using System.Collections.Generic;


namespace mrstruijk.SaveSystem
{
	/// <summary>
	/// All files that need to be Saved, need the [System.Serializable] attribute
	/// From GameDevGuide:
	/// https://www.youtube.com/watch?v=5roZtuqZyuw
	/// </summary>
	[System.Serializable]
	public class Saves
	{
		private static Saves _current;

		public static Saves current
		{
			get => _current ??= new Saves();
			set => _current = value;
		}

		public List<SaveData> saveData = new List<SaveData>();


		public void AddToSaveList(SaveData objectData)
		{
			saveData.Add(objectData);
		}
	}
}
