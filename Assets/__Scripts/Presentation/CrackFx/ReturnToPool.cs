using UnityEngine;
using UnityEngine.Pool;

namespace Shogi
{
	// This component returns the particle system to the pool when the OnParticleSystemStopped event is received.
	[RequireComponent( typeof( ParticleSystem ) )]
	public class ReturnToPool : MonoBehaviour
	{
		public ParticleSystem system;
		public IObjectPool<GameObject> pool;

		void Start() {
			system = GetComponent<ParticleSystem>();
			var main = system.main;
			main.stopAction = ParticleSystemStopAction.Callback;
		}

		void OnParticleSystemStopped() {
			// Return to the pool
			pool.Release( this.transform.parent.gameObject );
		}
	}
}
