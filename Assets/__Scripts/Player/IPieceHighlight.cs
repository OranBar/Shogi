using UnityEngine;

namespace Shogi
{
	public interface IPieceHighlight
	{
		void EnableHighlight( Color color );
		void DisableHighlight();
	}
}
