using UnityEngine;

namespace Shogi
{

	public class Logger
	{
		public static void Log( object message ) {
			Debug.Log( SetSquareBraketsBold( message ) );
		}

		public static void Log( object message, GameObject context ) {
			Debug.Log( SetSquareBraketsBold( message ), context );
		}

		private static string SetSquareBraketsBold( object msg ) {
			string messageWithBold = msg as string;
			messageWithBold = messageWithBold.Replace( "[", "<b>[" ).Replace( "]", "]</b>" );
			return messageWithBold;
		}
	}
}
