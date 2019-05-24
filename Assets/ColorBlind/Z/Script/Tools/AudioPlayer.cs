using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class z_AudioSetting {
	public string AudioName;
	public AudioClip clip;
}

[System.Serializable]
public class AudioList {
	public z_AudioSetting[] m_AudioSetting;
}

[RequireComponent (typeof (AudioSource))]
public class AudioPlayer : MonoBehaviour {
	[Header ("Audio Player")]
	public AudioList m_AudioList;
	public AudioSource m_AudioSource;
	private Dictionary<string, AudioClip> AudioDic = new Dictionary<string, AudioClip> ();
	[Header ("Audio set")]
	public float volume;
	public bool isMute;
	public float prevolume;
	// Use this for initialization
	void Start () {
		init ();
	}
	// Update is called once per frame
	void Update () {

	}
	void init () {
		foreach (var audioset in m_AudioList.m_AudioSetting) {
			AudioDic[audioset.AudioName] = audioset.clip;
		}
		AudioManager.Instance.SceneAudioPlayerList.Add (this);
	}
	public void GetVolume () {
		volume = AudioManager.Instance.AudioManager_Volume;
		SetVolume (volume);
	}
	public void Mute () {
		if (isMute) {
			isMute = false;
			volume = prevolume;
			SetVolume (volume);
		} else {
			isMute = true;
			prevolume = volume;
			volume = 0;
			SetVolume (volume);
		}
	}
	void SetVolume (float f) {
		m_AudioSource.volume = f;
	}
}