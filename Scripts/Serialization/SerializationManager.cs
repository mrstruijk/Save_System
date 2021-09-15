using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace _mrstruijk.SaveSystem
{
	/// <summary>
	/// All files that need to be Saved, need the [System.Serializable] attribute
	/// From GameDevGuide:
	/// https://www.youtube.com/watch?v=5roZtuqZyuw
	/// </summary>
	public class SerializationManager
	{
		public static readonly string saveDir = Application.persistentDataPath + "/saves/";
		public const string saveExtention = ".save";


		public static bool Save(string currentGroup, string saveName, object saveData)
		{
			var formatter = GetBinaryFormatter();

			if (!Directory.Exists(saveDir))
			{
				Directory.CreateDirectory(saveDir);
			}
			if (!Directory.Exists(saveDir + currentGroup + "/"))
			{
				Directory.CreateDirectory(saveDir + currentGroup + "/");
			}

			string fullPath = saveDir + currentGroup + "/" + saveName + saveExtention;

			var file = File.Create(fullPath);

			formatter.Serialize(file, saveData);

			file.Close();

			return true;
		}


		public static object Load(string fileName)
		{
			if (!File.Exists(fileName))
			{
				Debug.LogErrorFormat("File at path: {0} does not exist!", fileName);
				return null;
			}

			var formatter = GetBinaryFormatter();

			var file = File.Open(fileName, FileMode.Open); // Does not require the fullPath for some reason.

			try
			{
				object save = formatter.Deserialize(file);
				file.Close();
				return save;
			}
			catch
			{
				Debug.LogErrorFormat("Failed to load file {0}", fileName);
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
