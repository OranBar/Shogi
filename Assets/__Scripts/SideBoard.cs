using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


namespace Shogi{
	public class SideBoard : MonoBehaviour
	{
		public PlayerId ownerId;
		public IPlayer Owner => GameObjectEx.FindAll_InterfaceImplementors<IPlayer>().First( p => p.OwnerId == ownerId );

		
	}

}
