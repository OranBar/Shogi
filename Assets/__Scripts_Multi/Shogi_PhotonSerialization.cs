using System;
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
			// PhotonPeer.RegisterType( typeof( AShogiAction ), (byte)'A', Serialize_AShogiAction, Deserialize_AShogiAction );
			PhotonPeer.RegisterType( typeof( MovePieceAction ), (byte)'M', Serialize_MovePieceAction, Deserialize_MovePieceAction );
			PhotonPeer.RegisterType( typeof( DropPieceAction ), (byte)'D', Serialize_DropPieceAction, Deserialize_DropPieceAction );
			PhotonPeer.RegisterType( typeof( UndoLastAction ), (byte)'U', Serialize_UndoLastAction, Deserialize_UndoLastAction );
        }

		public static readonly byte [] memShogiActionType = new byte [5];
		private static short Serialize_AShogiAction( StreamBuffer outStream, object customobject ) {
			switch (customobject) {
				case UndoLastAction undoAction:
					break;
				case DropPieceAction dropPieceAction:
					return Serialize_DropPieceAction( outStream, dropPieceAction );
				case MovePieceAction movePieceAction:
					return Serialize_MovePieceAction( outStream, movePieceAction );
				default:
					break;
			}
			return -1;
		}

		private static object Deserialize_AShogiAction( StreamBuffer inStream, short length ) {
			inStream.Read( memShogiActionType, 0, 5 );
			int typeInt = 0;
			int index = 0;
			Protocol.Deserialize( out typeInt, memShogiActionType, ref index );

			char type = (char)typeInt;
			switch(type){
				case 'D':
					return Deserialize_DropPieceAction( inStream, length);
				case 'M':
					return Deserialize_MovePieceAction( inStream, length );
				case 'U':
					return new UndoLastAction();
				default:
					break;
			}

			return -1;
		}

		#region Move Piece Action
		public const int MOVEACTION_BYTESIZE = 16;
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
				memMovePieceAction [index] = Convert.ToByte(moveAction.Request_PromotePiece);
				outStream.Write( bytes, 0, MOVEACTION_BYTESIZE );
			}

			return MOVEACTION_BYTESIZE;
		}

		private static object Deserialize_MovePieceAction( StreamBuffer inStream, short length ) {
			int piecePhotonId = -1;
			int destinationX = -3;
			int destinationY = -3;
			bool requestPromotion = false;
			lock (memMovePieceAction) {
				inStream.Read( memMovePieceAction, 0, MOVEACTION_BYTESIZE );
				int index = 0;
				Protocol.Deserialize( out piecePhotonId, memMovePieceAction, ref index );
				Protocol.Deserialize( out destinationX, memMovePieceAction, ref index );
				Protocol.Deserialize( out destinationY, memMovePieceAction, ref index );
				requestPromotion = Convert.ToBoolean(memMovePieceAction [index]);
			}
			Piece actingPiece = PhotonView.Find( piecePhotonId ).GetComponent<Piece>();
			MovePieceAction result = new MovePieceAction( actingPiece, destinationX, destinationY );
			result.Request_PromotePiece = requestPromotion;
			return result;
		}
		#endregion

		#region Drop Piece Action

		public const int DROPACTION_BYTESIZE = 15;
		public static readonly byte [] memDropPieceAction = new byte [DROPACTION_BYTESIZE];
		private static short Serialize_DropPieceAction(StreamBuffer outStream, object customobject)
		{
			DropPieceAction dropAction = (DropPieceAction)customobject;
			lock (memDropPieceAction)
			{
				byte[] bytes = memDropPieceAction;
				int index = 0;
				int piecePhotonId = dropAction.ActingPiece.GetComponent<PhotonView>().ViewID;
				Protocol.Serialize( piecePhotonId, bytes, ref index );
				Protocol.Serialize( dropAction.DestinationX, bytes, ref index );
				Protocol.Serialize( dropAction.DestinationY, bytes, ref index );
				outStream.Write( bytes, 0, DROPACTION_BYTESIZE );
			}

			return DROPACTION_BYTESIZE;
		}

		private static object Deserialize_DropPieceAction( StreamBuffer inStream, short length ) {
			int piecePhotonId = -1;
			int destinationX = -3;
			int destinationY = -3;
			lock (memDropPieceAction) {
				inStream.Read( memDropPieceAction, 0, DROPACTION_BYTESIZE );
				int index = 0;
				Protocol.Deserialize( out piecePhotonId, memDropPieceAction, ref index );
				Protocol.Deserialize( out destinationX, memDropPieceAction, ref index );
				Protocol.Deserialize( out destinationY, memDropPieceAction, ref index );
			}
			Piece actingPiece = PhotonView.Find( piecePhotonId ).GetComponent<Piece>();
			DropPieceAction result = new DropPieceAction( actingPiece, destinationX, destinationY );

			return result;
		}
		#endregion


		#region Undo last Action
		// public static readonly byte [] memUndoAction = new byte [0];

		private static short Serialize_UndoLastAction( StreamBuffer outStream, object customobject ) {
			return 0;
		}
		private static object Deserialize_UndoLastAction( StreamBuffer inStream, short length ) {
			return new UndoLastAction();
		}
		#endregion

	}
}

