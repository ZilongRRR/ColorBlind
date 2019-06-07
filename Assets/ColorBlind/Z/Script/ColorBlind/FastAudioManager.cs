using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZTools;
public class FastAudioManager : Singleton<FastAudioManager> {
	[Header ("選擇音樂和音效物件")]
	public AudioSource sound;
	public AudioSource soundEffect;
	public AudioSource soundEffect2;
	[Header ("設置音樂和音效")]
	public AudioClip[] backGroundSound;
	public AudioClip[] effectSound;

	protected override void initializationSet () {
		if (backGroundSound.Length > 0) {
			sound.clip = backGroundSound[0];
			sound.Play ();
		}
	}

	public void PlayOneShotEffect (int index) {
		if (index >= effectSound.Length)
			return;

		soundEffect.PlayOneShot (effectSound[index]);
	}

	public void PlayEffect (int index) {
		if (index >= effectSound.Length)
			return;

		soundEffect.Pause ();
		soundEffect.clip = effectSound[index];
		soundEffect.Play ();
	}

	public void PlayEffect (int index, bool isFirst) {
		if (index >= effectSound.Length)
			return;

		if (isFirst) {
			PlayEffect (index);
		} else {
			soundEffect2.Pause ();
			soundEffect2.clip = effectSound[index];
			soundEffect2.Play ();
		}
	}
	public void PlayBackground (int index) {
		if (index >= effectSound.Length)
			return;

		sound.Pause ();
		sound.clip = backGroundSound[index];
		sound.Play ();
	}
}