using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace _mrstruijk.Components.SaveSystem.Scripts
{
	/// <summary>
	/// All files that need to be Saved, need the [System.Serializable] attribute
	/// From GameDevGuide:
	/// https://www.youtube.com/watch?v=5roZtuqZyuw
	/// </summary>
	public class SerializationManager
	{
		public static Action onLoadEvent;


		public static string saveDir = Application.dataPath + "/_mrstruijk/Components/SaveSystem/saves/";
		public static string saveName;
		public static string saveExtension = ".save";

		public static bool Save(string saveName, object saveData)
		{
			var formatter = GetBinaryFormatter();

			if (!Directory.Exists(saveDir))
			{
				Directory.CreateDirectory(saveDir);

			}
			var file = File.Create(saveDir + saveName + saveExtension);

			formatter.Serialize(file, saveData);

			file.Close();

			return true;
		}


		public static object Load(string saveName)
		{
			var formatter = GetBinaryFormatter();

			var file = File.Open(saveDir + saveName + saveExtension, FileMode.Open);

			try
			{
				object save = formatter.Deserialize(file);
				file.Close();
				return save;
			}
			catch
			{
				Debug.LogErrorFormat("Failed to load file {0}", saveDir + saveName + saveExtension);
				file.Close();
				return null;
			}

			onLoadEvent?.Invoke();
		}


		private static BinaryFormatter GetBinaryFormatter()
		{
			var formatter = new BinaryFormatter();

			var selector = new SurrogateSelector();

			var vector3Surrogate = new Vector3SerializationSurrogate();
			var quaternionSurrogate = new QuaterionSerializationSurrogate();

			selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
			selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);

			formatter.SurrogateSelector = selector;

			return formatter;
		}
	}
}
