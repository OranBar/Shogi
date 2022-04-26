using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Shogi
{
	public static class SerializationUtils
	{
		public static void SerializeToBinaryFile<T>( T toSerialize, string savePath ) {
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream( savePath, FileMode.Create, FileAccess.Write, FileShare.None );
			formatter.Serialize( stream, toSerialize );
			stream.Close();
			UnityEngine.Debug.Log( "File serialized at " + savePath );
		}

		public static T DeserializeFromBinaryFile<T>( string filePath ) {
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream( filePath, FileMode.Open, FileAccess.Read, FileShare.Read );
			T deserializedData = (T)formatter.Deserialize( stream );
			stream.Close();

			return deserializedData;
		}
	}
}