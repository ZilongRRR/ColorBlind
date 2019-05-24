using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZTools;

public class AudioManager : Singleton<AudioManager> {
	[Header ("Audio Manager")]
	[Range (0, 1)]
	public float AudioManager_Volume;
	public List<AudioPlayer> SceneAudioPlayerList = new List<AudioPlayer> ();

	protected override void initializationSet () {
		init ();
	}
	void init () {
		if (PlayerPrefs.HasKey ("AudioManager_Volume"))
			AudioManager_Volume = PlayerPrefs.GetFloat ("AudioManager_Volume");
	}

	public void Mute () {
		foreach (var audioPlayer in SceneAudioPlayerList) {
			audioPlayer.Mute ();
		}
	}
}