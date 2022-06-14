using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Shogi
{
	public class CrackFXPool : MonoBehaviour
	{
		public GameObject crackFx_prefab;
		public ObjectPool<GameObject> pool;

		void Awake() {
			pool = new ObjectPool<GameObject>(
				() => {
					var newFx = Instantiate( crackFx_prefab );
					var newParticleSystem = newFx.GetComponentInChildren<ParticleSystem>().gameObject;
					var returnToPool = newParticleSystem.AddComponent<ReturnToPool>();
					returnToPool.pool = pool;
					return newFx;
				},
				fx => fx.gameObject.SetActive( true ),
				fx => fx.gameObject.SetActive( false ),
				fx => Destroy( fx.gameObject ),
				true,
				5,
				20
			);
		}

	}
}
