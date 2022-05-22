using Mirror;
using Shogi;
using UnityEngine;

public static class CustomReadWriteFunctions
{
	public static void WriteAPlayer( this NetworkWriter writer, APlayer player ) {
		GameObject playerGameObject = player?.gameObject;
		writer.WriteGameObject( playerGameObject );
	}

	public static APlayer ReadAPlayer( this NetworkReader reader ) {
		GameObject playerGameObject = reader.ReadGameObject();
		APlayer player = playerGameObject?.GetComponent<APlayer>();

		return player;
	}
}