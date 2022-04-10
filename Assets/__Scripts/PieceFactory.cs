using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shogi{

	public class PieceFactory : MonoBehaviour
	{
		[SerializeField] private string editorLabel;
		public List<GameObject> piecesPrefabs;

		public GameObject CreatePiece( PieceType pieceType ) {
			GameObject piecePrefab = piecesPrefabs
				.First( p => p.GetComponent<Piece>().PieceType == pieceType );

			return GameObject.Instantiate( piecePrefab );
		}
	}
}