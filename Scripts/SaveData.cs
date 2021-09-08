using System.Collections.Generic;
using UnityEngine;


namespace _mrstruijk.Components.SaveSystem.Scripts
{
	/// <summary>
	/// All files that need to be Saved, need the [System.Serializable] attribute
	/// From GameDevGuide:
	/// https://www.youtube.com/watch?v=5roZtuqZyuw
	/// </summary>
	[System.Serializable]
	public class SaveData
	{
		private static SaveData _current;

		public static SaveData current
		{
			get
			{
				if (_current == null)
				{
					_current = new SaveData();
				}

				return _current;
			}
			set
			{
				_current = value;
			}
		}

		public List<ToyData> toys = new List<ToyData>();


		public void AddToToyList(ToyData toyData)
		{
			toys.Add(toyData);
			Debug.Log("Added " + toyData.toyType + " to list." + " List count == " + toys.Count);
		}
	}
}

