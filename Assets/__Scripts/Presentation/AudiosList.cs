using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Shogi/Audios_List")]
public class AudiosList : ScriptableObject
{
	public List<AudioClip> movementAudios;
	public List<AudioClip> deathAudios;

	private AudioClip lastMoveAudio, lastDeathAudio;

	public AudioClip GetMoveAudio() {
		AudioClip result = null;
		if (movementAudios.Count >= 2) {
			result = movementAudios.Except( new List<AudioClip>() { lastMoveAudio } ).ToList().GetRandomElement();
		}else {
			result = movementAudios.GetRandomElement();
		}
		lastMoveAudio = result;
		return result;
	}

	public AudioClip GetDeathAudio() {
		AudioClip result = null;
		if(deathAudios.Count >= 2){
			result = deathAudios.Except( new List<AudioClip>() { lastDeathAudio } ).ToList().GetRandomElement();
		} else {
			result = deathAudios.GetRandomElement();
		}
		lastDeathAudio = result;
		return result;
	}
}