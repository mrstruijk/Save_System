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
		// public static Action onLoadEvent;


		public static string saveDir = Application.dataPath + "/_mrstruijk/Components/_Packages/Save_System/saves/";
		public static string saveName;
		public static string saveExtension = ".save";

		private static string fullPath;

		public static bool Save(string saveName, object saveData)
		{
			var formatter = GetBinaryFormatter();

			if (!Directory.Exists(saveDir))
			{
				Directory.CreateDirectory(saveDir);

			}

			fullPath = saveDir + saveName + saveExtension;

			var file = File.Create(fullPath);

			formatter.Serialize(file, saveData);

			file.Close();

			return true;
		}


		public static object Load(string saveName)
		{
			var formatter = GetBinaryFormatter();

			fullPath = saveDir + saveName + saveExtension;

			var file = File.Open(saveName, FileMode.Open);

			try
			{
				object save = formatter.Deserialize(file);
				file.Close();
				return save;
			}
			catch
			{
				Debug.LogErrorFormat("Failed to load file {0}", saveName);
				file.Close();
				return null;
			}

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
