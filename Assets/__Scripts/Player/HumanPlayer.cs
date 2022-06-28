using System;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Shogi
{
	[Serializable]
	public enum PlayerId{
		Player1 = 1,
		Player2
	}

	public static class PlayerIdEx{
		public static PlayerId GetOtherPlayer(this PlayerId playerId){
			return playerId == PlayerId.Player1 ? PlayerId.Player2 : PlayerId.Player1;
		} 
	}

	[Serializable]
	public class HumanPlayer : APlayer
	{
		[SerializeField] private string _playerName;
		public override string PlayerName { get => _playerName; set => _playerName = value; }

		public PlayerId _playerId;
		public override PlayerId PlayerId {
			get { return _playerId; }
			set { _playerId = value; }
		}

		public PlayerId OpponentId => _playerId == PlayerId.Player1 ? PlayerId.Player2 : PlayerId.Player1;

		public RefAction<Piece> OnPiece_Selected = new RefAction<Piece>();
		public RefAction<Piece> OnCapturePiece_Selected = new RefAction<Piece>();
		public RefAction<Cell> OnMoveCell_Selected = new RefAction<Cell>();

		[ReadOnly] public Piece selectedPiece;
		[HideInInspector] public ShogiGame shogiGame;

		public AActionSelectionStrategy actionSelectionStrategy;

		protected virtual void Awake(){
			shogiGame = FindObjectOfType<ShogiGame>(true );
		}

		public async override UniTask<AShogiAction> RequestAction() {
			return await actionSelectionStrategy.RequestAction();
		}
	}
}
