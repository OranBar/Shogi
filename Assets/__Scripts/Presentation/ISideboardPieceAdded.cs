using Cysharp.Threading.Tasks;


namespace Shogi
{
	public interface ISideboardFX
	{
		public UniTask PieceAddedToSideboard_FX(Piece piece);
	}
}