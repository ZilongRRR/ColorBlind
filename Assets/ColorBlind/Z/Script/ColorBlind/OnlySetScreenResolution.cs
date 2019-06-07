using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlySetScreenResolution : MonoBehaviour {
	public float targetWidth = 1080;
	public float targetHeight = 1920;
	// Use this for initialization
	public void setDesignContentScale () {

		float width = Screen.currentResolution.width; //1300
		float height = Screen.currentResolution.height; //1920

		float newWidth = 0;
		float newHeight = 0;

		float wh = (float) targetWidth / (float) targetHeight;
		float hw = (float) targetHeight / (float) targetWidth;
		newWidth = wh * height;
		if (width < newWidth) {
			newWidth = width;
			newHeight = width * hw;
		} else {
			newHeight = height;
		}
		newWidth = Mathf.FloorToInt (newWidth);
		newHeight = Mathf.FloorToInt (newHeight);
#if UNITY_STANDALONE_WIN
		Screen.SetResolution (540, 960, false);
#elif UNITY_STANDALONE_OSX
		Screen.SetResolution (270, 480, false);
#else
		Screen.SetResolution ((int) newWidth, (int) newHeight, true);
#endif
	}

	void OnApplicationPause (bool paused) {
		if (paused) { } else {
			setDesignContentScale ();
		}
	}

	void Awake () {
		setDesignContentScale ();
	}

}