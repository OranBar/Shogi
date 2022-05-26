using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace Shogi
{
    public class Shogi_PhotonSerialization : MonoBehaviour
    {
        void Start()
        {
			PhotonPeer.RegisterType( typeof( MovePieceAction ), (byte)'M', Serialize_MovePieceAction, Deserialize_MovePieceAction );
			PhotonPeer.RegisterType( typeof( AShogiAction ), (byte)'A', Serialize_AShogiAction, Deserialize_AShogiAction );
        }

		private static short Serialize_AShogiAction( StreamBuffer outStream, object customobject ) {
			switch (customobject) {
				case UndoLastAction undoAction:
					break;
				case DropPieceAction dropPieceAction:
					break;
				case MovePieceAction movePieceAction:
					return Serialize_MovePieceAction( outStream, movePieceAction );
				default:
					break;
			}
			return -1;
		}

		public static readonly byte [] memShogiActionType = new byte [5];

		private static object Deserialize_AShogiAction( StreamBuffer inStream, short length ) {
			inStream.Read( memShogiActionType, 0, 5 );
			int typeInt = 0;
			int index = 0;
			Protocol.Deserialize( out typeInt, memShogiActionType, ref index );

			char type = (char)typeInt;
			switch(type){
				case 'M':
					return Deserialize_MovePieceAction( inStream, length );
				default:
					break;
			}

			return -1;
		}

		#region Move Piece Action
		public const int MOVEACTION_BYTESIZE = 15;
		public static readonly byte [] memMovePieceAction = new byte [MOVEACTION_BYTESIZE];
		private static short Serialize_MovePieceAction(StreamBuffer outStream, object customobject)
		{
			MovePieceAction moveAction = (MovePieceAction)customobject;
			lock (memMovePieceAction)
			{
				byte[] bytes = memMovePieceAction;
				int index = 0;
				int piecePhotonId = moveAction.ActingPiece.GetComponent<PhotonView>().ViewID;
				Protocol.Serialize( piecePhotonId, bytes, ref index );
				Protocol.Serialize( moveAction.DestinationX, bytes, ref index );
				Protocol.Serialize( moveAction.DestinationY, bytes, ref index );
				outStream.Write( bytes, 0, MOVEACTION_BYTESIZE );
			}

			return MOVEACTION_BYTESIZE;
		}

		private static object Deserialize_MovePieceAction( StreamBuffer inStream, short length ) {
			int piecePhotonId = -1;
			int destinationX = -3;
			int destinationY = -3;
			lock (memMovePieceAction) {
				inStream.Read( memMovePieceAction, 0, MOVEACTION_BYTESIZE );
				int index = 0;
				Protocol.Deserialize( out piecePhotonId, memMovePieceAction, ref index );
				Protocol.Deserialize( out destinationX, memMovePieceAction, ref index );
				Protocol.Deserialize( out destinationY, memMovePieceAction, ref index );
			}
			Piece actingPiece = PhotonView.Find( piecePhotonId ).GetComponent<Piece>();
			MovePieceAction result = new MovePieceAction( actingPiece, destinationX, destinationY );

			return result;
		}
		#endregion
    }
}
