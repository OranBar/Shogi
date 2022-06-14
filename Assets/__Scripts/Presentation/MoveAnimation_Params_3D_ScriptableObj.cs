using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{
	[CreateAssetMenu( fileName = "MoveAnim_Param", menuName = "Shogi/Move Animation Parameters", order = 1 )]
    public class MoveAnimation_Params_3D_ScriptableObj : ScriptableObject
    {
		public AnimationCurve forwardAnimCurve;
		public float animDuration;
		public float yLift_onMovement;
		public AnimationCurve liftPieceAnimCurve;
    }
}
