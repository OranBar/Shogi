using Cysharp.Threading.Tasks;


namespace Shogi
{
	public interface ISideboardPieceAdded
	{
		public UniTask OnNewPieceAdded(Piece piece);
	}
}