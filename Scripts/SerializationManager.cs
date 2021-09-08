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

		// public static string saveDir = Application.persistentDataPath + "/saves/";



		public static bool Save(string saveName, object saveData)
		{
			var formatter = GetBinaryFormatter();

			if (!Directory.Exists(Application.persistentDataPath + "/saves/"))
			{
				Directory.CreateDirectory(Application.persistentDataPath + "/saves/");
			}

			string path = Application.persistentDataPath + "/saves/" + saveName + ".save";

			var file = File.Create(path);

			formatter.Serialize(file, saveData);

			file.Close();

			return true;
		}


		public static object Load(string path)
		{
			if (!File.Exists(path))
			{
				Debug.LogErrorFormat("File at path: {0} does not exist!", path);
				return null;
			}

			var formatter = GetBinaryFormatter();

			var file = File.Open(path, FileMode.Open);

			try
			{
				object save = formatter.Deserialize(file);
				file.Close();
				return save;
			}
			catch
			{
				Debug.LogErrorFormat("Failed to load file {0}", path);
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
